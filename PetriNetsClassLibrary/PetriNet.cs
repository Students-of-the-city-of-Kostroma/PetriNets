using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public static class PetriNet
	{
		public static CArc cArc = new CArc();
		public static CTransition CTransition = new CTransition();
		public static CPlace CPlace = new CPlace();

		public static void setRule()
		{
			bool isEnable = true;
			foreach (var transition in CTransition.allTransition)
			{
				if (transition.outPlaces.Count != 0)
				{
					foreach (var arc in transition.inPlaces)
					{
						if (arc.edge.edge.Item1.tokens < arc.weight) { isEnable = false; CTransition.enabledTransition.Remove(transition); break; }
					}
					transition.isEnable = isEnable;
					if (isEnable)
						CTransition.enabledTransition.Add(transition);
					isEnable = true;
				}
				else transition.isEnable = false;
			}
		}

	}
}
