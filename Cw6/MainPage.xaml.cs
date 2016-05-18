using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

    public sealed partial class MainPage : Page
    {
        public MainPage() {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
            
        }
        private async void readLocalStorage() {
            startProgress();
            await getViewModel().readLocalData();
            stopProgress();
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

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(SyncModePage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e) {

        }
        private void startProgress() {
            bar.IsIndeterminate = true;
            
        }
        private void stopProgress() {
            bar.IsIndeterminate = false;
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            readLocalStorage();
        }
    }
}
