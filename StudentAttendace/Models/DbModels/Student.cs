using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace StudentAttendace.Models.DbModels;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentID { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string LastName { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Password { get; set; }
    
    public int GroupId { get; set; } // foreign key - Group
    
    public List<StudentsLecture> StudentsLectures { get; set; } = new List<StudentsLecture>();
    public List<StudentsSubject> StudentsSubjects { get; set; } = new List<StudentsSubject>();
    public List<SubjectAttendance> SubjectAttendances { get; set; } = new List<SubjectAttendance>();
}