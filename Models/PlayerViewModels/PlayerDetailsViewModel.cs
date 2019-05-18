using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models.PlayerViewModels
{
    public class PlayerDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
		public bool IsAdmin { get; set; }

        public string AvatarPath => string.Format("{0}.jpg", Id);
        public string ThumbnailPath => string.Format("thumb_{0}.jpg", Id);

        public int? Age
        {
            get
            {
                if (DateOfBirth > new DateTime(1900,1,1))
                {
                    int yearsDifference = DateTime.Now.Year - DateOfBirth.Value.Year;
                    if (new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day) <
                        new DateTime(2000, DateOfBirth.Value.Month, DateOfBirth.Value.Day))
                        yearsDifference--;

                    return yearsDifference;
                }

                return null;

            }
        }
    }
}
