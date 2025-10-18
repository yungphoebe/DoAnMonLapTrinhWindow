using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class UserSetting
{
    public int SettingId { get; set; }

    public int UserId { get; set; }

    public string? Theme { get; set; }

    public string? Language { get; set; }

    public string? TimeZone { get; set; }

    public int? DailyGoalMinutes { get; set; }

    public virtual User User { get; set; } = null!;
}
