-- ============================================
-- KI?M TRA D? LI?U TRONG DATABASE
-- ============================================
USE ToDoListApp;
GO

PRINT '========================================';
PRINT 'KI?M TRA D? LI?U SEED';
PRINT '========================================';
PRINT '';

-- 1. Ki?m tra t?t c? Users
PRINT '1. DANH S�CH USERS:';
PRINT '----------------------------------------';
SELECT 
    UserID,
    FullName,
    Email,
    PasswordHash,
    IsActive,
    CreatedAt
FROM Users
ORDER BY UserID;

DECLARE @UserCount INT = (SELECT COUNT(*) FROM Users);
PRINT '';
PRINT 'T?ng s? Users: ' + CAST(@UserCount AS VARCHAR);
PRINT '';

-- 2. Ki?m tra Projects
PRINT '2. DANH S�CH PROJECTS:';
PRINT '----------------------------------------';
SELECT 
    ProjectID,
    ProjectName,
    Description,
    UserID,
    CreatedAt
FROM Projects
ORDER BY ProjectID;

DECLARE @ProjectCount INT = (SELECT COUNT(*) FROM Projects);
PRINT '';
PRINT 'T?ng s? Projects: ' + CAST(@ProjectCount AS VARCHAR);
PRINT '';

-- 3. Ki?m tra Tasks
PRINT '3. S? L??NG TASKS:';
PRINT '----------------------------------------';
SELECT 
    Status,
    COUNT(*) as Count
FROM Tasks
GROUP BY Status
ORDER BY Status;

DECLARE @TaskCount INT = (SELECT COUNT(*) FROM Tasks);
PRINT '';
PRINT 'T?ng s? Tasks: ' + CAST(@TaskCount AS VARCHAR);
PRINT '';

-- 4. T?ng h?p t?t c?
PRINT '========================================';
PRINT '4. T?NG H?P D? LI?U:';
PRINT '========================================';

SELECT 'Users' as TableName, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL
SELECT 'Tasks', COUNT(*) FROM Tasks
UNION ALL
SELECT 'Tags', COUNT(*) FROM Tags
UNION ALL
SELECT 'Reminders', COUNT(*) FROM Reminders
UNION ALL
SELECT 'FocusSessions', COUNT(*) FROM FocusSessions
UNION ALL
SELECT 'UserSettings', COUNT(*) FROM UserSettings
UNION ALL
SELECT 'ActivityLog', COUNT(*) FROM ActivityLog;

PRINT '';
PRINT '========================================';
PRINT 'K?T TH�C KI?M TRA';
PRINT '========================================';

-- 5. Ki?m tra passwords ?? test ??ng nh?p
PRINT '';
PRINT '5. TH�NG TIN ??NG NH?P:';
PRINT '========================================';
SELECT 
    UserID,
    FullName,
    Email,
    PasswordHash,
    'Password ?? test: ' + PasswordHash as TestPassword
FROM Users
ORDER BY UserID;

PRINT '';
PRINT 'L?U �: Passwords trong seed data l� PLAIN TEXT!';
PRINT '?? ??ng nh?p, d�ng password ch�nh x�c l� gi� tr? trong c?t PasswordHash';
PRINT '========================================';
