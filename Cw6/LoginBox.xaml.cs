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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ToDoTaskList {
    public sealed partial class LoginBox : UserControl {
        public LoginBox() {
            this.InitializeComponent();
            
            DataContext = MainViewModel.I();
        }

       public void clearLogin() {
            LoginTextBox.Text = "";
            (DataContext as MainViewModel).OwnerID = "";
        }
        private void UserControl_LostFocus(object sender, RoutedEventArgs e) {
            if((DataContext as MainViewModel).OwnerID == "") {
                SolidColorBrush invalidColor = new SolidColorBrush();
                invalidColor.Color = Windows.UI.Color.FromArgb(255, 255, 0, 0);
                LoginTextBox.BorderBrush = invalidColor;
            } else {
                SolidColorBrush validColor = new SolidColorBrush();
                validColor.Color = Windows.UI.Color.FromArgb(255, 0, 255, 0);
                LoginTextBox.BorderBrush = validColor;
            }
        }

        private async void DoLogin_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if ((DataContext as MainViewModel).OwnerID != "") {
                App.RootFrame.Navigate(typeof(MainPage));
            }
            else {
                MessageDialog error = new MessageDialog(loader.GetString("EmptyLogin"));
                await error.ShowAsync();
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e) {
            if ((DataContext as MainViewModel).OwnerID == "") {
                SolidColorBrush invalidColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
                LoginTextBox.BorderBrush = invalidColor;
            }
            else {
                SolidColorBrush validColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
                
                LoginTextBox.BorderBrush = validColor;
            }
        }
    }
    
}
