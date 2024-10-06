using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class Lecture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LectureId { get; set; }
    
    [Required]
    [Column(TypeName = "date")]
    public DateTime LectureDate { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal AttendancePercentage { get; set; }
    
    [Column(TypeName = "tinyint(1)")]
    public bool IsAttended { get; set; }

    public List<StudentsLecture> StudentsLectures { get; set; } = new List<StudentsLecture>();
    
    public int SubjectId { get; set; } // foreign key - Subject
    public Subject Subject { get; } = null!;
}