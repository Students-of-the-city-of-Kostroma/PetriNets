using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Общий класс для хранения контроллера CTransition и выполнения некоторых операция над сетью петри
/// </summary>
namespace PetriNetsClassLibrary
{
	public static class PetriNet
	{
		/// <summary>
		/// Контроллер CTransition
		/// </summary>
		public static CTransition CTransition = new CTransition();

		/// <summary>
		/// Стэк для хранения выстрельнутых переходов MTransition 
		/// </summary>
		public static Stack<MTransition> FiredTransitions = new Stack<MTransition>();

		/// <summary>
		/// Метод устанавливающий поле isEnable для всех переходов MTransition из CTransition.
		/// А так же записывающий включеные переход MTransition
		/// </summary>
		public static void setRule()
		{
			bool isEnable = true;
			foreach (var transition in CTransition.allTransition)
			{
				if (transition.outPlaces.Count != 0)
				{
					foreach (var arc in transition.inPlaces)
					{
						if(!arc.isInhibitor)
							if (arc.edge.edge.Item1.tokens < arc.weight)
							{ isEnable = false; CTransition.enabledTransition.Remove(transition); break; }
						if(arc.isInhibitor && arc.edge.edge.Item1.tokens > 0)
						{ isEnable = false; CTransition.enabledTransition.Remove(transition); break; }
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
