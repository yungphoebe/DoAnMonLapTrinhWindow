﻿using System;
using System.Collections.Generic;

namespace TodoListApp.DAL.Models;

public partial class ProjectMember
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string? Role { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
