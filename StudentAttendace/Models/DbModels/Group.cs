using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupID { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string GroupName { get; set;}
    
    [Required]
    [Column(TypeName = "int")]
    public int StudentAmount { get; set; }
    
    public List<TeachersGroup> TeachersGroups { get; set; } = new List<TeachersGroup>();
    public List<Student> Students { get; set; } = new List<Student>();
    public List<GroupsSubject> GroupsSubjects { get; set; } = new List<GroupsSubject>();
}