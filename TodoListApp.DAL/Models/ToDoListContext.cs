using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TodoListApp.DAL.Models;

public partial class ToDoListContext : DbContext
{
    public ToDoListContext()
    {
    }

    public ToDoListContext(DbContextOptions<ToDoListContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<FocusSession> FocusSessions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<VUserTask> VUserTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Load connection string from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback connection string
                connectionString = "Server=DESKTOP-LN5QDF6\\SQLEXPRESS;Database=ToDoListApp;Integrated Security=True;TrustServerCertificate=True";
            }

            optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__5E5499A83EE997B6");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Entity).HasMaxLength(50);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ActivityL__UserI__5AEE82B9");
        });

        modelBuilder.Entity<FocusSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__FocusSes__C9F4927058E3129C");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Task).WithMany(p => p.FocusSessions)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__FocusSess__TaskI__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.FocusSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FocusSess__UserI__5629CD9C");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABED0E11E3A35");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.ColorCode).HasMaxLength(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsArchived).HasDefaultValue(false);
            entity.Property(e => e.ProjectName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Projects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__UserID__3C69FB99");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.UserId }).HasName("PK__ProjectM__A762321A1FCB6362");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Viewer");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__ProjectMe__Proje__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProjectMe__UserI__656C112C");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId).HasName("PK__Reminder__01A830A7EEA6BD2D");

            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.IsSent).HasDefaultValue(false);
            entity.Property(e => e.ReminderTime).HasColumnType("datetime");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Task).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Reminders__TaskI__52593CB8");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CFA4CC4EB7E58");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.ColorCode).HasMaxLength(10);
            entity.Property(e => e.TagName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Tags)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tags__UserID__4BAC3F29");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949D116655AE9");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ParentTaskId).HasColumnName("ParentTaskID");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("Medium");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ParentTask).WithMany(p => p.InverseParentTask)
                .HasForeignKey(d => d.ParentTaskId)
                .HasConstraintName("FK__Tasks__ParentTas__46E78A0C");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Tasks__ProjectID__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__UserID__4222D4EF");

            entity.HasMany(d => d.Tags).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__TaskTags__TagID__4F7CD00D"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK__TaskTags__TaskID__4E88ABD4"),
                    j =>
                    {
                        j.HasKey("TaskId", "TagId").HasName("PK__TaskTags__AA3E86753C8D6388");
                        j.ToTable("TaskTags");
                        j.IndexerProperty<int>("TaskId").HasColumnName("TaskID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC5F92BF47");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534DF6C6CE2").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__UserSett__54372AFD60164E63");

            entity.Property(e => e.SettingId).HasColumnName("SettingID");
            entity.Property(e => e.DailyGoalMinutes).HasDefaultValue(120);
            entity.Property(e => e.Language)
                .HasMaxLength(10)
                .HasDefaultValue("en");
            entity.Property(e => e.Theme)
                .HasMaxLength(20)
                .HasDefaultValue("Light");
            entity.Property(e => e.TimeZone).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSetti__UserI__5EBF139D");
        });

        modelBuilder.Entity<VUserTask>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vUserTasks");

            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.ProjectName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TaskTitle).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
