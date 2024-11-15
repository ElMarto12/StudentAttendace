using Microsoft.EntityFrameworkCore;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Config;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    
    public required DbSet<Teacher> Teachers { get; set; }
    
    public required DbSet<Group> Groups { get; set; }
    
    public required DbSet<GroupsSubject> GroupsSubjects { get; set; }
    
    public required DbSet<Lecture> Lectures { get; set; }
    
    public required DbSet<Student> Students { get; set; }
    
    public required DbSet<StudentsSubject> StudentsSubjects { get; set; }
    
    public required DbSet<Subject> Subjects { get; set; }
    
    
    public required DbSet<SubjectAttendance> SubjectAttendances { get; set; }
    
    public required DbSet<TeachersGroup> TeachersGroups { get; set; }
    
    public required DbSet<StudentsLecture> StudentsLectures { get; set; }
    
}