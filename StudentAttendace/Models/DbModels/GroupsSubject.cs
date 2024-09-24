using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class GroupsSubject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupsSubjectId { get; set; }
    
    public int GroupId { get; set; } // foreign key - Group
    public Group Group { get; } = null!;
    
    public int SubjectId { get; set; } // foreign key - Subject
    public Subject Subject { get; } = null!;
}