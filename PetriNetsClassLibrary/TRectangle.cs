using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetriNetsClassLibrary
{
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
        public bool delete(List<IShape> shapes)
        {
            try
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
                return true;
            }
            catch { return false; }
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
        public PointF getClosestEdge(PointF Location)
        {
            var bounds = GetPath().GetBounds();
            float d1, d2, d3, d4;
            double l1, l2, l3, l4;
            var leftClosest = FindClosestPointOnLine(new PointF((int)bounds.Left, Center.Y + height / 2 - 5),
                                              new PointF((int)bounds.Left, Center.Y - height / 2 + 5),
                                              Location, out d1, out l1);
            var rightClosest = FindClosestPointOnLine(new PointF((int)bounds.Right, Center.Y + height / 2 - 5),
                                              new PointF((int)bounds.Right, Center.Y - height / 2 + 5),
                                              Location, out d2, out l2);
            var bottomClosest = FindClosestPointOnLine(new PointF(Center.X + width / 2, (int)bounds.Bottom),
                                              new PointF(Center.X - width / 2, (int)bounds.Bottom),
                                              Location, out d3, out l3);
            var topClosest = FindClosestPointOnLine(new PointF(Center.X + width / 2, (int)bounds.Top),
                                              new PointF(Center.X - width / 2, (int)bounds.Top),
                                              Location, out d4, out l4);
            var minDistance = (new float[4] { d1, d2, d3, d4 }).Min();
            if (minDistance == d1)
                return leftClosest;
            else if (minDistance == d2)
                return rightClosest;
            else if (minDistance == d4)
                return topClosest;
            else
                return bottomClosest;
        }

        /// <summary>
        /// Поиск ближайшей точки на линии относительно точки вне линии
        /// </summary>
        /// <param name="edgeStart">Позиция начала точки</param>
        /// <param name="edgeEnd">Позиция конца точки</param>
        /// <param name="p">точка относительно которой происходит поиск</param>
        /// <param name="distance">Дистанция от точки до линии</param>
        /// <returns>ближайшая точка</returns>
        private PointF FindClosestPointOnLine(PointF edgeStart, PointF edgeEnd, PointF p, out float distance, out double lever)
        {
            float A = edgeStart.Y - edgeEnd.Y;
            float B = edgeEnd.X - edgeStart.X;
            float C = edgeStart.X * edgeEnd.Y - edgeEnd.X * edgeStart.Y;
            float ab = A * A + B * B;
            float cx = ((B * B * p.X - A * (B * p.Y + C)) / (float)ab);
            float cy = (float)(A * ((-B) * p.X + A * p.Y) - B * C) / (float)ab;
            lever = Math.Abs((A * (p.Y - edgeStart.Y) + B * (edgeStart.X - p.X)) / ab);
            distance = (float)Math.Abs(A * p.X + B * p.Y + C) / (float)Math.Sqrt(ab);
            if (cx > edgeStart.X || cx < edgeEnd.X)
                cx = (edgeStart.X + edgeEnd.X) / 2;
            if (cy > edgeStart.Y || cy < edgeEnd.Y)
                cy = (edgeStart.Y + edgeEnd.Y) / 2;
            return new PointF(cx, cy);
        }
    }

}
