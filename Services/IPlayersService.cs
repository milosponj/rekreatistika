using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekreatistika.Services
{
    public interface IPlayersService
    {
        int Save(string playerName, string CreatedBy, DateTime CreatedOn, string nickName = "", string Email="", DateTime? birthDate = null);
        Task<int> Update(int id, string playerName, string ModifiedBy, DateTime ModifiedOn, string nickName = "", string Email = "", DateTime? birthDate = null);
    }
}