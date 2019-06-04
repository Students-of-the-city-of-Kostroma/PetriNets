using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MPlace : Model
	{

		public MPlace() { }

		public uint tokens
		{
			get; set;
		}

		public string label
		{
			get; set;
		}

		public MPlace(string _label)
		{
			label = _label;
		}
	}
}
