using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetriNets;
using PetriNetsClassLibrary;

namespace PetriNetTests
{

	[TestClass]
	public class PetriNetTest
	{
		[TestMethod]
		public void AddCircle()
		{
			Point Loc = new Point(30, -30);
			bool expected = true;
			PetriNets.PetriNet p = new PetriNets.PetriNet();
			bool actual = p.DrawCircle(Loc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void AddRectangle()
		{
			Point Loc = new Point(30, -30);
			bool expected = true;
			PetriNets.PetriNet p = new PetriNets.PetriNet();
			bool actual = p.DrawRectangle(Loc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void AddLine()// не пашет
		{
			Point Loc = new Point(30, -30);
			Point Loc2 = new Point(60, -30);
			bool expected = true;
			PetriNets.PetriNet p = new PetriNets.PetriNet();
			p.DrawCircle(Loc);
			p.DrawRectangle(Loc2);
			p.hitTest(Loc);
			p.createNewLine(Loc);
			p.hitTest(Loc2);
			bool actual = p.resizingLine(Loc2);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void AddArc()
		{
			MTransition mTransition = new MTransition("Игорь");
			MPlace mPlace = new MPlace("Олег");
			MArc arc = new MArc(mPlace, mTransition, true);
			bool expected = true;
			CTransition c = new CTransition();
			bool actual = c.addArc(mTransition, arc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void RemoveLink()
		{
			MTransition mTransition = new MTransition("Игорь");
			MPlace mPlace = new MPlace("Олег");
			MArc arc = new MArc(mPlace, mTransition, true);
			bool expected = true;
			CTransition c = new CTransition();
			List<MArc> arcs = new List<MArc>();
			arcs.Add(arc);
			bool actual = c.removeLink(arc, arcs);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FireTransition()
		{
			MTransition mTransition = new MTransition("Игорь");
			mTransition.isEnable = true;
			bool expected = true;
			CTransition c = new CTransition();
			bool actual = c.fireTransition(mTransition);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UnfireTransition()
		{
			MTransition mTransition = new MTransition("Игорь");
			mTransition.isEnable = true;
			bool expected = true;
			CTransition c = new CTransition();
			c.fireTransition(mTransition);
			bool actual = c.unfireTransition();
			Assert.AreEqual(expected, actual);
		}
	}
}
