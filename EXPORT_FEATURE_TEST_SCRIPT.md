# ?? EXPORT FEATURE - QUICK TEST SCRIPT

## ? 5-Minute Test

### Test 1: PDF Export (Basic) ?
```
Time: ~1 minute

Steps:
1. Launch ToDoList App
2. Click "?? Báo cáo" button
3. Verify "?? Export" button visible (green, right side)
4. Click "?? Export"
5. Verify menu appears with 2 options
6. Click "?? Export to PDF"
7. In Save dialog:
   - Note auto-generated filename
   - Select Desktop
   - Click Save
8. Verify success message
9. Click "Yes" to open file
10. Verify PDF opens and contains:
    - Title: "BÁO CÁO TH?NG KÊ - TODOLIST"
    - Date stamp
    - Statistics table
    - Task details table

Expected Result: ? PDF created and displays correctly

If Failed: ? Check console errors, verify iText7 package
```

### Test 2: Excel Export (Basic) ?
```
Time: ~1 minute

Steps:
1. From Reports screen
2. Click "?? Export"
3. Click "?? Export to Excel"
4. Save to Desktop
5. Click "Yes" to open
6. Verify Excel has 2 sheets:
   - Sheet 1: "T?ng quan" (styled, merged cells)
   - Sheet 2: "Chi ti?t Tasks" (color-coded)
7. Check data accuracy
8. Close Excel

Expected Result: ? Excel created with proper formatting

If Failed: ? Check EPPlus license, verify data
```

### Test 3: File Naming ?
```
Time: ~30 seconds

Steps:
1. Export PDF again
2. Check filename format:
   BaoCao_ToDoList_YYYYMMDD_HHMMSS.pdf
3. Verify timestamp is current
4. Export Excel
5. Verify same naming pattern

Expected Result: ? Unique filenames with timestamps

If Failed: ? Check DateTime formatting in code
```

### Test 4: Error Handling ??
```
Time: ~1 minute

Test 4a: File Already Open
1. Export PDF
2. Keep PDF open
3. Export again to same location
   ? Should show error or auto-rename

Test 4b: No Permission
1. Try to save to C:\Windows\
   ? Should show permission error

Test 4c: Cancel Save
1. Click Export
2. Click Cancel in Save dialog
   ? Should exit gracefully

Expected Result: ? Graceful error handling

If Failed: ? Add better error messages
```

## ?? 10-Minute Comprehensive Test

### Test 5: Large Dataset ??
```
Time: ~3 minutes

Preparation:
1. Add 50+ tasks to database
   (Use Test Data button if available)

Steps:
1. Open Reports
2. Export PDF
3. Time the operation (should be < 10 seconds)
4. Verify all tasks included
5. Check file size (should be < 1 MB)
6. Export Excel
7. Check both sheets for all tasks
8. Verify formatting intact

Expected Result: ? Handles large data smoothly

If Failed: ? Consider async operations
```

### Test 6: Special Characters ??
```
Time: ~2 minutes

Preparation:
1. Create tasks with:
   - Vietnamese: "Làm vi?c"
   - Emoji: "Task ??"
   - Symbols: "Task #1 & @home"

Steps:
1. Export PDF
2. Verify characters display correctly
3. Export Excel
4. Check encoding in both sheets

Expected Result: ? UTF-8 encoding works

If Failed: ? Check font support, encoding settings
```

### Test 7: Empty Database ???
```
Time: ~2 minutes

Preparation:
1. Clear all tasks (backup first!)

Steps:
1. Export PDF
2. Verify table shows 0 values
3. No crash or error
4. Export Excel
5. Sheet 2 should be empty or show headers only

Expected Result: ? Handles empty data gracefully

If Failed: ? Add null checks
```

### Test 8: UI Responsiveness ???
```
Time: ~2 minutes

Steps:
1. Click Export button rapidly (5 times)
   ? Should not create multiple files
2. During export, try clicking other buttons
   ? Should wait or disable during export
3. Resize window during export
   ? Should not crash
4. Press Esc during Save dialog
   ? Should cancel properly

Expected Result: ? UI remains responsive

If Failed: ? Add loading indicators, disable buttons
```

## ?? Advanced Testing

### Test 9: Cross-Platform ??
```
Time: ~5 minutes (per platform)

Platforms to test:
- [ ] Windows 11
- [ ] Windows 10
- [ ] Windows Server (if applicable)

For each:
1. Export PDF - verify opens in default PDF reader
2. Export Excel - verify opens in Excel/Google Sheets
3. Check file permissions
4. Test network drive save

Expected Result: ? Works on all platforms

If Failed: ? Note platform-specific issues
```

### Test 10: Stress Test ??
```
Time: ~10 minutes

Steps:
1. Export 10 PDFs rapidly
2. Check memory usage (Task Manager)
3. Export 10 Excels
4. Monitor for memory leaks
5. Check disk space after exports
6. Delete all exported files
7. Restart app
8. Verify no lingering issues

Expected Result: ? No memory leaks, stable performance

If Failed: ? Profile memory usage, add cleanup
```

## ?? Test Results Template

```markdown
## Test Execution: [Date]

### Environment:
- OS: Windows 11 Pro
- .NET Version: 9.0
- Database: SQL Server 2022
- Tasks Count: 23

### Results:

| Test # | Test Name | Status | Duration | Notes |
|--------|-----------|--------|----------|-------|
| 1 | PDF Basic | ? | 2s | Perfect |
| 2 | Excel Basic | ? | 3s | Good formatting |
| 3 | File Naming | ? | 1s | Unique names |
| 4 | Error Handling | ?? | 5s | Need better messages |
| 5 | Large Dataset | ? | 8s | Acceptable speed |
| 6 | Special Chars | ? | 2s | UTF-8 works |
| 7 | Empty DB | ? | 2s | Handles gracefully |
| 8 | UI Responsive | ?? | 10s | Add loading indicator |
| 9 | Cross-Platform | ?? | N/A | Pending |
| 10 | Stress Test | ?? | N/A | Pending |

### Summary:
- **Passed**: 6/10
- **Warning**: 2/10
- **Pending**: 2/10
- **Failed**: 0/10

### Issues Found:
1. Need better error messages (Priority: Medium)
2. Add loading indicator (Priority: Low)

### Next Steps:
1. Fix warnings
2. Complete pending tests
3. Deploy to staging
```

## ?? Automated Test Script (Future)

```csharp
// TODO: Implement automated tests
[TestClass]
public class ExportFeatureTests
{
    [TestMethod]
    public void TestPDFExport_WithValidData_CreatesFile()
    {
        // Arrange
        var reportsForm = new ReportsForm(mockContext);
        
        // Act
        reportsForm.ExportToPDF();
        
        // Assert
        Assert.IsTrue(File.Exists(expectedFilePath));
    }
    
    [TestMethod]
    public void TestExcelExport_WithEmptyData_HandlesGracefully()
    {
        // Arrange
        var emptyContext = CreateEmptyContext();
        var reportsForm = new ReportsForm(emptyContext);
        
        // Act & Assert
        Assert.DoesNotThrow(() => reportsForm.ExportToExcel());
    }
}
```

## ?? Test Report Template

```
=== EXPORT FEATURE TEST REPORT ===

Date: [DD/MM/YYYY]
Tester: [Name]
Version: 1.0

SUMMARY:
? Passed: X tests
?? Warning: Y tests
? Failed: Z tests

DETAILS:
[List all test results]

RECOMMENDATIONS:
1. [Issue 1]
2. [Issue 2]

SIGN-OFF:
Tested by: _______________
Date: _______________
```

## ?? Quick Smoke Test (30 seconds)

```
THE FASTEST TEST:
1. Open app ? Click "Báo cáo" ? Click "Export" ? Choose PDF ? Save
2. Verify file created
3. Done!

If this works ? ? Basic functionality OK
If this fails ? ? Check setup, packages, permissions
```

---

**Use this script for**:
- ? Quick validation after code changes
- ? Before deployment
- ? User acceptance testing
- ? Regression testing

**Test frequency**:
- ?? Daily: Quick Smoke Test
- ?? Weekly: 5-Minute Test
- ?? Monthly: Comprehensive Test
- ?? Pre-release: Full Suite + Manual

---

**Last Updated**: 2024-10-22  
**Version**: 1.0
