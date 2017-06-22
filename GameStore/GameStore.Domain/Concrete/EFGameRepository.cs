using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Concrete
{
	class EFGameRepository : IGameRepository
	{
		EFDbContext context = new EFDbContext();

		public IEnumerable<Game> Games
		{
			get
			{
				return context.Games;
			}
		}
	}
}
