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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
        }

        private async void LoginButtton_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (LoginField.Text != "") {
                (DataContext as MainViewModel).OwnerID = LoginField.Text;
                Frame.Navigate(typeof(MainPage));
            }
            else {
                MessageDialog error = new MessageDialog(loader.GetString("EmptyLogin"));
                await error.ShowAsync();
            }
        }

        private async void AboutButton_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var authorString = loader.GetString("AboutAuthor");
            var autorEmailString = loader.GetString("AboutEmail");
            var copyright = loader.GetString("Copyright");
            MessageDialog about = new MessageDialog(authorString + "\n" + autorEmailString + "\n" + copyright);
            about.Commands.Clear();
            about.Commands.Add(new UICommand("OK"));
            await about.ShowAsync();

        }

        private void LoginField_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void LoginField_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) LoginButtton_Click(this, e);
        }
    }
}
