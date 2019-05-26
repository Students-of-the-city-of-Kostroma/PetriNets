using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	class MTransition : Model
	{
		List<MArc> inPlaces;
		List<MArc> outPlaces;
		bool isEnable;

		public string label
		{
			get;set;
		}

		MTransition(string _label)
		{
			isEnable = false;
			label = _label;
		}

	}
}
