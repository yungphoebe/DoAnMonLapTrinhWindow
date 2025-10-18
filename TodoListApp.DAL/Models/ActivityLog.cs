using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string Entity { get; set; } = null!;

    public int? EntityId { get; set; }

    public DateTime? ActionTime { get; set; }

    public virtual User User { get; set; } = null!;
}
