using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetriNetsClassLibrary;

namespace PetriNets
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		int c = 0;
		bool press = false;

		delegate void D(Point e);
		D d;

		PictureBox myPictureBox;
		private Point MouseDownLocation;
		PictureBox picName = new PictureBox();
		private void Form1_MouseClick(object sender, MouseEventArgs e)
		{
			if (d == Circle || d == myRectangle)
			{
				d(e.Location);
			}
		}
		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				press = true;
				myPictureBox = (PictureBox)sender;
				MouseDownLocation = e.Location;
				//label1.Text = myPictureBox.Name;
			}
		}
		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{

			if (e.Button == MouseButtons.Left && press)
			{
				myPictureBox = (PictureBox)sender;
				myPictureBox.Left = e.X + myPictureBox.Left - MouseDownLocation.X;
				myPictureBox.Top = e.Y + myPictureBox.Top - MouseDownLocation.Y;
			}
		}
		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			press = false;
		}
		private void circle_Click(object sender, EventArgs e)
		{
			d = new D(Circle);
		}
		private void Rectangle_Click(object sender, EventArgs e)// выбор прямоугольника
		{
			d = new D(myRectangle);
		}
		private void line_Click(object sender, EventArgs e)// выбор линии
		{
			//d = new D(Line);
		}
		public void Circle(Point e)// рисуем круг
		{
			myPictureBox = new PictureBox();
			//Pen pen = new Pen(Color.Red, 2);
			myPictureBox.BackColor = Color.Transparent;
			myPictureBox.Location = new Point(e.X - 25, e.Y - 25);
			myPictureBox.Size = new Size(60, 60);
			myPictureBox.Name = "s" + c.ToString();
			//label1.Text = myPictureBox.Name;
			myPictureBox.Image = Properties.Resources.circle;
			//picBoxs.Add(myPictureBox);
			Controls.Add(myPictureBox);

			MPlace mCircle = new MPlace(myPictureBox.Name);
			PetriNet.CPlace.allPlaces.Add(mCircle);
			myPictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
			myPictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
			myPictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
			c++;

		}

		public void myRectangle(Point e)// рисуем прямоугольник
		{
			myPictureBox = new PictureBox();
			//Pen pen = new Pen(Color.Red, 2);
			myPictureBox.Location = new Point(e.X - 25, e.Y - 25);
			myPictureBox.Size = new Size(60, 60);
			myPictureBox.Name = "r" + c.ToString();
			//label1.Text = myPictureBox.Name;
			myPictureBox.Image = Properties.Resources.rectangle;
			Controls.Add(myPictureBox);

			MTransition mRectangle = new MTransition(myPictureBox.Name);
			PetriNet.CTransition.allTransition.Add(mRectangle);
			myPictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
			myPictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
			myPictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
			c++;
		}

		private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
		{

		}
	}
}
