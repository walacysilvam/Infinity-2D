using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Infinity_2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Particle> particles;
        private Vector2 gravitationalCenter;
        private Texture2D particleTexture;
        private Random random;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            random = new Random();
        }

        protected override void Initialize()
        {
            base.Initialize();

            gravitationalCenter = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            particles = new List<Particle>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            particleTexture = new Texture2D(GraphicsDevice, 1, 1);
            particleTexture.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Vector2 position = new Vector2(random.Next(_graphics.PreferredBackBufferWidth), random.Next(_graphics.PreferredBackBufferHeight));
                Vector2 velocity = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
                particles.Add(new Particle(position, velocity));
            }

            foreach (var particle in particles)
            {
                particle.Update(gravitationalCenter);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            foreach (var particle in particles)
            {
                _spriteBatch.Draw(particleTexture, particle.Position, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Particle(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public void Update(Vector2 gravitationalCenter)
        {
            Vector2 direction = gravitationalCenter - Position;
            direction.Normalize();
            Velocity += direction * 0.1f; // Adjust the gravitational strength
            Position += Velocity;
        }
    }
}
