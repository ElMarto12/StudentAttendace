using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class StudentsLecture
 {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int StudentsLectureId { get; set; }
     
     [Column(TypeName = "tinyint(1)")]
     public bool IsParticipating { get; set; }
     
     [Column(TypeName = "datetime")]
     public DateTime Time { get; set; }
     
     public int LectureId { get; set; } // foreign key - Lecture
     public Lecture Lecture { get; set; }
     
     public int StudentId { get; set; } // foreign key - Student
     public Student Student { get; set; }
 }