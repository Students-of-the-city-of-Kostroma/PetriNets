using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MArc : Model
	{
		uint Weight;
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
			get { return Weight; }
			set
			{
				if (value < 0)
					throw new Exception("Можно вводить только положительные числа");
				Weight = value;
			}
		}

		public Edge edge
		{
			get; set;
		}
	}
}
