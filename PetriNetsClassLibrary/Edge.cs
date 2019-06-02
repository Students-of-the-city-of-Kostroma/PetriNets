using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class Edge
	{
		public Edge(MPlace mPlace, MTransition mTransition, bool isInEdge)
		{
			edge = new Tuple<MPlace, MTransition>(mPlace, mTransition);
			this.isInEdge = isInEdge;
		}
		public Tuple<MPlace, MTransition> edge
		{
			get; set;
		}
		public bool isInEdge { get; set; } = false;
	}
}
