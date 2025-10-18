using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ColorCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User User { get; set; } = null!;
}
