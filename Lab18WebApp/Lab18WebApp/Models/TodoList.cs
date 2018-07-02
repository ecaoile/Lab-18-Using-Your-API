using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab18WebApp.Models
{
    public class TodoList
    {
        public int ID { get; set; }

        [Required,Display(Name="List Name")]
        public string Name { get; set; }

        [Display(Name = "To-Do Items")]
        public List<TodoItem> TodoItems { get; set; }
    }
}
