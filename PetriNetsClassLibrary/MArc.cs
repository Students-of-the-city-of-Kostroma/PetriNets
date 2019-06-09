using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Класс-модель для представления объекта arc из теории сетей петри
/// </summary>
namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MArc : Model
	{

		public bool isInhibitor { get; private set; }

		/// <summary>
		/// Стандартный конструктор. По умолчанию вес арки - 1
		/// </summary>

		uint Weight;

		public MArc(bool isInhibitor)
		{
			Weight = 1;
			this.isInhibitor = isInhibitor;
		}

		/// <summary>
		/// Конструктор арки через объекты классов MPlace и MTransition, т.е с установленной связью
		/// </summary>
		/// <param name="mPlace">объект связи Edge типа MPlace</param>
		/// <param name="mTransition">объект связи Edge типа  MTransition</param>
		/// <param name="isIn">Входная или выходная арка</param>
		public MArc(MPlace mPlace, MTransition mTransition, bool isIn)
		{
			edge = new Edge(mPlace, mTransition, isIn);
			 Weight = 1;
		}

		/// <summary>
		/// Свойство обозначающие вес арки
		/// </summary>
		public uint weight
		{
			get { return Weight; }
			set
			{
				if (value <= 0)
					throw new Exception("Можно вводить только положительные числа");
				Weight = value;
			}
		}

		/// <summary>
		/// Ребро соответсвующее арке
		/// </summary>
		public Edge edge
		{
			get; set;
		}
	}
}
