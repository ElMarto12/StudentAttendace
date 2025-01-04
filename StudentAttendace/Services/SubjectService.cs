using System.Text.Json;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class SubjectService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    
    public async Task<IEnumerable<Subject>> GetSubjectsAsync()
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Subjects");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Subject>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }

            throw new HttpRequestException($"{responseMessage.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    public async Task<Subject> GetSubjectByIdAsync(string? subjectId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Subjects/{subjectId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Subject>(content, new JsonSerializerOptions
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
    
    public async Task<IEnumerable<Subject>> GetSubjectsByTeacherIdAsync(string? teacherId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Subjects/ByTeacher/{teacherId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Subject>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }

            throw new HttpRequestException($"{responseMessage.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<SubjectAttendance>> GetSubjectAttendancesAsync()
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/SubjectsAttendance");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<SubjectAttendance>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }

            throw new HttpRequestException($"{responseMessage.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<SubjectAttendance>> GetSubjectAttendanceBySubjectIdAsync(int subjectId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/SubjectsAttendance/BySubject/{subjectId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<SubjectAttendance>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }

            throw new HttpRequestException($"{responseMessage.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }
    
    public async Task<IEnumerable<SubjectAttendance>> GetSubjectAttendanceByStudentIdAsync(int studentId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/SubjectsAttendance/ByStudent/{studentId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<SubjectAttendance>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidCastException();
            }

            throw new HttpRequestException($"{responseMessage.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }
}