using Microsoft.EntityFrameworkCore;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Config;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext
{
    public required DbSet<User> Users { get; set; }
}