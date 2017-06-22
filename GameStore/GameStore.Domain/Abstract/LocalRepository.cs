using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
	public class LocalRepository : IGameRepository
	{
		public IEnumerable<Game> Games
		{
			get
			{
				return new[] {
					new Game { Name = "SimCity", Price = 1499 },
					new Game { Name = "TITANFALL", Price = 2299 },
					new Game { Name = "Battlefield 4", Price = 899.4M }
				};
			}
		}
	}
}
