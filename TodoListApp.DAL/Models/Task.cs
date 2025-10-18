using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int? ProjectId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? DueDate { get; set; }

    public int? EstimatedMinutes { get; set; }

    public int? ActualMinutes { get; set; }

    public int? ParentTaskId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<FocusSession> FocusSessions { get; set; } = new List<FocusSession>();

    public virtual ICollection<Task> InverseParentTask { get; set; } = new List<Task>();

    public virtual Task? ParentTask { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
