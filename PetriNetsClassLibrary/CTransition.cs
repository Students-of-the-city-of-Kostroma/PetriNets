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

		public List<MTransition> enabledTransition { get; set; } = new List<MTransition>();

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

		public void exchangeTokens(MTransition transition)
		{
			if (transition.isEnable)
			{
				foreach(var inArc in transition.inPlaces) {
					inArc.edge.edge.Item1.tokens = inArc.edge.edge.Item1.tokens - inArc.weight;
				}
				foreach(var outArc in transition.outPlaces)
				{
					outArc.edge.edge.Item1.tokens += outArc.weight;
				}
			}
		}

	}
}
