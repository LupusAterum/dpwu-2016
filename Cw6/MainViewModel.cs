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

        private StorageFolder localDataFolder { get; set; } = ApplicationData.Current.LocalFolder;
        private string ownerID { get; set; }
        public string OwnerID { get { return ownerID; } set { ownerID = value; } }
        private static MainViewModel instance { get; set; }
        private ToDoTask currentObject { get; set; }
        public ToDoTask CurrentObject { get { return currentObject; }
        set { currentObject = value; OnPropertyChanged("CurrentObject"); }
        }
        private ObservableCollection<ToDoTask> itemsCollection;
        public ObservableCollection<ToDoTask> ItemsCollection {
            get {
                return itemsCollection;
            }
            set {
                itemsCollection = value;
                OnPropertyChanged("ItemsCollection");
            }
        }
        private MainViewModel() {
            //readData();
        }
        public async void readRemoteData() {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks?ownerId=" + OwnerID);
            var items = await result.Content.ReadAsStringAsync();
            ItemsCollection = JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(items);
        }
        public async void readLocalData() {
            StorageFile localDataFile = await localDataFolder.CreateFileAsync(OwnerID + ".dat", CreationCollisionOption.OpenIfExists);
            var items = await FileIO.ReadTextAsync(localDataFile);
            ItemsCollection = JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(items);
            if(ItemsCollection == null) {
                ItemsCollection = new ObservableCollection<ToDoTask>();
            }
        }
           
        public async void saveLocalData() {
            StorageFile localDataFile = await localDataFolder.CreateFileAsync(OwnerID + ".dat", CreationCollisionOption.ReplaceExisting);
            var items = JsonConvert.SerializeObject(ItemsCollection);
            await FileIO.WriteTextAsync(localDataFile, items);
        }
        public static MainViewModel I() {
            if(instance == null) {
                instance = new MainViewModel();
            }
            return instance;
        }
        public void addTask(ToDoTask task) {
            ItemsCollection.Add(task);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://windowsphoneuam.azurewebsites.net/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/ToDoTasks");
            request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json"); ;
            client.SendAsync(request);
            saveLocalData();
            readLocalData();
        }
        public void updateTask(ToDoTask task) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://windowsphoneuam.azurewebsites.net/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/ToDoTasks/" + task.Id);
            request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json"); ;
            client.SendAsync(request);
            saveLocalData();
            readLocalData();
        }
        public void deleteTask(ToDoTask task) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://windowsphoneuam.azurewebsites.net/");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "api/ToDoTasks/" + task.Id);
            client.SendAsync(request);
            saveLocalData();
            readLocalData();
        }
    }
}
