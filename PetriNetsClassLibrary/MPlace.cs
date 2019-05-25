using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	class MPlace
	{

		public uint tokens
		{
			get; set;
		}

		public string label
		{
			get; set;
		}

		MPlace(string _label)
		{
			label = _label;
		}
	}
}
