-- =============================================
-- ToDoList App - Sample Data Seed Script
-- =============================================
-- Ch?y script này trong SQL Server Management Studio
-- ?? seed d? li?u m?u vào database ToDoListApp
-- =============================================

USE ToDoListApp;
GO

-- Xóa d? li?u c? (n?u có) theo th? t? ?? tránh l?i foreign key
DELETE FROM FocusSessions;
DELETE FROM ActivityLog;
DELETE FROM UserSettings;
DELETE FROM Reminders;
DELETE FROM TaskTags;
DELETE FROM Tasks;
DELETE FROM ProjectMembers;
DELETE FROM Projects;
DELETE FROM Tags;
DELETE FROM Users;
GO

-- Reset identity seeds
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Projects', RESEED, 0);
DBCC CHECKIDENT ('Tasks', RESEED, 0);
DBCC CHECKIDENT ('Tags', RESEED, 0);
DBCC CHECKIDENT ('Reminders', RESEED, 0);
DBCC CHECKIDENT ('UserSettings', RESEED, 0);
DBCC CHECKIDENT ('ActivityLog', RESEED, 0);
DBCC CHECKIDENT ('FocusSessions', RESEED, 0);
GO

-- =============================================
-- SEED USERS
-- =============================================
SET IDENTITY_INSERT Users ON;
GO

INSERT INTO Users (UserID, FullName, Email, PasswordHash, CreatedAt, LastLogin, IsActive)
VALUES 
(1, N'Nguy?n V?n An', 'nguyenvanan@example.com', 'hashed_password_1', DATEADD(MONTH, -6, GETDATE()), DATEADD(DAY, -1, GETDATE()), 1),
(2, N'Tr?n Th? Bình', 'tranthibinh@example.com', 'hashed_password_2', DATEADD(MONTH, -4, GETDATE()), DATEADD(DAY, -2, GETDATE()), 1),
(3, N'Lê Minh C??ng', 'leminhcuong@example.com', 'hashed_password_3', DATEADD(MONTH, -3, GETDATE()), DATEADD(HOUR, -5, GETDATE()), 1),
(4, N'Ph?m Thu Duyên', 'phamthuduyen@example.com', 'hashed_password_4', DATEADD(MONTH, -2, GETDATE()), GETDATE(), 1);

SET IDENTITY_INSERT Users OFF;
GO

-- =============================================
-- SEED TAGS
-- =============================================
SET IDENTITY_INSERT Tags ON;
GO

INSERT INTO Tags (TagID, UserID, TagName, ColorCode)
VALUES 
(1, 1, 'Urgent', '#FF0000'),
(2, 1, 'Work', '#0000FF'),
(3, 1, 'Personal', '#00FF00'),
(4, 2, 'Meeting', '#FFA500'),
(5, 2, 'Development', '#800080'),
(6, 3, 'Bug Fix', '#FF1493'),
(7, 3, 'Feature', '#00CED1'),
(8, 4, 'Research', '#FFD700');

SET IDENTITY_INSERT Tags OFF;
GO

-- =============================================
-- SEED PROJECTS
-- =============================================
SET IDENTITY_INSERT Projects ON;
GO

INSERT INTO Projects (ProjectID, UserID, ProjectName, Description, ColorCode, CreatedAt, IsArchived)
VALUES 
(1, 1, 'Website Redesign', N'Thi?t k? l?i giao di?n website công ty', '#4A90E2', DATEADD(MONTH, -2, GETDATE()), 0),
(2, 1, 'Mobile App Development', N'Phát tri?n ?ng d?ng di ??ng cho khách hàng', '#F5A623', DATEADD(MONTH, -1, GETDATE()), 0),
(3, 2, 'Marketing Campaign Q4', N'Chi?n d?ch marketing quý 4', '#7ED321', DATEADD(DAY, -45, GETDATE()), 0),
(4, 3, 'Database Optimization', N'T?i ?u hóa hi?u su?t database', '#BD10E0', DATEADD(DAY, -30, GETDATE()), 0),
(5, 3, 'API Integration', N'Tích h?p API bên th? ba', '#50E3C2', DATEADD(DAY, -20, GETDATE()), 0),
(6, 4, 'User Research', N'Nghiên c?u ng??i dùng và UX', '#FF6B6B', DATEADD(DAY, -15, GETDATE()), 0);

SET IDENTITY_INSERT Projects OFF;
GO

-- =============================================
-- SEED TASKS
-- =============================================
SET IDENTITY_INSERT Tasks ON;
GO

INSERT INTO Tasks (TaskID, ProjectID, UserID, Title, Description, Priority, Status, DueDate, EstimatedMinutes, ActualMinutes, CreatedAt, UpdatedAt, IsDeleted)
VALUES 
-- Website Redesign tasks
(1, 1, 1, N'Thi?t k? mockup trang ch?', N'T?o mockup cho trang ch? m?i', 'High', 'Completed', DATEADD(DAY, -10, GETDATE()), 240, 220, DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -10, GETDATE()), 0),
(2, 1, 1, N'Phát tri?n giao di?n responsive', N'Code HTML/CSS responsive cho t?t c? các trang', 'High', 'In Progress', DATEADD(DAY, 5, GETDATE()), 480, 300, DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -1, GETDATE()), 0),
(3, 1, 1, N'Tích h?p CMS', N'Tích h?p h? th?ng qu?n lý n?i dung', 'Medium', 'Pending', DATEADD(DAY, 15, GETDATE()), 360, NULL, DATEADD(DAY, -15, GETDATE()), NULL, 0),

-- Mobile App Development tasks
(4, 2, 1, N'Thi?t k? UI/UX cho màn hình ??ng nh?p', N'T?o mockup và prototype cho flow ??ng nh?p', 'High', 'Completed', DATEADD(DAY, -5, GETDATE()), 180, 200, DATEADD(DAY, -25, GETDATE()), DATEADD(DAY, -5, GETDATE()), 0),
(5, 2, 1, N'Phát tri?n ch?c n?ng chat', N'Implement real-time chat v?i Firebase', 'High', 'In Progress', DATEADD(DAY, 7, GETDATE()), 600, 400, DATEADD(DAY, -18, GETDATE()), GETDATE(), 0),
(6, 2, 1, N'Testing trên nhi?u thi?t b?', N'Test app trên iOS và Android', 'Medium', 'Pending', DATEADD(DAY, 20, GETDATE()), 240, NULL, DATEADD(DAY, -10, GETDATE()), NULL, 0),

-- Marketing Campaign tasks
(7, 3, 2, N'L?p k? ho?ch content', N'Lên k? ho?ch n?i dung cho 3 tháng', 'High', 'Completed', DATEADD(DAY, -15, GETDATE()), 300, 280, DATEADD(DAY, -40, GETDATE()), DATEADD(DAY, -15, GETDATE()), 0),
(8, 3, 2, N'Ch?y Facebook Ads', N'Setup và qu?n lý chi?n d?ch Facebook Ads', 'High', 'In Progress', DATEADD(DAY, 3, GETDATE()), 180, 120, DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -2, GETDATE()), 0),
(9, 3, 2, N'Phân tích k?t qu? campaign', N'?o l??ng và báo cáo hi?u qu?', 'Medium', 'Pending', DATEADD(DAY, 30, GETDATE()), 120, NULL, DATEADD(DAY, -25, GETDATE()), NULL, 0),

-- Database Optimization tasks
(10, 4, 3, N'Phân tích slow queries', N'Tìm và phân tích các query ch?m', 'High', 'Completed', DATEADD(DAY, -8, GETDATE()), 240, 260, DATEADD(DAY, -28, GETDATE()), DATEADD(DAY, -8, GETDATE()), 0),
(11, 4, 3, N'T?o indexes m?i', N'Thêm indexes ?? t?i ?u performance', 'High', 'In Progress', DATEADD(DAY, 4, GETDATE()), 180, 100, DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -1, GETDATE()), 0),
(12, 4, 3, N'Refactor stored procedures', N'T?i ?u hóa các stored procedures', 'Medium', 'Pending', DATEADD(DAY, 10, GETDATE()), 360, NULL, DATEADD(DAY, -15, GETDATE()), NULL, 0),

-- API Integration tasks
(13, 5, 3, N'Nghiên c?u API documentation', N'??c và hi?u API docs c?a nhà cung c?p', 'Medium', 'Completed', DATEADD(DAY, -12, GETDATE()), 120, 90, DATEADD(DAY, -18, GETDATE()), DATEADD(DAY, -12, GETDATE()), 0),
(14, 5, 3, N'Implement authentication flow', N'Xây d?ng OAuth flow v?i API', 'High', 'In Progress', DATEADD(DAY, 6, GETDATE()), 300, 200, DATEADD(DAY, -15, GETDATE()), GETDATE(), 0),
(15, 5, 3, N'Vi?t unit tests', N'Test coverage cho API integration', 'Medium', 'Pending', DATEADD(DAY, 12, GETDATE()), 240, NULL, DATEADD(DAY, -10, GETDATE()), NULL, 0),

-- User Research tasks
(16, 6, 4, N'Ph?ng v?n ng??i dùng', N'Th?c hi?n 10 cu?c ph?ng v?n user', 'High', 'In Progress', DATEADD(DAY, 8, GETDATE()), 600, 300, DATEADD(DAY, -14, GETDATE()), DATEADD(DAY, -3, GETDATE()), 0),
(17, 6, 4, N'Phân tích d? li?u nghiên c?u', N'T?ng h?p và phân tích k?t qu? nghiên c?u', 'Medium', 'Pending', DATEADD(DAY, 15, GETDATE()), 360, NULL, DATEADD(DAY, -12, GETDATE()), NULL, 0),
(18, 6, 4, N'T?o user personas', N'Xây d?ng personas d?a trên nghiên c?u', 'Medium', 'Pending', DATEADD(DAY, 20, GETDATE()), 240, NULL, DATEADD(DAY, -8, GETDATE()), NULL, 0),

-- Tasks không thu?c project
(19, NULL, 1, N'??c sách v? Design Patterns', N'H?c t?p và nghiên c?u Design Patterns', 'Low', 'In Progress', DATEADD(DAY, 30, GETDATE()), 600, 180, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, -2, GETDATE()), 0),
(20, NULL, 2, N'Tham gia workshop marketing', N'Workshop v? digital marketing trends', 'Low', 'Pending', DATEADD(DAY, 14, GETDATE()), 480, NULL, DATEADD(DAY, -5, GETDATE()), NULL, 0);

SET IDENTITY_INSERT Tasks OFF;
GO

-- =============================================
-- SEED TASK TAGS
-- =============================================
INSERT INTO TaskTags (TaskID, TagID)
VALUES 
(1, 1), -- Urgent
(1, 2), -- Work
(2, 2), -- Work
(2, 5), -- Development
(3, 2), -- Work
(3, 7), -- Feature
(5, 1), -- Urgent
(5, 5), -- Development
(8, 4), -- Meeting
(8, 2), -- Work
(11, 6), -- Bug Fix
(11, 1), -- Urgent
(14, 5), -- Development
(14, 7), -- Feature
(17, 8); -- Research
GO

-- =============================================
-- SEED REMINDERS
-- =============================================
SET IDENTITY_INSERT Reminders ON;
GO

INSERT INTO Reminders (ReminderID, TaskID, ReminderTime, IsSent)
VALUES 
(1, 2, DATEADD(DAY, 4, GETDATE()), 0),
(2, 5, DATEADD(DAY, 6, GETDATE()), 0),
(3, 8, DATEADD(DAY, 2, GETDATE()), 0),
(4, 11, DATEADD(DAY, 3, GETDATE()), 0),
(5, 14, DATEADD(DAY, 5, GETDATE()), 0);

SET IDENTITY_INSERT Reminders OFF;
GO

-- =============================================
-- SEED USER SETTINGS
-- =============================================
SET IDENTITY_INSERT UserSettings ON;
GO

INSERT INTO UserSettings (SettingID, UserID, Theme, Language, TimeZone, DailyGoalMinutes)
VALUES 
(1, 1, 'Dark', 'vi', 'SE Asia Standard Time', 480),
(2, 2, 'Light', 'vi', 'SE Asia Standard Time', 360),
(3, 3, 'Dark', 'en', 'SE Asia Standard Time', 420),
(4, 4, 'Light', 'vi', 'SE Asia Standard Time', 300);

SET IDENTITY_INSERT UserSettings OFF;
GO

-- =============================================
-- SEED ACTIVITY LOGS
-- =============================================
SET IDENTITY_INSERT ActivityLog ON;
GO

INSERT INTO ActivityLog (LogID, UserID, Entity, EntityID, Action, ActionTime)
VALUES 
(1, 1, 'Task', 1, 'Created', DATEADD(DAY, -30, GETDATE())),
(2, 1, 'Task', 1, 'Updated', DATEADD(DAY, -20, GETDATE())),
(3, 1, 'Task', 1, 'Completed', DATEADD(DAY, -10, GETDATE())),
(4, 2, 'Project', 3, 'Created', DATEADD(DAY, -45, GETDATE())),
(5, 3, 'Task', 10, 'Created', DATEADD(DAY, -28, GETDATE()));

SET IDENTITY_INSERT ActivityLog OFF;
GO

-- =============================================
-- SEED FOCUS SESSIONS
-- =============================================
SET IDENTITY_INSERT FocusSessions ON;
GO

INSERT INTO FocusSessions (SessionID, UserID, TaskID, StartTime, EndTime, DurationMinutes, Notes)
VALUES 
(1, 1, 2, DATEADD(HOUR, -2, DATEADD(DAY, -5, GETDATE())), DATEADD(DAY, -5, GETDATE()), 120, N'Làm vi?c hi?u qu?, hoàn thành ???c ph?n responsive header'),
(2, 1, 5, DATEADD(HOUR, -3, DATEADD(DAY, -3, GETDATE())), DATEADD(DAY, -3, GETDATE()), 180, N'Implement chat UI, còn ph?n real-time ch?a xong'),
(3, 2, 8, DATEADD(HOUR, -2, DATEADD(DAY, -2, GETDATE())), DATEADD(DAY, -2, GETDATE()), 120, N'Setup campaign, t?o ads content'),
(4, 3, 11, DATEADD(MINUTE, -90, DATEADD(DAY, -1, GETDATE())), DATEADD(DAY, -1, GETDATE()), 90, N'T?o indexes cho b?ng Users và Tasks'),
(5, 1, 2, DATEADD(HOUR, -3, GETDATE()), DATEADD(HOUR, -1, GETDATE()), 120, N'Hoàn thành ph?n responsive cho mobile');

SET IDENTITY_INSERT FocusSessions OFF;
GO

-- =============================================
-- VERIFY DATA
-- =============================================
PRINT '========================================';
PRINT 'Seed Data Summary:';
PRINT '========================================';
PRINT 'Users: ' + CAST((SELECT COUNT(*) FROM Users) AS VARCHAR);
PRINT 'Projects: ' + CAST((SELECT COUNT(*) FROM Projects) AS VARCHAR);
PRINT 'Tasks: ' + CAST((SELECT COUNT(*) FROM Tasks) AS VARCHAR);
PRINT 'Tags: ' + CAST((SELECT COUNT(*) FROM Tags) AS VARCHAR);
PRINT 'TaskTags: ' + CAST((SELECT COUNT(*) FROM TaskTags) AS VARCHAR);
PRINT 'Reminders: ' + CAST((SELECT COUNT(*) FROM Reminders) AS VARCHAR);
PRINT 'UserSettings: ' + CAST((SELECT COUNT(*) FROM UserSettings) AS VARCHAR);
PRINT 'ActivityLogs: ' + CAST((SELECT COUNT(*) FROM ActivityLog) AS VARCHAR);
PRINT 'FocusSessions: ' + CAST((SELECT COUNT(*) FROM FocusSessions) AS VARCHAR);
PRINT '========================================';
PRINT 'Seed completed successfully!';
PRINT '========================================';
GO
