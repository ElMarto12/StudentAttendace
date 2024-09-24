using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class TeachersGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TeachersGroupId { get; set; }

    public int TeacherId { get; set; } // foreign key - Teacher
    public Teacher Teacher { get; set; } = null!;
    
    public int GroupId { get; set; } // foreign key - Group
    public Group Group { get; set; } = null!;
}