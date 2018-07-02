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
        /// <summary>
        /// GET: TodoItem/
        /// </summary>
        /// <param name="searchString">string to search for specific todo items by name</param>
        /// <returns></returns>
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

        /// <summary>
        /// GET: TodoItem/Details/id#
        /// </summary>
        /// <param name="id">the ID# of the item to get details</param>
        /// <returns>view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                
                try
                {
                    // add the appropriate properties on top of the client base address.
                    client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                    //the .Result is important for us to extract the result of the response from the call
                    var itemResponse = client.GetAsync($"/api/todo/{id}").Result;
                    var listsResponse = client.GetAsync($"/api/todolist/").Result;

                    if (itemResponse.EnsureSuccessStatusCode().IsSuccessStatusCode
                        && listsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        var itemStringResult = await itemResponse.Content.ReadAsStringAsync();
                        var listsStringResult = await listsResponse.Content.ReadAsStringAsync();

                        TodoItem datTodoItem = JsonConvert.DeserializeObject<TodoItem>(itemStringResult);
                        List<TodoList> demTodoLists = JsonConvert.DeserializeObject<List<TodoList>>(listsStringResult);

                        var matchedList = demTodoLists.Where(l => l.ID == datTodoItem.DatListID);
                        ViewData["TodoList"] = matchedList;
                        return View(datTodoItem);
                    }
                }
                catch
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: TodoItem/Create
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> Create()
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var response = client.GetAsync($"/api/todolist/").Result;
                if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    List<TodoList> demTodoLists = JsonConvert.DeserializeObject<List<TodoList>>(stringResult);

                    ViewData["TodoList"] = demTodoLists;
                }
            }
            return View();
        }

        /// <summary>
        /// POST: TodoItem/Create
        /// </summary>
        /// <param name="item">TodoItem object to add</param>
        /// <returns>view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,IsComplete,DatListID")] TodoItem item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        // add the appropriate properties on top of the client base address.
                        client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                        //the .Result is important for us to extract the result of the response from the call
                        var response = await client.PostAsJsonAsync($"/api/todo/", item);
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        /// <summary>
        /// GET: TodoItem/Edit/id#
        /// </summary>
        /// <param name="id">the ID # to edit</param>
        /// <returns>view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var itemResponse = client.GetAsync($"/api/todo/{id}").Result;
                var listsResponse = client.GetAsync($"/api/todolist/").Result;

                if (itemResponse.EnsureSuccessStatusCode().IsSuccessStatusCode 
                    && listsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var itemStringResult = await itemResponse.Content.ReadAsStringAsync();
                    var listsStringResult = await listsResponse.Content.ReadAsStringAsync();

                    TodoItem datTodoItem = JsonConvert.DeserializeObject<TodoItem>(itemStringResult);
                    List<TodoList> demTodoLists = JsonConvert.DeserializeObject<List<TodoList>>(listsStringResult);

                    ViewData["TodoList"] = demTodoLists;
                    return View(datTodoItem);
                }
                return View();
            }
        }

        /// <summary>
        /// POST: Edit/TodoItem/id#
        /// </summary>
        /// <param name="id">the ID # of the item to edit</param>
        /// <param name="item">the item with changes</param>
        /// <returns>view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,IsComplete,DatListID")] TodoItem item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        // add the appropriate properties on top of the client base address.
                        client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                        //the .Result is important for us to extract the result of the response from the call
                        var response = await client.PutAsJsonAsync($"/api/todo/{id}", item);
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        /// <summary>
        /// GET: Delete/TodoItem/id#
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {

                try
                {
                    // add the appropriate properties on top of the client base address.
                    client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                    //the .Result is important for us to extract the result of the response from the call
                    var response = client.GetAsync($"/api/todo/{id}").Result;
                    if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        var stringResult = await response.Content.ReadAsStringAsync();
                        TodoItem datTodoItem = JsonConvert.DeserializeObject<TodoItem>(stringResult);

                        return View(datTodoItem);
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var response = await client.DeleteAsync($"/api/todo/{id}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}