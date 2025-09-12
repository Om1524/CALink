using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Common_Interface
{
    public interface IDefaultMetadataSeeder
    {
        Task SeedAsync(Guid companyId);
    }
}
