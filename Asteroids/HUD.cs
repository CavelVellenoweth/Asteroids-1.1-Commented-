using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class HUD
    {
        float elapsedTime = 0;
        public SpriteFont font;
        public Texture2D life;
        public int score = 0;
        public int lives = 3;
        public int objectivenumber;
        public string objective;
        public void Update(GameTime gametime)
        {
            if (lives > 5)
                lives = 5;
            if (objective == "SURVIVE")
            {
                //counts down timer if in survive game mode.
                elapsedTime += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (elapsedTime >= 1)
                {
                    objectivenumber--;
                    elapsedTime = 0.0f;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(font, "SCORE:" + score,new Vector2(10, 0), Color.White);
            spritebatch.DrawString(font, objective, new Vector2((GraphicsDeviceManager.DefaultBackBufferWidth/2) - 100, 0), Color.White);
            if (objectivenumber > -1)
            {
                spritebatch.DrawString(font, "REMAINING: " + objectivenumber, new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth - 150, 0), Color.White);
            }
            for (int i = 0; i < lives; i++)
            {
                spritebatch.Draw(life, new Rectangle((life.Height * i), 20, life.Height , life.Height), Color.White);   
            }
        }


    }
}
