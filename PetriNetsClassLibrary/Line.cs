using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
    /// <summary>
	/// Класс для визуального представления MArc
	/// </summary>
	[Serializable]
    public class Line : IShape
    {
        /// <summary>
        /// Определяет тип линии: обычная или inhibitor
        /// </summary>
        bool isInhibitor;
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
        public Line(bool isInhibitor)
        {
            LineWidth = 2; LineColor = Color.Black; points = new List<PointF>(); mArc = new MArc(isInhibitor);
            this.isInhibitor = isInhibitor;
        }

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
            if (!isInhibitor)
                pen.EndCap = LineCap.ArrowAnchor;
            if (isInhibitor)
                pen.EndCap = LineCap.RoundAnchor;
            using (var path = GetPath())
            {
                g.DrawPath(pen, path);
                g.DrawString("In", font.ToFont(), Brushes.Brown, points[points.Count - 1].X - 40, points[points.Count - 1].Y - 20);
                //Подсказска о весе линии
                g.DrawString(String.Format("[{0}]", mArc.weight), font.ToFont(), Brushes.Brown, points[(points.Count - 1) / 2].X, points[(points.Count - 1) / 2].Y + 20);
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
            PointF newPoint = new PointF(points[points.Count - 1].X + d.X, points[points.Count - 1].Y + d.Y);
            if (endShape == null)
                points[points.Count - 1] = newPoint;
            if (endShape != null)
            {
                var z = points[points.Count - 2];
                var p = (endShape as INotArch).getClosestEdge(new PointF(z.X, z.Y));
                points[points.Count - 1] = new PointF(p.X, p.Y);
            }
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
        public void Select()
        {
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
        public bool delete(List<IShape> shapes)
        {
            try
            {
                Circle circle;
                TRectangle rectangle;
                List<MArc> lines;
                if (this.startShape is Circle) { circle = (Circle)this.startShape; rectangle = (TRectangle)this.endShape; lines = ((TRectangle)this.endShape).model.inPlaces; }
                else { circle = (Circle)this.endShape; rectangle = (TRectangle)this.startShape; lines = ((TRectangle)this.startShape).model.outPlaces; }
                circle.inLines.Remove(this);
                PetriNetsClassLibrary.PetriNet.CTransition.removeLink(this.mArc, lines);
                rectangle.inLines.Remove(this);
                shapes.Remove(this);
                return true;
            }
            catch { return false; }
        }


    }
}
