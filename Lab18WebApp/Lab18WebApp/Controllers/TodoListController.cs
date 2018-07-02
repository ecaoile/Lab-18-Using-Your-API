using Lab18WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lab18WebApp.Controllers
{
    public class TodoListController : Controller
    {
        /// <summary>
        /// GET: TodoList/
        /// </summary>
        /// <param name="searchString">string to search for specific todo list by name</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string searchString)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var listsResponse = client.GetAsync("/api/todolist/").Result;
                var itemsResponse = client.GetAsync("/api/todo/").Result;
                if (listsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode 
                    && listsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringListResult = await listsResponse.Content.ReadAsStringAsync();
                    var stringItemsResult = await itemsResponse.Content.ReadAsStringAsync();

                    List<TodoList> demTodoLists = JsonConvert.DeserializeObject<List<TodoList>>(stringListResult);
                    List<TodoItem> demTodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(stringItemsResult);

                    var todoLists = from l in demTodoLists
                                    select l;

                    var todoItems = from i in demTodoItems
                                    select i;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        todoLists = todoLists.Where(s => s.Name.Contains(searchString));
                    }

                    var todoListsVM = new TodoListsViewModel();
                    todoListsVM.TodoLists = todoLists.ToList();
                    todoListsVM.TodoItems = todoItems.ToList();
                    return View(todoListsVM);
                }
                return View();
            }
        }


    }
}
