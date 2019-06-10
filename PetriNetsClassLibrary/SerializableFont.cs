using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetriNetsClassLibrary
{
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

}
