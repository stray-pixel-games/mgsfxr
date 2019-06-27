using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

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

        private KeyboardState lastState;
        private KeyboardState thisState;
        private SFXGenerator sfxGen;





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
            /*
            mysound.ResetSample(false);
            byte[] buffer = mysound.laser();
            SoundEffect se = new SoundEffect(buffer, 44100, AudioChannels.Mono);
            while (true)
            {
                se.Play();
            }
            */
            //mysound.laser();
            sfxGen = new SFXGenerator();
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
        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastState = thisState;
            thisState = Keyboard.GetState();
            if (lastState.IsKeyDown(Keys.D1) == false && thisState.IsKeyDown(Keys.D1) == true)
            {
                SoundParams parms = sfxGen.RandomCoin();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }
            if (lastState.IsKeyDown(Keys.D2) == false && thisState.IsKeyDown(Keys.D2) == true)
            {
                SoundParams parms = sfxGen.Laser();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }
            if (lastState.IsKeyDown(Keys.D3) == false && thisState.IsKeyDown(Keys.D3) == true)
            {
                SoundParams parms = sfxGen.Explosion();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }

            if (lastState.IsKeyDown(Keys.D4) == false && thisState.IsKeyDown(Keys.D4) == true)
            {
                SoundParams parms = sfxGen.Powerup();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }

            if (lastState.IsKeyDown(Keys.D5) == false && thisState.IsKeyDown(Keys.D5) == true)
            {
                SoundParams parms = sfxGen.HitHurt();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }

            if (lastState.IsKeyDown(Keys.D6) == false && thisState.IsKeyDown(Keys.D6) == true)
            {
                SoundParams parms = sfxGen.Jump();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }

            if (lastState.IsKeyDown(Keys.D7) == false && thisState.IsKeyDown(Keys.D7) == true)
            {
                SoundParams parms = sfxGen.BlipSelect();
                byte[] buf = mysound.GenerateFullSound(parms, 4);
                SoundEffect se = new SoundEffect(buf, 44100, AudioChannels.Mono);
                se.Play();
            }


            // TODO: Add your update logic here
            /*
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
            */
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
