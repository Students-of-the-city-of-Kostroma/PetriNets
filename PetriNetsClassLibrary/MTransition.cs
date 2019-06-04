using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MTransition : Model
	{
		public List<MArc> inPlaces { get; } = new List<MArc>();
		public List<MArc> outPlaces { get; } = new List<MArc>();
		bool isEnable;

		public string label
		{
			get;set;
		}

		public MTransition(string _label)
		{
			isEnable = false;
			label = _label;
		}

		public bool addArc(MArc arc, List<MArc> places)
		{
			foreach(var place in places)
			{
				if(place.edge.edge.Item1.label == arc.edge.edge.Item1.label)
					if(place.edge.edge.Item2.label == arc.edge.edge.Item2.label) { return false; }
			}
			places.Add(arc);
			return true;
		}


	}
}
