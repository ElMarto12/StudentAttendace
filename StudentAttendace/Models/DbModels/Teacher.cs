using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendace.Models.DbModels;

public class Teacher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TeacherID { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Name { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? LastName { get; set; }

    public List<Subject> Subjects { get; } = new List<Subject>(); 

    public List<TeachersGroup> TeachersGroups { get; } = new List<TeachersGroup>();
    
    public int UserId { get; set; }
}