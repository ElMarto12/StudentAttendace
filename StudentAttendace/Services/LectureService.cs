using System.Text.Json;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class LectureService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IEnumerable<Lecture>> GetLecturesBySubjectIdAsync(IEnumerable<Subject> subjects)
    {
        var allLectures = new List<Lecture>();

        try
        {
            foreach (var subject in subjects)
            {
                HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Lectures/BySubject/{subject.SubjectId}");
                
                if (responseMessage.IsSuccessStatusCode)
                {
                    string content = await responseMessage.Content.ReadAsStringAsync();
                    
                    var lectures = JsonSerializer.Deserialize<IEnumerable<Lecture>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (lectures != null)
                    {
                        allLectures.AddRange(lectures);
                    }
                }
                else
                {
                    throw new HttpRequestException($"Failed to get lectures for SubjectId {subject.SubjectId}. Status: {responseMessage.StatusCode}");
                }
            }
            
            return allLectures;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Exception: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Exception: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Lecture>> GetLecturesWhichAreAttended(string subjectId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/Lectures/IsAttended/{subjectId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<Lecture>>(content, new JsonSerializerOptions
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