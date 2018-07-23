using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    class SpriteStripManager
    {
        private SpriteStrip[] myAnimatedSpriteStrips;
        private int actionsAddedCount = 0;
        private int currentAction = 0;
        private string currentDirection = "left";
        private string previousAction;

        public bool isCollectable = false;
        public int myID = 0;
        public float XPos = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
        public float YPos = GraphicsDeviceManager.DefaultBackBufferHeight / 2;
        public Vector2 Speed;
        public float rotation;
        public float Rotate;
        public float Scale = 1;
        public float shotcooldown = 1;
        public string currentActionName;
        private SpriteEffects mySpriteEffects;
        


        public SpriteStripManager(int numActions, bool isPlayer, Random ranGen,int itemid)
        {
            myAnimatedSpriteStrips = new SpriteStrip[numActions];
            if (!isPlayer)
            {
                // itemid 0 = playership, 1 = asteroid, 2 = alien ,3 = collectable
                if (itemid == 1)
                {
                    myID = itemid;
                    // this will spawn them on the right border of the screen so they don't randomly pop onto the screen
                    XPos = ((float)ranGen.NextDouble() * ((GraphicsDeviceManager.DefaultBackBufferWidth + 50) - GraphicsDeviceManager.DefaultBackBufferWidth)) + GraphicsDeviceManager.DefaultBackBufferWidth;
                    YPos = (float)ranGen.NextDouble() * GraphicsDeviceManager.DefaultBackBufferHeight;
                    Speed.X = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    Speed.Y = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    Rotate = (float)ranGen.NextDouble() * 0.1f;
                    
                }
                if (itemid == 2)
                {
                    myID = itemid;
                    // this will spawn them on the right border of the screen so they don't randomly pop onto the screen
                    XPos = ((float)ranGen.NextDouble() * ((GraphicsDeviceManager.DefaultBackBufferWidth + 50) - GraphicsDeviceManager.DefaultBackBufferWidth)) + GraphicsDeviceManager.DefaultBackBufferWidth;
                    YPos = (float)ranGen.NextDouble() * GraphicsDeviceManager.DefaultBackBufferHeight - 50;
                    Speed.X = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    if (Speed.X > -1 && Speed.X < 1)
                    {
                        if (Speed.X < 0)
                            Speed.X--;
                        else Speed.X++;
                    }
                    if (Speed.X < 0)
                        Speed.X -= Difficulty.difficulty / 2;
                    else Speed.X += Difficulty.difficulty / 2;
                    Speed.Y = 0;
                    Rotate = 0;
                }
                if (itemid == 3)
                {
                    myID = itemid;
                    XPos = (float)ranGen.NextDouble() * GraphicsDeviceManager.DefaultBackBufferWidth - 10; //the - 10 stops it from being inside of the border so it's easier to see
                    YPos = (float)ranGen.NextDouble() * GraphicsDeviceManager.DefaultBackBufferHeight - 10;
                    Speed.X = 0;
                    Speed.Y = 0;
                    Rotate = 0;
                    isCollectable = true;
    }
                //this is the portal
                if (itemid == 4)
                {
                    myID = itemid;
                    Scale = 0;
                    XPos = GraphicsDeviceManager.DefaultBackBufferWidth / 2; 
                    YPos = GraphicsDeviceManager.DefaultBackBufferHeight/2;
                    Speed.X = 0;
                    Speed.Y = 0;
                    Rotate = 0;
                }
                
            }
        }
        public SpriteStripManager(int numActions, bool isPlayer, Random ranGen, float x, float y, float scale,int itemid)
        {
            myAnimatedSpriteStrips = new SpriteStrip[numActions];
            if (!isPlayer)
            {
                // itemid 0 = playership, 1 = asteroid, 2 = alien ,3 = collectable
                if (itemid == 1)
                {
                    myID = itemid;
                    Scale = scale;
                    // this will spawn them on the right border of the screen so they don't randomly pop onto the screen
                    XPos = x;
                    YPos = y;
                    Speed.X = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    Speed.Y = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    Rotate = (float)ranGen.NextDouble() * 0.1f;
                }
                if (itemid == 2)
                {
                    myID = itemid;
                    Scale = scale;
                    // this will spawn them on the right border of the screen so they don't randomly pop onto the screen
                    XPos = x;
                    YPos = y;
                    Speed.X = ((float)ranGen.NextDouble() * (2 + 2)) - 2;
                    if (Speed.X > -1 && Speed.X < 1)
                    {
                        if (Speed.X < 0)
                            Speed.X--;
                        else Speed.X++;
                    }
                    if (Speed.X < 0)
                        Speed.X -= Difficulty.difficulty / 2;
                    else Speed.X += Difficulty.difficulty / 2;
                    Speed.Y = 0;
                    Rotate = 0;
                }
                if (itemid == 3)
                {
                    myID = itemid;
                    Scale = scale;
                    XPos = x;
                    YPos = y;
                    Speed.X = 0;
                    Speed.Y = 0;
                    Rotate =  0;
                    isCollectable = true;
                }
            }
            else scale = 1f;
        }

        public void addAnimatedSpriteStrip(SpriteStrip thisAnim)
        {
            if (actionsAddedCount > myAnimatedSpriteStrips.Length)
            {
                Console.WriteLine("adding too many actions for your actions manager");
            }
            else
            {
                myAnimatedSpriteStrips[actionsAddedCount] = thisAnim;
                actionsAddedCount = actionsAddedCount + 1;
            }
        }


        public void setCurrentAction(string actionName)
        {

            for (int n = 0; n < actionsAddedCount; n++)
            {
                if (actionName == myAnimatedSpriteStrips[n].myName)
                {

                    currentAction = n;
                    previousAction = currentActionName;
                    currentActionName = actionName;
                    setCurrentDirection(currentDirection);
                    return;
                }
            }
            Console.WriteLine("Cannot find this action in action list");
        }


        public void setCurrentDirection(string dir)
        {
            // assumes all actions drawn facing to the LEFT
            if (actionsAddedCount == 0) return;

            if (dir == "left")
            {
                currentDirection = "left";
                mySpriteEffects = SpriteEffects.None;
            }
            if (dir == "right")
            {
                currentDirection = "right";
                mySpriteEffects = SpriteEffects.FlipHorizontally; 
            }
        }
        public Texture2D getTexture()
        {
            return myAnimatedSpriteStrips[currentAction].getTexture();
        }
        public Collision GetCollider()
        {
            return myAnimatedSpriteStrips[currentAction].collider;
        }
      
        public void Update()
        {
            if (actionsAddedCount == 0) return;
            if (XPos < -50 || XPos > GraphicsDeviceManager.DefaultBackBufferWidth + 50)
            {
                if (XPos < -50)
                {
                    XPos = GraphicsDeviceManager.DefaultBackBufferWidth + 50;
                }
                else XPos = -50;
            }
            if (YPos < -50 || YPos > GraphicsDeviceManager.DefaultBackBufferHeight + 50)
            {
                if (YPos < -50)
                {
                    YPos = GraphicsDeviceManager.DefaultBackBufferHeight + 50;
                }
                else YPos = -50;
            }
            myAnimatedSpriteStrips[currentAction].Update(XPos,YPos,Scale);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (actionsAddedCount == 0) return;

            myAnimatedSpriteStrips[currentAction].Draw(gameTime, spriteBatch, XPos, YPos, rotation, Scale, mySpriteEffects);
        }

    }


}

