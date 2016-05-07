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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoTaskList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage() {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
            readLocalStorage();
        }
        private async void readLocalStorage() {
            await getViewModel().readLocalData();
        }
        private MainViewModel getViewModel() {
            return DataContext as MainViewModel;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(AddTask));
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ListBox1.SelectedItem == null) {
                return;
            }
            else {
                getViewModel().CurrentObject = (ToDoTask) ListBox1.SelectedItem;
                Frame.Navigate(typeof(EditTask));
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e) {
            try {
                await getViewModel().synchronizeWithRest();
            } catch {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                MessageDialog errorPopup = new MessageDialog(loader.GetString("SynchronizationErrorMessage"));
                await errorPopup.ShowAsync();
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e) {

        }
    }
}
