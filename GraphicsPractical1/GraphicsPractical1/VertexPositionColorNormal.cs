using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
        }

        // This is C# implementation for HLSL (High Level Shading Language) to get POSITION0, COLOR0 and NORMAL0
        // like used in the struct.
        public static VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof (float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
        };

        // Initializes a vertex declarartion that consists a list of vertex elements, which describe the kind of data,
        // what it is used for, in what order they appear in the struct and the size of the elements.
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexPositionColorNormal.VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionColorNormal.VertexDeclaration; }
        }
    }
}
