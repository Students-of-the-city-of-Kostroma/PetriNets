using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	class Edge
	{
		public Tuple<MPlace, MTransition> edge
		{
			get; set;
		}
	}
}
