using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Asteroids
{
    public class Collision
    {
        public Rectangle bounds;
        public Color color;
        public Texture2D texture;
        private uint[] rawData;

        public Collision(Texture2D Tex, Rectangle Bounds)
        {

            texture = Tex;
            bounds = Bounds;
            color = Color.White;
            rawData = new uint[texture.Width * texture.Height];
            texture.GetData<uint>(rawData);
        }



        public bool boundingBoxIntersection(Collision b)
        {
            // check if two Rectangles intersect
            return (bounds.Right > b.bounds.Left && bounds.Left < b.bounds.Right &&
                    bounds.Bottom > b.bounds.Top && bounds.Top < b.bounds.Bottom);
        }

        public bool visiblePixelCollision(Collision b)
        {
            return boundingBoxIntersection(b);
        }











    }
}
