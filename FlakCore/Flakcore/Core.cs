using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Display;
using Microsoft.Xna.Framework.Content;
using Flakcore.Utils;
using Flakcore.Physics;
using Flakcore.Display.Level;
using System.Diagnostics;
using System.Threading;

namespace Flakcore
{
    public class Core
    {
        public List<Camera> Cameras { get; private set; }
        public CollisionSolver CollisionSolver { get; private set; }

        private QuadTree CollisionQuad;
        private Stopwatch Stopwatch;

        private State CurrentState;

        public Core(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content)
        {
            GameManager.Initialize(screenSize, graphics, content, this);

            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.ApplyChanges();

            this.Cameras = new List<Camera>();
            Camera camera = new Camera(0,0,(int)screenSize.X, (int)screenSize.Y);
            this.Cameras.Add(camera);
            GameManager.CurrentDrawCamera = camera;

            GameManager.WorldBounds = new Rectangle(0, 0, (int)Level.LEVEL_WIDTH * Level.ROOM_WIDTH * Level.BLOCK_WIDTH, (int)Level.LEVEL_HEIGHT * Level.ROOM_HEIGHT * Level.BLOCK_HEIGHT);

            SetupQuadTree();

            this.Stopwatch = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            if (this.CurrentState == null)
                return;

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            ResetCollisionQuadTree();
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Reset Collision Quad", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            this.CurrentState.Update(gameTime);
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Update", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            this.CollisionSolver.resolveCollisions(gameTime);
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Resolve Collisions", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            this.CurrentState.PostUpdate(gameTime);
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Post Update", this.Stopwatch.ElapsedMilliseconds + " ms");

            DebugInfo.AddDebugItem("Update calls", GameManager.UpdateCalls + " times");
            DebugInfo.AddDebugItem("Allocated memory", System.GC.GetTotalMemory(false) / 131072 + " mb");
            GameManager.UpdateCalls = 0;

            GameManager.Input.Update();

            foreach (Camera camera in Cameras)
                camera.update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (this.CurrentState == null)
                return;

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            GameManager.Graphics.GraphicsDevice.Clear(CurrentState.BackgroundColor);

            foreach (Camera camera in Cameras)
            {
                GameManager.CurrentDrawCamera = camera;
                GameManager.Graphics.GraphicsDevice.Viewport = camera.Viewport;

                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformMatrix());
                this.CurrentState.Draw(spriteBatch);
                spriteBatch.End();

#if(DEBUG)
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformMatrix());
                DrawCollisionQuad(spriteBatch);
                spriteBatch.End();

                this.Stopwatch.Stop();
                DebugInfo.AddDebugItem("Draw", this.Stopwatch.ElapsedMilliseconds + " ms");
                DebugInfo.AddDebugItem("FPS", "" + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds));

                spriteBatch.Begin();
                DebugInfo.Draw(spriteBatch);
                spriteBatch.End();
#endif

                Node.ResetDrawDepth();

            }
        }

        public void SwitchState(State state)
        {
            this.CurrentState = null;
            this.CurrentState = state;
        }

        public void SwitchState(Type state, StateTransition startTransition, StateTransition endTransition)
        {
            this.CurrentState = null;
            this.CurrentState = (State)Activator.CreateInstance(state);
        }

        private void ResetCollisionQuadTree()
        {
            this.CollisionQuad.clear();

            List<Node> children = this.CurrentState.GetAllCollidableChildren(new List<Node>());

            foreach (Node child in children)
            {
                this.CollisionQuad.insert(child);
            }
#if(DEBUG)
            DebugInfo.AddDebugItem("Collidable Children", children.Count + " children");
#endif
        }

        private void DrawCollisionQuad(SpriteBatch spriteBatch)
        {
            Texture2D blank = new Texture2D(GameManager.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[]{Color.White});

            List<BoundingRectangle> quads = CollisionQuad.getAllQuads(new List<BoundingRectangle>());

            foreach (BoundingRectangle quad in quads)
            {
                // left
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y), new Vector2(quad.X, quad.Y + quad.Height));
                // right
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X + quad.Width, quad.Y), new Vector2(quad.X + quad.Width, quad.Y + quad.Height));
                // top
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y), new Vector2(quad.X + quad.Width, quad.Y));
                // bottom
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y + quad.Height), new Vector2(quad.X + quad.Width, quad.Y + quad.Height));
            }
        }

        public void SetupQuadTree()
        {
            CollisionQuad = new QuadTree(0, new BoundingRectangle(0, 0, GameManager.WorldBounds.Width, GameManager.WorldBounds.Height));
            CollisionSolver = new CollisionSolver(CollisionQuad);
        }
    }
}
