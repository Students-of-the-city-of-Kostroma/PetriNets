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
		/// Тип рисуемой линии
		/// </summary>
		bool isInhibitor;
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
		public IShape selectedShape;
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
		public Line curLine;
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
						bool l = createNewLine(location);
					}
				}
			}
			else
			{
				hitTest(location);
				if(selectedShape != null && selectedShape is TRectangle)
				{
					//Выполнение сети Петри
					PetriNetsClassLibrary.PetriNet.CTransition.fireTransition((selectedShape as TRectangle).model);
					if (PetriNetsClassLibrary.PetriNet.FiredTransitions.Count > 0) back.Enabled = true;
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
			isInhibitor = false;
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
		public bool DrawCircle(Point Location)
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
		public bool DrawRectangle(Point Location)
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
		public bool createNewLine(Point location)
		{
			if (isInhibitor && selectedShape is TRectangle)
				return false;
			try
			{
				Line line = new Line(isInhibitor);
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
		public void hitTest(Point location)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(location)) { if (!(Shapes[i] is Line)) { selectedShape = Shapes[i]; break; } }
		}
		public void hitTestWithLine(Point location)
		{
			for (var i = Shapes.Count - 1; i >= 0; i--)
				if (Shapes[i].HitTest(location)) { { selectedShape = Shapes[i]; break; } }
		}

		/// <summary>
		/// Добавление новой точки в кривую и проверка валидности в случае если попали в объект
		/// </summary>
		/// <param name="location">точка для добавления</param>
		public bool resizingLine(Point location)
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
					return false;
				}
				curLine.points.RemoveAt(curLine.points.Count - 1);
				var p = (selectedShape as INotArch).getClosestEdge(location);
				if (!isInhibitor)
					curLine.points.Add(p);
				if (isInhibitor)
					curLine.points.Add(new PointF(p.X - 5, p.Y));
				(selectedShape as INotArch).getLines().Add(curLine);
				selectedShape.Unselect();
				selectedShape = null;
				curLine = null;
				return true;
			}
			this.Invalidate();
			return false;
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
		public bool deleteElement(Point location)
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
			while(PetriNetsClassLibrary.PetriNet.FiredTransitions.Count > 0)
			{
				PetriNetsClassLibrary.PetriNet.CTransition.unfireTransition();
			}
			this.Invalidate();
			Cursor = Cursors.Default;
		}

		/// <summary>
		/// Вернуться на шаг назад в выполнении сети Петри
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void back_Click(object sender, EventArgs e)
		{
			if (isEvaluating)
			{
				PetriNetsClassLibrary.PetriNet.CTransition.unfireTransition();
				if (PetriNetsClassLibrary.PetriNet.FiredTransitions.Count == 0) {
					back.Enabled = false; }
			}
			this.Invalidate();
		}

		private void inhibitorArc_Click(object sender, EventArgs e)
		{
			isInhibitor = true;
			isDeliting = false;
			isLinening = true;
			DrawElement = null;
			this.Cursor = Cursors.Arrow;
		}

		private void randomFire_Click(object sender, EventArgs e)
		{
			if (isEvaluating)
			{
				Random random = new Random();
				if (PetriNetsClassLibrary.PetriNet.CTransition.enabledTransition.Count > 0)
				{
					int i = random.Next(0, PetriNetsClassLibrary.PetriNet.CTransition.enabledTransition.Count);
					PetriNetsClassLibrary.PetriNet.CTransition.fireTransition(PetriNetsClassLibrary.PetriNet.CTransition.enabledTransition[i]);
					PetriNetsClassLibrary.PetriNet.setRule();
				}
				this.Invalidate();
			}
		}
	}

}
