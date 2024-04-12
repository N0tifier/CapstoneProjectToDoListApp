// <copyright file="ToDoListManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1309 // Field names should not begin with underscore

namespace ToDoListApp.Managers
{
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using ToDoListApp.Database;
    using ToDoListApp.Models.DatabaseEntities;
    using ToDoListApp.Models.Input;
    using ToDoListApp.Models.Output;

    public class ToDoListManager
    {
        private readonly TodoListContext _context;
        private readonly ILogger<ToDoListManager> _logger;

        public ToDoListManager(TodoListContext context, ILogger<ToDoListManager> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        // update to do
        public async Task<GenericResponse> UpdateTodoItemAsync(int id, GetToDoOutput model)
        {
            try
            {
                this._logger.LogInformation($"UpdateTodoItemAsync: Started looking for todoItem (id = {id})");
                var todoItem = await this._context.ToDoList.FirstOrDefaultAsync(todo => todo.Id == id);
                if (todoItem == null)
                {
                    this._logger.LogError($"UpdateTodoItemAsync: todoItem could not be found (id = {id})");
                    return new GenericResponse
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Not Found",
                    };
                }

                todoItem.Text = model.Text;
                todoItem.DueDate = model.DueDate;
                todoItem.ReminderDate = model.ReminderDate;
                todoItem.RepeatType = model.RepeatType;
                todoItem.Important = model.Important;
                todoItem.Note = model.Note;

                await this._context.SaveChangesAsync();
                return new GenericResponse
                {
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (Exception ex)
            {
                this._logger.LogError($"UpdateTodoItemAsync: Something went wrong (id = {id}), exception = {ex}");
                return new GenericResponse
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Something Went Wrong",
                };
            }
        }

        // delete to do
        public async Task<GenericResponse> DeleteToDoAsync(int id)
        {
            try
            {
                var result = await this._context.ToDoList.FirstOrDefaultAsync(todo => todo.Id == id);
                if (result == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Not Found",
                    };
                }

                this._context.ToDoList.Remove(result);
                await this._context.SaveChangesAsync();
                return new GenericResponse
                {
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Something Went Wrong",
                };
            }
        }

        // get to do list (orderby date)
        public async Task<GetToDoResponse> GetToDoListAsync(GetToDoInput model)
        {
            try
            {
                var list = await this._context.ToDoList.OrderBy(todo => todo.DueDate).ToListAsync();

                if (model.Id != null)
                {
                    list.RemoveAll(todo => todo.Id != model.Id);
                }

                if (model.ImportantOnly)
                {
                    list.RemoveAll(todo => !todo.Important);
                }

                if (model.TodayOnly)
                {
                    list.RemoveAll(todo => todo.DueDate != DateTime.Today);
                }

                var result = list.Select(todo => new GetToDoOutput
                {
                    DueDate = todo.DueDate,
                    Important = todo.Important,
                    Note = todo.Note,
                    ReminderDate = todo.ReminderDate,
                    RepeatType = todo.RepeatType,
                    Text = todo.Text,
                    Id = todo.Id,
                }).ToList();

                return new GetToDoResponse
                {
                    Success = true,
                    StatusCode = 200,
                    Output = result,
                };
            }
            catch (Exception ex)
            {
                return new GetToDoResponse
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Something went wrong",
                };
            }
        }

        /// <summary>
        /// create task.
        /// </summary>
        /// <param name="model">.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ToDoListEntity> AddTodoItemAsync(AddToDoInput model)
        {
            var dueDate = model.DueDate;

            if (dueDate != null)
            {
                dueDate = ((DateTime)dueDate).Date;
            }

            var todoItem = new ToDoListEntity
            {
                Text = model.Text,
                DueDate = dueDate,
                ReminderDate = model.ReminderDate,
                RepeatType = model.RepeatType,
                Important = model.Important,
                Note = model.Note,
            };
            this._context.ToDoList.Add(todoItem);
            await this._context.SaveChangesAsync();
            return todoItem;
        }

        // Restore Password
        public async Task<GenericResponse> RestorePasswordAsync(RestorePasswordInput user)
        {
            try
            {
                var existUser = await this._context.Users
                               .FirstOrDefaultAsync(usermodel => usermodel.Username == user.Username && usermodel.Email == user.Email);

                if (existUser == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "username or email does not match",
                        StatusCode = 400,
                    };
                }

                return new GenericResponse
                {
                    Success = true,
                    Message = "New Password has been sent",
                    StatusCode = 200,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    StatusCode = 500,
                };
            }
        }

        // Sign in
        public async Task<GenericResponse> SignInAsync(string username, string password)
        {
            try
            {
                var user = await this._context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Invalid Username or Password",
                        StatusCode = 400,
                    };
                }

                if (!this.DecryptPassword(password, user.Password))
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Invalid Username or Password",
                        StatusCode = 400,
                    };
                }

                return new GenericResponse
                {
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    StatusCode = 500,
                };
            }
        }

        // sign up
        public async Task<GenericResponse> SignUpAsync(CreateUserInput user)
        {
            try
            {
                var existUser = await this._context.Users
                               .FirstOrDefaultAsync(usermodel => usermodel.Username == user.Username || usermodel.Email == user.Email);

                if (existUser != null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "username or email already exists",
                        StatusCode = 400,
                    };
                }

                var hashedPassword = this.EncryptPassword(user.Password);

                var newUser = new UserEntity
                {
                    Username = user.Username,
                    Password = hashedPassword,
                    Email = user.Email,
                };
                this._context.Users.Add(newUser);
                await this._context.SaveChangesAsync();
                return new GenericResponse
                {
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    StatusCode = 500,
                };
            }
        }

        // password decryption
        private bool DecryptPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = this.EncryptPassword(password);

            return hashedInputPassword == hashedPassword;
        }

        // password encryption
        private string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}