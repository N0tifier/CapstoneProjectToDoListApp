// <copyright file="GetToDoInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented

namespace ToDoListApp.Models.Input
{
    public class GetToDoInput
    {
        public int? Id { get; set; }

        public bool ImportantOnly { get; set; }

        public bool TodayOnly { get; set; }
    }
}