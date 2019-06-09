using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Вспомогательный класс ребро для отображения связи между объектами mPlace и mTransition
/// </summary>
namespace PetriNetsClassLibrary
{
	[Serializable]
	public class Edge
	{
		/// <summary>
		/// Конструктор ребра Edge
		/// </summary>
		/// <param name="mPlace">Место MPlace с одной стороны связи</param>
		/// <param name="mTransition">Переход MTransition с другой стороны связи</param>
		/// <param name="isInEdge">Если true - mPlace начало связи, а mTransition - конец связи</param>
		public Edge(MPlace mPlace, MTransition mTransition, bool isInEdge)
		{
			edge = new Tuple<MPlace, MTransition>(mPlace, mTransition);
			this.isInEdge = isInEdge;
		}
		/// <summary>
		/// Кортеж отображающий связь между MPlace и MTransition>
		/// </summary>
		public Tuple<MPlace, MTransition> edge
		{
			get; set;
		}
		/// <summary>
		/// Свойство для хранения статуса ребра Edge. true - входное в MTransition, false - выходное из MTransition
		/// </summary>
		public bool isInEdge { get; set; } = false;
	}
}
