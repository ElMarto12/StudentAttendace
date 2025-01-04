using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class LectureTime
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LectureTimeId { get; set; }
    
    [Column(TypeName = "time")]
    public TimeSpan TimeStart { get; set; }
    
    [Column(TypeName = "time")]
    public TimeSpan TimeEnd { get; set; }

    public List<Lecture> Lectures { get; } = new List<Lecture>();
}