using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class CPlace
	{
		private List<MPlace> _allPlaces;
		public List<MPlace> allPlaces
		{
			get
			{
				return _allPlaces;
			}
		}

		public CPlace()
		{
			_allPlaces = new List<MPlace>();
		}
	}
}
