using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class CTransition
	{
		List<MTransition> _allTransition;
		public List<MTransition> allTransition
		{
			get
			{
				return _allTransition;
			}
		}

		public CTransition()
		{
			_allTransition = new List<MTransition>();
		}

		public bool addArc(MTransition mTransition, MArc arc)
		{
			if(arc.edge.isInEdge) { return mTransition.addArc(arc, mTransition.inPlaces); }
			else { return mTransition.addArc(arc, mTransition.outPlaces); }
		}

		public void removeLink(MArc arc, List<MArc> arcs)
		{
			arcs.Remove(arc);
		}
	}
}
