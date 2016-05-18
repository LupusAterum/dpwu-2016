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

/*
// Author: Karol Mazurek <karmaz@st.amu.edu.pl>
*/


namespace ToDoTaskList {

    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            DataContext = MainViewModel.I();
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
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            LoginUserControl.clearLogin();
        }
        private void LoginField_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void LoginField_KeyDown(object sender, KeyRoutedEventArgs e) {
           // if (e.Key == Windows.System.VirtualKey.Enter) //LoginButtton_Click(this, e);
        }

        private void LoginButtton_Click(object sender, RoutedEventArgs e) {

        }

        private async void ExitButton_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            MessageDialog confirmExitDialog = new MessageDialog(loader.GetString("ExitConfirmation"));
            confirmExitDialog.Commands.Clear();
            confirmExitDialog.Commands.Add(new UICommand { Label = loader.GetString("YesBtn"), Id = 0 });
            confirmExitDialog.Commands.Add(new UICommand { Label = loader.GetString("NoBtn"), Id = 1 });
            var res = await confirmExitDialog.ShowAsync();
            if ((int)res.Id == 0) {
                Application.Current.Exit();
            }
            else {
                return;
            }
        }
    }
}
