using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public static class EmailTemplateHelper
    {
        public static string GetSuperAdminAccountCreationEmailBody(string firstName, string email, string temporaryPassword, string loginUrl)
        {
            return $@"
            <html>
            <body style='font-family:Segoe UI, Tahoma, Geneva, Verdana, sans-serif; color:#333; font-size:16px; line-height:1.6;'>
                <p>Dear {firstName},</p>

                <p>Welcome to <strong>CALink</strong>! Your <strong>Super Admin</strong> account has been successfully created.</p>

                <p><strong>Login Credentials:</strong></p>
                <ul style='list-style:none; padding-left:0;'>
                    <li><strong>Email:</strong> <code style='background:#f4f4f4; padding:2px 6px; border-radius:4px;'>{email}</code></li>
                    <li><strong>Temporary Password:</strong> <code style='background:#f4f4f4; padding:2px 6px; border-radius:4px;'>{temporaryPassword}</code></li>
                </ul>

                <p>As a Super Admin, you have full access to manage users, roles, permissions, and system settings within CALink.</p>

                <p>Please log in to your account and change your password immediately to ensure security.</p>
                 <p>You can continue logging in at 
                            <a href='{loginUrl}' style='color:#1a73e8; text-decoration:none;'>
                                CALink Login
                            </a> 
                            using your email.
                        </p>

                <p>Best regards,<br/>
                <strong>CALink Team</strong></p>
            </body>
            </html>";
        }

        public static string GetSuperAdminEmailChangedNotificationBody(string firstName, string newEmail, string loginUrl)
        {
            return $@"
            <html>
            <body style='font-family:Segoe UI, Tahoma, Geneva, Verdana, sans-serif; font-size:16px; line-height:1.6; color:#333;'>
                <p>Dear {firstName},</p>
                <p>This is to inform you that your email for your SecureGRC Super-Admin account has been changed to <strong>{newEmail}</strong>.</p>
                <p>You can continue logging in at: <a href='{loginUrl}'>SecureGRC Login</a></p>
                <p>If you did not make this change, please contact your system administrator immediately.</p>
                <br/>
                <p>Regards,<br/>SecureGRC Team</p>
            </body>
            </html>";
        }

    }
}
