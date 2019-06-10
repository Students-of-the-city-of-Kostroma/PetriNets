using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
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
        bool delete(List<IShape> shapes);
    }
}
