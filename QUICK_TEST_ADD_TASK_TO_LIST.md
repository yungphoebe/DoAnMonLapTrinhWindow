# Quick Test Guide - Add Task to List

## ?? Quick Start

### Build & Run:
```bash
# Build
dotnet build

# Or press F5 in Visual Studio
```

## ? Test Checklist

### Test 1: Open Form from SearchForm
```
? Run application
? Click nút ?? (Search) ? góc trên ph?i
? SearchForm m?
? Click "? Add new task" trong Quick Actions
? AddTaskToListForm m? ra
? PASS: Form hi?n th? ?úng, tr?ng, rounded corners
```

### Test 2: Open Form from Form1
```
? On Form1 main screen
? Click nút "? Add Task" (màu xanh lá)
? AddTaskToListForm m? ra
? PASS: Form hi?n th? ?úng
```

### Test 3: Close Button
```
? Form ?ang m?
? Click nút ? ? góc trên ph?i
? Form ?óng l?i
? PASS: Close button works
```

### Test 4: Title Placeholder
```
? Form m?i m?, title textbox có text "Enter task title*"
? Click vào textbox
? PASS: Placeholder text bi?n m?t, màu ??i thành ?en
? Click ra ngoài (không nh?p gì)
? PASS: Placeholder text xu?t hi?n l?i
```

### Test 5: Title Input
```
? Click vào title textbox
? Gõ "Create homepage design"
? PASS: Text hi?n th? màu ?en
? Click ra ngoài
? PASS: Text v?n còn, không m?t
```

### Test 6: Project Dropdown
```
? Click vào dropdown "?? danh ngu (Copy)"
? PASS: Dropdown menu m? ra
? PASS: Hi?n th? danh sách projects v?i icon ??
? Click ch?n m?t project
? PASS: Project ???c ch?n, dropdown ?óng l?i
```

### Test 7: Validation - Empty Title
```
? ?? title tr?ng (ho?c placeholder)
? Ch?n m?t project
? Click "Save"
? PASS: Message box "Vui lòng nh?p tiêu ?? công vi?c!"
? PASS: Form v?n m?, không save
```

### Test 8: Validation - No Project
```
? Nh?p title: "Test task"
? Không ch?n project (ho?c ch?n "No lists available")
? Click "Save"
? PASS: Message box "Vui lòng ch?n danh sách!"
? PASS: Form v?n m?, không save
```

### Test 9: Successful Save
```
? Nh?p title: "Create mockup for homepage"
? Ch?n project: "Website Redesign"
? Click "Save"
? PASS: Message "Thêm công vi?c thành công!"
? PASS: Form t? ??ng ?óng
? PASS: Quay l?i SearchForm ho?c Form1
```

### Test 10: Database Verification
```
? Sau khi save thành công
? M? ProjectDetailsForm c?a project ?ã ch?n
? PASS: Task m?i xu?t hi?n trong danh sách
? PASS: Title ?úng
? PASS: Status = "Pending"
? PASS: Priority = "Medium"
```

### Test 11: Cancel Button
```
? Nh?p m?t s? thông tin vào form
? Click "Cancel"
? PASS: Form ?óng
? PASS: Không save gì vào database
```

### Test 12: UI Visual Check
```
? Form background màu tr?ng
? Form có rounded corners
? Title "Add task to list" c?n gi?a, font l?n
? TextBox có border light gray
? ComboBox có border light gray
? Cancel button: White bg, black border, rounded
? Save button: Green bg, rounded, no border
? PASS: All visual elements match design
```

### Test 13: Hover Effects
```
? Hover over Close button (?)
? PASS: Background chuy?n sang light gray
? Hover over Save button
? PASS: Background chuy?n sang lighter green
? Hover over Cancel button
? PASS: (Optional) Subtle change
```

### Test 14: No Projects Available
```
? Database không có projects nào (ho?c test case)
? M? AddTaskToListForm
? PASS: ComboBox hi?n th? "No lists available"
? PASS: Save button b? disabled (gray)
? Try to click Save
? PASS: Nothing happens (button disabled)
```

### Test 15: Multiple Saves
```
? Open form
? Nh?p task 1, save
? PASS: Success
? Open form again
? Nh?p task 2, save
? PASS: Success
? Verify: Both tasks in database
```

### Test 16: Long Task Title
```
? Nh?p title r?t dài (>100 characters)
? Click Save
? PASS: Task saved successfully
? PASS: Title không b? c?t trong database
```

### Test 17: Special Characters
```
? Nh?p title v?i special chars: "Task #1 - @Home (50%)"
? Click Save
? PASS: Task saved with all special chars
? Verify in database
? PASS: Characters intact
```

### Test 18: Form Position
```
? Open AddTaskToListForm
? PASS: Form xu?t hi?n ? gi?a màn hình
? PASS: Không b? che b?i c?a s? khác
```

### Test 19: Tab Navigation
```
? Open form
? Press Tab
? PASS: Focus di chuy?n: Title ? ComboBox ? Cancel ? Save
? Press Shift+Tab
? PASS: Focus di chuy?n ng??c l?i
```

### Test 20: Enter Key
```
? Nh?p title
? Press Enter
? PASS: Focus chuy?n sang ComboBox (not save yet)
? Ch?n project
? Press Enter
? PASS: Form save (if valid)
```

## ?? Test Results Summary

```
Total Tests: 20
Category:
- Form Opening: 2 tests
- UI Elements: 4 tests
- Input Validation: 3 tests
- Save Functionality: 4 tests
- Visual Design: 2 tests
- Edge Cases: 3 tests
- Keyboard Nav: 2 tests

Expected: All PASS ?
```

## ?? Common Issues & Fixes

### Issue 1: Form không m?
```
Symptom: Click button không có gì x?y ra
Check:
- AddTaskToListForm.cs t?n t?i?
- Using statement ?úng?
- Build successful?
Fix: Rebuild solution
```

### Issue 2: Projects không load
```
Symptom: ComboBox r?ng
Check:
- Database có projects?
- UserId ?úng?
- Projects không b? archived?
Fix: Add test data
```

### Issue 3: Rounded corners không hi?n th?
```
Symptom: Form góc vuông
Check:
- CreateRoundRectRgn có ???c import?
- Region assignment có l?i?
Fix: Check DllImport
```

### Issue 4: Save button không work
```
Symptom: Click Save không có gì x?y ra
Check:
- Event handler attached?
- Validation pass?
- Database connection OK?
Fix: Check error logs
```

### Issue 5: Form không center
```
Symptom: Form xu?t hi?n góc màn hình
Check:
- StartPosition = CenterScreen?
Fix: Set in InitializeComponent
```

## ? Performance Check

```
? Form m? trong < 500ms
? Project load trong < 1 second
? Save operation < 500ms
? Form close instant
? No memory leaks
? PASS: All performance OK
```

## ?? Final Checklist

```
? All 20 tests PASS
? No crashes
? No errors in console
? UI looks good
? Functionality works
? Data persists in database
? Parent forms refresh correctly
? Documentation complete
? Build successful
? Ready for production

STATUS: ? READY TO SHIP
```

## ?? Test Log Template

```
Date: ___________
Tester: ___________
Version: ___________

Test 1: PASS/FAIL - Notes: __________
Test 2: PASS/FAIL - Notes: __________
Test 3: PASS/FAIL - Notes: __________
...

Summary: 
- Total: 20
- Pass: ___
- Fail: ___
- Issues found: __________

Recommendation: SHIP / FIX ISSUES
```

---

## ? Quick Verification

### 30-Second Test:
```bash
1. Run app (F5)
2. Click "Add Task" button
3. Enter "Quick Test"
4. Select any project
5. Click "Save"
6. See success message?

YES ? ? ALL GOOD!
NO  ? Check error logs
```

### Visual Quick Check:
```
? White background? ?
? Rounded corners? ?
? Green Save button? ?
? Text readable? ?
? Layout clean? ?

All YES ? Perfect! ??
```

---

**Remember**: Test early, test often! ??

**Status**: All systems GO! ??
