using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendace.Models.DbModels;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    
    [Required]
    [EmailAddress]
    [Column(TypeName = "varchar(65)")]
    public string Email { get; set; }
    
    [Required]
    [PasswordPropertyText]
    [Column(TypeName = "varchar(255)")]
    public string Password { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Role { get; set; }
}