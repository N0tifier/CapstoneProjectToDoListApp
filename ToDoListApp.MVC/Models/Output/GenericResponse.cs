// <copyright file="GenericResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented

namespace ToDoListApp.Models.Output
{
    public class GenericResponse
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public int StatusCode { get; set; }
    }
}