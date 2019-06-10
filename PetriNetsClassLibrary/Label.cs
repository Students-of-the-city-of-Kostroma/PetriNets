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
    /// Класс для изображения наименования объекта
    /// </summary>
    [Serializable]
    public class Label
    {
        public Label(Point location, string text) { Location = location; Text = text; }
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
}
