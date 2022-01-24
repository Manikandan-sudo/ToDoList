using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListContext _toDoListContext;

        public ToDoListController(ToDoListContext toDoListContext)
        {
            _toDoListContext = toDoListContext;
        }
        //GET
        public async Task<IActionResult> Index()
        {
            IQueryable<TodoList> items = from i in _toDoListContext.ToDoList orderby i.Id select i;
            List<TodoList> todoLists = await items.ToListAsync();
            return View(todoLists);
        }

        // GET toDO/create

        public IActionResult Create() => View();

        //POST toDo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if(ModelState.IsValid)
            {
                _toDoListContext.Add(item);
                await _toDoListContext.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET /todo/edit/5

        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await _toDoListContext.ToDoList.FindAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        //POST /todo/edit/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                _toDoListContext.Update(item);
                await _toDoListContext.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await _toDoListContext.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item does not exist";
            }
            else
            {
                _toDoListContext.ToDoList.Remove(item);
                await _toDoListContext.SaveChangesAsync();
                TempData["Success"] = "The item has been deleted";
            }
            return RedirectToAction("Index");
        }
            
    }
}
