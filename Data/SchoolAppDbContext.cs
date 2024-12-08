using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data
{
    public class SchoolAppDbContext : DbContext
    {

        public SchoolAppDbContext()
        {
        }

        public SchoolAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.UserRole).HasConversion<string>();

                //entity.HasKey(e => e.Id);   // optional if convention holds
                //entity.Property(e => e.Email).HasMaxLength(50);
                //entity.Property(e => e.Firstname);  // default MAX
                //entity.Property(e => e.Lastname);
                //entity.Property(e => e.Password).HasMaxLength(60);
                //entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.InsertedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
               .ValueGeneratedOnAddOrUpdate()
               .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers");

                entity.Property(e => e.InsertedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

                //entity.HasKey(e => e.Id);   // Optional since convension holds
                //entity.Property(e => e.Institution);
                //entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasIndex(e => e.Institution, "IX_Teachers_Institution");
                entity.HasIndex(e => e.UserId, "IX_Teachers_UserId").IsUnique();

                // Optional if conventions hold i.e. Id, UserId and references
                //entity.HasOne(d => d.User).WithOne(p => p.Teacher)
                //    .HasForeignKey<Teacher>(d => d.UserId)
                //    .HasConstraintName("FK_Teachers_Users");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.Property(e => e.InsertedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.Am)
                //    .HasMaxLength(10)
                //    .HasColumnName("AM");
                //entity.Property(e => e.Department);
                //entity.Property(e => e.Institution);

                entity.HasIndex(e => e.Institution, "IX_Students_Institution");
                entity.HasIndex(e => e.UserId, "IX_Students_UserId").IsUnique();
                entity.HasIndex(e => e.Am, "IX_Students_ΑΜ").IsUnique();

                // Optional if conventions hold i.e. Id, UserId and references
                //entity.HasOne(d => d.User).WithOne(p => p.Student)
                //    .HasForeignKey<Student>(d => d.UserId)
                //    .HasConstraintName("FK_Students_Users");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasIndex(e => e.Description, "IX_Courses_Description");
                //entity.Property(e => e.Id);
                //entity.Property(e => e.TeacherId);
                //entity.Property(e => e.Description);

                //entity.HasOne(c => c.Teacher)                   // Course has one Teacher
                //    .WithMany(t => t.Courses)             // Teacher has many Courses
                //    .HasForeignKey(c => c.TeacherId);
                //.IsRequired(false);                 // Optional: Since convention is TeacherId
                //.HasConstraintName("FK_TEACHERS_COURSES");  // Optional: But we can define a custom constraint name                   

                entity.HasMany(e => e.Students).WithMany(s => s.Courses)
                .UsingEntity("StudentsCourses");  
            });
        }
    }
}
