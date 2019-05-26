using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class CArc
	{
		private List<MArc> _allArcs;
		public List<MArc> allArcs
		{
			get
			{
				return _allArcs;
			}
		}

		public CArc()
		{
			_allArcs = new List<MArc>();
		}
	}
}
