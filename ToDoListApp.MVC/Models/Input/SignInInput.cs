// <copyright file="SignInInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ToDoListApp.Models.Input
{
    using System.ComponentModel.DataAnnotations;

    public class SignInInput
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(5000)]
        public string Password { get; set; }
    }
}