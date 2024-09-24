using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class StudentsSubject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentsSubjectId { get; set; }
    
    public int StudentId { get; set; } // foreign key - Student
    public Student Student { get; } = null!;
    
    public int SubjectId { get; set; } // foreign key - Subject
    public Subject Subject { get; } = null!;
}