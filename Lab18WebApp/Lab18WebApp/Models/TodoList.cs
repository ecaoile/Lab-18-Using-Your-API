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

        [Required]
        public string Name { get; set; }

        public List<TodoItem> TodoItems { get; set; }
    }
}
