// <copyright file="AddToDoInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented

namespace ToDoListApp.Models.Input
{
    public class AddToDoInput
    {
        public string? Text { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ReminderDate { get; set; }

        public string? RepeatType { get; set; }

        public bool Important { get; set; }

        public string? Note { get; set; }
    }
}