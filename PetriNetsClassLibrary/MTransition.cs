using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	class MTransition
	{
		List<MPlace> inPlaces;
		List<MPlace> outPlaces;
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
