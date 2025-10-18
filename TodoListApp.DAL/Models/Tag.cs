using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public int UserId { get; set; }

    public string TagName { get; set; } = null!;

    public string? ColorCode { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
