using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ToDoTaskList {
    public class MainViewModel : ViewModel {
        //constants
        private const string REST_BASE_URL = "http://windowsphoneuam.azurewebsites.net/";
        private const string REST_PATH = "api/ToDoTasks/";
        private const int ONLY_IN_LOCAL = 1;
        private const int ONLY_IN_REMOTE = -1;
        private const int IN_BOTH = 0;
        //sync modes constants
        public const int DEVICE_TO_SERVER = 0;
        public const int ADD_SERVER_CONTENT = 1;
        public const int SERVER_TO_DEVICE = 2;
        //end constants
        private StorageFolder localDataFolder { get; set; } = ApplicationData.Current.LocalFolder;
        private string ownerID { get; set; }
        public string OwnerID { get { return ownerID; } set { ownerID = value; } }
        private static MainViewModel instance { get; set; }
        private ToDoTask currentObject { get; set; }
        public ToDoTask CurrentObject {
            get { return currentObject; }
            set { currentObject = value; OnPropertyChanged("CurrentObject"); }
        }
        private ObservableCollection<ToDoTask> localCollection;
        public ObservableCollection<ToDoTask> LocalCollection {
            get {
                return localCollection;
            }
            set {
                localCollection = value;
                OnPropertyChanged("LocalCollection");
            }
        }
        private ObservableCollection<ToDoTask> remoteCollection { get; set; }
        private MainViewModel() {
            OwnerID = "";
        }
        public async Task readRemoteData() {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(REST_BASE_URL + REST_PATH + "?ownerId=" + OwnerID);
            var items = await result.Content.ReadAsStringAsync();
            remoteCollection = JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(items);
        }
        public async Task readLocalData() {
            StorageFile localDataFile = await localDataFolder.CreateFileAsync(OwnerID + ".dat",
                CreationCollisionOption.OpenIfExists);
            var items = await FileIO.ReadTextAsync(localDataFile);
            ObservableCollection<ToDoTask> collectionFromFile = JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(items);
            if (collectionFromFile == null) {
                LocalCollection = new ObservableCollection<ToDoTask>();
            }
            else
                LocalCollection = collectionFromFile;
        }

        public async Task saveLocalData() {
            StorageFile localDataFile = await localDataFolder.CreateFileAsync(OwnerID + ".dat",
                CreationCollisionOption.ReplaceExisting);
            var items = JsonConvert.SerializeObject(LocalCollection);
            await FileIO.WriteTextAsync(localDataFile, items);
        }
        public static MainViewModel I() {
            if (instance == null) {
                instance = new MainViewModel();
            }
            return instance;
        }
        public async Task addTaskLocal(ToDoTask task) {
            LocalCollection.Add(task);
            task.Id = DateTime.Now.ToBinary();
            OnPropertyChanged("LocalCollection");
            await saveLocalData();
        }
        public async Task updateTaskLocal(ToDoTask task) {
            ToDoTask toUpdate = LocalCollection.Where(X => X.Id == task.Id).FirstOrDefault();
            toUpdate.Title = task.Title;
            toUpdate.Value = task.Value;
            OnPropertyChanged("LocalCollection");
            await saveLocalData();
        }
        public async void deleteTaskLocal(ToDoTask task) {
            removeLocalTaskById(task.Id);
            await saveLocalData();
        }

        private bool removeLocalTaskById(long id) {
            ToDoTask toFind = LocalCollection.Where(X => X.Id == id).FirstOrDefault();
            if (toFind != null) {
                LocalCollection.Remove(toFind);
                return true;
            }
            else return false;
        }

        private async Task deleteRemoteTask(ToDoTask task) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(REST_BASE_URL);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, REST_PATH + task.Id);
            await client.SendAsync(request);
        }
        private async Task updateRemoteTask(ToDoTask task) {
            var dateString = DateTime.Parse(DateTime.Now.ToString());
            task.CreatedAt = dateString.ToString("dd'-'MM'-'yyyy HH:mm:ss");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(REST_BASE_URL);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, REST_PATH + task.Id);
            var serialized = JsonConvert.SerializeObject(task);
            request.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
            await client.SendAsync(request);
        }
        private async Task addRemoteTask(ToDoTask task) {
            task.Id = 0;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(REST_BASE_URL);
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, REST_PATH);
            request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json"); ;
            await client.SendAsync(request);
        }
        public async Task synchronizeWithRest(int syncMode) {
            await readRemoteData();
            switch(syncMode) {
                case DEVICE_TO_SERVER:
                    await synchronizeDeviceToServer();
                    break;
                case ADD_SERVER_CONTENT:
                    await synchronizeAddServerOnly();
                    break;
                case SERVER_TO_DEVICE:
                    await synchronizeServerToDevice();
                    break;
            }

        }

        private int isTaskInLocalOrRemoteOrBoth(long taskId) {
            ToDoTask local = LocalCollection.Where(X => X.Id == taskId).FirstOrDefault();
            ToDoTask remote = remoteCollection.Where(X => X.Id == taskId).FirstOrDefault();
            if (local != null && remote != null) {
                return IN_BOTH;
            }
            else if (local == null) {
                return ONLY_IN_REMOTE;
            }
            else if (remote == null) {
                return ONLY_IN_LOCAL;
            }
            return -999; //this won't happen
        }
        private async Task synchronizeDeviceToServer() {
            //this is only method that updates existing data in both
            //remove tasks that are not in local, update tasks in both
            foreach (ToDoTask current in remoteCollection) {
                int taskIs = isTaskInLocalOrRemoteOrBoth(current.Id);
                if (taskIs == ONLY_IN_REMOTE) {
                    await deleteRemoteTask(current);
                }
                else if (taskIs == IN_BOTH) {
                    await updateRemoteTask(current);
                }
            }
            //add tasks only in local
            foreach (ToDoTask current in LocalCollection) {
                int taskIs = isTaskInLocalOrRemoteOrBoth(current.Id);
                if (taskIs == ONLY_IN_LOCAL) {
                    await addRemoteTask(current);
                }
            }
            //2 re-read collection from rest
            await readRemoteData();
            //3 now save remote to local (will update IDs et cetera)
            LocalCollection = remoteCollection;
            await saveLocalData();
        }
        private async Task synchronizeAddServerOnly() {
            //this method does not update data on server
            foreach (ToDoTask current in remoteCollection) {
                int taskIs = isTaskInLocalOrRemoteOrBoth(current.Id);
                if (taskIs == ONLY_IN_REMOTE) {
                    await addTaskLocal(current);
                }
            }
            await saveLocalData();
            
        }
        private async Task synchronizeServerToDevice() {
            //replaces device content with server content.
           await readRemoteData();
            LocalCollection = remoteCollection;
            await saveLocalData();
        }
    }
}
