using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekreatistika.Data;
using Rekreatistika.Models;

namespace Rekreatistika.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly ApplicationDbContext _context;

        public PlayersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Save(string playerName, string createdBy, DateTime createdOn, string nickName = "", string email = "", DateTime? birthDate = null)
        {
            var player = new Player()
            {
                Name = playerName,
                Nickname = nickName,
                Email = email,
                DateOfBirth = birthDate ?? DateTime.MinValue,
				CreatedBy = createdBy,
				CreatedOn = createdOn
			};

            _context.Add(player);
            _context.SaveChanges();

            return player.Id;
        }

        public async Task<int> Update(int id, string playerName, string modifiedBy, DateTime modifiedOn, string nickName = "", string email = "", DateTime? birthDate = null)
        {
            var player = _context.Players.Single(x => x.Id == id);
            player.Name = playerName;
            player.Nickname = nickName;
            player.DateOfBirth = birthDate;
            player.Email = email;
			player.ModifiedBy = modifiedBy;
			player.ModifiedOn = modifiedOn;

            _context.Update(player);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
