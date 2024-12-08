using System.Text.Json;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class StudentService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Students");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<Student>>(content, new JsonSerializerOptions
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
    
    
    public async Task<IEnumerable<Student>> GetStudentsByGroupIdAsync(string? groupId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Students/ByGroup/{groupId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<Student>>(content, new JsonSerializerOptions
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

    public async Task<IEnumerable<SubjectAttendance>> GetSubjectAttendancesByStudentIdAsync(string? studentId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Students/ByStudent/{studentId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<SubjectAttendance>>(content, new JsonSerializerOptions
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

    public async Task<IEnumerable<StudentsLecture>> GetStudentsLecturesByLectureIdAsync(string? lectureId)
    {
        try
        {
            HttpResponseMessage responseMessage =
                await _httpClient.GetAsync($"api/Students/StudentLectures/{lectureId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<StudentsLecture>>(content, new JsonSerializerOptions
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