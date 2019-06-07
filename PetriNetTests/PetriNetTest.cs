using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetriNets;

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
			PetriNet p = new PetriNet();
			bool actual = p.DrawCircle(Loc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void AddRectangle()
		{
			Point Loc = new Point(30, -30);
			bool expected = true;
			PetriNet p = new PetriNet();
			bool actual = p.DrawRectangle(Loc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void AddLine()
		{
			Point Loc = new Point(30, -30);
			Point Loc2 = new Point(60, -30);
			bool expected = true;
			PetriNet p = new PetriNet();
			bool actual2 = p.DrawCircle(Loc);
			actual2 = p.DrawRectangle(Loc2);
			bool actual = p.createNewLine(Loc);
			actual = p.createNewLine(Loc2);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DellCircle()
		{
			Point Loc = new Point(30, -30);
			bool expected = true;
			PetriNet p = new PetriNet();
			bool actual2 = p.DrawCircle(Loc);
			bool actual = p.deleteElement(Loc);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DellRectangle()
		{
			Point Loc = new Point(30, -30);
			bool expected = true;
			PetriNet p = new PetriNet();
			bool actual2 = p.DrawRectangle(Loc);
			bool actual = p.deleteElement(Loc);
			Assert.AreEqual(expected, actual);
		}
	}
}
