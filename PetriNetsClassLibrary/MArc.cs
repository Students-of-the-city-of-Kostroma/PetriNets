using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	class MArc : Model
	{
		public uint weight
		{
			get;set;
		}

		public Edge edge
		{
			get; set;
		}
	}
}
