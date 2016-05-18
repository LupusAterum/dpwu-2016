using System.ComponentModel;
/*
// Author: Karol Mazurek <karmaz@st.amu.edu.pl>
*/

namespace ToDoTaskList {
    public class ViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
