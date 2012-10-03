using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Collections;

using xTile;
using xTile.Dimensions;
using xTile.Display;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Devices.Sensors;
using xTile.Layers;
using xTile.ObjectModel;
namespace xTile_testProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite player;

        Map map;
        IDisplayDevice mapDisplayDevice;

        #endregion
        
        //Farseer
        World world;
        Body playerBody;
        Body boundary;

        Accelerometer accelerometer;
        Vector2 accelerometerInput;
        xTile.Dimensions.Rectangle viewport;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);
            map.LoadTileSheets(mapDisplayDevice);
           // map.GetLayer()
            viewport = new xTile.Dimensions.Rectangle(new Size(800, 480));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = Content.Load<Map>("Maps\\simpleTileMap"); // tIDE map load

            //foreach (Layer l in map.Layers)
            //{
            //    if (l.Properties["CollisionLayer"])
            //    {
            //        //Add to farser physics
            //        //l.
            //    }
            //}
            int fooProp = map.Layers[0].Properties["foo"];
            Layer col = map.GetLayer("CollisionLayer");
            //for(int i = 0; i < col.Tiles;)

            var viewport = this.GraphicsDevice.Viewport;
            Texture2D playerTexture = Content.Load<Texture2D>("player");//todo: add player texture
            player = new Sprite(playerTexture);

            world = new World(Vector2.Zero);//0 gravity world

            playerBody = BodyFactory.CreateBody(world);
            CircleShape playerShape = new CircleShape(ConvertUnits.ToSimUnits(playerTexture.Bounds.Width / 2), 5f);
            Fixture playerFixture = playerBody.CreateFixture(playerShape);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Position = ConvertUnits.ToSimUnits(new Vector2(450, 0));
            //playerBody.Restitution = 0.8f;

        
            accelerometerInput = new Vector2();
            if (accelerometer == null)
            {
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += (s, e) =>
                    {
                        accelerometerInput.X = e.SensorReading.Acceleration.X;
                        accelerometerInput.Y = -e.SensorReading.Acceleration.Y;
                    };
                accelerometer.Start();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if(input)

            map.Update(gameTime.ElapsedGameTime.Milliseconds);

            playerBody.ApplyForce(accelerometerInput);
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f/30f)));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            map.Draw(mapDisplayDevice, viewport);


            spriteBatch.Begin();

            Sprite.Draw(spriteBatch, player,
                ConvertUnits.ToDisplayUnits(playerBody.Position), 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool input { get; set; }
    }


}
