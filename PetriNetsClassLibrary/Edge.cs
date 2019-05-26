using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class Edge
	{
		public Tuple<MPlace, MTransition> edge
		{
			get; set;
		}
	}
}
