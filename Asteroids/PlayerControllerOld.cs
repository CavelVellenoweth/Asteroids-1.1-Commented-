using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace Asteroids
{
    class PlayerController
    {
        public void Move(SpriteStripManager sprite,int x)
        {
            if (x == 0)
            {
                if (sprite.Speed.Length() < 3)
                {
                    sprite.Speed += Vector2.Transform(new Vector2(0, -0.1f), Matrix.CreateRotationZ(sprite.rotation));
                }
                else StopX(sprite);
            }
            if (x == 1)
            {
                float circle = MathHelper.Pi * 2;
                sprite.rotation += 0.1f;
                sprite.rotation = sprite.rotation % circle; //resets rotation to 0 after full spin
                if (sprite.rotation < 0) //prevents rotation becoming a negative number
                    sprite.rotation = circle + sprite.rotation;
                //if (sprite.XSpeed > -5)
                //{
                //    sprite.XSpeed -= 0.05f;
                //}
            }
            else if (x == 2)
            {
                if (sprite.XSpeed < 5)
                {
                    sprite.XSpeed += 0.05f;
                }
            }
            else if (x == 3)
            {
                if (sprite.YSpeed > -5)
                {
                    sprite.YSpeed -= 0.05f;
                }
            }
            else if (x == 4)
            {
                if (sprite.YSpeed < 5)
                {
                    sprite.YSpeed += 0.05f;
                }
            }
        }
        public void StopX(SpriteStripManager sprite)
        {
            if (sprite.Speed.Length() != 0)
            {
                //if (sprite.Speed.X > 0.2f)
                //{
                    sprite.Speed.X *= 0.99f;
                //}
                //else sprite.Speed.X = 0;
                //if (sprite.Speed.Y > 0.2f)
                //{
                    sprite.Speed.Y *= 0.99f;
                // }
                // else sprite.Speed.Y = 0;
                if (sprite.Speed.Length() < 0.1)
                {
                    sprite.Speed.X = 0;
                    sprite.Speed.Y = 0;
                }
                //sprite.Speed += Vector2.Transform(new Vector2(0, -1), Matrix.CreateRotationZ(sprite.rotation));
            }
            //if (sprite.XSpeed != 0.0f)
            //{
            //    if (sprite.XSpeed < 0.0f)
            //    {
            //        sprite.XSpeed += 0.05f;
            //    }
            //    if (sprite.XSpeed > 0.0f)
            //    {
            //        sprite.XSpeed -= 0.05f;
            //    }
            //}
            //if (sprite.XSpeed > -0.05f && sprite.XSpeed < 0.05f)
            //    sprite.XSpeed = 0.0f;
        }
        public void StopY(SpriteStripManager sprite)
        {
            if (sprite.YSpeed != 0.0f)
            {
                if (sprite.YSpeed < 0.0f)
                {
                    sprite.YSpeed += 0.05f;
                }
                if (sprite.YSpeed > 0.0f)
                {
                    sprite.YSpeed -= 0.05f;
                }
            }
            if (sprite.YSpeed > -0.05f && sprite.YSpeed < 0.05f)
                sprite.YSpeed = 0.0f;
        }
        public void Rotate(SpriteStripManager sprite,float angle)
        {
            float circle = MathHelper.Pi * 2;
            if (angle >= MathHelper.Pi)
            {
                if (sprite.rotation != angle)
                {
                    if (sprite.rotation > (angle + MathHelper.Pi) % circle && sprite.rotation < angle)
                        sprite.rotation += 0.03f;
                    else
                        sprite.rotation -= 0.03f;

                    if (sprite.rotation < angle + 0.03 && sprite.rotation > angle - 0.03)
                        sprite.rotation = angle;
                }
            }
            else
            {

                if (sprite.rotation != angle)
                {
                    if (sprite.rotation > (angle + MathHelper.Pi) % circle || sprite.rotation < angle)
                        sprite.rotation += 0.03f;
                    else
                        sprite.rotation -= 0.03f;

                    if (sprite.rotation < angle + 0.03 && sprite.rotation > angle - 0.03)
                        sprite.rotation = angle;
                }
            }

            sprite.rotation = sprite.rotation % circle; //resets rotation to 0 after full spin
            if (sprite.rotation < 0) //prevents rotation becoming a negative number
                sprite.rotation = circle + sprite.rotation;
        }
        public void Shoot(SpriteStripManager sprite, ParticleManager particle)
        {
            particle.EmitterLocation = new Vector2(sprite.XPos, sprite.YPos);
            particle.Rangle = sprite.rotation;
            particle.NewParticle();
        }
        public void Update(SpriteStripManager sprite)
        {
            
            sprite.XPos += sprite.Speed.X;
            sprite.YPos += sprite.Speed.Y;
        }
    }
}
