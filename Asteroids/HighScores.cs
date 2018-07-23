using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class HighScores
    {
        public SpriteFont font;
        List<String> Names = new List<string>();
        List<int> Scores = new List<int>();
        public void GetHighScores()
        {
           
            using (var stream = TitleContainer.OpenStream("HighScores.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    string[] array;
                    while ((line = reader.ReadLine()) != null)
                    {
                        array = line.Split(' ');
                        Names.Add(array[0]);
                        Scores.Add(Convert.ToInt32(array[1]));
                    }
                }
                stream.Close();
            }

        }
        public void PrintHighScores(SpriteBatch spritebatch)
        {
            int i = 0;
            spritebatch.DrawString(font, "HIGHSCORES", new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 100, 40 * i), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0.5f);
            while (i < Names.Capacity)
            {
                //my font doesn't have a "\" so I can't use string literals :(
                spritebatch.DrawString(font, Names[i]+ "    " + Scores[i], new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 100, 40 * (i + 1)), Color.White,0,new Vector2(0,0),2,SpriteEffects.None,0.5f);
                i++;
            }
        }
       public bool CheckHighScores(int score)
        {
            if (score > Scores[Scores.Capacity - 1])
            {
                return true;   
            }
            return false;
        }
        public void UpdateHighScores(string name, int score)
        {
            Scores[Scores.Capacity - 1] = score;
            Names[Names.Capacity - 1] = name;
            OrderHighScores();

        }

        public void OrderHighScores()
        {
            for (int i = Scores.Capacity; i > 1; i--)
            {
                int tempint;
                string tempstring;
                if (Scores[i - 1] > Scores[i - 2])
                {
                    tempint = Scores[i - 2];
                    tempstring = Names[i - 2];
                    Scores[i - 2] = Scores[i - 1];
                    Names[i - 2] = Names[i - 1];
                    Scores[i - 1] = tempint;
                    Names[i - 1] = tempstring;
                }
                else break;
            }
            SaveHighScores();

        }
        public void SaveHighScores()
        {
            

            using (StreamWriter writer = new StreamWriter("HighScores.txt"))
            {
                    for( int i = 0; i < Scores.Capacity; i++)
                    {
                        writer.WriteLine(Names[i] + " " + Scores[i]);
                    }
            }
                
            
        }
    }
}
