using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
namespace Asteroids
{
    class ObjectController
    {
        public void Move(SpriteStripManager sprite)
        {
            if (sprite.Speed.Length() < 3)
            {
                sprite.Speed += Vector2.Transform(new Vector2(0, -0.1f), Matrix.CreateRotationZ(sprite.rotation));
            }
            else Stop(sprite);              
        }
        public void Move(SpriteStripManager sprite,float angle, int dir)
        {
            
         
                sprite.Speed = Vector2.Transform(new Vector2(0, -2f * dir), Matrix.CreateRotationZ(angle));
            
          
            
        }
        public void Stop(SpriteStripManager sprite)
        {
            if (sprite.Speed.Length() != 0)
            {

                sprite.Speed.X *= 0.99f;
                sprite.Speed.Y *= 0.99f;

                if (sprite.Speed.Length() < 0.1)
                {
                    sprite.Speed.X = 0;
                    sprite.Speed.Y = 0;
                }
            }
        }
        public void Rotate(SpriteStripManager sprite,float angle)
        {
            float circle = MathHelper.Pi * 2;
            sprite.rotation += angle;
            sprite.rotation = sprite.rotation % circle; //resets rotation to 0 after full spin
            if (sprite.rotation < 0) //prevents rotation becoming a negative number
                sprite.rotation = circle + sprite.rotation;
        }
        public void RotateTo(SpriteStripManager sprite,float angle)
        {
            //rotates ship to a specific angle for when a controller is used as the player input
            float circle = MathHelper.Pi * 2;
            if (angle < 0) //prevents rotation becoming a negative number
                angle = circle + angle;

            if (sprite.rotation > angle + 0.3f || sprite.rotation < angle - 0.3f)
            {
                if (sprite.rotation + MathHelper.Pi <= circle)
                {
                    if (angle < sprite.rotation + MathHelper.Pi && sprite.rotation < angle)
                    {
                        sprite.rotation += 0.3f;
                    }
                    else sprite.rotation -= 0.3f;
                }
                else if (sprite.rotation - MathHelper.Pi < angle && sprite.rotation > angle)
                {
                    sprite.rotation -= 0.3f;
                }
                else sprite.rotation += 0.3f;
            }
            else sprite.rotation = angle;
            sprite.rotation = sprite.rotation % circle; //resets rotation to 0 after full spin
            if (sprite.rotation < 0) //prevents rotation becoming a negative number
                sprite.rotation = circle + sprite.rotation;

        }
        public void Shoot(SpriteStripManager sprite, ParticleManager particle,SoundEffectInstance sound)
        {
            if (particle.particles.Count < 3)
            {
                if (sound.State == SoundState.Playing)
                {
                    sound.Pause();
                }
                sound.Play();

                particle.EmitterLocation = new Vector2(sprite.XPos, sprite.YPos);
                particle.Rangle = sprite.rotation;
                particle.NewParticle();
            }
        }
        public void Shoot(SpriteStripManager sprite, ParticleManager particle, float angle, SoundEffectInstance sound)
        {
            //shoots at a specific angle this is used for when the AI fires
            if (particle.particles.Count < 3)
            {
                if (sound.State == SoundState.Playing)
                {
                    sound.Pause();
                }
                sound.Play();

                particle.EmitterLocation = new Vector2(sprite.XPos, sprite.YPos);
                particle.Rangle = angle;
                particle.NewParticle();
            }
        }
        public void Update(SpriteStripManager sprite)
        {
            Rotate(sprite, sprite.Rotate);
            sprite.XPos += sprite.Speed.X;
            sprite.YPos += sprite.Speed.Y;
        }
    }
}
