using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class Terrain
    {
        // Declares and sets variables for epic scenery.
        public const int tresholdSnow = 23;
        public const int WaterTreshold = 50;
        public const int Vulcano = 15;
        public const int Lava = 7;
        public const int VulcanoTreshold = 18;
        public const int LavaTreshold = 18;

        // Declates dimentions of terrain.
        private int width;
        private int height;
        private short[] indices;
        private VertexPositionColorNormal[] vertices;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        // Terrain construct method.
        public Terrain(HeightMap heightMap, float heightScale, GraphicsDevice device)
        {
            // Sets width and height.
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            // Calls created functions to complete terrain model.
            this.vertices = this.loadVertices(heightMap, heightScale);
            this.setupIndices();
            this.calculateNormals();

            this.copyToBuffers(device);
        }

        // Turns height map into vertices and modifies the height for epic landscape.
        private VertexPositionColorNormal[] loadVertices(HeightMap heightMap, float heightScale)
        {
            VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[this.width * this.height];

            // Turns every height data point into vertex whith dubble for loop.
            for (int x = 0; x < this.width; ++x)
                for (int y = 0; y < this.height; ++y)
                {     
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;
                    vertices[v].Position = new Vector3(x, h, -y);
                    
                    if (heightMap[x, y] > (WaterTreshold + 1))
                        // Colors everything above the snow treshold white.
                        vertices[v].Color = (h > tresholdSnow)
                                                ? Color.White
                                                : Color.Green;
                    else
                        // Everything below the water treshold is colored blue.
                        vertices[v].Color = Color.Blue;
                }
            // Makes the brown part of the vulcano by choosing specific x and y values form the middle
            // of the map.
            for (int x = this.width/2 - Vulcano; x < this.width/ 2 + Vulcano; ++x)
                for (int y = this.height/ 2 - Vulcano; y < this.height/ 2 + Vulcano; ++y)
                {
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;
                    vertices[v].Position = new Vector3(x, h, -y);
                    // Everything above the vulcano treshold is colored brown, everything else green
                    vertices[v].Color = (h > VulcanoTreshold) ? Color.Brown : Color.Green;
                }

            // Makes the red part of the vulcano by choosing specific x and y values form the middle
            // of the map.
            for (int x = this.width / 2 - Lava; x < this.width / 2 + Lava; ++x)
                for (int y = this.height / 2 - Lava; y < this.height / 2 + Lava; ++y)
                {
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;
                    vertices[v].Position = new Vector3(x, h, -y);
                    // Everything above the vulcano treshold is colored red, everything else brown
                    vertices[v].Color = (h < LavaTreshold) ? Color.Red : Color.Brown;
                }

            return vertices;
        }

        // Creates triangles 
        private void setupIndices()
        {
            this.indices = new short[(this.width - 1) * (this.height - 1) * 6];
            int counter = 0;
            for (int x = 0; x < this.width - 1; x++)
                for (int y = 0; y < this.height - 1; y++)
                {
                    // sets vertices.
                    int lowerLeft = x + y * this.width;
                    int lowerRight = (x + 1) + y * this.width;
                    int topLeft = x + (y + 1) * this.width;
                    int topRight = (x + 1) + (y + 1) * this.width;

                    // Lower left triangle.
                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)lowerRight;
                    this.indices[counter++] = (short)lowerLeft;

                    // Top right triangle.
                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)topRight;
                    this.indices[counter++] = (short)lowerRight;
                }
        }

        // Fetches the vertex and index data from its own memory .
        public void Draw(GraphicsDevice device)
        {
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.vertices.Length, 0, this.indices.Length / 3);
        }

        // For each triangle is the normal being calculated.
        private void calculateNormals()
        {
             for (int i = 0; i < this.indices.Length / 3; i++)
             {
                 short i1 = this.indices[i * 3];
                 short i2 = this.indices[i * 3 + 1];
                 short i3 = this.indices[i * 3 + 2];

                 Vector3 side1 = this.vertices[i3].Position - this.vertices[i1].Position;
                 Vector3 side2 = this.vertices[i2].Position - this.vertices[i1].Position;
                 Vector3 normal = Vector3.Cross(side1, side2);
                 normal.Normalize();

                 this.vertices[i1].Normal += normal;
                 this.vertices[i2].Normal += normal;
                 this.vertices[i3].Normal += normal;
             }
             for (int i = 0; i < this.vertices.Length; i++)
                 this.vertices[i].Normal.Normalize();
        }

        // Fills the vertex- and indexbuffers.
        private void copyToBuffers(GraphicsDevice device)
        {
            // The first/third line allocates a piece of memory on the graphics card to store all vertices.
            // The second/fourth line copies the data from our local vertices array into the memory on our graphics card.
            this.vertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration, this.vertices.Length, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(this.vertices);
            this.indexBuffer = new IndexBuffer(device, typeof(short), this.indices.Length, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(this.indices);

            // Let's the graphics card know it should read from the buffers stored here above.
            device.Indices = this.indexBuffer;
            device.SetVertexBuffer(this.vertexBuffer);
        }

        // Property methods for width and height.
        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }
    }
}
