using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab18WebApp.Models
{
    public class TodoItem
    {
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, Display(Name = "Complete?")]
        public bool IsComplete { get; set; }

        [Required,Display(Name = "To-do List ID")]
        public int DatListID { get; set; }

        public TodoList TodoList { get; set; }
    }
}
