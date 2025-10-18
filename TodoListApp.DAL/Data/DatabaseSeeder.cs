using System;
using System.Collections.Generic;
using System.Linq;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoListApp.DAL.Data
{
    /// <summary>
    /// Class ?? seed d? li?u m?u v�o database
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly ToDoListContext _context;

        public DatabaseSeeder(ToDoListContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Seed t?t c? d? li?u m?u
        /// </summary>
        public void SeedAll()
        {
            try
            {
                // T?o Users
                var users = SeedUsers();
                _context.SaveChanges();

                // T?o Tags
                var tags = SeedTags(users);
                _context.SaveChanges();

                // T?o Projects
                var projects = SeedProjects(users);
                _context.SaveChanges();

                // T?o Tasks
                var tasks = SeedTasks(users, projects);
                _context.SaveChanges();

                // T?o TaskTags
                SeedTaskTags(tasks, tags);
                _context.SaveChanges();

                // T?o Reminders
                SeedReminders(tasks);
                _context.SaveChanges();

                // T?o UserSettings
                SeedUserSettings(users);
                _context.SaveChanges();

                // T?o ActivityLogs
                SeedActivityLogs(users);
                _context.SaveChanges();

                // T?o FocusSessions
                SeedFocusSessions(users, tasks);
                _context.SaveChanges();

                Console.WriteLine("? Seed d? li?u th�nh c�ng!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? L?i khi seed d? li?u: {ex.Message}");
                Console.WriteLine($"Chi ti?t: {ex.InnerException?.Message}");
                throw;
            }
        }

        private List<User> SeedUsers()
        {
            // Ki?m tra xem ?� c� users ch?a
            if (_context.Users.Any())
            {
                Console.WriteLine("?? Database ?� c� users, b? qua seed users");
                return _context.Users.ToList();
            }

            var users = new List<User>
            {
                new User
                {
                    FullName = "Nguy?n V?n An",
                    Email = "nguyenvanan@example.com",
                    PasswordHash = "hashed_password_1", // Trong th?c t? n�n hash password
                    CreatedAt = DateTime.Now.AddMonths(-6),
                    LastLogin = DateTime.Now.AddDays(-1),
                    IsActive = true
                },
                new User
                {
                    FullName = "Tr?n Th? B�nh",
                    Email = "tranthibinh@example.com",
                    PasswordHash = "hashed_password_2",
                    CreatedAt = DateTime.Now.AddMonths(-4),
                    LastLogin = DateTime.Now.AddDays(-2),
                    IsActive = true
                },
                new User
                {
                    FullName = "L� Minh C??ng",
                    Email = "leminhcuong@example.com",
                    PasswordHash = "hashed_password_3",
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    LastLogin = DateTime.Now.AddHours(-5),
                    IsActive = true
                },
                new User
                {
                    FullName = "Ph?m Thu Duy�n",
                    Email = "phamthuduyen@example.com",
                    PasswordHash = "hashed_password_4",
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    LastLogin = DateTime.Now,
                    IsActive = true
                }
            };

            _context.Users.AddRange(users);
            Console.WriteLine($"? ?� t?o {users.Count} users");
            return users;
        }

        private List<Tag> SeedTags(List<User> users)
        {
            if (_context.Tags.Any())
            {
                Console.WriteLine("?? Database ?� c� tags, b? qua seed tags");
                return _context.Tags.ToList();
            }

            // Ki?m tra ?? users kh�ng
            if (users.Count < 4)
            {
                Console.WriteLine("? Kh�ng ?? users ?? t?o tags. C?n �t nh?t 4 users.");
                return new List<Tag>();
            }

            var tags = new List<Tag>
            {
                new Tag { UserId = users[0].UserId, TagName = "Urgent", ColorCode = "#FF0000" },
                new Tag { UserId = users[0].UserId, TagName = "Work", ColorCode = "#0000FF" },
                new Tag { UserId = users[0].UserId, TagName = "Personal", ColorCode = "#00FF00" },
                new Tag { UserId = users[1].UserId, TagName = "Meeting", ColorCode = "#FFA500" },
                new Tag { UserId = users[1].UserId, TagName = "Development", ColorCode = "#800080" },
                new Tag { UserId = users[2].UserId, TagName = "Bug Fix", ColorCode = "#FF1493" },
                new Tag { UserId = users[2].UserId, TagName = "Feature", ColorCode = "#00CED1" },
                new Tag { UserId = users[3].UserId, TagName = "Research", ColorCode = "#FFD700" },
            };

            _context.Tags.AddRange(tags);
            Console.WriteLine($"? ?� t?o {tags.Count} tags");
            return tags;
        }

        private List<Project> SeedProjects(List<User> users)
        {
            if (_context.Projects.Any())
            {
                Console.WriteLine("?? Database ?� c� projects, b? qua seed projects");
                return _context.Projects.ToList();
            }

            // Ki?m tra ?? users kh�ng
            if (users.Count < 4)
            {
                Console.WriteLine("? Kh�ng ?? users ?? t?o projects. C?n �t nh?t 4 users.");
                return new List<Project>();
            }

            var projects = new List<Project>
            {
                new Project
                {
                    UserId = users[0].UserId,
                    ProjectName = "Website Redesign",
                    Description = "Thi?t k? l?i giao di?n website c�ng ty",
                    ColorCode = "#4A90E2",
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    IsArchived = false
                },
                new Project
                {
                    UserId = users[0].UserId,
                    ProjectName = "Mobile App Development",
                    Description = "Ph�t tri?n ?ng d?ng di ??ng cho kh�ch h�ng",
                    ColorCode = "#F5A623",
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    IsArchived = false
                },
                new Project
                {
                    UserId = users[1].UserId,
                    ProjectName = "Marketing Campaign Q4",
                    Description = "Chi?n d?ch marketing qu� 4",
                    ColorCode = "#7ED321",
                    CreatedAt = DateTime.Now.AddDays(-45),
                    IsArchived = false
                },
                new Project
                {
                    UserId = users[2].UserId,
                    ProjectName = "Database Optimization",
                    Description = "T?i ?u h�a hi?u su?t database",
                    ColorCode = "#BD10E0",
                    CreatedAt = DateTime.Now.AddDays(-30),
                    IsArchived = false
                },
                new Project
                {
                    UserId = users[2].UserId,
                    ProjectName = "API Integration",
                    Description = "T�ch h?p API b�n th? ba",
                    ColorCode = "#50E3C2",
                    CreatedAt = DateTime.Now.AddDays(-20),
                    IsArchived = false
                },
                new Project
                {
                    UserId = users[3].UserId,
                    ProjectName = "User Research",
                    Description = "Nghi�n c?u ng??i d�ng v� UX",
                    ColorCode = "#FF6B6B",
                    CreatedAt = DateTime.Now.AddDays(-15),
                    IsArchived = false
                }
            };

            _context.Projects.AddRange(projects);
            Console.WriteLine($"? ?� t?o {projects.Count} projects");
            return projects;
        }

        private List<Models.Task> SeedTasks(List<User> users, List<Project> projects)
        {
            if (_context.Tasks.Any())
            {
                Console.WriteLine("?? Database ?� c� tasks, b? qua seed tasks");
                return _context.Tasks.ToList();
            }

            // Ki?m tra ?? users v� projects
            if (users.Count < 4)
            {
                Console.WriteLine("? Kh�ng ?? users ?? t?o tasks. C?n �t nh?t 4 users.");
                return new List<Models.Task>();
            }

            if (projects.Count < 6)
            {
                Console.WriteLine("? Kh�ng ?? projects ?? t?o tasks. C?n �t nh?t 6 projects.");
                return new List<Models.Task>();
            }

            var tasks = new List<Models.Task>
            {
                // Website Redesign tasks
                new Models.Task
                {
                    ProjectId = projects[0].ProjectId,
                    UserId = users[0].UserId,
                    Title = "Thi?t k? mockup trang ch?",
                    Description = "T?o mockup cho trang ch? m?i",
                    Priority = "High",
                    Status = "Completed",
                    DueDate = DateTime.Now.AddDays(-10),
                    EstimatedMinutes = 240,
                    ActualMinutes = 220,
                    CreatedAt = DateTime.Now.AddDays(-30),
                    UpdatedAt = DateTime.Now.AddDays(-10),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[0].ProjectId,
                    UserId = users[0].UserId,
                    Title = "Ph�t tri?n giao di?n responsive",
                    Description = "Code HTML/CSS responsive cho t?t c? c�c trang",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(5),
                    EstimatedMinutes = 480,
                    ActualMinutes = 300,
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[0].ProjectId,
                    UserId = users[0].UserId,
                    Title = "T�ch h?p CMS",
                    Description = "T�ch h?p h? th?ng qu?n l� n?i dung",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(15),
                    EstimatedMinutes = 360,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    IsDeleted = false
                },

                // Mobile App Development tasks
                new Models.Task
                {
                    ProjectId = projects[1].ProjectId,
                    UserId = users[0].UserId,
                    Title = "Thi?t k? UI/UX cho m�n h�nh ??ng nh?p",
                    Description = "T?o mockup v� prototype cho flow ??ng nh?p",
                    Priority = "High",
                    Status = "Completed",
                    DueDate = DateTime.Now.AddDays(-5),
                    EstimatedMinutes = 180,
                    ActualMinutes = 200,
                    CreatedAt = DateTime.Now.AddDays(-25),
                    UpdatedAt = DateTime.Now.AddDays(-5),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[1].ProjectId,
                    UserId = users[0].UserId,
                    Title = "Ph�t tri?n ch?c n?ng chat",
                    Description = "Implement real-time chat v?i Firebase",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(7),
                    EstimatedMinutes = 600,
                    ActualMinutes = 400,
                    CreatedAt = DateTime.Now.AddDays(-18),
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[1].ProjectId,
                    UserId = users[0].UserId,
                    Title = "Testing tr�n nhi?u thi?t b?",
                    Description = "Test app tr�n iOS v� Android",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(20),
                    EstimatedMinutes = 240,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    IsDeleted = false
                },

                // Marketing Campaign tasks
                new Models.Task
                {
                    ProjectId = projects[2].ProjectId,
                    UserId = users[1].UserId,
                    Title = "L?p k? ho?ch content",
                    Description = "L�n k? ho?ch n?i dung cho 3 th�ng",
                    Priority = "High",
                    Status = "Completed",
                    DueDate = DateTime.Now.AddDays(-15),
                    EstimatedMinutes = 300,
                    ActualMinutes = 280,
                    CreatedAt = DateTime.Now.AddDays(-40),
                    UpdatedAt = DateTime.Now.AddDays(-15),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[2].ProjectId,
                    UserId = users[1].UserId,
                    Title = "Ch?y Facebook Ads",
                    Description = "Setup v� qu?n l� chi?n d?ch Facebook Ads",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(3),
                    EstimatedMinutes = 180,
                    ActualMinutes = 120,
                    CreatedAt = DateTime.Now.AddDays(-30),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[2].ProjectId,
                    UserId = users[1].UserId,
                    Title = "Ph�n t�ch k?t qu? campaign",
                    Description = "?o l??ng v� b�o c�o hi?u qu?",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(30),
                    EstimatedMinutes = 120,
                    CreatedAt = DateTime.Now.AddDays(-25),
                    IsDeleted = false
                },

                // Database Optimization tasks
                new Models.Task
                {
                    ProjectId = projects[3].ProjectId,
                    UserId = users[2].UserId,
                    Title = "Ph�n t�ch slow queries",
                    Description = "T�m v� ph�n t�ch c�c query ch?m",
                    Priority = "High",
                    Status = "Completed",
                    DueDate = DateTime.Now.AddDays(-8),
                    EstimatedMinutes = 240,
                    ActualMinutes = 260,
                    CreatedAt = DateTime.Now.AddDays(-28),
                    UpdatedAt = DateTime.Now.AddDays(-8),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[3].ProjectId,
                    UserId = users[2].UserId,
                    Title = "T?o indexes m?i",
                    Description = "Th�m indexes ?? t?i ?u performance",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(4),
                    EstimatedMinutes = 180,
                    ActualMinutes = 100,
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[3].ProjectId,
                    UserId = users[2].UserId,
                    Title = "Refactor stored procedures",
                    Description = "T?i ?u h�a c�c stored procedures",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(10),
                    EstimatedMinutes = 360,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    IsDeleted = false
                },

                // API Integration tasks
                new Models.Task
                {
                    ProjectId = projects[4].ProjectId,
                    UserId = users[2].UserId,
                    Title = "Nghi�n c?u API documentation",
                    Description = "??c v� hi?u API docs c?a nh� cung c?p",
                    Priority = "Medium",
                    Status = "Completed",
                    DueDate = DateTime.Now.AddDays(-12),
                    EstimatedMinutes = 120,
                    ActualMinutes = 90,
                    CreatedAt = DateTime.Now.AddDays(-18),
                    UpdatedAt = DateTime.Now.AddDays(-12),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[4].ProjectId,
                    UserId = users[2].UserId,
                    Title = "Implement authentication flow",
                    Description = "X�y d?ng OAuth flow v?i API",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(6),
                    EstimatedMinutes = 300,
                    ActualMinutes = 200,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[4].ProjectId,
                    UserId = users[2].UserId,
                    Title = "Vi?t unit tests",
                    Description = "Test coverage cho API integration",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(12),
                    EstimatedMinutes = 240,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    IsDeleted = false
                },

                // User Research tasks
                new Models.Task
                {
                    ProjectId = projects[5].ProjectId,
                    UserId = users[3].UserId,
                    Title = "Ph?ng v?n ng??i d�ng",
                    Description = "Th?c hi?n 10 cu?c ph?ng v?n user",
                    Priority = "High",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(8),
                    EstimatedMinutes = 600,
                    ActualMinutes = 300,
                    CreatedAt = DateTime.Now.AddDays(-14),
                    UpdatedAt = DateTime.Now.AddDays(-3),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[5].ProjectId,
                    UserId = users[3].UserId,
                    Title = "Ph�n t�ch d? li?u nghi�n c?u",
                    Description = "T?ng h?p v� ph�n t�ch k?t qu? nghi�n c?u",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(15),
                    EstimatedMinutes = 360,
                    CreatedAt = DateTime.Now.AddDays(-12),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = projects[5].ProjectId,
                    UserId = users[3].UserId,
                    Title = "T?o user personas",
                    Description = "X�y d?ng personas d?a tr�n nghi�n c?u",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(20),
                    EstimatedMinutes = 240,
                    CreatedAt = DateTime.Now.AddDays(-8),
                    IsDeleted = false
                },

                // M?t s? tasks kh�ng thu?c project n�o
                new Models.Task
                {
                    ProjectId = null,
                    UserId = users[0].UserId,
                    Title = "??c s�ch v? Design Patterns",
                    Description = "H?c t?p v� nghi�n c?u Design Patterns",
                    Priority = "Low",
                    Status = "In Progress",
                    DueDate = DateTime.Now.AddDays(30),
                    EstimatedMinutes = 600,
                    ActualMinutes = 180,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    IsDeleted = false
                },
                new Models.Task
                {
                    ProjectId = null,
                    UserId = users[1].UserId,
                    Title = "Tham gia workshop marketing",
                    Description = "Workshop v? digital marketing trends",
                    Priority = "Low",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(14),
                    EstimatedMinutes = 480,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    IsDeleted = false
                }
            };

            _context.Tasks.AddRange(tasks);
            Console.WriteLine($"? ?� t?o {tasks.Count} tasks");
            return tasks;
        }

        private void SeedTaskTags(List<Models.Task> tasks, List<Tag> tags)
        {
            // Ki?m tra ?? d? li?u
            if (tasks.Count < 17 || tags.Count < 8)
            {
                Console.WriteLine("?? Kh�ng ?? tasks ho?c tags ?? g�n, b? qua seed task tags");
                return;
            }

            // G�n tags cho tasks
            tasks[0].Tags.Add(tags[0]); // Urgent
            tasks[0].Tags.Add(tags[1]); // Work

            tasks[1].Tags.Add(tags[1]); // Work
            tasks[1].Tags.Add(tags[4]); // Development

            tasks[2].Tags.Add(tags[1]); // Work
            tasks[2].Tags.Add(tags[6]); // Feature

            tasks[4].Tags.Add(tags[0]); // Urgent
            tasks[4].Tags.Add(tags[4]); // Development

            tasks[7].Tags.Add(tags[3]); // Meeting
            tasks[7].Tags.Add(tags[1]); // Work

            tasks[10].Tags.Add(tags[5]); // Bug Fix
            tasks[10].Tags.Add(tags[0]); // Urgent

            tasks[13].Tags.Add(tags[4]); // Development
            tasks[13].Tags.Add(tags[6]); // Feature

            tasks[16].Tags.Add(tags[7]); // Research

            Console.WriteLine("? ?� g�n tags cho tasks");
        }

        private void SeedReminders(List<Models.Task> tasks)
        {
            if (_context.Reminders.Any())
            {
                Console.WriteLine("?? Database ?� c� reminders, b? qua seed reminders");
                return;
            }

            // Ki?m tra ?? tasks
            if (tasks.Count < 14)
            {
                Console.WriteLine("?? Kh�ng ?? tasks ?? t?o reminders, b? qua");
                return;
            }

            var reminders = new List<Reminder>
            {
                new Reminder
                {
                    TaskId = tasks[1].TaskId,
                    ReminderTime = DateTime.Now.AddDays(4),
                    IsSent = false
                },
                new Reminder
                {
                    TaskId = tasks[4].TaskId,
                    ReminderTime = DateTime.Now.AddDays(6),
                    IsSent = false
                },
                new Reminder
                {
                    TaskId = tasks[7].TaskId,
                    ReminderTime = DateTime.Now.AddDays(2),
                    IsSent = false
                },
                new Reminder
                {
                    TaskId = tasks[10].TaskId,
                    ReminderTime = DateTime.Now.AddDays(3),
                    IsSent = false
                },
                new Reminder
                {
                    TaskId = tasks[13].TaskId,
                    ReminderTime = DateTime.Now.AddDays(5),
                    IsSent = false
                }
            };

            _context.Reminders.AddRange(reminders);
            Console.WriteLine($"? ?� t?o {reminders.Count} reminders");
        }

        private void SeedUserSettings(List<User> users)
        {
            if (_context.UserSettings.Any())
            {
                Console.WriteLine("?? Database ?� c� user settings, b? qua seed");
                return;
            }

            // Ki?m tra ?? users
            if (users.Count < 4)
            {
                Console.WriteLine("?? Kh�ng ?? users ?? t?o settings, b? qua");
                return;
            }

            var settings = new List<UserSetting>
            {
                new UserSetting
                {
                    UserId = users[0].UserId,
                    Theme = "Dark",
                    Language = "vi",
                    TimeZone = "SE Asia Standard Time",
                    DailyGoalMinutes = 480
                },
                new UserSetting
                {
                    UserId = users[1].UserId,
                    Theme = "Light",
                    Language = "vi",
                    TimeZone = "SE Asia Standard Time",
                    DailyGoalMinutes = 360
                },
                new UserSetting
                {
                    UserId = users[2].UserId,
                    Theme = "Dark",
                    Language = "en",
                    TimeZone = "SE Asia Standard Time",
                    DailyGoalMinutes = 420
                },
                new UserSetting
                {
                    UserId = users[3].UserId,
                    Theme = "Light",
                    Language = "vi",
                    TimeZone = "SE Asia Standard Time",
                    DailyGoalMinutes = 300
                }
            };

            _context.UserSettings.AddRange(settings);
            Console.WriteLine($"? ?� t?o {settings.Count} user settings");
        }

        private void SeedActivityLogs(List<User> users)
        {
            if (_context.ActivityLogs.Any())
            {
                Console.WriteLine("?? Database ?� c� activity logs, b? qua seed");
                return;
            }

            // Ki?m tra ?? users
            if (users.Count < 3)
            {
                Console.WriteLine("?? Kh�ng ?? users ?? t?o activity logs, b? qua");
                return;
            }

            var logs = new List<ActivityLog>
            {
                new ActivityLog
                {
                    UserId = users[0].UserId,
                    Entity = "Task",
                    EntityId = 1,
                    Action = "Created",
                    ActionTime = DateTime.Now.AddDays(-30)
                },
                new ActivityLog
                {
                    UserId = users[0].UserId,
                    Entity = "Task",
                    EntityId = 1,
                    Action = "Updated",
                    ActionTime = DateTime.Now.AddDays(-20)
                },
                new ActivityLog
                {
                    UserId = users[0].UserId,
                    Entity = "Task",
                    EntityId = 1,
                    Action = "Completed",
                    ActionTime = DateTime.Now.AddDays(-10)
                },
                new ActivityLog
                {
                    UserId = users[1].UserId,
                    Entity = "Project",
                    EntityId = 3,
                    Action = "Created",
                    ActionTime = DateTime.Now.AddDays(-45)
                },
                new ActivityLog
                {
                    UserId = users[2].UserId,
                    Entity = "Task",
                    EntityId = 10,
                    Action = "Created",
                    ActionTime = DateTime.Now.AddDays(-28)
                }
            };

            _context.ActivityLogs.AddRange(logs);
            Console.WriteLine($"? ?� t?o {logs.Count} activity logs");
        }

        private void SeedFocusSessions(List<User> users, List<Models.Task> tasks)
        {
            if (_context.FocusSessions.Any())
            {
                Console.WriteLine("?? Database ?� c� focus sessions, b? qua seed");
                return;
            }

            // Ki?m tra ?? users v� tasks
            if (users.Count < 3 || tasks.Count < 11)
            {
                Console.WriteLine("?? Kh�ng ?? users ho?c tasks ?? t?o focus sessions, b? qua");
                return;
            }

            var sessions = new List<FocusSession>
            {
                new FocusSession
                {
                    UserId = users[0].UserId,
                    TaskId = tasks[1].TaskId,
                    StartTime = DateTime.Now.AddDays(-5).AddHours(-2),
                    EndTime = DateTime.Now.AddDays(-5),
                    DurationMinutes = 120,
                    Notes = "L�m vi?c hi?u qu?, ho�n th�nh ???c ph?n responsive header"
                },
                new FocusSession
                {
                    UserId = users[0].UserId,
                    TaskId = tasks[4].TaskId,
                    StartTime = DateTime.Now.AddDays(-3).AddHours(-3),
                    EndTime = DateTime.Now.AddDays(-3),
                    DurationMinutes = 180,
                    Notes = "Implement chat UI, c�n ph?n real-time ch?a xong"
                },
                new FocusSession
                {
                    UserId = users[1].UserId,
                    TaskId = tasks[7].TaskId,
                    StartTime = DateTime.Now.AddDays(-2).AddHours(-2),
                    EndTime = DateTime.Now.AddDays(-2),
                    DurationMinutes = 120,
                    Notes = "Setup campaign, t?o ads content"
                },
                new FocusSession
                {
                    UserId = users[2].UserId,
                    TaskId = tasks[10].TaskId,
                    StartTime = DateTime.Now.AddDays(-1).AddHours(-1.5),
                    EndTime = DateTime.Now.AddDays(-1),
                    DurationMinutes = 90,
                    Notes = "T?o indexes cho b?ng Users v� Tasks"
                },
                new FocusSession
                {
                    UserId = users[0].UserId,
                    TaskId = tasks[1].TaskId,
                    StartTime = DateTime.Now.AddHours(-3),
                    EndTime = DateTime.Now.AddHours(-1),
                    DurationMinutes = 120,
                    Notes = "Ho�n th�nh ph?n responsive cho mobile"
                }
            };

            _context.FocusSessions.AddRange(sessions);
            Console.WriteLine($"? ?� t?o {sessions.Count} focus sessions");
        }

        /// <summary>
        /// X�a t?t c? d? li?u
        /// </summary>
        public void ClearAll()
        {
            try
            {
                // X�a theo th? t? ng??c l?i ?? tr�nh l?i foreign key
                _context.FocusSessions.RemoveRange(_context.FocusSessions);
                _context.ActivityLogs.RemoveRange(_context.ActivityLogs);
                _context.UserSettings.RemoveRange(_context.UserSettings);
                _context.Reminders.RemoveRange(_context.Reminders);
                
                // X�a TaskTags relationship
                var tasks = _context.Tasks.Include(t => t.Tags).ToList();
                foreach (var task in tasks)
                {
                    task.Tags.Clear();
                }
                
                _context.Tasks.RemoveRange(_context.Tasks);
                _context.Projects.RemoveRange(_context.Projects);
                _context.Tags.RemoveRange(_context.Tags);
                _context.Users.RemoveRange(_context.Users);
                
                _context.SaveChanges();
                
                Console.WriteLine("? ?� x�a t?t c? d? li?u");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? L?i khi x�a d? li?u: {ex.Message}");
                throw;
            }
        }
    }
}
