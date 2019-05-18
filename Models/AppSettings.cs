using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class AppSettings
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string AdminUserEmail { get; set; }
        public string StorageFolder { get; set; }
    }
}
