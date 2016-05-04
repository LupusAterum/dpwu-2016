using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoTaskList {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditTask : Page {

        public EditTask() {
            this.InitializeComponent();
            DataContext = MainViewModel.I();

        }
        private MainViewModel getViewModel() {
            return DataContext as MainViewModel;
        }
        private async void deleteEntry(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            MessageDialog confirm = new MessageDialog(loader.GetString("DeleteConfirmationText"));
            confirm.Commands.Clear();
            confirm.Commands.Add(new UICommand { Label = loader.GetString("YesBtn"), Id = 0 });
            confirm.Commands.Add(new UICommand { Label = loader.GetString("NoBtn"), Id = 1 });
            var res = await confirm.ShowAsync();
            if ((int) res.Id == 0) {
                getViewModel().ItemsCollection.Remove(getViewModel().CurrentObject);
                getViewModel().deleteTask(getViewModel().CurrentObject);
                getViewModel().CurrentObject = null;
                Frame.GoBack();
            } else {
                return;
            }
        }

        private async void confirmEdition(object sender, RoutedEventArgs e) {

            await getViewModel().updateTask(getViewModel().CurrentObject);
            Frame.GoBack();


        }
    }
}
