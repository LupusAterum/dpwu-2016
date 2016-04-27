using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTaskList {
    public class ToDoTask {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string OwnerId { get; set; }
        public string CreatedAt { get; set; }
        public ToDoTask(string title, string value) {
            this.OwnerId = MainViewModel.I().OwnerID;
            this.Title = title;
            this.Value = value;
        }
        public ToDoTask() {
            this.OwnerId = MainViewModel.I().OwnerID;
            this.Title = "New Title";
            this.Value = "New Description";
        }
        public string Serialize() {
            string serialized = JsonConvert.SerializeObject(this);
            return serialized;
        }
    }
}
