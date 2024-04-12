// <copyright file="RestorePasswordInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented

namespace ToDoListApp.Models.Input
{
    using System.ComponentModel.DataAnnotations;

    public class RestorePasswordInput
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(500)]
        [EmailAddress]
        public string Email { get; set; }
    }
}