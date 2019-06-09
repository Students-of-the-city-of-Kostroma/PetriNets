using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Класс контроллер для работы с объектами MTransition
/// </summary>
namespace PetriNetsClassLibrary
{
	public class CTransition
	{
		/// <summary>
		/// Свойство для хранения всех переходов
		/// </summary>
		public List<MTransition> allTransition { get; private set; } = new List<MTransition>();
		/// <summary>
		/// Свойство для хранения включенных переходов, т.е таких переходов для которых выполняется fire rule
		/// </summary>
		public List<MTransition> enabledTransition { get; private set; } = new List<MTransition>();


		/// <summary>
		/// Добавление арки в один из листов объекта mTransition: inPlaces, outPlaces,
		/// т.е добавление указаного объекта MArc в нужный список.
		/// </summary>
		/// <param name="mTransition">Объекта класса MTransition в свойства которого добавлется MArc </param>
		/// <param name="arc">Добавляемый объекты MArc в указаный список mTransition</param>
		/// <returns>Результат добавление объекта MArc в список mTransition(true-успех, false - неудача)</returns>
		public bool addArc(MTransition mTransition, MArc arc)
		{
			if(arc.edge.isInEdge) { return mTransition.addArc(arc, mTransition.inPlaces); }
			else { return mTransition.addArc(arc, mTransition.outPlaces); }
		}

		/// <summary>
		/// Удаляет из списка указанный объект MArc
		/// </summary>
		/// <param name="arc">Удаляемый объект</param>
		/// <param name="arcs">Список из которого надо удалить</param>
		/// <returns>Результат удаления из списка</returns>
		public bool removeLink(MArc arc, List<MArc> arcs)
		{
			try
			{
				arcs.Remove(arc);
				return true;
			}
			catch { return false; }
		}

		/// <summary>
		/// Выполняет перевод токенов из входящих MPlace в MTranstition в выходящие MPlace из MTranstition
		/// </summary>
		/// <param name="transition">переход для входящих и выходящих mPlace которого надо сделать перевод</param>
		/// <returns>Результат перевода токенов</returns>
		public bool fireTransition(MTransition transition)
		{
			try
			{
				if (transition.isEnable)
				{

					foreach (var inArc in transition.inPlaces)
					{
						inArc.edge.edge.Item1.tokens = inArc.edge.edge.Item1.tokens - inArc.weight;
					}
					foreach (var outArc in transition.outPlaces)
					{
						outArc.edge.edge.Item1.tokens += outArc.weight;
					}
					PetriNetsClassLibrary.PetriNet.FiredTransitions.Push(transition);
				}
			}
			catch
			{
				return false;
			}
			return transition.isEnable;
		}

		/// <summary>
		/// Сделать обратный переход для позиции лежащей на вершине стека FiredTransitions
		/// </summary>
		/// <returns>Результат обратного перехода</returns>
		public bool unfireTransition()
		{
			try {
				var transition = PetriNetsClassLibrary.PetriNet.FiredTransitions.Pop();

				transition.isEnable = true;
				foreach (var inArc in transition.inPlaces)
				{
					inArc.edge.edge.Item1.tokens += inArc.weight;
				}
				foreach (var outArc in transition.outPlaces)
				{
					outArc.edge.edge.Item1.tokens -= outArc.weight;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}
