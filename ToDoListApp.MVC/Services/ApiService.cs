// <copyright file="ApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1309 // Field names should not begin with underscore

namespace ToDoListApp.MVC.Services
{
    using System.Net;
    using ToDoListApp.Models.Input;
    using ToDoListApp.Models.Output;
    using ToDoListApp.MVC.Models.Input;

    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        // add to do
        public async Task<ApiResponse> AddToDoAsync(AddToDoInput model)
        {
            var response = await this._httpClient.PostAsJsonAsync($"{this._apiBaseUrl}/ToDoList/add-todo", model);
            return new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
            };
        }

        // update to do
        public async Task<ApiResponse> UpdateToDoAsync(int id, GetToDoOutput model)
        {
            var response = await this._httpClient.PutAsJsonAsync($"{this._apiBaseUrl}/ToDoList/update/{id}", model);
            return new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
            };
        }

        // delete to do
        public async Task<bool> DeleteToDoAsync(int id)
        {
            var response = await this._httpClient.DeleteAsync($"{this._apiBaseUrl}/ToDoList/delete/{id}");
            return response.IsSuccessStatusCode;
        }

        // get to do
        public async Task<IEnumerable<GetToDoOutput>> GetToDoListAsync(GetToDoInput input)
        {
            var response = await this._httpClient.PostAsJsonAsync($"{this._apiBaseUrl}/ToDoList/get-list", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IEnumerable<GetToDoOutput>>();
        }

        // sign in
        public async Task<ApiResponse> SignInAsync(SignInInput model)
        {
            var response = await this._httpClient.PostAsJsonAsync($"{this._apiBaseUrl}/ToDoList/sign-in", model);
            var result = new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
            };
            return result;
        }

        // sign up
        public async Task<ApiResponse> SignUpAsync(CreateUserInput user)
        {
            var response = await this._httpClient.PostAsJsonAsync($"{this._apiBaseUrl}/ToDoList/sign-up", user);
            return new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
            };
        }

        // restore pass
        public async Task<ApiResponse> RestorePasswordAsync(RestorePasswordInput user)
        {
            var response = await this._httpClient.PostAsJsonAsync($"{this._apiBaseUrl}/ToDoList/restore-password", user);
            return new ApiResponse
            {
                Success = response.IsSuccessStatusCode,
                Message = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
            };
        }

        public class ApiResponse
        {
            public bool Success { get; set; }

            public string? Message { get; set; }

            public HttpStatusCode StatusCode { get; set; }
        }
    }
}
