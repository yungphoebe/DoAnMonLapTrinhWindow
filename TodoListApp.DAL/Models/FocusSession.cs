using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class FocusSession
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public int? TaskId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? DurationMinutes { get; set; }

    public string? Notes { get; set; }

    public virtual Task? Task { get; set; }

    public virtual User User { get; set; } = null!;
}
