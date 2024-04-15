// <copyright file="ToDoListController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Database;
using ToDoListApp.Managers;
using ToDoListApp.Models.Input;
using ToDoListApp.Models.Output;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1309 // Field names should not begin with underscore

namespace ToDoListApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoListManager _toDoListManager;

        public ToDoListController(ILogger<ToDoListController> logger, ToDoListManager toDoListManager)
        {
            this._toDoListManager = toDoListManager;
        }

        // Update to do by id
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateToDo(int id, [FromBody] GetToDoOutput model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this._toDoListManager.UpdateTodoItemAsync(id, model);
            if (result.StatusCode == 204 && result.Success)
            {
                return this.NoContent();
            }

            if (result.StatusCode == 400)
            {
                return this.BadRequest(result.Message);
            }

            return this.StatusCode(500, result.Message);
        }

        // delete to do by id
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var result = await this._toDoListManager.DeleteToDoAsync(id);
            if (result.StatusCode == 204 && result.Success)
            {
                return this.Ok(result.Message);
            }

            if (result.StatusCode == 400)
            {
                return this.BadRequest(result.Message);
            }

            return this.StatusCode(500, result.Message);
        }

        // view all todos (orderby date)
        [HttpPost("get-list")]
        public async Task<IActionResult> GetToDoList([FromBody] GetToDoInput model)
        {
            var result = await this._toDoListManager.GetToDoListAsync(model);
            return this.Ok(result.Output);
        }

        // add to do
        [HttpPost("add-todo")]
        public async Task<IActionResult> AddToDo([FromBody] AddToDoInput model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this._toDoListManager.AddTodoItemAsync(model);
            return this.Ok(result);
        }

        // restore password
        [HttpPost("restore-password")]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordInput user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this._toDoListManager.RestorePasswordAsync(user);
            if (result.StatusCode == 200 && result.Success)
            {
                return this.Ok(result.Message);
            }

            if (result.StatusCode == 400)
            {
                return this.BadRequest(result.Message);
            }

            return this.StatusCode(500, result.Message);
        }

        // sign in
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInInput request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this._toDoListManager.SignInAsync(request.Username, request.Password);
            if (result.StatusCode == 204 && result.Success)
            {
                return this.Ok(result.Message);
            }

            if (result.StatusCode == 400)
            {
                return this.BadRequest(result.Message);
            }

            return this.StatusCode(500, result.Message);
        }

        // sign up
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserInput user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this._toDoListManager.SignUpAsync(user);
            if (result.StatusCode == 204 && result.Success)
            {
                return this.Ok(result.Message);
            }

            if (result.StatusCode == 400)
            {
                return this.BadRequest(result.Message);
            }

            return this.StatusCode(500, result.Message);
        }

        // sign out (just a form of sign out)
        [HttpPost("signout")]
        public IActionResult SignOutForm()
        {
            return this.Ok("Signed out successfully");
        }
    }
}