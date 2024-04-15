// <copyright file="SignInInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

#pragma warning disable SA1600 // Elements should be documented
namespace ToDoListApp.Models.Input
{
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