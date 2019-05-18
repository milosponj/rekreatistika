using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Models.TeamViewModels
{
    public class AddPlayersViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<Player> PlayersToSelect { get; set; }
        public IList<Player> CurrentPlayers { get; set; }
        public string SelectedPlayerIds { get; set; }
    }
}
