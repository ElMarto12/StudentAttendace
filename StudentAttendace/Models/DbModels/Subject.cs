using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class Subject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubjectId { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string SubjectName { get; set; }
    
    [Required]
    [Column(TypeName = "int")]
    public int LectureNumber { get; set; }

    public List<GroupsSubject> GroupsSubjects { get; set; } = new List<GroupsSubject>();
    public List<Lecture> Lectures { get; set; } = new List<Lecture>();
    public List<SubjectAttendance> SubjectAttendances { get; set; } = new List<SubjectAttendance>();
    public List<StudentsSubject> StudentsSubjects { get; set; } = new List<StudentsSubject>();
    
    public int TeacherId { get; set; } // foreign key - Teacher
    public Teacher Teacher { get; set; } = null!;
}