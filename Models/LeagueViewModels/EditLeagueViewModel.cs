using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models
{
    public class EditLeagueViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime lige je obavezno.")]
        public string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        [Required(ErrorMessage = "Barem jedan admin je obavezan.")]
        public string LeagueAdminEmails { get; set; }
        public string CurrentUserEmail { get; set; }
    }
}
