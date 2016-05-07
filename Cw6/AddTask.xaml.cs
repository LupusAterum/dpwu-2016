using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AddTask : Page {
        public AddTask() {
            this.InitializeComponent();
            DataContext = MainViewModel.I() as MainViewModel;
        }
        private MainViewModel getViewModel() {
            return DataContext as MainViewModel;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e) {
            string title = taskTitle.Text;
            string desc = taskDesc.Text;
            ToDoTask task = new ToDoTask(title, desc);
            await getViewModel().addTaskLocal(task);
            Frame.GoBack();
        }
    }
}
