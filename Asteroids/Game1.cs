using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    enum GameState
        {
            TitleScreen,
            Gameplay,
            HighScores,
        }
   
    public class Game1 : Game
    {
        
        GameState _state;
        int gothighscores = 0;
        string playername ="";
        int numAsteroid = 0;
        int numAliens = 0;
        int numStars = 0;
        int extralifecounter = 0; 
        AI ai;
        Spawner spawner;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private InputHandler input;
        private ObjectController controller;
        ParticleManager PlayerBulletParticles;
        ParticleManager EnemyBulletParticles;
        ParticleManager ParticleEffects;
        Collision sprite1;
        Collision sprite2;
        float elapsedtime = 0;
        float timesincelasthit = 5;
        HUD hud = new HUD();
        private SpriteStripManager myShip = new SpriteStripManager(2, true, new Random(),1);
        private SpriteStripManager myPortal = new SpriteStripManager(1, false, new Random(), 4);
        private List<SpriteStripManager> objectlist = new List<SpriteStripManager>();
        private List<SoundEffect> soundlist = new List<SoundEffect>();
        private List<SoundEffectInstance> soundlistinstance = new List<SoundEffectInstance>();
        SpriteStripManager newobject;
        SpriteStrip starIdle;
        SpriteStrip alienIdle;
        SpriteStrip asteroidIdle;
        SpriteStrip myShipMoving;
        HighScores highscores;
        SpriteFont font;
        int lastPlayed;

        Random ranGen = new Random();
        public Game1()
           
        {
            graphics = new GraphicsDeviceManager(this);
            input = new InputHandler();
            controller = new ObjectController();
            spawner = new Spawner();
            ai = new AI();
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
            _state = GameState.TitleScreen;
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }
        void Reinitialize()
        {
            //when a new game is started this resets the game to it's starting settings again
            hud.objective = "";
            hud.objectivenumber = -1;
            hud.lives = 1;
            hud.score = 0;
            playername = "";
            gothighscores = 0;
            Difficulty.difficulty = 0;
            extralifecounter = 0;
    }
        void InitializeNewGame()
        {
            //whenever a objective is completed this gets a new objective and increases the difficulty and awards 1000 points for completing the objective
            hud.score += 1000;
            extralifecounter += 1000;
            elapsedtime = 0;
            timesincelasthit = 2;
            if (Difficulty.difficulty < 8)
                Difficulty.difficulty++; ;
            int x = ranGen.Next(4);
            int asteroidboost = 0;
            int alienboost = 0;
            //asteroidboost and alienboost increases the amount of asteroids that will be spawned
            switch (x)
            {
                case 0:
                    hud.objective = "DESTROY ALL ALIENS";
                    asteroidboost = 1;
                    alienboost = 2;
                    break;
                case 1:
                    hud.objective = "DESTROY ALL ASTEROIDS";
                    asteroidboost = 2;
                    alienboost = 1;
                    break;

                case 2:
                    hud.objective = "SURVIVE";
                    asteroidboost = 0;
                    alienboost = 0;
                    break;

                case 3:
                    hud.objective = "COLLECT";
                    asteroidboost = 0;
                    alienboost = 0;
                    break;
            }
            hud.objectivenumber = -1;
            //if the objective number is -1 it will not be displayed on the hud. this is for the beginning game mode only.
            for (int i = 0; i < ranGen.Next(3 + asteroidboost) + (int)Difficulty.difficulty/2; i++)
            {
                spawner.NewItem(newobject, objectlist, asteroidIdle, 1, ranGen);
                numAsteroid++;
            }
            
            for (int i = 0; i < ranGen.Next(1 + alienboost) + (int)Difficulty.difficulty/2; i++)
            {
                spawner.NewItem(newobject, objectlist, alienIdle, 2, ranGen);
                numAliens++;
            }
            if (hud.objective == "DESTROY ALL ALIENS")
            {
                hud.objectivenumber = numAliens;
            }
            if (hud.objective == "DESTROY ALL ASTEROIDS")
            {
                hud.objectivenumber = numAsteroid;
            }

            if (hud.objective == "SURVIVE")
            {
                hud.objectivenumber = 10 * Difficulty.difficulty / 2;
                //the time to survive increases as difficulty increases
            }
            if (hud.objective == "COLLECT")
            {
                
                for (int i = 0; i < ranGen.Next(3) + Difficulty.difficulty; i++)
                {
                    spawner.NewItem(newobject, objectlist, starIdle, 3, ranGen);
                    numStars++;
                    //more collectables spawn as difficulty increases
                }
                hud.objectivenumber = numStars;
            }
            MoveTo(myShip, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            //resets the ship to the centre of the screen.
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content. 
        /// </summary>
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("asteroids");
            hud.font = font;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            Texture2D ship = Content.Load<Texture2D>("ship");
            Texture2D shipgo = Content.Load<Texture2D>("shipgo");
            Texture2D shot = Content.Load<Texture2D>("bullet");
            Texture2D asteroid = Content.Load<Texture2D>("asteroid");
            Texture2D alien = Content.Load<Texture2D>("alien");
            Texture2D spark = Content.Load<Texture2D>("spark");
            Texture2D beam = Content.Load<Texture2D>("beam"); 
            Texture2D star = Content.Load<Texture2D>("star");
            Texture2D portal = Content.Load<Texture2D>("portal");
            SoundEffect fire = Content.Load<SoundEffect>("fire");
            soundlist.Add(fire);
            SoundEffect bangSmall = Content.Load<SoundEffect>("bangSmall");
            soundlist.Add(bangSmall);
            SoundEffect bangMedium = Content.Load<SoundEffect>("bangMedium");
            soundlist.Add(bangMedium);
            SoundEffect bangLarge = Content.Load<SoundEffect>("bangLarge");
            soundlist.Add(bangSmall);
            SoundEffect saucerBig = Content.Load<SoundEffect>("saucerBig");
            soundlist.Add(saucerBig);
            SoundEffect extraShip = Content.Load<SoundEffect>("extraShip");
            soundlist.Add(extraShip);
            SoundEffect thrust = Content.Load<SoundEffect>("thrust");
            soundlist.Add(thrust);
            SoundEffect beat1 = Content.Load<SoundEffect>("beat1");
            soundlist.Add(beat1);
            SoundEffect beat2 = Content.Load<SoundEffect>("beat2");
            soundlist.Add(beat2);
            //SoundEffect titlemusic = Content.Load<SoundEffect>("TitleMusic");
            //soundlist.Add(titlemusic);
            //SoundEffect gamemusic = Content.Load<SoundEffect>("GameMusic");
           // soundlist.Add(gamemusic);
            foreach (SoundEffect effect in soundlist)
            {
                soundlistinstance.Add(effect.CreateInstance());
            }

            List<Texture2D> particleeffects = new List<Texture2D>();
            particleeffects.Add(spark);
            particleeffects.Add(beam);

            starIdle = new SpriteStrip(star, 1, false);
            starIdle.setName("Idle");
            
            alienIdle = new SpriteStrip(alien, 0.1f, true);
            alienIdle.setName("Idle");

            PlayerBulletParticles = new ParticleManager(shot, new Vector2(400, 240),0);
            EnemyBulletParticles = new ParticleManager(shot, new Vector2(400, 240), 0);
            ParticleEffects = new ParticleManager(particleeffects, new Vector2(400, 240), 0);

            asteroidIdle = new SpriteStrip(asteroid, 0.1f, true);
            asteroidIdle.setName("Idle");

            //these two are used if the game spawns asteroids or aliens at the very beggining.
            for (int i = 0; i < numAsteroid; i++)
            {
                asteroidIdle = new SpriteStrip(asteroid, 0.1f, true);
                asteroidIdle.setName("Idle");
                objectlist[i].addAnimatedSpriteStrip(asteroidIdle);
            }
            for (int i = 0; i < numAliens; i++)
            {
                alienIdle = new SpriteStrip(asteroid, 0.1f, true);
                alienIdle.setName("Idle");
                objectlist[i].addAnimatedSpriteStrip(alienIdle);
            }

            SpriteStrip myShipIdle = new SpriteStrip(ship, 0.1f, true);
            myShipIdle.setName("Idle");
            myShipMoving = new SpriteStrip(shipgo, 0.1f, true);
            myShipMoving.setName("Moving");
            myShip.addAnimatedSpriteStrip(myShipIdle);
            myShip.addAnimatedSpriteStrip(myShipMoving);
            hud.life = ship;

            SpriteStrip portalIdle = new SpriteStrip(portal, 0.1f, true);
            portalIdle.setName("Idle");
            myPortal.addAnimatedSpriteStrip(portalIdle);
            
           
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

            base.Update(gameTime);
            switch (_state)
            {
                case GameState.TitleScreen:
                    UpdateTitleScreen(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.HighScores:
                    UpdateHighScores(gameTime);
                    break;
            }
        }

        void UpdateTitleScreen(GameTime gameTime)
        {
            //this is a mess because it's the same everytime and it isn't important enough to not hardcode everything
            elapsedtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            input.Update();
            //if (soundlistinstance[9].State != SoundState.Playing)
            //{
            //    soundlistinstance[9].IsLooped = true;
            //    soundlistinstance[9].Play();
            //}
            if (numAsteroid == 0)
            {
                numAsteroid++;
                spawner.NewItem(newobject, objectlist, asteroidIdle, 1, ranGen);
                objectlist[0].Speed = new Vector2(0, 0);
                objectlist[0].XPos = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
                objectlist[0].YPos = GraphicsDeviceManager.DefaultBackBufferHeight / 2;
                objectlist[0].Scale = 4;
                objectlist[0].Rotate = 0.01f; 
                spawner.NewItem(newobject, objectlist, alienIdle, 2, ranGen);
                objectlist[1].Speed = new Vector2(-1, 0);
                objectlist[1].YPos = 20;
                myShip.Speed = new Vector2(0.5f, 0);
                myShip.XPos = 0;
                myShip.YPos = GraphicsDeviceManager.DefaultBackBufferHeight - 100;
                myShip.Scale = 1;
                myShip.rotation = MathHelper.Pi / 2;
            }
            objectlist[1].shotcooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            ai.Think(objectlist[1], myShip, controller, EnemyBulletParticles,soundlistinstance[0]);
            objectlist[0].Update();
            objectlist[1].Update();
            myShip.Update();
            controller.Rotate(objectlist[0], objectlist[0].Rotate);
            controller.Update(objectlist[1]);
            controller.Update(myShip);

            if (elapsedtime % 10 < 0.1)
            {
                controller.Shoot(myShip,PlayerBulletParticles, soundlistinstance[0]);
            }
            PlayerBulletParticles.Update();
            EnemyBulletParticles.Update();
            if (input.WasKeyPressed(Keys.Enter) || input.WasButtonPressed(Buttons.Start))
            {
                //soundlistinstance[9].Stop();
                elapsedtime = 0;
                RemoveEnemies();
                myShip.Scale = 0;
                _state = GameState.Gameplay;
                Reinitialize();
            }
        }
        void UpdateGameplay(GameTime gameTime)
        {
            //if (soundlistinstance[10].State != SoundState.Playing)
            //{
            //    soundlistinstance[10].Volume = 0.1f;
            //    soundlistinstance[10].IsLooped = true;
            //    soundlistinstance[10].Play();
            //}

            //main gameplay loop for each gametype
            elapsedtime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (CheckObjectiveComplete())
            {
                RemoveEnemies();
                if (hud.objective == "")
                    InitializeNewGame();
                else
                myPortal.Scale = 1;
                
            }
            if (extralifecounter > 10000)
            {
                extralifecounter = 0;
                hud.lives++;
                soundlist[6].Play();
            }

            if (hud.lives <= 0)
            {
                //soundlistinstance[10].Stop();
                _state = GameState.HighScores;
                RemoveEnemies();
                Update(gameTime);
            }
            Respawn();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (myShip.Scale == 1)
            {
                //if the ships scale isn't one that means that the player is currently dead so inputs aren't needed.
                input.Update();
                if (input.UpdateThumbStickAngle() != 0)
                {
                    controller.RotateTo(myShip, input.UpdateThumbStickAngle());
                }
                if (input.IsKeyDown(Keys.Left))
                {
                    controller.Rotate(myShip, -0.05f);
                }
                if (input.IsKeyDown(Keys.Right))
                {
                    controller.Rotate(myShip, 0.05f);
                }
                if (input.IsKeyDown(Keys.Up) || input.IsButtonDown(Buttons.RightTrigger))
                {
                    soundlistinstance[6].IsLooped = true;
                    soundlistinstance[6].Play();
                    myShip.setCurrentAction("Moving");
                    controller.Move(myShip);
                }
                else if (input.isMouseButtonDown(1))
                {
                    soundlistinstance[6].IsLooped = true;
                    soundlistinstance[6].Play();
                    myShip.setCurrentAction("Moving");
                    Vector2 mouse = input.getMousePos();
                    float angle = (float)((Math.Atan2(mouse.Y - myShip.YPos, mouse.X - myShip.XPos) + MathHelper.Pi / 2));
                    controller.RotateTo(myShip, angle);
                    controller.Move(myShip);
                }
                else
                {
                    soundlistinstance[6].Pause();
                    myShip.setCurrentAction("Idle");
                    controller.Stop(myShip);
                }
                if (input.isMouseButtonDown(2))
                {
                    Vector2 mouse = input.getMousePos();
                    float angle = (float)((Math.Atan2(mouse.Y - myShip.YPos, mouse.X - myShip.XPos) + MathHelper.Pi / 2));
                    controller.RotateTo(myShip, angle);
                }
                if (input.WasKeyPressed(Keys.Space) || input.WasButtonPressed(Buttons.A))
                {
                    controller.Shoot(myShip, PlayerBulletParticles, soundlistinstance[0]);
                }
                else if (input.isMouseButtonClick(2))
                {
                    Vector2 mouse = input.getMousePos();
                    float angle = (float)((Math.Atan2(mouse.Y - myShip.YPos, mouse.X - myShip.XPos) + MathHelper.Pi / 2));
                    controller.RotateTo(myShip, angle);
                    controller.Shoot(myShip, PlayerBulletParticles, soundlistinstance[0]);
                }

            }

            controller.Update(myShip);
            myShip.Update();
            PlayerBulletParticles.Update();
            EnemyBulletParticles.Update();
            ParticleEffects.Update();

            //this causes the game to pause for 2 seconds before starting each time it changes gametype
            if (elapsedtime > 2)
            {

                if (hud.objective != "DESTROY ALL ASTEROIDS" && hud.objective != "DESTROY ALL ALIENS")
                {
                    if (elapsedtime % (10 - Difficulty.difficulty) < gameTime.ElapsedGameTime.TotalSeconds)
                    {
                        //spawns asteroids every 10 seconds to begin with and increases the spawning rate as time goes on
                        spawner.NewItem(newobject, objectlist, asteroidIdle, 1, ranGen);
                        numAsteroid++;
                        if(hud.objective == "")
                        {
                            //everytime an asteroid is spawned in the tutorial the difficulty increases
                            Difficulty.difficulty++;
                        }
                    }
                    if (elapsedtime % (12 - Difficulty.difficulty) < gameTime.ElapsedGameTime.TotalSeconds)
                    {
                        //spawns aliens every 12 seconds to begin with and increases the spawning rate as time goes on
                        spawner.NewItem(newobject, objectlist, alienIdle, 2, ranGen);
                        numAliens++;
                        if(ranGen.Next(1,4) > 3)
                        {
                            //25% chance of spawning a small alien.
                            objectlist[objectlist.Count - 1].Scale = 0.5f;
                        }
                    }
                }

                timesincelasthit += (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateList(objectlist, newobject, gameTime);
                sprite1 = myShip.GetCollider();
                if (timesincelasthit > 2)
                {
                    //the player is invincble for 2 seconds after being hit
                    if (CheckParticleCollision(sprite1, EnemyBulletParticles))
                    {
                        GenerateParticleEffects(myShip);
                        hud.lives--;
                        myShip.Scale = 0;
                        timesincelasthit = 0;
                        soundlist[2].Play();
                    }
                }

                if (myPortal.Scale == 1)
                {
                    //after the objective is completed a portal spawns and sucks the player into it.

                    if (lastPlayed == 7 && soundlistinstance[7].State == SoundState.Stopped)
                    {
                        soundlistinstance[8].Play();
                        lastPlayed = 8;
                    }
                    else if (soundlistinstance[8].State == SoundState.Stopped)
                    {
                        soundlistinstance[7].Play();
                        lastPlayed = 7;
                    }
                    controller.Move(myShip, (float)(Math.Atan2(myShip.YPos - myPortal.YPos, myShip.XPos - myPortal.XPos) + MathHelper.Pi / 2), -1);
                    myPortal.Update();
                    sprite2 = myPortal.GetCollider();
                    if (sprite1.visiblePixelCollision(sprite2))
                    {
                        soundlistinstance[8].Stop();
                        myPortal.Scale = 0;
                        InitializeNewGame();
                    }
                }
            }

            if (hud.objective == "DESTROY ALL ALIENS")
            {
                hud.objectivenumber = numAliens;
            }
            if (hud.objective == "DESTROY ALL ASTEROIDS")
            {
                hud.objectivenumber = numAsteroid;
            }
            if (hud.objective == "COLLECT")
            {
                hud.objectivenumber = numStars;
            }
            hud.Update(gameTime);

        }
        void UpdateHighScores(GameTime gameTime)
        {
            //if (soundlistinstance[9].State != SoundState.Playing)
            //{
            //    soundlistinstance[9].IsLooped = true;
            //    soundlistinstance[9].Play();
            //}
            input.Update();
            
            if (gothighscores == 0)
            {
                elapsedtime = 0;
                highscores = new HighScores();
                highscores.font = font;
                highscores.GetHighScores();
                gothighscores++;
            }
            if (highscores.CheckHighScores(hud.score))
            {
                if (input.WasKeyPressed(Keys.Enter))
                {
                    highscores.UpdateHighScores(playername, hud.score);
                    hud.score = 0;
                }
                foreach (Keys key in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    elapsedtime = 0;
                    if (input.WasKeyPressed(key))
                    {
                        if (playername.Length < 3)
                        playername += key;
                    }
                }
                if (input.WasKeyPressed(Keys.Back))
                    if (playername.Length > 0)
                    playername = playername.Remove(playername.Length - 1, 1);
                

            }
            else
            if (input.WasKeyPressed(Keys.Enter))
            {
                //soundlistinstance[9].Stop();
                _state = GameState.TitleScreen;
            }


        }
        void UpdateList(List<SpriteStripManager> list, SpriteStripManager item,GameTime gameTime)
        {
           
            for (int i = 0; i < list.Count; i++)
            {
                int hit = 0;
                if (list[i].myID == 2)
                {
                    // if the item in the list is an alien it reduces the shotcooldown and allows the ai to control it.
                    list[i].shotcooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (myShip.Scale == 1)
                    ai.Think(list[i], myShip, controller, EnemyBulletParticles, soundlistinstance[0]);
                }

                controller.Update(list[i]);
                list[i].Update();

                sprite1 = list[i].GetCollider();
                if (list[i].isCollectable == false)
                {
                    //these checks are for when the player hits objects with it's bullets

                    for (int particle = 0; particle < PlayerBulletParticles.particles.Count; particle++)
                    {
                        sprite2 = PlayerBulletParticles.GetCollider(particle);
                        if (CheckParticleCollision(sprite1, PlayerBulletParticles))
                        {
                            //create 2 small asteroids when a large one is destroyed
                            if (list[i].Scale >= 0.5 && list[i].myID == 1)
                            {
                               spawner.SpawnChildren(item, list, asteroidIdle, i, (int)Difficulty.difficulty/2, ranGen);
                                numAsteroid += (int)Difficulty.difficulty / 2;
                            }
                            if (list[i].Scale > 0.5 && list[i].myID == 2)
                            {
                                //spawns 0 aliens at difficulties 0-2, 1 alien from 3-5 and 2 aliens on the other difficulties when an alien is destroyed
                                spawner.SpawnChildren(item, list, alienIdle, i, (int)Difficulty.difficulty / 3, ranGen);
                                numAliens += (int)Difficulty.difficulty / 3;
                            }
                            //remove asteroid when hit by player shot
                            GenerateParticleEffects(list[i]);
                            if (list[i].myID == 1)
                            {
                                if (list[i].Scale == 1) soundlist[1].Play();
                                else if (list[i].Scale == 0.5) soundlist[2].Play();
                                else soundlist[3].Play();
                                hud.score += 50;
                                extralifecounter += 50;
                                numAsteroid--;
                            }
                            else
                            {
                                numAliens--;
                                hud.score += 100;
                                extralifecounter += 100;
                                soundlist[4].Play();
                            }
                            list.RemoveAt(i);
                            hit = 1;
                        }
                    }
                }
                //if the player has been hit before than the following is skipped
                if (hit == 0)
                {
                    //these checks are for when the player collides with other objects.
                    if (timesincelasthit > 2 || list[i].isCollectable)
                    {
                        sprite2 = myShip.GetCollider();
                        if (sprite1.visiblePixelCollision(sprite2))
                        {
                            if (list[i].Scale >= 0.5 && list[i].myID == 1)
                            {
                                spawner.SpawnChildren(item, list, asteroidIdle, i, (int)Difficulty.difficulty / 2, ranGen);
                                numAsteroid += (int)Difficulty.difficulty / 2;
                            }
                            if (list[i].Scale > 0.5 && list[i].myID == 2)
                            {
                                //spawns 0 aliens at difficulties 0-2, 1 alien from 3-5 and 2 aliens on the other difficulties when an alien is destroyed
                                spawner.SpawnChildren(item, list, alienIdle, i, (int)Difficulty.difficulty / 3, ranGen);
                                numAliens += (int)Difficulty.difficulty / 3;
                            }
                            //remove asteroid when hit by player shot
                            if (list[i].myID == 1)
                            {
                                numAsteroid--;
                                hud.score += 25;
                                extralifecounter += 25;
                                if (list[i].Scale == 1) soundlist[1].Play();
                                else if (list[i].Scale == 0.5) soundlist[2].Play();
                                else soundlist[3].Play();
                            }
                            else if (list[i].myID == 2)
                            {
                                numAliens--;
                                hud.score += 50;
                                extralifecounter += 50;
                                soundlist[4].Play();
                            }
                            else
                            {
                                numStars--;
                                hud.score += 100;
                                extralifecounter += 50;
                                soundlist[5].Play();

                            }
                            if (!list[i].isCollectable)
                            {
                                //if the item collided with is not a collectable then the player is destroyed.
                                GenerateParticleEffects(list[i]);
                                GenerateParticleEffects(myShip);
                                hud.lives--;
                                myShip.Scale = 0;
                                timesincelasthit = 0;
                                soundlist[1].Play();
                            }

                            list.RemoveAt(i);

                        }
                    }
                }
            }
        }

        bool CheckObjectiveComplete()
        {
            bool complete = false;
            switch(hud.objective)
            {
                case "":
                    if (hud.lives <= 0)
                    {
                        //resets the difficulty and gives the player 3 lives after the tutorial is completed.
                        Difficulty.difficulty = 0;
                        hud.lives = 3;
                        return true;
                    }
                    break;
                case "SURVIVE":
                    if (hud.objectivenumber <= 0)
                        return true;
                    break;
                case "COLLECT":
                    if (numStars <= 0)
                        return true;
                    break;
                case "DESTROY ALL ALIENS":
                    if (numAliens <= 0)
                        return true;
                    break;
                case "DESTROY ALL ASTEROIDS":
                    if (numAsteroid <= 0)
                        return true;
                    break;
            }
            return complete;
        }
        void RemoveEnemies()
        {
            //removes all current enemies
            for (int i = objectlist.Count - 1; i >= 0; i--)
            {
                objectlist.RemoveAt(i);
                
            }
            numAliens = 0;
            numAsteroid = 0;
            numStars = 0;
        }
        void GenerateParticleEffects(SpriteStripManager item)
        {
            for (int i = 0; i < 3; i++)
            {
                ParticleEffects.EmitterLocation = new Vector2(item.XPos, item.YPos);
                ParticleEffects.Rangle = (float)ranGen.NextDouble() * 6.2f;
                ParticleEffects.NewParticle(item.myID);
            }
        }
        void Respawn()
        {
            if (timesincelasthit > 1 && myShip.Scale == 0)
            {
                //after 1 second the player respawns at the centre of the screen
                myShip.Scale = 1;
                MoveTo(myShip, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                myShip.Speed = new Vector2(0,0);
            }
        }
        void MoveTo(SpriteStripManager sprite, float Xpos, float Ypos)
        {
            sprite.XPos = Xpos;
            sprite.YPos = Ypos;
        }
        bool CheckParticleCollision(Collision sprite1,ParticleManager particles)
        {
            for (int particle = 0; particle < particles.particles.Count; particle++)
            {
                sprite2 = particles.GetCollider(particle);
                if (sprite1.visiblePixelCollision(sprite2))
                {
                    particles.Kill(particle);
                    particles.Update();
                    particle--;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawList(List<SpriteStripManager> spritelist, GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < spritelist.Count; i++)
            {
                spritelist[i].Draw(gameTime, spriteBatch);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            switch (_state)
            {
                case GameState.Gameplay:
                    {
                        myPortal.Draw(gameTime, spriteBatch);
                        
                        if (elapsedtime > 2)
                        {
                            DrawList(objectlist, gameTime, spriteBatch);
                        }
                        PlayerBulletParticles.Draw(spriteBatch);
                        EnemyBulletParticles.Draw(spriteBatch);

                        myShip.Draw(gameTime, spriteBatch);
                        ParticleEffects.Draw(spriteBatch);
                        
                        hud.Draw(spriteBatch);
                        break;
                    }
                case GameState.HighScores:
                    {
                        highscores.PrintHighScores(spriteBatch);
                        if (highscores.CheckHighScores(hud.score))
                        { 
                            spriteBatch.DrawString(font, "Enter Your Initials", new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 200, GraphicsDeviceManager.DefaultBackBufferHeight - 100), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0.5f);
                        }
                            spriteBatch.DrawString(font, playername, new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 75, GraphicsDeviceManager.DefaultBackBufferHeight - 50), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0.5f);
                        break;
                    }
                case GameState.TitleScreen:
                    {
                        PlayerBulletParticles.Draw(spriteBatch);
                        EnemyBulletParticles.Draw(spriteBatch);
                        DrawList(objectlist, gameTime, spriteBatch);
                        myShip.Draw(gameTime, spriteBatch);
                        spriteBatch.DrawString(font, "ASTEROIDS", new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 175, 50), Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0.5f);
                        spriteBatch.DrawString(font, "PRESS ENTER TO PLAY", new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 200, GraphicsDeviceManager.DefaultBackBufferHeight - 50), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0.5f);
                        break;
                    }
            }
            
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
