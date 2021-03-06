﻿using Lab18WebApp.Models;
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
                    var listResponse = client.GetAsync($"/api/todolist/{id}").Result;
                    var itemsResponse = client.GetAsync("/api/todo/").Result;

                    if (listResponse.EnsureSuccessStatusCode().IsSuccessStatusCode 
                        && itemsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        var listStringResult = await listResponse.Content.ReadAsStringAsync();
                        var itemsStringResult = await itemsResponse.Content.ReadAsStringAsync();

                        TodoList datTodoList = JsonConvert.DeserializeObject<TodoList>(listStringResult);
                        List<TodoItem> demTodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(itemsStringResult);

                        List<TodoItem> matchedItems = demTodoItems.Where(i => i.DatListID == id).ToList();

                        datTodoList.TodoItems = matchedItems;
                        return View(datTodoList);
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
        /// GET: TodoList/Create
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: TodoList/Create
        /// </summary>
        /// <param name="list">the TodoList object to add</param>
        /// <returns>view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] TodoList list)
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
                        var response = await client.PostAsJsonAsync($"/api/todolist/", list);
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }

        /// <summary>
        /// GET: TodoList/edit/id#
        /// </summary>
        /// <param name="id">ID # of the TodoList object to edit</param>
        /// <returns>view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://todoapilab17.azurewebsites.net/");

                //the .Result is important for us to extract the result of the response from the call
                var response = client.GetAsync($"/api/todolist/{id}").Result;
                if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    TodoList datTodoList = JsonConvert.DeserializeObject<TodoList>(stringResult);

                    return View(datTodoList);
                }
                return View();
            }
        }

        /// <summary>
        /// POST: TodoList/Edit/id#
        /// </summary>
        /// <param name="id">ID # of the TodoList object to edit</param>
        /// <param name="list">a new TodoList object with the edited properties</param>
        /// <returns>view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] TodoList list)
        {
            if (id != list.ID)
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
                        var response = await client.PutAsJsonAsync($"/api/todolist/{id}", list);
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }

        /// <summary>
        /// GET: TodoList/Delete/id#
        /// </summary>
        /// <param name="id">ID # of the TodoList object to delete</param>
        /// <returns>view</returns>
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
                    var listResponse = client.GetAsync($"/api/todolist/{id}").Result;
                    var itemsResponse = client.GetAsync("/api/todo/").Result;

                    if (listResponse.EnsureSuccessStatusCode().IsSuccessStatusCode
                        && itemsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        var listStringResult = await listResponse.Content.ReadAsStringAsync();
                        var itemsStringResult = await itemsResponse.Content.ReadAsStringAsync();

                        TodoList datTodoList = JsonConvert.DeserializeObject<TodoList>(listStringResult);
                        List<TodoItem> demTodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(itemsStringResult);

                        List<TodoItem> matchedItems = demTodoItems.Where(i => i.DatListID == id).ToList();

                        datTodoList.TodoItems = matchedItems;
                        return View(datTodoList);
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
                var itemsResponse = client.GetAsync("/api/todo/").Result;
                if (itemsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringItemsResult = await itemsResponse.Content.ReadAsStringAsync();

                    List<TodoItem> demTodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(stringItemsResult);

                    var doomedTodos = from i in demTodoItems
                                      where i.DatListID == id
                                      select i;

                    foreach (var item in doomedTodos)
                    {
                        await client.DeleteAsync($"/api/todo/{item.ID}");
                    }

                    var listResponse = await client.DeleteAsync($"/api/todolist/{id}");
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
