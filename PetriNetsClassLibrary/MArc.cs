using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	public class MArc : Model
	{
		public MArc()
		{
			weight = 1;
		}

		public MArc(MPlace mPlace, MTransition mTransition, bool isIn)
		{
			edge = new Edge(mPlace, mTransition, isIn);
		}
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
