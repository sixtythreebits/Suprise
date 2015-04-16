using System.Configuration;
using System.Web;

namespace Core.Utilities
{
    public class AppSettings
    {
        public static string AdminUser
        {
            get { return ConfigurationManager.AppSettings["AdminUser"]; }            
        }

        public static string AdminPass
        {
            get { return ConfigurationManager.AppSettings["AdminPass"]; }
        }

        public static string LogFilePath
        {
            get { return string.Format("{0}\\App_data\\ErrorLog.txt", HttpRuntime.AppDomainAppPath); }
        }


        public static string UploadFilePhysicalPath
        {
            get { return string.Format("{0}\\uploads\\", HttpRuntime.AppDomainAppPath); }
        }

        public static string UploadFileHttpPath
        {
            get { return "/uploads/"; }
        }

        public static string WebsiteHttpFullPath
        {
            get { return ConfigurationManager.AppSettings["WebsiteHttpFullPath"]; }
        }
    }
}
