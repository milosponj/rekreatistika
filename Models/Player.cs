using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                DateTime now = DateTime.Today;
                int age = now.Year - DateOfBirth.Value.Year;
                if (DateOfBirth > now.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
