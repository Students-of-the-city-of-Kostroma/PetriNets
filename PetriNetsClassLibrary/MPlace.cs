using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  Класс-модель для представления объекта place из теории сетей петри
/// </summary>
namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MPlace : Model
	{

		public MPlace() { }

		/// <summary>
		/// Количество токенов текущего объекта MPlace.
		/// </summary>
		public uint tokens
		{
			get; set;
		}

		/// <summary>
		/// Принадлежащее данному объекту строковое название
		/// </summary>
		public string label
		{
			get; set;
		}

		/// <summary>
		/// Конструктор через строковый литерал label
		/// </summary>
		/// <param name="_label">название для текущего объекта(place)</param>
		public MPlace(string _label)
		{
			label = _label;
		}
	}
}
