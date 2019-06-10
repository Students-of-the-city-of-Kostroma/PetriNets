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
            drawToken(g, (int)model.tokens);
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
        public bool delete(List<IShape> shapes)
        {
            try
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
                    else if (line.endShape is TRectangle)
                    {
                        rectangle = line.endShape as TRectangle;
                        deleteReference(line.mArc, rectangle.model.outPlaces);
                        rectangle.inLines.Remove(line);
                    }
                    shapes.Remove(line);
                }
                return true;
            }
            catch { return false; }
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
            for (int i = 0; i < numberOfToken; i++)
            {
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
        public PointF getClosestEdge(PointF Location)
        {
            float sqrt = (float)Math.Sqrt(Math.Pow(Location.X - Center.X, 2) + Math.Pow(Location.Y - Center.Y, 2));
            float cx = Center.X + Radious * (Location.X - Center.X) / sqrt;
            float cy = Center.Y + Radious * (Location.Y - Center.Y) / sqrt;
            return new PointF(cx, cy);
        }

    }
}
