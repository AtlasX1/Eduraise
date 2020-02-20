using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Eduraise.Models
{
    public partial class EduraiseContext : DbContext
    {
        public EduraiseContext()
        {
        }

        public EduraiseContext(DbContextOptions<EduraiseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CourseCategory> CourseCategory { get; set; }
        public virtual DbSet<CourseStudent> CourseStudent { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Lessons> Lessons { get; set; }
        public virtual DbSet<Marks> Marks { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-6BABV49;Initial Catalog=Eduraise;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.AdminEmail)
                    .IsRequired()
                    .HasColumnName("admin_email")
                    .HasMaxLength(50);

                entity.Property(e => e.AdminPassword)
                    .IsRequired()
                    .HasColumnName("admin_password")
                    .HasMaxLength(20);

                entity.Property(e => e.IsEmailConfirmed).HasColumnName("isEmailConfirmed");
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.BlockName)
                    .IsRequired()
                    .HasColumnName("block_name")
                    .HasMaxLength(50);

                entity.Property(e => e.BlockNumber).HasColumnName("block_number");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Block)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Block_Courses");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CourseCategory>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.CategoryId });

                entity.ToTable("Course/category");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CourseCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course/category_Categories");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseCategory)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course/category_Courses");
            });

            modelBuilder.Entity<CourseStudent>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.StudentId });

                entity.ToTable("Course/student");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.DateOfOverview)
                    .HasColumnName("date_of_overview")
                    .HasColumnType("date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseStudent)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course/student_Courses");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.CourseStudent)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course/student_Students");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name")
                    .HasMaxLength(50);

                entity.Property(e => e.CourseRating).HasColumnName("course_rating");

                entity.Property(e => e.DataOfCreation)
                    .HasColumnName("data_of_creation")
                    .HasColumnType("date");

                entity.Property(e => e.IsVerified).HasColumnName("isVerified");

                entity.Property(e => e.TeachersId).HasColumnName("teachers_id");

                entity.HasOne(d => d.Teachers)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeachersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Courses_Teachers");
            });

            modelBuilder.Entity<Lessons>(entity =>
            {
                entity.HasKey(e => e.LessonId);

                entity.Property(e => e.LessonId).HasColumnName("lesson_id");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.LessonName)
                    .IsRequired()
                    .HasColumnName("lesson_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LessonNumber).HasColumnName("lesson_number");

                entity.Property(e => e.LessonType)
                    .IsRequired()
                    .HasColumnName("lesson_type")
                    .HasMaxLength(10);

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Block");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_Lessons_Tests");
            });

            modelBuilder.Entity<Marks>(entity =>
            {
                entity.HasKey(e => e.MarkId);

                entity.Property(e => e.MarkId).HasColumnName("mark_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marks_Courses");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.IsEmailConfirmed).HasColumnName("isEmailConfirmed");

                entity.Property(e => e.StudentBio)
                    .HasColumnName("student_bio")
                    .HasColumnType("text");

                entity.Property(e => e.StudentEmail)
                    .IsRequired()
                    .HasColumnName("student_email")
                    .HasMaxLength(50);

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasColumnName("student_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StudentPassword)
                    .IsRequired()
                    .HasColumnName("student_password")
                    .HasMaxLength(20);

                entity.Property(e => e.StudentPhoto)
                    .HasColumnName("student_photo")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<Teachers>(entity =>
            {
                entity.HasKey(e => e.TeacherId);

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.Property(e => e.IsEmailConfirmed).HasColumnName("isEmailConfirmed");

                entity.Property(e => e.TeacherBio)
                    .HasColumnName("teacher_bio")
                    .HasColumnType("text");

                entity.Property(e => e.TeacherEmail)
                    .IsRequired()
                    .HasColumnName("teacher_email")
                    .HasMaxLength(50);

                entity.Property(e => e.TeacherName)
                    .IsRequired()
                    .HasColumnName("teacher_name")
                    .HasMaxLength(50);

                entity.Property(e => e.TeacherPassword)
                    .IsRequired()
                    .HasColumnName("teacher_password")
                    .HasMaxLength(20);

                entity.Property(e => e.TeacherPhoto)
                    .HasColumnName("teacher_photo")
                    .HasColumnType("image");
            });

            modelBuilder.Entity<Tests>(entity =>
            {
                entity.HasKey(e => e.TestId);

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.Answers)
                    .HasColumnName("answers")
                    .HasColumnType("text");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasColumnName("question")
                    .HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
