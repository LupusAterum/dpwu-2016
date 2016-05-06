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
            ActualLogin = "";
            DataContext = MainViewModel.I();
        }

        
        public string ActualLogin {
            get { return (string)GetValue(ActualLoginProperty); }
            set { SetValue(ActualLoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualLoginProperty =
            DependencyProperty.Register("ActualLogin", typeof(string), typeof(LoginBox), null);

        private void UserControl_LostFocus(object sender, RoutedEventArgs e) {
            if(ActualLogin != "") {
                SolidColorBrush invalidColor = new SolidColorBrush();
                invalidColor.Color = Windows.UI.Color.FromArgb(0, 255, 0, 0);
                LoginBox.BorderBrush = invalidColor;
            } else {
                SolidColorBrush validColor = new SolidColorBrush();
                validColor.Color = Windows.UI.Color.FromArgb(0, 0, 255, 0);
                LoginBox.BorderBrush = validColor;
            }
        }

        private async void DoLogin_Click(object sender, RoutedEventArgs e) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (ActualLogin != "") {
                (DataContext as MainViewModel).OwnerID = ActualLogin;
                App.RootFrame.Navigate(typeof(MainPage));
            }
            else {
                MessageDialog error = new MessageDialog(loader.GetString("EmptyLogin"));
                await error.ShowAsync();
            }
        }
    }
    
}
