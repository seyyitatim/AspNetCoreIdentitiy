using Asp.NetCoreIdentity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class UserListDto
    {
        public IList<AppUser> Users { get; set; }
    }
}
