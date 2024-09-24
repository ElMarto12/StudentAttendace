using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class SubjectAttendance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubjectAttendanceId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal AttendancePercentage { get; set; }
    
    public int SubjectId { get; set; } // foreign key - Subject
    public Subject Subject { get; } = null!;
    
    public int StudentId { get; set; } // foreign key - Student
    public Student Student { get; } = null!; 
}
