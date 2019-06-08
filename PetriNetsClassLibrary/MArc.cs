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
		/// <summary>
		/// Стандартный конструктор. По умолчанию вес арки - 1
		/// </summary>
		public MArc()
		{
			weight = 1;
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
		}

		/// <summary>
		/// Свойство обозначающие вес арки
		/// </summary>
		public uint weight
		{
			get;set;
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
