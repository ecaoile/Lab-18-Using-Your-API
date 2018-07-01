using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab18WebApp.Models
{
    public class TodoItemsViewModel
    {
        public List<TodoItem> TodoItems;
        public TodoList TodoList { get; set; }
    }
}
