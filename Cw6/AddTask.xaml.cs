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

/*
// Author: Karol Mazurek <karmaz@st.amu.edu.pl>
*/


namespace ToDoTaskList {

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
