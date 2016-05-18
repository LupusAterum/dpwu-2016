using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
// Author: Karol Mazurek <karmaz@st.amu.edu.pl>
*/

namespace ToDoTaskList {
    public class ToDoTask {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string OwnerId { get; set; }
        public string CreatedAt { get; set; }
        
        public ToDoTask(string title, string value) {
            var now = DateTime.Now;
            Id = 0;
            Id -= now.ToBinary(); //negative id to indicate that it's local
            var toDoTask = this;
            toDoTask.OwnerId = MainViewModel.I().OwnerID;
            toDoTask.Title = title;
            toDoTask.Value = value;
            var dateString = DateTime.Parse(now.ToString());
            toDoTask.CreatedAt = dateString.ToString("dd'-'MM'-'yyyy HH:mm:ss");
        }
        public ToDoTask() {
            var now = DateTime.Now;
            Id = 0;
            Id -= now.ToBinary(); //negative id to indicate that it's local
            OwnerId = MainViewModel.I().OwnerID;
            Title = "New Title";
            Value = "New Description";
            var dateString = DateTime.Parse(now.ToString());
            CreatedAt = dateString.ToString("dd'-'MM'-'yyyy HH:mm:ss");
            
        }
        public string Serialize() {
            string serialized = JsonConvert.SerializeObject(this);
            return serialized;
        }
    }
}
