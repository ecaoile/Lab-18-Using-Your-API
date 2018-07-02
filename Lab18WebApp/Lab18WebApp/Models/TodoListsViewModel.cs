using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab18WebApp.Models
{
    public class TodoListsViewModel
    {
        public List<TodoList> TodoLists { get; set; }

        [Display(Name="To-Do Items")]
        public IEnumerable<TodoItem> TodoItems { get; set; }
    }
}
