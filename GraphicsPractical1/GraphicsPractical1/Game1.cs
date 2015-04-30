using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GraphicsPractical1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private FrameRateCounter frameRateCounter;
        private BasicEffect effect;
        private Camera camera;
        private Terrain terrain;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            this.frameRateCounter = new FrameRateCounter(this);
            this.Components.Add(this.frameRateCounter);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.ApplyChanges();

            this.IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D map = Content.Load<Texture2D>("heightmap");
            this.terrain = new Terrain(new HeightMap(map), 0.2f, GraphicsDevice);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.effect = new BasicEffect(this.GraphicsDevice);
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = true;
            this.effect.DirectionalLight0.Enabled = true;
            this.effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            this.effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            this.effect.AmbientLightColor = new Vector3(0.3f);
            this.camera = new Camera(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;
            float deltaAngle = 0;

            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Left))
                deltaAngle += -3 * timeStep;
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngle += 3 * timeStep;
            if (deltaAngle != 0)
                this.camera.Eye = Vector3.Transform(this.camera.Eye, Matrix.CreateRotationY(deltaAngle));

            if (kbState.IsKeyDown(Keys.Down))
                this.camera.Eye = Vector3.Transform(this.camera.Eye, Matrix.CreateScale(1.001f));
            if (kbState.IsKeyDown(Keys.Up))
                this.camera.Eye = Vector3.Transform(this.camera.Eye, Matrix.CreateScale(0.999f));


            if (kbState.IsKeyDown(Keys.A))
                this.camera.Eye = Vector3.Add(this.camera.Eye, new Vector3(-0.1f, 0.0f, 0.0f));
            if (kbState.IsKeyDown(Keys.D))
                this.camera.Eye = Vector3.Add(this.camera.Eye, new Vector3(0.1f, 0.0f, 0.0f));
            if (kbState.IsKeyDown(Keys.W))
                this.camera.Eye = Vector3.Add(this.camera.Eye, new Vector3(0.0f, 0.1f, 0.0f));
            if (kbState.IsKeyDown(Keys.S))
                this.camera.Eye = Vector3.Add(this.camera.Eye, new Vector3(0.0f, -0.1f, 0.0f));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState();
            Matrix translation = Matrix.CreateTranslation(-0.5f * this.terrain.Width, 0, 0.5f * this.terrain.Width);

            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            this.GraphicsDevice.RasterizerState = rs;
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;
            this.effect.World = translation;

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.terrain.Draw(this.GraphicsDevice);                
            }

            base.Draw(gameTime);
        }
    }
}
