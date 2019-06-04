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
		bool isContextMenu = false;


		#region onMouse methods
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (((MouseEventArgs)e).Button == MouseButtons.Right)
			{
				isContextMenu = true;
			}
			hitTest(e.Location);
			if (selectedShape != null && !isContextMenu) { moving = true; previousPoint = e.Location; selectedShape.ChangeColor(); }
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
			if(selectedShape != null && !isContextMenu) { selectedShape.ChangeColor(); }
			if ((moving && DrawElement == null && !isContextMenu)) { selectedShape = null; moving = false;}
			isContextMenu = false;
			this.Refresh();
			base.OnMouseUp(e);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (var shape in Shapes)
				shape.Draw(e.Graphics);
		}
		#endregion

		#region click methods
		private void circle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawCircle;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		private void CanvasClick(object sender, EventArgs e)
		{
			var location = ((MouseEventArgs)e).Location;
			if (((MouseEventArgs)e).Button == MouseButtons.Right)
			{
				ContextMenuT(location);
			}
			else
			{
				if (isDeliting)
				{
					deleteElement(location);
				}
				if (DrawElement != null)
				{
					DrawElement(location);
				}
				if (isLinening && resize)
				{
					resizingLine(location);
				}
				if (isLinening && !resize && selectedShape != null)
				{
					createNewLine(location);
				}
			}
		}

		private void ContextMenuT(Point location)
		{
			selectedShape = null;
			hitTestWithLine(location);
			Shapes.Remove(curLine);
			var p = location;
			p.Offset(50, 50);
			if (selectedShape != null)
			{
				var pos = selectedShape.getCenter();
				if (selectedShape is Circle) { cmPlace.Show(p); }
				if (selectedShape is TRectangle) { cmTransition.Show(p); }
				if (selectedShape is Line) { cmArc.Show(p); }
			}
		}

		private void rectangle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawRectangle;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
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
			hitTest(location);
			if (selectedShape != null)
			{
				EditLabelName editLabel = new EditLabelName();
				editLabel.ShowDialog();
				if (editLabel.currentName != "")
					selectedShape.RenameLabel(editLabel.currentName);
				this.Invalidate();
			}

		}
		#endregion

		#region drawing methods
		void DrawCircle(Point Location)
		{
			Circle circle = new Circle(Location, 20, "P" + unicalLabelId++);
			Shapes.Add(circle);
			Graphics g = this.CreateGraphics();
			circle.Draw(g);
			g.Dispose();
		}

		void DrawRectangle(Point Location)
		{
			TRectangle rectangle = new TRectangle(Location, 50, 20, "t" + unicalLabelId++);
			Shapes.Add(rectangle);
			Graphics g = this.CreateGraphics();
			rectangle.Draw(g);
			g.Dispose();
		}
		#endregion

		#region private methods 
		private void createNewLine(Point location)
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
			curLine.startShape = selectedShape;
			(startShape as INotArch).getLines().Add(line);
			selectedShape = null;
		}

		private bool isValidArch(IShape startShape, IShape endShape)
		{
			if (startShape is Circle) { if (endShape is TRectangle) { return true; } }
			if (startShape is TRectangle) { if (endShape is Circle) { return true; } }
			return false;
		}
		private void hitTest(Point location)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(location)) { if (!(Shapes[i] is Line)) { selectedShape = Shapes[i]; break; } }
		}
		private void hitTestWithLine(Point location)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(location)) { { selectedShape = Shapes[i]; break; } }
		}

		private void resizingLine(Point location)
		{
			curLine.points.Add(location);
			if (selectedShape != null)
			{

				resize = false;
				curLine.endShape = selectedShape;
				if (!isValidArch(startShape, selectedShape) || !addTransition(curLine))
				{
					Shapes.Remove(curLine);
					(startShape as INotArch).getLines().Remove(curLine);
				}
				(selectedShape as INotArch).getLines().Add(curLine);
				selectedShape.ChangeColor();
				selectedShape = null;
				curLine = null;
			}
			this.Invalidate();
		}

		private Edge createEdge(Line line)
		{
			MTransition mTransition;
			MPlace mPlace;
			bool status;
			if (line.startShape is Circle) { mPlace = (line.startShape as Circle).model;
				mTransition = (line.endShape as TRectangle).model; status = true; }
			else { mPlace = (line.endShape as Circle).model;
				mTransition = (line.startShape as TRectangle).model; status = false; }
			return new Edge(mPlace, mTransition, status);
		}

		private bool addTransition(Line line)
		{
			MTransition mTransition;
			if (line.startShape is Circle) { mTransition = (line.endShape as TRectangle).model; }
			else { mTransition = (line.startShape as TRectangle).model; }
			line.mArc.edge = createEdge(line);
			return PetriNetsClassLibrary.PetriNet.CTransition.addArc(mTransition, line.mArc);
		}

		private void deleteElement(Point location)
		{
			hitTestWithLine(location);
			if (selectedShape != null)
			{
				this.Invalidate();
				if (DialogResult.Yes == MessageBox.Show("Вы хотите удалить этот объект?", "Удаление объекта", MessageBoxButtons.YesNo))
				{
					selectedShape.delete(Shapes);
				}
			}
		}


		private void tbEdit_Click(object sender, EventArgs e)
		{
			if (selectedShape != null)
			{
				EditLabelName editLabel = new EditLabelName();
				editLabel.ShowDialog();
				if (editLabel.currentName != "")
					selectedShape.RenameLabel(editLabel.currentName);
				this.Invalidate();
			}
		}

		private void tsmDelete_Click(object sender, EventArgs e)
		{
			selectedShape.delete(Shapes);
			this.Invalidate();
		}

		private void editNumberOfTokens_Click(object sender, EventArgs e)
		{
			EditNumberOfSomething edit = new EditNumberOfSomething();
			edit.Text = "Изменить количество токенов";
			edit.label1.Text = "Новое кол-во токенов: ";
			edit.ShowDialog();
			if (edit.numberOfToken != -1)
				(selectedShape as Circle).model.tokens = (uint)edit.numberOfToken;
			this.Invalidate();
		}

		private void tsmEditWeight_Click(object sender, EventArgs e)
		{
			EditNumberOfSomething edit = new EditNumberOfSomething();
			edit.Text = "Изменить вес арки";
			edit.label1.Text = "Новый вес арки: ";
			edit.ShowDialog();
			if (edit.numberOfToken != -1)
				(selectedShape as Line).mArc.weight = (uint)edit.numberOfToken;
			this.Invalidate();
		}
	#endregion
	}



	public class Circle : IShape, INotArch
	{
		Font font = new Font(new FontFamily("Arial"), 12, FontStyle.Bold, GraphicsUnit.Pixel);
		private Color DefaultColor = ControlPaint.Light(Color.Red);
		private Color SelectedColor = Color.Green;
		private int TokenRadius = 3;
		private Point[] offsets;

		public Circle(Point _Center, int _Radious, string labelText)
		{
			FillColor = ControlPaint.Light(Color.Red);
			Center = _Center;
			Radious = _Radious;
			label = new Label(new Point(_Center.X + 15, _Center.Y + 15), labelText);
			inLines = new List<Line>();
			model = new MPlace(labelText);
			fillOfssets();
		}
		public MPlace model;
		public List<Line> inLines { get; set; }
		public Label label { get; set; }
		public Color FillColor { get; set; }
		public Point Center { get; set; }
		public int Radious { get; set; }
		#region select, draw, move
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
			drawToken(g,(int)model.tokens);
		}
		public void Move(Point d)
		{
			Center = new Point(Center.X + d.X, Center.Y + d.Y);
			label.Move(d);
			foreach (Line line in inLines)
			{
				if (line.startShape == this) { line.ResizeStart(d); }
				else { line.Resize(d); }
			}
		}
		#endregion

		#region IShape other methods
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
		public Point getCenter()
		{
			return Center;
		}
		public List<Line> getLines() { return inLines; }
		public void delete(List<IShape> shapes)
		{
			shapes.Remove(this);
			foreach (var line in inLines)
			{
				TRectangle rectangle;
				if (line.startShape is TRectangle)
				{
					rectangle = line.startShape as TRectangle;
					deleteReference(line.mArc, rectangle.model.inPlaces);
					rectangle.inLines.Remove(line);
				}
				else if(line.endShape is TRectangle)
				{
					rectangle = line.endShape as TRectangle;
					deleteReference(line.mArc, rectangle.model.outPlaces);
					rectangle.inLines.Remove(line);
				}
				shapes.Remove(line);
			}
		}
		private void deleteReference(MArc arc, List<MArc> arcs)
		{
			PetriNetsClassLibrary.PetriNet.CTransition.removeLink(arc, arcs);
		}
		#endregion

		#region token draw method
		void drawToken(Graphics g, int numberOfToken)
		{
			if (numberOfToken > 5)
			{
				var z = Center;
				z.Offset(-6, -6);
				g.DrawString(numberOfToken + "", font, Brushes.Black, z);
				return;
			}
			for (int i = 0; i < numberOfToken; i++) {
				var p = Center;
				p.Offset(offsets[i]);
				g.FillEllipse(Brushes.Black, p.X - TokenRadius, p.Y - TokenRadius, TokenRadius * 2, TokenRadius * 2);
			}
		}


		void fillOfssets()
		{
			offsets = new Point[5] {new Point(0, 0),
									new Point(+Radious - 12, -Radious + 12),
									new Point(-Radious + 12, +Radious - 12),
									new Point(Radious - 12, Radious - 12),
									new Point(-Radious + 12, -Radious + 12)
		};
		}
		#endregion

	}

	public class TRectangle : IShape, INotArch
	{
		private Color DefaultColor = ControlPaint.Light(Color.DarkBlue);
		private Color SelectedColor = Color.Green;

		public TRectangle(Point _Center, int _height, int _width, string labelText)
		{
			FillColor = ControlPaint.Light(Color.DarkBlue);
			Center = _Center;
			height = _height;
			width = _width;
			label = new Label(new Point(_Center.X + 10, _Center.Y + 10), labelText);
			inLines = new List<Line>();
			model = new MTransition(labelText);
		}
		public MTransition model;
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
			p.Offset(-width / 2, -height / 2);
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
				if (line.startShape == this) { line.ResizeStart(d); }
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
		public Point getCenter() { return Center; }

		public List<Line> getLines() { return inLines; }

		public void delete(List<IShape> shapes)
		{
			shapes.Remove(this);
			foreach (var line in inLines)
			{
				Circle circle;
				if (line.startShape is Circle)
				{
					circle = line.startShape as Circle;
				}
				else
				{
					circle = line.endShape as Circle;
				}
				circle.inLines.Remove(line);
				shapes.Remove(line);
			}
		}
	}

	public class Line : IShape
	{
		public Line() { LineWidth = 2; LineColor = Color.Black; points = new List<PointF>(); mArc = new MArc(); }
		Font font = new Font(new FontFamily("Arial"),12,FontStyle.Regular,GraphicsUnit.Pixel);
		public IShape startShape { get; set; }
		public IShape endShape { get; set; }
		public int LineWidth { get; set; }
		public Color LineColor { get; set; }
		public List<PointF> points { get; set; }
		public MArc mArc;
		public Edge edge;
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
			using (var pen = new Pen(LineColor, LineWidth + 8))
				result = path.IsOutlineVisible(p, pen);
			return result;
		}
		public void Draw(Graphics g)
		{
			var pen = new Pen(LineColor, LineWidth + 2);
			pen.EndCap = LineCap.ArrowAnchor;
			using (var path = GetPath())
			{
				g.DrawPath(pen, path);
				g.DrawString("in", font, Brushes.Brown, points[points.Count-1].X - 40 , points[points.Count - 1].Y - 20);
				g.DrawString(String.Format("[{0}]", mArc.weight), font, Brushes.Brown, points[(points.Count - 1)/2].X, points[(points.Count - 1) / 2].Y + 20);
			}
		}
		public void Move(Point d)
		{
		}

		public void Resize(Point d)
		{
			points[points.Count - 1] = new PointF(points[points.Count - 1].X + d.X, points[points.Count - 1].Y + d.Y);
		}

		public void ResizeStart(Point d)
		{
			points[0] = new PointF(points[0].X + d.X, points[0].Y + d.Y);
		}

		public void RenameLabel(string newName) { }

		public void ChangeColor() { }

		public Point getCenter() { return Point.Empty; }

		public PointF getEdge(Point curLocation) { return PointF.Empty; }

		public void delete(List<IShape> shapes)
		{
			Circle circle;
			TRectangle rectangle;
			if(this.startShape is Circle) { circle = (Circle)this.startShape; rectangle = (TRectangle)this.endShape; }
			else { circle = (Circle)this.endShape; rectangle = (TRectangle)this.startShape; }
			circle.inLines.Remove(this);
			rectangle.inLines.Remove(this);
			shapes.Remove(this);
		}

	}

	public class Label
	{
		public Label(Point location, string text) { Location = location; Text = text; FF = new FontFamily("Arial"); }
		public Point Location { get; set; }
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
		Point getCenter();
		void delete(List<IShape> shapes);
	}

	public interface INotArch
	{
		List<Line> getLines();
	}

	public abstract class Shape
	{
		public Color FillColor { get; set; }
		public Label label { get; set; }
	}
}
