using System;
using System.Collections.Generic;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<GoogleAccessToken> GoogleAccessTokens { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71872B73E499");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.QualificationId).HasColumnName("QualificationID");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Courses__Instruc__04E4BC85");

            entity.HasOne(d => d.Qualification).WithMany(p => p.Courses)
                .HasForeignKey(d => d.QualificationId)
                .HasConstraintName("FK__Courses__Qualifi__05D8E0BE");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Departme__0148818E2475C5D3");

            entity.ToTable("Department");

            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.DeptDescription).HasColumnType("text");
            entity.Property(e => e.DeptName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F6877FB7403111A");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => new { e.StudentId, e.QualificationId }, "UQ_Enrollment_Student_Qualification").IsUnique();

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.QualificationId).HasColumnName("QualificationID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Qualification).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.QualificationId)
                .HasConstraintName("FK_Enrollment_Qualification");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<GoogleAccessToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__GoogleAc__658FEE8AB5F6FFAB");

            entity.ToTable("GoogleAccessToken");

            entity.HasIndex(e => e.UserId, "UQ__GoogleAc__1788CCADA797C9D1").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.AccessToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AccessTokenProvider)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.GoogleAccessToken)
                .HasForeignKey<GoogleAccessToken>(d => d.UserId)
                .HasConstraintName("FK_User_ID");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__9D010B7BC39B0CF3");

            entity.ToTable("Instructor");

            entity.HasIndex(e => e.UserId, "UQ__Instruct__1788CCAD46D78469").IsUnique();

            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.StaffNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Instructor_Department");

            entity.HasOne(d => d.User).WithOne(p => p.Instructor)
                .HasForeignKey<Instructor>(d => d.UserId)
                .HasConstraintName("FK_Instructor_User");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK__Qualific__C95C128AF909DA95");

            entity.ToTable("Qualification");

            entity.Property(e => e.QualificationId).HasColumnName("QualificationID");
            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.QualificationName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Dept).WithMany(p => p.Qualifications)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Qualification_Department");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Qualifications)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK_Qualification_Instructor");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79FD73D201");

            entity.ToTable("Student");

            entity.HasIndex(e => e.StudentNumber, "UQ_Student_StudentNumber").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EnrollmentStatus)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StudentNumber)
                .HasMaxLength(8)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE31D3DD6");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4744AD4DD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
