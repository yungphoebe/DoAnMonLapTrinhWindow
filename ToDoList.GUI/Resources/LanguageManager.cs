using System.Globalization;

namespace ToDoList.GUI.Resources
{
    public enum SupportedLanguage
    {
        Vietnamese,
        English
    }

    public static class LanguageManager
    {
        private static SupportedLanguage _currentLanguage = SupportedLanguage.Vietnamese;
        
        public static event EventHandler? LanguageChanged;

        public static SupportedLanguage CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    ApplyLanguage();
                    LanguageChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static void Initialize()
        {
            // Load saved language preference from settings
            var savedLanguage = Properties.Settings.Default.Language;
            if (!string.IsNullOrEmpty(savedLanguage))
            {
                if (Enum.TryParse<SupportedLanguage>(savedLanguage, out var language))
                {
                    CurrentLanguage = language;
                }
            }
            else
            {
                // Default to Vietnamese
                CurrentLanguage = SupportedLanguage.Vietnamese;
            }
        }

        public static void SetLanguage(SupportedLanguage language)
        {
            CurrentLanguage = language;
            
            // Save preference
            Properties.Settings.Default.Language = language.ToString();
            Properties.Settings.Default.Save();
        }

        private static void ApplyLanguage()
        {
            var culture = CurrentLanguage == SupportedLanguage.Vietnamese
                ? new CultureInfo("vi-VN")
                : new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Copy strings from appropriate language class to Strings
            if (CurrentLanguage == SupportedLanguage.Vietnamese)
            {
                CopyStringsFromVietnamese();
            }
            else
            {
                CopyStringsFromEnglish();
            }
        }

        private static void CopyStringsFromVietnamese()
        {
            // Application
            Strings.AppName = "Qu?n Lý Công Vi?c";
            Strings.AppVersion = "Phiên b?n 1.0";
            
            // Common
            Strings.OK = "OK";
            Strings.Cancel = "H?y";
            Strings.Yes = "Có";
            Strings.No = "Không";
            Strings.Save = "L?u";
            Strings.Delete = "Xóa";
            Strings.Edit = "S?a";
            Strings.Add = "Thêm";
            Strings.Close = "?óng";
            Strings.Search = "Tìm ki?m";
            Strings.Refresh = "Làm m?i";
            Strings.Loading = "?ang t?i...";
            Strings.Success = "Thành công";
            Strings.Error = "L?i";
            Strings.Warning = "C?nh báo";
            Strings.Information = "Thông tin";
            Strings.Confirm = "Xác nh?n";
            
            // Menu & Navigation
            Strings.Home = "Trang ch?";
            Strings.Tasks = "Công vi?c";
            Strings.Projects = "D? án";
            Strings.Tags = "Nhãn";
            Strings.Settings = "Cài ??t";
            Strings.About = "Gi?i thi?u";
            Strings.Help = "Tr? giúp";
            Strings.Logout = "??ng xu?t";
            
            // Database Connection
            Strings.DbConnectionTitle = "Ki?m Tra K?t N?i Database";
            Strings.DbServer = "Máy ch?";
            Strings.DbDatabase = "C? s? d? li?u";
            Strings.DbAuthentication = "Xác th?c";
            Strings.DbConnecting = "?ang k?t n?i...";
            Strings.DbConnectionSuccess = "K?t n?i thành công!";
            Strings.DbConnectionFailed = "K?t n?i th?t b?i!";
            Strings.DbCheckingTables = "?ang ki?m tra các b?ng...";
            Strings.DbTotalRecords = "T?ng c?ng";
            Strings.DbRecords = "b?n ghi";
        }

        private static void CopyStringsFromEnglish()
        {
            // Application
            Strings.AppName = StringsEN.AppName;
            Strings.AppVersion = StringsEN.AppVersion;
            
            // Common
            Strings.OK = StringsEN.OK;
            Strings.Cancel = StringsEN.Cancel;
            Strings.Yes = StringsEN.Yes;
            Strings.No = StringsEN.No;
            Strings.Save = StringsEN.Save;
            Strings.Delete = StringsEN.Delete;
            Strings.Edit = StringsEN.Edit;
            Strings.Add = StringsEN.Add;
            Strings.Close = StringsEN.Close;
            Strings.Search = StringsEN.Search;
            Strings.Refresh = StringsEN.Refresh;
            Strings.Loading = StringsEN.Loading;
            Strings.Success = StringsEN.Success;
            Strings.Error = StringsEN.Error;
            Strings.Warning = StringsEN.Warning;
            Strings.Information = StringsEN.Information;
            Strings.Confirm = StringsEN.Confirm;
            
            // Menu & Navigation
            Strings.Home = StringsEN.Home;
            Strings.Tasks = StringsEN.Tasks;
            Strings.Projects = StringsEN.Projects;
            Strings.Tags = StringsEN.Tags;
            Strings.Settings = StringsEN.Settings;
            Strings.About = StringsEN.About;
            Strings.Help = StringsEN.Help;
            Strings.Logout = StringsEN.Logout;
            
            // Database Connection
            Strings.DbConnectionTitle = StringsEN.DbConnectionTitle;
            Strings.DbServer = StringsEN.DbServer;
            Strings.DbDatabase = StringsEN.DbDatabase;
            Strings.DbAuthentication = StringsEN.DbAuthentication;
            Strings.DbConnecting = StringsEN.DbConnecting;
            Strings.DbConnectionSuccess = StringsEN.DbConnectionSuccess;
            Strings.DbConnectionFailed = StringsEN.DbConnectionFailed;
            Strings.DbCheckingTables = StringsEN.DbCheckingTables;
            Strings.DbTotalRecords = StringsEN.DbTotalRecords;
            Strings.DbRecords = StringsEN.DbRecords;
        }

        public static string GetLanguageDisplayName(SupportedLanguage language)
        {
            return language switch
            {
                SupportedLanguage.Vietnamese => "Ti?ng Vi?t",
                SupportedLanguage.English => "English",
                _ => "Ti?ng Vi?t"
            };
        }

        public static List<KeyValuePair<SupportedLanguage, string>> GetAvailableLanguages()
        {
            return new List<KeyValuePair<SupportedLanguage, string>>
            {
                new KeyValuePair<SupportedLanguage, string>(SupportedLanguage.Vietnamese, "Ti?ng Vi?t"),
                new KeyValuePair<SupportedLanguage, string>(SupportedLanguage.English, "English")
            };
        }
    }
}
