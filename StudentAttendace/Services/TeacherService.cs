﻿using System.Text.Json;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class TeacherService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IEnumerable<Teacher>> GetTeachersAsync()
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Teachers");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Teacher>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }
            else
            {
                throw new HttpRequestException($"{responseMessage.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }
    
    public async Task<Teacher> GetTeacherIdAsync(int teacherId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Teachers/{teacherId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Teacher>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }
            else
            {
                throw new HttpRequestException($"{responseMessage.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }
    
    public async Task<Teacher> GetTeacherByUserIdAsync(string? userId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Teachers/ById/{userId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Teacher>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }
            else
            {
                throw new HttpRequestException($"{responseMessage.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }
}