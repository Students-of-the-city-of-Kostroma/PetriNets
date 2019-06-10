using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
    public interface INotArch
    {
        List<Line> getLines();
        PointF getClosestEdge(PointF Location);
    }
}
