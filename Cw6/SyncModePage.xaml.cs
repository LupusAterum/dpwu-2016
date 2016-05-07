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
    public sealed partial class SyncModePage : Page {
        public SyncModePage() {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
        }
        private MainViewModel getViewModel() {
            return DataContext as MainViewModel;
        }
        private async void syncS2D_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                await getViewModel().synchronizeWithRest(MainViewModel.SERVER_TO_DEVICE);
            } catch {

                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            Frame.GoBack();
        }

        private async void syncASC_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                await getViewModel().synchronizeWithRest(MainViewModel.ADD_SERVER_CONTENT);
                
            }
            catch {

                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            Frame.GoBack();
        }

        private async void syncD2S_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try {
                await getViewModel().synchronizeWithRest(MainViewModel.DEVICE_TO_SERVER);
            }
            catch {

                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
            Frame.GoBack();
        }
    }
}
