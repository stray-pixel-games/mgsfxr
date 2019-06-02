using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;



namespace Game3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SFXR mysound = new SFXR();






        public Game1()
        {
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //test = System.IO.File.ReadAllBytes(path1);
            //string hex = BitConverter.ToString(test);
            //Console.WriteLine(hex);
            //test = new byte[30000];

            spriteBatch = new SpriteBatch(GraphicsDevice);
            mysound.coin();
            //mysound.laser();
            mysound.ResetSample(false);
            //playing_sample = true;

            //while (playing_sample) { 
            //SynthSample(8, 0, "yes");
            //Console.WriteLine(file_sampleswritten);

            //string hex = BitConverter.ToString(test);
            //Console.WriteLine(hex);
            //}
            

            //effect = new SoundEffect(test, 32000, AudioChannels.Mono);
            //effectinstance = effect.CreateInstance();
            //effectinstance.Pitch = 0;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
        int time;
        int timelimit = 60;

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (mysound.State() != SoundState.Playing)
            {
                time += 1;
                if (time > timelimit)
                {
                    time = 0;
                    Console.WriteLine("play");
                    mysound.coin();
                    mysound.Play();
                }

            }

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
