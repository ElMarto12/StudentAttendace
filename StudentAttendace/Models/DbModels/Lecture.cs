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
    
    [Column(TypeName = "tinyint(1)")]
    public bool IsAttended { get; set; }

    public List<StudentsLecture> StudentsLectures { get; set; } = new List<StudentsLecture>();
    
    public int GroupId { get; set; } // foreign key - Group
    public Group Group { get; } = null!;
    
    public int SubjectId { get; set; } // foreign key - Subject
    public Subject Subject { get; } = null!;
}