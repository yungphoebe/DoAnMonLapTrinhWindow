# ?? H? TH?NG ?A NG�N NG? - MULTILANGUAGE SYSTEM

## ? T�nh n?ng ?� ho�n th�nh

### 1. **H? tr? 2 ng�n ng?**
- ???? **Ti?ng Vi?t** (M?c ??nh)
- ???? **English**

### 2. **C?u tr�c h? th?ng**
```
ToDoList.GUI/
??? Resources/
?   ??? Strings.cs          # Strings hi?n t?i (dynamic)
?   ??? StringsEN.cs        # English strings
?   ??? LanguageManager.cs  # Qu?n l� ng�n ng?
??? Forms/
?   ??? LanguageSettingsForm.cs  # Form settings
??? Properties/
    ??? Settings.settings    # User settings
    ??? Settings.Designer.cs # Generated code
```

### 3. **Strings ?� ???c ??nh ngh?a**
- ? Common (OK, Cancel, Save, Delete, Edit...)
- ? Menu & Navigation
- ? Task Management
- ? Project Management
- ? User Management
- ? Database Connection
- ? Validation Messages
- ? Confirmation Messages
- ? Success/Error Messages
- ? Date & Time
- ? Statistics
- ? Filter & Sort
- ...v� nhi?u h?n n?a

## ?? C�CH S? D?NG

### 1. **Trong Form/Control**

```csharp
using ToDoList.GUI.Resources;

// S? d?ng strings
this.Text = Strings.AppName;
btnSave.Text = Strings.Save;
btnCancel.Text = Strings.Cancel;
lblTitle.Text = Strings.CreateTask;

// Messages
MessageBox.Show(
    Strings.SuccessTaskCreated,
    Strings.Success,
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);

// Confirmations
var result = MessageBox.Show(
    Strings.ConfirmDelete,
    Strings.Confirm,
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);
```

### 2. **Thay ??i ng�n ng? programmatically**

```csharp
using ToDoList.GUI.Resources;

// ??i sang ti?ng Anh
LanguageManager.SetLanguage(SupportedLanguage.English);

// ??i sang ti?ng Vi?t
LanguageManager.SetLanguage(SupportedLanguage.Vietnamese);

// L?y ng�n ng? hi?n t?i
var currentLang = LanguageManager.CurrentLanguage;
```

### 3. **L?ng nghe s? ki?n thay ??i ng�n ng?**

```csharp
public class MyForm : Form
{
    public MyForm()
    {
        InitializeComponent();
        
        // Subscribe to language change event
        LanguageManager.LanguageChanged += OnLanguageChanged;
    }
    
    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        // Update UI with new language
        UpdateLanguage();
    }
    
    private void UpdateLanguage()
    {
        this.Text = Strings.AppName;
        btnSave.Text = Strings.Save;
        btnCancel.Text = Strings.Cancel;
        // ... update other controls
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Unsubscribe
        LanguageManager.LanguageChanged -= OnLanguageChanged;
        base.OnFormClosing(e);
    }
}
```

### 4. **M? form Language Settings**

```csharp
private void btnLanguage_Click(object sender, EventArgs e)
{
    using (var form = new LanguageSettingsForm())
    {
        if (form.ShowDialog() == DialogResult.OK)
        {
            // Language changed - refresh UI
            UpdateAllForms();
        }
    }
}
```

## ?? V� D? C? TH?

### Example 1: Task Form v?i ?a ng�n ng?

```csharp
using ToDoList.GUI.Resources;

public class TaskForm : Form
{
    private Label lblTitle;
    private Label lblDescription;
    private Label lblPriority;
    private Button btnSave;
    private Button btnCancel;
    
    public TaskForm()
    {
        InitializeComponent();
        ApplyLanguage();
        LanguageManager.LanguageChanged += (s, e) => ApplyLanguage();
    }
    
    private void ApplyLanguage()
    {
        // Form
        this.Text = Strings.CreateTask;
        
        // Labels
        lblTitle.Text = Strings.TaskTitle + ":";
        lblDescription.Text = Strings.TaskDescription + ":";
        lblPriority.Text = Strings.TaskPriority + ":";
        
        // Buttons
        btnSave.Text = Strings.Save;
        btnCancel.Text = Strings.Cancel;
        
        // Priority ComboBox
        cboPriority.Items.Clear();
        cboPriority.Items.Add(Strings.PriorityLow);
        cboPriority.Items.Add(Strings.PriorityMedium);
        cboPriority.Items.Add(Strings.PriorityHigh);
        cboPriority.Items.Add(Strings.PriorityUrgent);
    }
    
    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            MessageBox.Show(
                Strings.ValidationRequired,
                Strings.Warning,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }
        
        // Save task...
        MessageBox.Show(
            Strings.SuccessTaskCreated,
            Strings.Success,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
```

### Example 2: Database Test Form (?� �p d?ng)

File `DatabaseTestForm.cs` ?� ???c c?p nh?t ?? s? d?ng:
- `Strings.DbConnectionTitle`
- `Strings.DbServer`
- `Strings.DbDatabase`
- `Strings.DbConnecting`
- `Strings.DbConnectionSuccess`
- ... v� nhi?u h?n

## ?? TH�M NG�N NG? M?I

### B??c 1: T?o file StringsXX.cs

```csharp
// StringsFR.cs - V� d? ti?ng Ph�p
namespace ToDoList.GUI.Resources
{
    public static class StringsFR
    {
        public static string AppName = "Gestionnaire de T�ches";
        public static string OK = "OK";
        public static string Cancel = "Annuler";
        public static string Save = "Enregistrer";
        // ... th�m c�c strings kh�c
    }
}
```

### B??c 2: C?p nh?t LanguageManager.cs

```csharp
public enum SupportedLanguage
{
    Vietnamese,
    English,
    French  // Th�m ng�n ng? m?i
}

public static List<KeyValuePair<SupportedLanguage, string>> GetAvailableLanguages()
{
    return new List<KeyValuePair<SupportedLanguage, string>>
    {
        new(SupportedLanguage.Vietnamese, "Ti?ng Vi?t"),
        new(SupportedLanguage.English, "English"),
        new(SupportedLanguage.French, "Fran�ais")  // Th�m
    };
}

// Th�m case trong CopyStrings methods
```

## ?? C?U H�NH

### appsettings.json
```json
{
  "AppSettings": {
    "DefaultLanguage": "Vietnamese",
    "EnableDatabaseTestOnStartup": true,
    "UseMessageBoxForTest": true
  }
}
```

### Properties/Settings.settings
```xml
<Setting Name="Language" Type="System.String" Scope="User">
  <Value Profile="(Default)">Vietnamese</Value>
</Setting>
```

## ? L?I �CH

### 1. **D? b?o tr�**
- T?t c? strings t?p trung ? 1 n?i
- Kh�ng hard-code text trong code
- D? t�m v� s?a

### 2. **D? m? r?ng**
- Th�m ng�n ng? m?i d? d�ng
- Kh�ng c?n s?a code logic

### 3. **Tr?i nghi?m ng??i d�ng t?t**
- Ng??i d�ng ch?n ng�n ng? ?a th�ch
- To�n b? app t? ??ng c?p nh?t
- L?u preference

### 4. **Chu?n qu?c t?**
- S? d?ng CultureInfo ?�ng chu?n
- H? tr? format date/time theo region
- H? tr? currency format

## ?? DANH S�CH STRINGS AVAILABLE

### Common
- OK, Cancel, Yes, No
- Save, Delete, Edit, Add, Close
- Search, Refresh, Loading
- Success, Error, Warning, Information

### Task Management
- TaskTitle, TaskDescription, TaskPriority
- CreateTask, EditTask, DeleteTask
- MarkAsCompleted, CompleteTask

### Priority & Status
- PriorityLow, PriorityMedium, PriorityHigh
- StatusPending, StatusInProgress, StatusCompleted

### Validation
- ValidationRequired
- ValidationInvalidEmail
- ValidationPasswordTooShort

### Messages
- SuccessTaskCreated, SuccessTaskUpdated
- ErrorLoadData, ErrorSaveData
- ConfirmDelete, ConfirmDeleteTask

... v� 100+ strings kh�c

## ?? K?T LU?N

H? th?ng ?a ng�n ng? ?� ???c tri?n khai ho�n ch?nh v?i:
- ? 2 ng�n ng? (Ti?ng Vi?t & English)
- ? 100+ strings ???c ??nh ngh?a
- ? Form settings ?? ch?n ng�n ng?
- ? L?u preference ng??i d�ng
- ? Event system ?? update UI t? ??ng
- ? D? m? r?ng th�m ng�n ng? m?i

**S? d?ng `Strings.TenString` trong to�n b? ?ng d?ng ?? h? tr? ?a ng�n ng?!** ???
