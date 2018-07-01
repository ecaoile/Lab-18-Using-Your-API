using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lab18WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Lab18WebApp.Controllers
{
    public class TodoItemController : Controller
    {
        public async Task<IActionResult> Index(string searchString)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var response = client.GetAsync("/api/todo/").Result;
                if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    List<TodoItem> demTodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(stringResult);
                    var todoItems = from t in demTodoItems
                                        select t;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        todoItems = todoItems.Where(s => s.Name.Contains(searchString));
                    }

                    var todoItemsVM = new TodoItemsViewModel();
                    todoItemsVM.TodoItems = todoItems.ToList();

                    return View(todoItemsVM);
                }
                return View();
            }

        }
    }
}