using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Data
{
    public class TaskManagementDbContext: IdentityDbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> dbContext): base(dbContext)
        {

        }
        public DbSet <BaseProject> Projects { get; set; }
        public DbSet<BaseTask> Tasks { get; set; }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<ImageUpload> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminId = "285a31ba-407b-4e8a-b50d-f0e943071935";
            var userId = "f6c6f528-1ab0-42bf-9349-150485c56a0e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id=adminId,ConcurrencyStamp=adminId,Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id=userId,ConcurrencyStamp=userId,Name = "User", NormalizedName = "USER" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
