using System;
using System.Collections.Generic;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Data;

public partial class FtacademyStudentManagementContext : DbContext
{
    public FtacademyStudentManagementContext()
    {
    }

    public FtacademyStudentManagementContext(DbContextOptions<FtacademyStudentManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;DataBase=FTACADEMY_STUDENT_MANAGEMENT;Trusted_connection=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187D9E4583C");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses).HasConstraintName("FK__Courses__Instruc__208CD6FA");

            entity.HasOne(d => d.Qualification).WithMany(p => p.Courses).HasConstraintName("FK__Courses__Qualifi__1F98B2C1");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__0148818EC098FAA6");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F6877FB8818495D");

            entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Qualification).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_Qualification");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__9D010B7BEE79FFF3");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors).HasConstraintName("FK_Instructor_Department");

            entity.HasOne(d => d.User).WithOne(p => p.Instructor).HasConstraintName("FK_Instructor_User");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK__Qualific__C95C128AAB15A2C1");

            entity.HasOne(d => d.Dept).WithMany(p => p.Qualifications).HasConstraintName("FK_Qualification_Department");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Qualifications).HasConstraintName("FK_Qualification_Instructor");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A7964CE6E4A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF4EE8206");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
