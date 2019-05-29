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
	public partial class PetriNet : Form
	{
		public List<IShape> Shapes { get; private set; }
		delegate void DrawPetriNetElement(Point location);
		bool isLinening = false;
		DrawPetriNetElement DrawElement;
		bool isDeliting = false;
		int unicalLabelId = 0;

		public PetriNet()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Shapes = new List<IShape>();
			DrawElement = DrawCircle;
		}
		IShape selectedShape;
		bool moving;
		Point previousPoint = Point.Empty;
		Line curLine;
		bool resize;
		IShape startShape;
		IShape endShape;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(e.Location)) { if (!(Shapes[i] is Line)) { selectedShape = Shapes[i]; break; } }
			if (selectedShape != null) { moving = true; previousPoint = e.Location; selectedShape.ChangeColor(); }
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (moving && DrawElement == null && !resize)
			{
				var d = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);
				selectedShape.Move(d);
				previousPoint = e.Location;
				this.Invalidate();
			}
			if (isLinening && resize)
			{
				var d = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);
				curLine.Resize(d);
				previousPoint = e.Location;
				this.Invalidate();
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (selectedShape != null) { selectedShape.ChangeColor(); }
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
			if (((MouseEventArgs)e).Button == MouseButtons.Right) { Shapes.Remove(curLine); }
			var location = ((MouseEventArgs)e).Location;
			if (isDeliting)
			{
				if (selectedShape != null)
				{
					selectedShape.ChangeColor();
					this.Refresh();
					MessageBox.Show("Вы действительно хотите удалить этот объект");
					selectedShape.ChangeColor();
				}
			}
			if (DrawElement != null)
			{
				DrawElement(location);
			}
			if(isLinening && resize)
			{
				curLine.points.Add(location);
				if (selectedShape != null)
				{
					resize = false;
					if (!isValidArch(startShape, selectedShape))
					{
						Shapes.Remove(curLine);
						(startShape as INotArch).getLines().Remove(curLine);
					}
					(selectedShape as INotArch).getLines().Add(curLine);
					curLine.points.RemoveAt(curLine.points.Count-1);
					curLine.points.Add(selectedShape.getCenter());
					selectedShape.ChangeColor();
					selectedShape = null;
				}
				this.Invalidate();
			}
			if (isLinening && !resize && selectedShape != null)
			{
				Line line = new Line();
				line.points.Add(selectedShape.getCenter());
				line.points.Add(new PointF(selectedShape.getCenter().X + 1, selectedShape.getCenter().Y + 1));
				Shapes.Insert(0, line);
				line.Draw(this.CreateGraphics());
				curLine = line;
				previousPoint = location;
				resize = true;
				startShape = selectedShape;
				selectedShape.ChangeColor();
				(startShape as INotArch).makeObjectStartInLine();
				(startShape as INotArch).getLines().Add(line);
				selectedShape = null;
			}
		}

		private bool isValidArch(IShape startShape, IShape endShape)
		{
			if (startShape is Circle) { if (endShape is TRectangle) { return true; } }
			if(startShape is TRectangle) { if (endShape is Circle) { return true; } }
			return false;
		}

		private void circle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawCircle;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		void DrawCircle(Point Location)
		{
			Circle circle = new Circle(Location, 20, "P"+unicalLabelId++);
			Shapes.Add(circle);
			Graphics g = this.CreateGraphics();
			circle.Draw(g);
			g.Dispose();
		}

		private void rectangle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawRectangle;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		void DrawRectangle(Point Location)
		{
			TRectangle rectangle = new TRectangle(Location, 50, 20, "t" + unicalLabelId++);
			Shapes.Add(rectangle);
			Graphics g = this.CreateGraphics();
			rectangle.Draw(g);
			g.Dispose();
		}

		private void line_Click(object sender, EventArgs e)
		{
			isDeliting = false;
			isLinening = true;
			DrawElement = null;
			this.Cursor = Cursors.Arrow;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = true;
			isLinening = false;
			this.Cursor = Cursors.Cross;
		}

		private void Form1_DoubleClick(object sender, EventArgs e)
		{
			var location = ((MouseEventArgs)e).Location;
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(location)) { selectedShape = Shapes[i]; break; }
			if (selectedShape != null)
			{
				EditLabelName editLabel = new EditLabelName();
				editLabel.ShowDialog();
				if(editLabel.currentName != "")
					selectedShape.RenameLabel(editLabel.currentName);
			}
			
		}

		private void line_MouseMove(object sender, MouseEventArgs e)
		{
		}
	}






	public class Circle : IShape, INotArch
	{
		private Color DefaultColor = ControlPaint.Light(Color.Red);
		private Color SelectedColor = Color.Green;
		private bool isStart = false;

		public Circle(Point _Center, int _Radious, string labelText)
		{
			FillColor = ControlPaint.Light(Color.Red);
			Center = _Center;
			Radious = _Radious;
			label = new Label(new Point(_Center.X + 15, _Center.Y+15),labelText);
			inLines = new List<Line>();
		}
		public List<Line> inLines { get; set; } 
		public Label label { get; set; }
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
			return result || label.HitTest(p);
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
			label.Draw(g);
		}
		public void Move(Point d)
		{
			Center = new Point(Center.X + d.X, Center.Y + d.Y);
			label.Move(d);
			foreach (Line line in inLines)
			{
				if (isStart) { line.ResizeStart(d); }
				else { line.Resize(d); }	
			}
		}

		public void RenameLabel(string newName)
		{
			this.label.Text = newName;
		}
		public void ChangeColor()
		{
			if (FillColor == DefaultColor)
				FillColor = SelectedColor;
			else
				FillColor = DefaultColor;
		}
		public PointF getCenter()
		{
			return (PointF)Center;
		}
		public List<Line> getLines()
		{
			return inLines;
		}
		public void makeObjectStartInLine()
		{
			isStart = true;
		}
	}

	public class TRectangle : IShape, INotArch
	{
		private Color DefaultColor = ControlPaint.Light(Color.DarkBlue);
		private Color SelectedColor = Color.Green;
		private bool isStart = false;

		public TRectangle(Point _Center, int _height, int _width, string labelText)
		{
			FillColor = ControlPaint.Light(Color.DarkBlue);
			Center = _Center;
			height = _height;
			width = _width;
			label = new Label(new Point(_Center.X+10, _Center.Y +10), labelText);
			inLines = new List<Line>();
		}
		public List<Line> inLines { get; set; }
		public Label label { get; set; }
		public Color FillColor { get; set; }
		public Point Center { get; set; }
		public int height { get; set; }
		public int width { get; set; }
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			var p = Center;
			p.Offset(-width/2, -height/2);
			path.AddRectangle(new Rectangle(p.X, p.Y, width, height));
			return path;
		}

		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
				result = path.IsVisible(p);
			return result || label.HitTest(p);
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
			label.Draw(g);
		}
		public void Move(Point d)
		{
			Center = new Point(Center.X + d.X, Center.Y + d.Y);
			label.Move(d);
			foreach (Line line in inLines)
			{
				if (isStart) { line.ResizeStart(d); }
				else { line.Resize(d); }
			}
		}
		public void RenameLabel(string newName)
		{
			this.label.Text = newName;
		}
		public void ChangeColor()
		{
			if (FillColor == DefaultColor)
				FillColor = SelectedColor;
			else
				FillColor = DefaultColor;
		}
		public PointF getCenter()
		{
			var p = Center;
			return (PointF)p;
		}

		public List<Line> getLines()
		{
			return inLines;
		}

		public void makeObjectStartInLine()
		{
			isStart = true;
		}
	}

	public class Line : IShape
	{
		public Line() { LineWidth = 2; LineColor = Color.Black; points = new List<PointF>(); }
		public int LineWidth { get; set; }
		public Color LineColor { get; set; }
		public List<PointF> points {get; set;}
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			path.AddCurve(points.ToArray());
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
		}

		public void Resize(Point d)
		{
			points[points.Count-1] = new PointF(points[points.Count-1].X + d.X, points[points.Count-1].Y + d.Y);
		}

		public void ResizeStart(Point d)
		{
			points[0] = new PointF(points[0].X + d.X, points[0].Y + d.Y);
		}

		public void RenameLabel(string newName)
		{
			
		}

		public void ChangeColor()
		{

		}

		public PointF getCenter()
		{
			return PointF.Empty;
		}

	}

	public class Label
	{
		public Label(Point location, string text) {Location = location;Text = text; FF = new FontFamily("Arial"); }
		public Point Location{ get; set; }
		public string Text { get; set; }
		private FontFamily FF;
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			path.AddString(Text, FF, (int)FontStyle.Regular, 12, Location, StringFormat.GenericDefault);
			return path;
		}
		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
			using (var pen = new Pen(Color.Black, 4))
				result = path.IsOutlineVisible(p, pen);
			return result;
		}
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var pen = new Pen(Color.Black, 2))
				g.DrawPath(pen, path);
		}
		public void Move(Point d)
		{
			Location = new Point(Location.X + d.X, Location.Y + d.Y);
		}
	}

	public interface IShape
	{
		GraphicsPath GetPath();
		bool HitTest(Point p);
		void Draw(Graphics g);
		void Move(Point d);
		void RenameLabel(string newName);
		void ChangeColor();
		PointF getCenter();
	}

	public interface INotArch
	{
		List<Line> getLines();
		void makeObjectStartInLine();
	}

	public abstract class Shape
	{
		public Color FillColor { get; set; }
		public Label label { get; set; }
	}
}
