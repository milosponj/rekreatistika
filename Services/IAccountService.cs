using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public interface IAccountService
    {
        IList<Player> GetUserPlayers(string identityName);
        IList<Team> GetUserTeams(string identityName);
        IList<League> GetUserLeagues(string identityName);
        IList<Match> GetUserMatches(string identityName);
    }
}
