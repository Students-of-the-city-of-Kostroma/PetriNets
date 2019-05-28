using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetriNetsClassLibrary;

namespace PetriNets
{
	public partial class Form1 : Form
	{
		public List<IShape> Shapes { get; private set; }
		delegate void DrawPetriNetElement(Point location);
		DrawPetriNetElement DrawElement;
		bool isDeliting;

		public Form1()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Shapes = new List<IShape>();
			DrawElement = DrawCircle;
		}
		IShape selectedShape;
		bool moving;
		Point previousPoint = Point.Empty;
		Point LineStart = Point.Empty;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(e.Location)) { selectedShape = Shapes[i];; break; }
			if (selectedShape != null) { moving = true; previousPoint = e.Location; }
			if(selectedShape != null)
			{
				if (selectedShape is Circle)
					(selectedShape as Circle).FillColor = Color.Green;
				if(selectedShape is TRectangle)
					(selectedShape as TRectangle).FillColor = Color.Green;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (moving && DrawElement == null)
			{
				var d = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);
				selectedShape.Move(d);
				previousPoint = e.Location;
				this.Invalidate();
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (selectedShape != null)
			{
				if (selectedShape is Circle)
					(selectedShape as Circle).FillColor = ControlPaint.Light(Color.Red);
				if (selectedShape is TRectangle)
					(selectedShape as TRectangle).FillColor = ControlPaint.Light(Color.DarkBlue);
			}
			if (moving && DrawElement == null) { selectedShape = null; moving = false; }
			this.Refresh();
			base.OnMouseUp(e);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (var shape in Shapes)
				shape.Draw(e.Graphics);
		}



		private void CanvasClick(object sender, EventArgs e)
		{
			var location = ((MouseEventArgs)e).Location;
			if (isDeliting)
			{
				for (var i = Shapes.Count - 1; i >= 0; i--)
					if (Shapes[i].HitTest(location)) { selectedShape = Shapes[i]; ; break; }
				if (selectedShape != null)
				{
					if (selectedShape is Circle)
						(selectedShape as Circle).FillColor = Color.Green;
					if (selectedShape is TRectangle)
						(selectedShape as TRectangle).FillColor = Color.Green;
					this.Refresh();
					MessageBox.Show("Вы действительно хотите удалить этот объект");
					if (selectedShape is Circle)
						(selectedShape as Circle).FillColor = ControlPaint.Light(Color.Red);
					if (selectedShape is TRectangle)
						(selectedShape as TRectangle).FillColor = ControlPaint.Light(Color.DarkBlue);
				}
			}
			if (DrawElement != null)
				DrawElement(location);
		}

		private void circle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawCircle;
			isDeliting = false;
			this.Cursor = Cursors.Arrow;
		}

		void DrawCircle(Point Location)
		{
			Circle circle = new Circle(Location, 20);
			Shapes.Add(circle);
			Graphics g = this.CreateGraphics();
			circle.Draw(g);
			g.Dispose();
		}

		private void rectangle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawRectangle;
			isDeliting = false;
			this.Cursor = Cursors.Arrow;
		}

		void DrawRectangle(Point Location)
		{
			TRectangle rectangle = new TRectangle(Location, 50, 20);
			Shapes.Add(rectangle);
			Graphics g = this.CreateGraphics();
			rectangle.Draw(g);
			g.Dispose();
		}

		private void line_Click(object sender, EventArgs e)
		{
			isDeliting = false;
			this.Cursor = Cursors.Arrow;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = false;
			this.Cursor = Cursors.Arrow;
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = true;
			this.Cursor = Cursors.Cross;
		}
	}

	public class Circle : IShape
	{
		public Circle(Point _Center, int _Radious)
		{
			FillColor = ControlPaint.Light(Color.Red);
			Center = _Center;
			Radious = _Radious;
		}
		public Color FillColor { get; set; }
		public Point Center { get; set; }
		public int Radious { get; set; }
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			var p = Center;
			p.Offset(-Radious, -Radious);
			path.AddEllipse(p.X, p.Y, 2 * Radious, 2 * Radious);
			return path;
		}

		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
				result = path.IsVisible(p);
			return result;
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
		}
		public void Move(Point d)
		{
			Center = new Point(Center.X + d.X, Center.Y + d.Y);
		}
	}

	public class TRectangle : IShape
	{
		public TRectangle(Point _Center, int _height, int _width)
		{
			FillColor = ControlPaint.Light(Color.DarkBlue);
			Center = _Center;
			height = _height;
			width = _width;
		}
		public Color FillColor { get; set; }
		public Point Center { get; set; }
		public int height { get; set; }
		public int width { get; set; }
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			var p = Center;
			p.Offset(-width, -height);
			path.AddRectangle(new Rectangle(Center.X, Center.Y, width, height));
			return path;
		}

		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
				result = path.IsVisible(p);
			return result;
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
		}
		public void Move(Point d)
		{
			Center = new Point(Center.X + d.X, Center.Y + d.Y);
		}
	}

	public class Line : IShape
	{
		public Line() { LineWidth = 2; LineColor = Color.Black; }
		public int LineWidth { get; set; }
		public Color LineColor { get; set; }
		public Point Point1 { get; set; }
		public Point Point2 { get; set; }
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			path.AddLine(Point1, Point2);
			return path;
		}
		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
			using (var pen = new Pen(LineColor, LineWidth + 2))
				result = path.IsOutlineVisible(p, pen);
			return result;
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var pen = new Pen(LineColor, LineWidth))
				g.DrawPath(pen, path);
		}
		public void Move(Point d)
		{
			Point1 = new Point(Point1.X + d.X, Point1.Y + d.Y);
			Point2 = new Point(Point2.X + d.X, Point2.Y + d.Y);
		}
	}

	public interface IShape
	{
		GraphicsPath GetPath();
		bool HitTest(Point p);
		void Draw(Graphics g);
		void Move(Point d);
	}

	public abstract class Shape
	{
		public Color FillColor { get; set; }
	}
}
