#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

// AnimatedSprite.  This class  handles the animation and drawing of a 2D animation
// based on a single imported texture, which is a single horizontal strip of 
// sequential images (cells). AnimatedSprite expects the cells of the sprite strip to be 
// square; the class then calculates the number of cells in the strip based ion the strip's pixel height
// and length 
namespace Asteroids
{
    class SpriteStrip
    {

        // The tiled image from which we animate
        public Texture2D myCellsTexture;

        // Duration of time to show each frame.
        private float myFrameTime;

        //  is it looping... probably!
        private bool myIsLooping;

        // The amount of time in seconds that the current frame has been shown for.
        private float elapsedFrameTime;

        // The actual cell being addressed at this GameTime (0... numCells-1) 
        private int myFrameIndex;

        // counts from 0 to everupwards as the object lives on
        private int myFrameCounter;

        public Rectangle rect;
        public Collision collider;
        private float myDrawingDepth;

        public string myName;

        public SpriteStrip(Texture2D texture, float frameTime, bool isLooping)
        {
            myCellsTexture = texture;
            myFrameTime = frameTime;
            myIsLooping = isLooping;
            elapsedFrameTime = 0.0f;
            myFrameIndex = 0;
            myFrameCounter = 0;
            myDrawingDepth = 0.5f;
            collider = new Collision(texture, new Rectangle(0,0,0,0));
        }

        public void setName(string actionName)
        {
            myName = actionName;
        }

        public int FrameCount()
        {
            return myCellsTexture.Width / myCellsTexture.Height;
        }

        public Texture2D getTexture()
        {
            return myCellsTexture;
        }
        public Vector2 Origin()
        {
            return new Vector2(myCellsTexture.Height / 2.0f, myCellsTexture.Height / 2.0f);
        }
        
        public void setDrawingDepth(float z)
        {
            myDrawingDepth = z;
        }

        public void UpdateCollider(float x,float y,float scale)
        {
            collider = new Collision(myCellsTexture, new Rectangle((int)(x - ((myCellsTexture.Height / 2) * scale)), (int)(y - ((myCellsTexture.Height / 2)* scale)), (int)(myCellsTexture.Height * scale), (int)(myCellsTexture.Height * scale) ));
        }
        public void Update(float x, float y,float scale)
        {
            UpdateCollider(x, y,scale);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch,float x,float y,float rotation,float scale,SpriteEffects effects)
        {

            // Process passing time. ElapsedGameTime returns the amount of time elapsed since the last Update
            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedFrameTime > myFrameTime)
            {
                // Advance the frame index; looping or clamping as appropriate.
                myFrameCounter++;

                if (myIsLooping)
                {
                    myFrameIndex = myFrameCounter % FrameCount();
                }
                else
                {
                    // freezes on the last frame
                    myFrameIndex = Math.Min(myFrameCounter, FrameCount() - 1);

                }

                elapsedFrameTime = 0.0f;
            }

            // Calculate the source rectangle of the current frame
            int cellWidth = myCellsTexture.Height;
            int leftMostPixel = myFrameIndex * cellWidth;
            Rectangle sourceRect = new Rectangle(leftMostPixel, 0, cellWidth, cellWidth);
            rect = sourceRect;
            // Draw the current frame.
            // (bigTexture, posOnScreen, sourceRect in big texture, col, rotation, origin, scale, effect, depth)
            Vector2 myPosition;
            myPosition.X = x;
            myPosition.Y = y;
            spriteBatch.Draw(myCellsTexture, myPosition, sourceRect, Color.White, rotation, Origin(), scale, effects, myDrawingDepth);
        }


    }
}