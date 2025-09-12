using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Enums
{
    public class Enums
    {
        public enum UserStatus
        {
            Inactive = 0,
            Active = 1,            
            //Deleted = 3
        }

        public enum RoleStatus
        {
            Inactive = 0,
            Active = 1,
            //Deleted = 3
        }        
    }
}
