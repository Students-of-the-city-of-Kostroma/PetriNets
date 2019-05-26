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

		public CTransition()
		{
			_allTransition = new List<MTransition>();
		}
	}
}
