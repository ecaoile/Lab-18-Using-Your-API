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
        //public static async Task<TodoListsViewModel> 
        //    FromIDAsync(List<TodoList> demTodoLists, List<TodoItem> demTodoItems, string searchString)
        //{

        //    var todoLists = from l in demTodoLists
        //                    select l;

        //    var todoItems = from i in demTodoItems
        //                    select i;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        todoLists = todoLists.Where(s => s.Name.Contains(searchString));
        //    }

        //    var todoListsVM = new TodoListsViewModel();
        //    todoListsVM.TodoLists = todoLists.ToList();
        //    todoListsVM.TodoItems = await demTodoItems.Where(i => i.DatListID == TodoLists.TodoList.ID)
        //        .Select(i => i).ToListAsync();
        //}
    }
}
