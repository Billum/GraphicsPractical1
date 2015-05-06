using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class Camera
    {
        // declares the two matrices for 3d to 2d transformation.
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        // Vectors for view matrix
        private Vector3 up;
        private Vector3 eye;
        private Vector3 focus;
        
        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            // Constructs vectors for view matrix.
            // Location of what is considered up.
            this.up = camUp;
            // Location of camera.
            this.eye = camEye;
            // Location at which the camera looks.
            this.focus = camFocus;

            // Calls update view matrix method.
            this.updateViewMatrix();
            // Projects view matrix on to projection matrix.
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);
        }

        private void updateViewMatrix()
        {
            // creates view matrix.
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
        }

        // Declaration of matrix and vector properties.
        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        public Vector3 Eye
        {
            get { return this.eye; }
            set { this.eye = value; this.updateViewMatrix(); }
        }

        public Vector3 Focus
        {
            get { return this.focus; }
            set { this.focus = value; this.updateViewMatrix(); }
        }
    }
}
