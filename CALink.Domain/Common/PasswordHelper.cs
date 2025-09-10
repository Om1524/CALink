using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public static class PasswordHelper
    {
        public static string PasswordGenerator(int length = 8)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(valid, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
