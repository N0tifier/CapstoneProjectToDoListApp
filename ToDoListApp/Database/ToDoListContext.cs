// <copyright file="ToDoListContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models.DatabaseEntities;

#pragma warning disable SA1600 // Elements should be documented

namespace ToDoListApp.Database
{
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
