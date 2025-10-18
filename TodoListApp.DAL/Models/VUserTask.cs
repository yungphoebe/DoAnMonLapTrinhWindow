using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class VUserTask
{
    public string FullName { get; set; } = null!;

    public string? ProjectName { get; set; }

    public string TaskTitle { get; set; } = null!;

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public DateTime? DueDate { get; set; }
}
