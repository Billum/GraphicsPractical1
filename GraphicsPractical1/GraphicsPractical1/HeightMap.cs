using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class HeightMap
    {
        private int width;
        private int height;
        private byte[,] heightData; // Holds 2D array with height of terrain on all x,y coords

        public HeightMap(Texture2D heightMap)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;
            this.loadHeightData(heightMap);
        }

        private void loadHeightData(Texture2D heightMap)
        {
            this.heightData = new byte[this.width, this.height];

            // Load color data from heightmap
            Color[] colorData = new Color[this.width * this.height];
            heightMap.GetData(colorData);

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    if (colorData[x + y * this.width].R > Terrain.WaterTreshold)
                        // Use the color data from heightmap.bmp to determine the height
                        // of the terrain on any point
                        this.heightData[x, y] = colorData[x + y * this.width].R;
                    else
                        // Flatten the terrain on the water treshold so it looks
                        // like a lake
                        this.heightData[x, y] = Terrain.WaterTreshold;
                }
            }
        }

        public byte this[int x, int y]
        {
            // Property
            get { return this.heightData[x, y]; }
            set { this.heightData[x, y] = value; }
        }

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
