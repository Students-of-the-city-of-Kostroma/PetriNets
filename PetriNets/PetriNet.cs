using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using PetriNetsClassLibrary;

namespace PetriNets
{
	/// <summary>
	/// View форма для редактирования и выполнения сетей Петри
	/// </summary>
	public partial class PetriNet : Form
	{
		/// <summary>
		/// Хранение фигур отображенных на форме
		/// </summary>
		public List<IShape> Shapes { get; private set; }
		/// <summary>
		/// Делегат для методов рисования Circle и TRectangle
		/// </summary>
		/// <param name="location">Центр-Позиция в которой происходит рисовани</param>
		/// <returns></returns>
		delegate bool DrawPetriNetElement(Point location);
		/// <summary>
		/// Линия выбрана в качестве элемента рисования?
		/// </summary>
		bool isLinening = false;
		DrawPetriNetElement DrawElement;
		/// <summary>
		/// Выбрано ли удаление в качестве текущего режима
		/// </summary>
		bool isDeliting = false;
		/// <summary>
		/// Уникальный ид для наименования лейблов по умолчанию
		/// </summary>
		int unicalLabelId = 0;
		/// <summary>
		/// Включен ли режим выполения сети
		/// </summary>
		bool isEvaluating = false;
		/// <summary>
		/// Хранени всех объектов типа TRectangle на View
		/// </summary>
		public List<TRectangle> rectangles { get; private set; }


		public PetriNet()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Shapes = new List<IShape>();
			rectangles = new List<TRectangle>();
			DrawElement = DrawCircle;
		}
		/// <summary>
		/// Фигура на форме выбранная в настоящий момент
		/// </summary>
		IShape selectedShape;
		/// <summary>
		/// Происходит ли передвижение объектов
		/// </summary>
		bool moving;
		/// <summary>
		/// Предыдущая позиция объекта сетей петри на View для реализации передвижения
		/// </summary>
		Point previousPoint = Point.Empty;
		/// <summary>
		/// Текущая линия Line
		/// </summary>
		Line curLine;
		/// <summary>
		/// Изменяется ли сейчас линия
		/// </summary>
		bool resize;
		/// <summary>
		/// Фигура Circle или TRectangle к которой зацепилась линия
		/// </summary>
		IShape startShape;
		/// <summary>
		/// Режим контекстного меню
		/// </summary>
		bool isContextMenu = false;


		#region onMouse methods
		/// <summary>
		/// Происходит хит-тест на попадание в фигура на форме.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!isEvaluating)
			{
				if (((MouseEventArgs)e).Button == MouseButtons.Right)
				{
					isContextMenu = true;
				}
				hitTest(e.Location);
				if (selectedShape != null && !isContextMenu) { moving = true; previousPoint = e.Location; selectedShape.Select(); }
				base.OnMouseDown(e);
			}
		}

		/// <summary>
		/// Если в OnMouseDown хит-тест успешен, то происходит передвижение объкта IShape
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!isEvaluating)
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
		}
		/// <summary>
		/// Значения булевых переменных нужных для OnMouseMove устанавливается в начальное положение
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!isEvaluating)
			{
				if (selectedShape != null && !isContextMenu) { selectedShape.Unselect(); }
				if ((moving && DrawElement == null && !isContextMenu)) { selectedShape = null; moving = false; }
				isContextMenu = false;
				this.Refresh();
				base.OnMouseUp(e);
			}
		}

		/// <summary>
		/// Событие переотрисовки объектов на форме, при ее обновлении
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if(isEvaluating)
			{
				foreach(var rectangle in rectangles) { rectangle.selectRectangle(g); }
			}
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (var shape in Shapes)
				shape.Draw(g);
			
		}
		#endregion

		#region click methods

		/// <summary>
		/// Выбор в качестве делегата DrawElement функции DrawCircle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
			if (!isEvaluating)
			{
				if (((MouseEventArgs)e).Button == MouseButtons.Right)
				{
					///Вызов контекстного меню
					ContextMenuT(location);
				}
				else
				{
					if (isDeliting)
					{
						//Удаление элемента по указаной позиции 
						deleteElement(location);
					}
					if (DrawElement != null)
					{
						//Рисование выбраного объекта через делегат
						DrawElement(location);
					}
					if (isLinening && resize)
					{
						//Resize существующей линии
						resizingLine(location);
					}
					if (isLinening && !resize && selectedShape != null)
					{
						//Создание новой линии
						createNewLine(location);
					}
				}
			}
			else
			{
				hitTest(location);
				if(selectedShape != null && selectedShape is TRectangle)
				{
					//Выполнение сети Петри
					PetriNetsClassLibrary.PetriNet.CTransition.exchangeTokens((selectedShape as TRectangle).model);
				}
				PetriNetsClassLibrary.PetriNet.setRule();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Вызов соответсвуещего объкту контекстного меню
		/// </summary>
		/// <param name="location">Положение вызова меню</param>
		private void ContextMenuT(Point location)
		{
			if (resize)
			{
				Shapes.Remove(curLine);
				resize = false;
			}
			if (!resize)
			{
				selectedShape = null;
				hitTestWithLine(location);
				Point p;
				if (selectedShape is Line) p = location;
				if (selectedShape != null)
				{
					p = selectedShape.getCenter();
					p.Offset(100, 100);
					var pos = selectedShape.getCenter();
					if (selectedShape is Circle) { cmPlace.Show(p, ToolStripDropDownDirection.Default); }
					if (selectedShape is TRectangle) { cmTransition.Show(p, ToolStripDropDownDirection.Default); }
					if (selectedShape is Line) { cmArc.Show(p, ToolStripDropDownDirection.Default); }
				}
			}
		}

		/// <summary>
		/// Выбор в качестве делегата DrawElement функции DrawRectangle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rectangle_Click(object sender, EventArgs e)
		{
			DrawElement = DrawRectangle;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// Выбор режима рисования линий
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void line_Click(object sender, EventArgs e)
		{
			isDeliting = false;
			isLinening = true;
			DrawElement = null;
			this.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// Выбор режима управления курсором
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = false;
			isLinening = false;
			this.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// Выбор режима удаления объекта
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			DrawElement = null;
			isDeliting = true;
			isLinening = false;
			this.Cursor = Cursors.Cross;
		}

		/// <summary>
		/// Открытие формы изменения названия объекта при двойном щелчке
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DoubleClick(object sender, EventArgs e)
		{
			if (!isEvaluating)
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
		}
		#endregion

		#region drawing methods
		/// <summary>
		/// Рисование объекта Circle по указаной позиции
		/// </summary>
		/// <param name="Location">Центр-Позиция для рисования</param>
		/// <returns></returns>
		bool DrawCircle(Point Location)
		{
			try
			{
				Circle circle = new Circle(Location, 20, "P" + unicalLabelId++);
				Shapes.Add(circle);
				Graphics g = this.CreateGraphics();
				circle.Draw(g);
				g.Dispose();
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Рисование объекта TRectangle по указаной позиции
		/// </summary>
		/// <param name="Location">Центр-Позиция для рисования</param>
		/// <returns></returns>
		bool DrawRectangle(Point Location)
		{
			try
			{
				TRectangle rectangle = new TRectangle(Location, 50, 20, "t" + unicalLabelId++);
				Shapes.Add(rectangle);
				rectangles.Add(rectangle);
				Graphics g = this.CreateGraphics();
				rectangle.Draw(g);
				g.Dispose();
				return true;
			}
			catch { return false; }
		}
		#endregion

		#region private methods 

		/// <summary>
		/// Создание новой линии в указанной позиции. И добавлении фигуры которая является началом линии
		/// </summary>
		/// <param name="location">Позиция</param>
		/// <returns></returns>
		private bool createNewLine(Point location)
		{
			try
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
				selectedShape.Unselect();
				curLine.startShape = selectedShape;
				(startShape as INotArch).getLines().Add(line);
				selectedShape = null;
				return true;
			}
			catch { return false; }
		}

		/// <summary>
		/// Проверяет валидна ли такая арка. Арка валидна если начало и конец разных типов {TRectangle, Circle}
		/// </summary>
		/// <param name="startShape">Фигура начала объекта Line </param>
		/// <param name="endShape">Фигура на конца объекта Line</param>
		/// <returns>Валидна ли арка</returns>
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

		/// <summary>
		/// Добавление новой точки в кривую и проверка валидности в случае если попали в объект
		/// </summary>
		/// <param name="location">точка для добавления</param>
		private void resizingLine(Point location)
		{
			if(selectedShape == null)
				curLine.points.Add(location);
			if (selectedShape != null)
			{

				resize = false;
				curLine.endShape = selectedShape;
				if (!isValidArch(startShape, selectedShape) || !addTransition(curLine))
				{
					Shapes.Remove(curLine);
					(startShape as INotArch).getLines().Remove(curLine);
					selectedShape.Unselect();
					selectedShape = null;
					curLine = null;
					return;
				}
				curLine.points.RemoveAt(curLine.points.Count - 1);
				curLine.points.Add((selectedShape as INotArch).getClosestEdge(location));
				(selectedShape as INotArch).getLines().Add(curLine);
				selectedShape.Unselect();
				selectedShape = null;
				curLine = null;
			}
			this.Invalidate();
		}

		/// <summary>
		/// Создать ребро между указанными в линии startShape и endShape
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Добавить переход MTransition в соответствии с объектами указанными в линии startShape и endShape
		/// </summary>
		/// <param name="line">объект Line откуда берутся объекты</param>
		/// <returns></returns>
		private bool addTransition(Line line)
		{
			MTransition mTransition;
			if (line.startShape is Circle) { mTransition = (line.endShape as TRectangle).model; }
			else { mTransition = (line.startShape as TRectangle).model; }
			line.mArc.edge = createEdge(line);
			return PetriNetsClassLibrary.PetriNet.CTransition.addArc(mTransition, line.mArc);
		}


		/// <summary>
		/// Удаление элемента находящегося в указаной локации
		/// </summary>
		/// <param name="location"></param>
		/// <returns>Удален ли элемент по указаной позиции</returns>
		private bool deleteElement(Point location)
		{
			hitTestWithLine(location);
			if (selectedShape != null)
			{
				this.Invalidate();
				if (DialogResult.Yes == MessageBox.Show("Вы хотите удалить этот объект?", "Удаление объекта", MessageBoxButtons.YesNo))
				{
					selectedShape.delete(Shapes);
					if(selectedShape is TRectangle) { rectangles.Remove(selectedShape as TRectangle); }
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Открытие формы-редактора наименования фигуры из контекстного меню
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Событие для пункта Удалить контекстного меню
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmDelete_Click(object sender, EventArgs e)
		{
			if (selectedShape != null)
			{
				selectedShape.delete(Shapes);
				this.Invalidate();
			}
		}

		/// <summary>
		/// Редактирование количества токенов выбранного объекта Circle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns>Успешно ли прошло редактирование</returns>
		private void editNumberOfTokens_Click(object sender, EventArgs e)
		{
			EditNumberOfSomething edit = new EditNumberOfSomething();
			edit.Text = "Изменить количество токенов";
			edit.label1.Text = "Новое кол-во токенов: ";
			edit.ShowDialog();
			if (edit.numberOfToken != -1)
			{
				(selectedShape as Circle).model.tokens = (uint)edit.numberOfToken;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Редактировать вес для выбранного объекта Line
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns>Успешность редактирования</returns>
		private void tsmEditWeight_Click(object sender, EventArgs e)
		{
			try
			{
				EditNumberOfSomething edit = new EditNumberOfSomething();
				edit.Text = "Изменить вес арки";
				edit.label1.Text = "Новый вес арки: ";
				edit.ShowDialog();
				if (edit.numberOfToken != -1)
					(selectedShape as Line).mArc.weight = (uint)edit.numberOfToken;
				this.Invalidate();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		/// <summary>
		/// Метод сохранения построенной Сети Петри
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Save_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "dat файл (*.dat)|*.dat|Все файлы (*.*)|*.*";
			saveFileDialog.DefaultExt = "*.dat";

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string FileName = saveFileDialog.FileName;
				BinaryFormatter binFormat = new BinaryFormatter();
				using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
				{
					binFormat.Serialize(fs, Shapes);
					fs.Close();
				}
				MessageBox.Show("Файл сохранен");
			}
		}

		/// <summary>
		/// Метод сохраненной сети Петри
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Open_Click(object sender, EventArgs e)
		{
			OpenFileDialog op = new OpenFileDialog();
			op.Filter = "dat файл (*.dat)|*.dat|Все файлы (*.*)|*.*";
			op.DefaultExt = "*.dat";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Shapes.Clear();

				string FileName = openFileDialog1.FileName;
				BinaryFormatter binFormat = new BinaryFormatter();
				using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
				{
					List<IShape> Fig = (List<IShape>)binFormat.Deserialize(fs);
					foreach (IShape f in Fig)
					{
						Shapes.Add(f);
					}
					fs.Close();
				}
			}
			Refresh();
		}

		/// <summary>
		/// Включение режима выполнения и
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
		{
			isEvaluating = true;
			PetriNetsClassLibrary.PetriNet.setRule();
			Cursor = Cursors.Cross;
			Graphics g = this.CreateGraphics();
			foreach(var rectangle in rectangles)
			{
				rectangle.selectRectangle(g);
			}
			g.Dispose();
			this.Invalidate();
		}

		/// <summary>
		/// Закончить выполнение сети
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void stop_Click(object sender, EventArgs e)
		{
			isEvaluating = false;
			this.Invalidate();
			Cursor = Cursors.Default;
		}

		private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
		{

		}
	}


	/// <summary>
	/// Класс для визуального представления MPlace
	/// </summary>
	[Serializable]
	public class Circle : IShape, INotArch
	{
		/// <summary>
		/// Шрифт для написания количества токенов в объекте MPlace связаного с этим объектом
		/// </summary>
		SerializableFont font = new SerializableFont("Arial", GraphicsUnit.Pixel, 12, FontStyle.Regular);

		/// <summary>
		/// Дефолтный цвет фигуры
		/// </summary>
		private Color DefaultColor = ControlPaint.Light(Color.Red);
		/// <summary>
		/// Цвет фигуры при выделении
		/// </summary>
		private Color SelectedColor = Color.Green;
		/// <summary>
		/// Радиус для рисования токена
		/// </summary>
		private int TokenRadius = 3;
		/// <summary>
		/// Смещения для рисования токенов внутри объкта Circe
		/// </summary>
		private Point[] offsets;

		/// <summary>
		/// Конструктор на основании Центра, Радиуса и наимнеования
		/// </summary>
		/// <param name="_Center">Место где будет центр рисуемого объекта</param>
		/// <param name="_Radious">Радиус объекта</param>
		/// <param name="labelText">наименование</param>
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
		/// <summary>
		/// Соответсвующая визуальному представлению модель MPlace
		/// </summary>
		public MPlace model;
		/// <summary>
		/// Линии принадлежащих объекту
		/// </summary>
		public List<Line> inLines { get; set; }
		/// <summary>
		/// Наименование
		/// </summary>
		public Label label { get; set; }
		/// <summary>
		/// Цвет заполнения фигуры
		/// </summary>
		public Color FillColor { get; set; }
		/// <summary>
		/// Центр фигуры
		/// </summary>
		public Point Center { get; set; }
		/// <summary>
		/// Радиус фигуры
		/// </summary>
		public int Radious { get; set; }
		#region select, draw, move
		/// <summary>
		/// Построение GraphicsPath для объекта Circle в виде эллипса 
		/// </summary>
		/// <returns>полученый GraphicsPath</returns>
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			var p = Center;
			p.Offset(-Radious, -Radious);
			path.AddEllipse(p.X, p.Y, 2 * Radious, 2 * Radious);
			return path;
		}
		/// <summary>
		/// Проверка попадания точки в построенный GraphicsPath для объекта Circle
		/// </summary>
		/// <param name="p">Точка для которой проверяется попадание</param>
		/// <returns></returns>
		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
				result = path.IsVisible(p);
			return result || label.HitTest(p);
		}
		/// <summary>
		/// Метод отрисовки объекта
		/// </summary>
		/// <param name="g">Место отрисовки</param>
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
			label.Draw(g);
			drawToken(g,(int)model.tokens);
		}
		/// <summary>
		/// Метод вызываемый при событии onMouseMove. 
		/// Устанавливает новые координаты центра, поэтому при onPaint перемещается
		/// </summary>
		/// <param name="d"></param>
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
		/// <summary>
		/// Переименовать объект Circle
		/// </summary>
		/// <param name="newName">новое наименование</param>
		public void RenameLabel(string newName)
		{
			this.label.Text = newName;
		}
		/// <summary>
		/// Поменять цвет на цвет выделения
		/// </summary>
		public void Select()
		{
			FillColor = SelectedColor;
		}
		/// <summary>
		/// Поменять цвет на дефолтный
		/// </summary>
		public void Unselect()
		{
			FillColor = DefaultColor;
		}
		/// <summary>
		/// Получить центр фигуры
		/// </summary>
		/// <returns>Центр фигуры</returns>
		public Point getCenter()
		{
			return Center;
		}
		/// <summary>
		/// Получить линии связанные с объектом
		/// </summary>
		/// <returns>Список связанных линий</returns>
		public List<Line> getLines() { return inLines; }
		/// <summary>
		/// Удалить фигуру из данного массива объектов типа IShape. А так же удаляет все зависимости из модели
		/// </summary>
		/// <param name="shapes">массива объектов типа IShape</param>
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
		/// <summary>
		/// Метод удаления зависимостей из данного листа объекто типа MArc
		/// </summary>
		/// <param name="arc">арка с объекто MPlace для которого удаляем зависимость</param>
		/// <param name="arcs">Список откдуа удаляем зависимость</param>
		/// <returns>Результат удаления</returns>
		private bool deleteReference(MArc arc, List<MArc> arcs)
		{
			return PetriNetsClassLibrary.PetriNet.CTransition.removeLink(arc, arcs);
		}
		#endregion

		#region token draw method
		void drawToken(Graphics g, int numberOfToken)
		{
			if (numberOfToken > 5)
			{
				var z = Center;
				z.Offset(-6, -6);
				g.DrawString(numberOfToken + "", font.ToFont(), Brushes.Black, z);
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

		/// <summary>
		/// Метод получения ближайшей точки границы объекта Circle относительно переденаной точки
		/// </summary>
		/// <param name="Location">Поиск ближайшей точки относительно этой точки</param>
		/// <returns>Ближайшая точка на границе</returns>
		public PointF getClosestEdge(Point Location)
		{
			float sqrt = (float)Math.Sqrt(Math.Pow(Location.X - Center.X, 2) + Math.Pow(Location.Y - Center.Y, 2));
			float cx = Center.X + Radious * (Location.X - Center.X) / sqrt;
			float cy = Center.Y + Radious * (Location.Y - Center.Y) / sqrt;
			return new PointF(cx, cy);
		}

	}


	/// <summary>
	/// Класс для визуального представления MTransition
	/// </summary>
	[Serializable]
	public class TRectangle : IShape, INotArch
	{
		/// <summary>
		/// Цвет рисования
		/// </summary>
		private Color DefaultColor = ControlPaint.Light(Color.DarkBlue);
		/// <summary>
		/// Цвет выделения
		/// </summary>
		private Color SelectedColor = Color.Green;

		/// <summary>
		/// Конструктор нового объекта TRectangle с известным местоположением и габаритами
		/// </summary>
		/// <param name="_Center">Центр-Позиция рисования</param>
		/// <param name="_height">Выоста</param>
		/// <param name="_width">Ширина</param>
		/// <param name="labelText">наименование</param>
		public TRectangle(Point _Center, int _height, int _width, string labelText)
		{
			FillColor = ControlPaint.Light(Color.DarkBlue);
			Center = _Center;
			height = _height;
			width = _width;
			label = new Label(new Point(_Center.X + 10, _Center.Y + 10), labelText);
			inLines = new List<Line>();
			model = new MTransition(labelText);
			PetriNetsClassLibrary.PetriNet.CTransition.allTransition.Add(model);
		}
		/// <summary>
		/// Модель MTransition соответствующая этому объекту TRectangle
		/// </summary>
		public MTransition model;
		/// <summary>
		/// Все зависимые от этого объекта линии
		/// </summary>
		public List<Line> inLines { get; set; }
		/// <summary>
		/// наименование
		/// </summary>
		public Label label { get; set; }
		/// <summary>
		/// Выбранный цвет заполнения
		/// </summary>
		public Color FillColor { get; set; }
		/// <summary>
		/// Центр фигуры
		/// </summary>
		public Point Center { get; set; }
		/// <summary>
		/// Высота фигуры
		/// </summary>
		public int height { get; set; }
		/// <summary>
		/// Ширина фигуры
		/// </summary>
		public int width { get; set; }
		/// <summary>
		/// Получить объект GraphicsPath формы Прямоугольника
		/// </summary>
		/// <returns>объект GraphicsPath формы Прямоугольника</returns>
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			var p = Center;
			p.Offset(-width / 2, -height / 2);
			path.AddRectangle(new Rectangle(p.X, p.Y, width, height));
			return path;
		}
		/// <summary>
		/// Проверка попадания точки в контур GraphicsPath
		/// </summary>
		/// <param name="p">проверяемая точка</param>
		/// <returns>Результат попадания</returns>
		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
				result = path.IsVisible(p);
			return result || label.HitTest(p);
		}
		/// <summary>
		/// Рисование контура GraphicsPath
		/// </summary>
		/// <param name="g">Место для рисования</param>
		public void Draw(Graphics g)
		{
			using (var path = GetPath())
			using (var brush = new SolidBrush(FillColor))
				g.FillPath(brush, path);
			//var bounds = GetPath().GetBounds();
			//g.DrawLine(Pens.Black, new Point((int)bounds.Left - 10, Center.Y + height / 2), new Point((int)bounds.Left - 10, Center.Y - height / 2));
			//var pos = getLineEquation(new Point((int)bounds.Left - 10, Center.Y + height / 2), new Point((int)bounds.Left - 10, Center.Y - height / 2), new Point(Center.X, Center.Y));
			//g.DrawEllipse(Pens.Black, pos.X,pos.Y, 5, 5);
			label.Draw(g);
		}
		/// <summary>
		/// Метод для имитации передвижения объекта. Замена старого местоположения на новое
		/// </summary>
		/// <param name="d">Смещение относительно старой позиции</param>
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
		/// <summary>
		/// Переименовать объект
		/// </summary>
		/// <param name="newName"></param>
		public void RenameLabel(string newName)
		{
			this.label.Text = newName;
		}
		/// <summary>
		/// Поменять цвет объекта на SelectedColor
		/// </summary>
		public void Select()
		{
			FillColor = SelectedColor;
		}
		/// <summary>
		/// Поменять цвет объекта на DefaultColor
		/// </summary>
		public void Unselect()
		{
			FillColor = DefaultColor;
		}
		/// <summary>
		/// Получить позицию центра фигуры
		/// </summary>
		/// <returns>позиция центра фигуры</returns>
		public Point getCenter() { return Center; }

		/// <summary>
		/// Получить все зависимые от объекта TRectangle линии
		/// </summary>
		/// <returns>все зависимые от объекта линии</returns>
		public List<Line> getLines() { return inLines; }

		/// <summary>
		/// Удалить из указаного листа IShape текущий объект типа TRectangle
		/// </summary>
		/// <param name="shapes"></param>
		public void delete(List<IShape> shapes)
		{
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
			shapes.Remove(this);
		}

		/// <summary>
		/// Метод выделения объекта Rectangle для режима выполнения сетей петри
		/// </summary>
		/// <param name="g"></param>
		public void selectRectangle(Graphics g)
		{
			Pen pen;
			if (this.model.isEnable)
				pen = new Pen(Brushes.Green, 10);
			else pen = new Pen(Brushes.Red, 10);
			using (var path = GetPath())
				g.DrawPath(pen, path);
		}

		/// <summary>
		/// Метод получения ближайшей точки границы объекта TRectangle относительно переденаной точки
		/// </summary>
		/// <param name="Location">Поиск относительно этой точки</param>
		/// <returns>Ближайшая точка на границе</returns>
		public PointF getClosestEdge(Point Location)
		{
			var bounds = GetPath().GetBounds();
			float d1, d2, d3, d4;
			var leftClosest = FindClosestPointOnLine(new PointF((int)bounds.Left, Center.Y + height / 2), 
											  new PointF((int)bounds.Left, Center.Y - height / 2), 
											  Location, out d1);
			var rightClosest = FindClosestPointOnLine(new PointF((int)bounds.Right, Center.Y + height / 2),
											  new PointF((int)bounds.Right, Center.Y - height / 2),
											  Location, out d2);
			var bottomClosest = FindClosestPointOnLine(new PointF(Center.X + width / 2, (int)bounds.Bottom),
											  new PointF(Center.X - width / 2, (int)bounds.Bottom),
											  Location, out d3);
			var topClosest = FindClosestPointOnLine(new PointF(Center.X + width / 2, (int)bounds.Top),
											  new PointF(Center.X - width / 2, (int)bounds.Top),
											  Location, out d4);
			var minDistance = (new double[4] { d1, d2, d3, d4 }).Min();
			if (minDistance == d1)
				return leftClosest;
			if (minDistance == d2)
				return rightClosest;
			if (minDistance == d3)
				return new PointF(bottomClosest.X, bounds.Bottom);
			if (minDistance == d4)
				return new PointF(topClosest.X, bounds.Top);
			return leftClosest;
		}
		
		/// <summary>
		/// Поиск ближайшей точки на линии относительно точки вне линии
		/// </summary>
		/// <param name="edgeStart">Позиция начала точки</param>
		/// <param name="edgeEnd">Позиция конца точки</param>
		/// <param name="p">точка относительно которой происходит поиск</param>
		/// <param name="distance">Дистанция от точки до линии</param>
		/// <returns>ближайшая точка</returns>
		private PointF FindClosestPointOnLine(PointF edgeStart, PointF edgeEnd, PointF p, out float distance)
		{
			var A = edgeStart.Y - edgeEnd.Y;
			var B = edgeEnd.X - edgeStart.X;
			var C = edgeStart.X * edgeEnd.Y - edgeEnd.X * edgeStart.Y;
			var ab = A * A + B * B;
			float cx = ((B * B * p.X - A * (B * p.Y + C)) / (float)ab);
			float cy = ((A*A * p.Y - B * (B * p.X + C)) / (float)ab);
			var levelRatio = (float)A * (float)(p.Y - edgeStart.Y) + (float)B * (float)(edgeStart.X - p.X) / (float)ab;
			distance = (float)Math.Abs(A*p.X + B*p.Y + C) / (float)ab;
			return new PointF(cx, cy);
		}
	}

	/// <summary>
	/// Класс для визуального представления MArc
	/// </summary>
	[Serializable]
	public class Line : IShape
	{
		/// <summary>
		/// Цвет рисования
		/// </summary>
		private Color DefaultColor = Color.Black;
		/// <summary>
		/// Цвет выделения
		/// </summary>
		private Color SelectedColor = Color.Green;
		/// <summary>
		/// Конструктор выставляющий по дефолту некоторые переменные
		/// </summary>
		public Line() { LineWidth = 2; LineColor = Color.Black; points = new List<PointF>(); mArc = new MArc(); }
		/// <summary>
		/// Шрифт для рисования некоторых подсказок на линии
		/// </summary>
		SerializableFont font = new SerializableFont("Arial", GraphicsUnit.Pixel, 12, FontStyle.Regular);
		/// <summary>
		/// Фигура к которой линия зацепилась в начале
		/// </summary>
		public IShape startShape { get; set; }
		/// <summary>
		/// Фигура к которой линия зацепилась в конце
		/// </summary>
		public IShape endShape { get; set; }
		/// <summary>
		/// Ширина рисуемой линии
		/// </summary>
		public int LineWidth { get; set; }
		/// <summary>
		/// Цвет линии
		/// </summary>
		public Color LineColor { get; set; }
		/// <summary>
		/// Масив точек для отрисовки кривой
		/// </summary>
		public List<PointF> points { get; set; }
		/// <summary>
		/// Соответсвующий объекту Line объект mArc 
		/// </summary>
		public MArc mArc;
		/// <summary>
		/// Соответсвующий объекту Line объект edge
		/// </summary>
		public Edge edge;
		/// <summary>
		/// Получить GraphicsPath для кривой
		/// </summary>
		/// <returns></returns>
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			path.AddCurve(points.ToArray());
			return path;
		}
		/// <summary>
		/// Проверка попадания точки в GraphicsPath построенный по points
		/// </summary>
		/// <param name="p">Проверяемая точка</param>
		/// <returns>Результат попадания</returns>
		public bool HitTest(Point p)
		{
			var result = false;
			using (var path = GetPath())
			using (var pen = new Pen(LineColor, LineWidth + 8))
				result = path.IsOutlineVisible(p, pen);
			return result;
		}
		/// <summary>
		/// Отрисовка кривой по точкам points
		/// </summary>
		/// <param name="g">место для отрисовки</param>
		public void Draw(Graphics g)
		{
			var pen = new Pen(LineColor, LineWidth + 2);
			pen.EndCap = LineCap.ArrowAnchor;
			using (var path = GetPath())
			{
				g.DrawPath(pen, path);
				//Подскаска о конце линии
				g.DrawString("in", font.ToFont(), Brushes.Brown, points[points.Count-1].X - 40 , points[points.Count - 1].Y - 20);
				//Подсказска о весе линии
				g.DrawString(String.Format("[{0}]", mArc.weight), font.ToFont(), Brushes.Brown, points[(points.Count - 1)/2].X, points[(points.Count - 1) / 2].Y + 20);
			}
		}
		public void Move(Point d)
		{
		}
		/// <summary>
		/// Изменение размера линии путем замены последнего поинта в массиве точек points для кривой
		/// </summary>
		/// <param name="d">отклонение от последней точки</param>
		public void Resize(Point d)
		{
			points[points.Count - 1] = new PointF(points[points.Count - 1].X + d.X, points[points.Count - 1].Y + d.Y);
		}
		/// <summary>
		/// Изменение размера линии путем замены первого поинта в массиве точек points для кривой
		/// </summary>
		/// <param name="d">отклонение от первой точки точки</param>
		public void ResizeStart(Point d)
		{
			points[0] = new PointF(points[0].X + d.X, points[0].Y + d.Y);
		}

		public void RenameLabel(string newName) { }

		/// <summary>
		/// Изменение цвета и ширины для режима Select
		/// </summary>
		public void Select() {
			LineColor = SelectedColor;
			LineWidth = 4;
		}
		/// <summary>
		/// Изменение цвета и ширины для режима Unselect
		/// </summary>
		public void Unselect()
		{
			LineColor = DefaultColor;
			LineWidth = 2;
		}
		/// <summary>
		/// Получить местоположение центра фигуры
		/// </summary>
		/// <returns>Point.Empty</returns>
		public Point getCenter() { return Point.Empty; }

		public PointF getEdge(Point curLocation) { return PointF.Empty; }

		/// <summary>
		/// Удалить объект Line со всеми его зависимостями из листа IShape
		/// </summary>
		/// <param name="shapes">лист откуда удаляем</param>
		public void delete(List<IShape> shapes)
		{
			Circle circle;
			TRectangle rectangle;
			List<MArc> lines; 
			if(this.startShape is Circle) { circle = (Circle)this.startShape; rectangle = (TRectangle)this.endShape; lines = ((TRectangle)this.endShape).model.inPlaces;  }
			else { circle = (Circle)this.endShape; rectangle = (TRectangle)this.startShape; lines = ((TRectangle)this.endShape).model.outPlaces;  }
			circle.inLines.Remove(this);
			PetriNetsClassLibrary.PetriNet.CTransition.removeLink(this.mArc, lines);
			rectangle.inLines.Remove(this);
			shapes.Remove(this);
		}


	}

	/// <summary>
	/// Класс для изображения наименования объекта
	/// </summary>
	[Serializable]
	public class Label
	{
		public Label(Point location, string text) { Location = location; Text = text;  }
		public Point Location { get; set; }
		public string Text { get; set; }
		[NonSerialized]
		private FontFamily FF;
		public GraphicsPath GetPath()
		{
			var path = new GraphicsPath();
			path.AddString(Text, FF = new FontFamily("Arial"), (int)FontStyle.Regular, 12, Location, StringFormat.GenericDefault);
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

	[Serializable]
	public class SerializableFont
	{
		public string FontFamily { get; set; }
		public GraphicsUnit GraphicsUnit { get; set; }
		public float Size { get; set; }
		public FontStyle Style { get; set; }

		/// <summary>
		/// Intended for xml serialization purposes only
		/// </summary>
		private SerializableFont() { }

		public SerializableFont(string fontFamily, GraphicsUnit graphicsUnit, float size, FontStyle style)
		{
			FontFamily = fontFamily;
			GraphicsUnit = graphicsUnit;
			Size = size;
			Style = style;
		}

		public static SerializableFont FromFont(string fontFamily, GraphicsUnit graphicsUnit, float size, FontStyle style)
		{
			return new SerializableFont(fontFamily, graphicsUnit, size, style);
		}

		public Font ToFont()
		{
			return new Font(FontFamily, Size, Style, GraphicsUnit);
		}
	}
	public interface IShape
	{
		GraphicsPath GetPath();
		bool HitTest(Point p);
		void Draw(Graphics g);
		void Move(Point d);
		void RenameLabel(string newName);
		void Select();
		void Unselect();
		Point getCenter();
		void delete(List<IShape> shapes);
	}

	public interface INotArch
	{
		List<Line> getLines();
		PointF getClosestEdge(Point Location);
	}

	public abstract class Shape
	{
		public Color FillColor { get; set; }
		public Label label { get; set; }
	}
}
