// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1309 // Field names should not begin with underscore

namespace ToDoListApp.MVC.Controllers
{
#pragma warning disable SA1200 // Using directives should be placed correctly
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using ToDoListApp.Models.Input;
    using ToDoListApp.Models.Output;
    using ToDoListApp.MVC.Models.Input;
    using ToDoListApp.MVC.Models.Output;
    using ToDoListApp.MVC.Services;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            this._logger = logger;
            this._apiService = apiService;
        }

        // add to do
        public IActionResult AddToDo()
        {
            return this.View(new AddToDoInput());
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(AddToDoInput model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this._apiService.AddToDoAsync(model);
            if (result.Success)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, result.Message);
                return this.View(model);
            }
        }

        // update to do
        public async Task<IActionResult> EditToDo(int id)
        {
            this._logger.LogInformation($"EditToDo Start Calling GetToDoListAsync id = {id}");
            var toDoItem = await this._apiService.GetToDoListAsync(new GetToDoInput { Id = id });

            var lastitem = toDoItem.FirstOrDefault();

            if (lastitem == null)
            {
                return this.NotFound();
            }

            return this.View("EditToDo", lastitem);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToDo(GetToDoOutput model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("EditToDo", model);
            }

            var result = await this._apiService.UpdateToDoAsync(model.Id, model);
            if (result.Success)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, result.Message);
                return this.View("EditToDo", model);
            }
        }

        // delete to do
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await this._apiService.DeleteToDoAsync(id);
            if (success)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View("Error");
            }
        }

        // get to do
        public async Task<IActionResult> IndexAsync()
        {
            var model = new GetToDoInput();
            var toDoItems = await this._apiService.GetToDoListAsync(model);
            return this.View(toDoItems);
        }

        // Sign in
        public IActionResult SignIn()
        {
            return this.View(new SignInInput());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this._apiService.SignInAsync(model);
            if (result.Success)
            {
                return this.RedirectToAction("Index", "Home");
            }

            this.ModelState.AddModelError(string.Empty, result.Message);
            return this.View(model);
        }

        // sign up
        public IActionResult SignUp()
        {
            return this.View(new CreateUserInput());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserInput user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(user);
            }

            var result = await this._apiService.SignUpAsync(user);
            if (result.Success)
            {
                return this.RedirectToAction("SignIn");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, result.Message);
                return this.View(user);
            }
        }

        // restore pass
        public IActionResult RestorePassword()
        {
            return this.View(new RestorePasswordInput());
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(RestorePasswordInput user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(user);
            }

            var result = await this._apiService.RestorePasswordAsync(user);
            if (result.Success)
            {
                return this.RedirectToAction("RestorePasswordConfirmation");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, result.Message);
                return this.View(user);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}