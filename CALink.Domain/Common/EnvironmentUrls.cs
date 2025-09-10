using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class EnvironmentUrls
    {
        public static string GetAdminLoginUrl(string connectionString)
        {
            return IsLocal(connectionString)
                ? "http://localhost:4200/login"
                : "";
        }

        private static bool IsLocal(string connectionString)
        {
            return connectionString.Contains("CALinkDb", StringComparison.OrdinalIgnoreCase);
        }
    }
}
