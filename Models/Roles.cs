using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Flags]
    public enum Roles
    {
        Read = 1 << 0,
        Write = 1 << 1,
        Delete = 1 << 2,
        Update = 1 << 3
    }
}
