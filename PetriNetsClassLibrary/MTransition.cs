using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Класс-модель для представления объекта transition из теории сетей петри
/// </summary>
namespace PetriNetsClassLibrary
{
	[Serializable]
	public class MTransition : Model
	{
		/// <summary>
		/// Хранение входных арок MArc в текущий переход MTransition
		/// </summary>
		public List<MArc> inPlaces { get; } = new List<MArc>();
		/// <summary>
		/// Хранение выходных арок MArc в текущий переход MTransition
		/// </summary>
		public List<MArc> outPlaces { get; } = new List<MArc>();
		/// <summary>
		/// Это рабочий переход MTransition?
		/// </summary>
		public bool isEnable;

		/// <summary>
		/// Хранение наименование объекта MTransition
		/// </summary>
		public string label
		{
			get;set;
		}

		/// <summary>
		/// Конструктор через наименование объекта. По дефолту переход выключен
		/// </summary>
		/// <param name="_label">наименование объекта</param>
		public MTransition(string _label)
		{
			isEnable = false;
			label = _label;
		}

		/// <summary>
		/// Добавление арки MArc в соответсвующий лист Арок: inPlaces или outPlaces 
		/// </summary>
		/// <param name="arc">добавляемая арка MArc</param>
		/// <param name="places">лист арок в который происходит добавление</param>
		/// <returns>Успешно ли прошло добавление (Если уже есть такая арка, то false)</returns>
		public bool addArc(MArc arc, List<MArc> places)
		{
			foreach(var place in places)
			{
				if(place.edge.edge.Item1.label == arc.edge.edge.Item1.label)
					if(place.edge.edge.Item2.label == arc.edge.edge.Item2.label) { return false; }
			}
			places.Add(arc);
			return true;
		}

	}
}
