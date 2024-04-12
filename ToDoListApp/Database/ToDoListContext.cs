// <copyright file="ToDoListContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1200 // Using directives should be placed correctly

namespace ToDoListApp.Database
{
    using Microsoft.EntityFrameworkCore;
    using ToDoListApp.Models.DatabaseEntities;

    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoListEntity> ToDoList { get; set; }

        public DbSet<UserEntity> Users { get; set; }
    }
}
