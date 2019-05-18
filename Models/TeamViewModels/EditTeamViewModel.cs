using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class EditTeamViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime tima je obavezan podatak.")]
        public string Name { get; set; }
        public IFormFile Logo { get; set; }
        public ICollection<Player> Players { get; set; }
        [Required]
        public string TeamAdminEmails { get; set; }
        public string CurrentUserEmail { get; set; }
        public string LogoFilename
        {
            get { return Id + ".jpg"; }
        }
    }
}
