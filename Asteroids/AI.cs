using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace Asteroids
{
    class AI
        
    {
        public void Think(SpriteStripManager Enemy, SpriteStripManager Player, ObjectController controller,ParticleManager particle,SoundEffectInstance sound)
        {
            //Pretty simple AI if player is too close the enemy will run into the player, if it's too far away to run into the enemy it fires and if it's really far away it does nothing.
            Vector2 enemyloc = new Vector2(Enemy.XPos, Enemy.YPos);
            Vector2 playerloc = new Vector2(Player.XPos, Player.YPos);
            float angle = (float)((Math.Atan2(playerloc.Y - enemyloc.Y, playerloc.X - enemyloc.X) + MathHelper.Pi / 2));
            angle += MathHelper.ToRadians((new Random().Next(10 - Difficulty.difficulty)- (10 - Difficulty.difficulty))); //generates a random number between -(10 - difficulty) and 10 - difficulty and converts this to radians to stop the ai from being perfectly accurate but the ai will get more accurate as difficulty increases.
            if (Vector2.Distance(enemyloc, playerloc) < 600 && (Vector2.Distance(enemyloc, playerloc) > 100))
            {
                Enemy.Speed.Y = 0;
                if (Enemy.shotcooldown <= 0)
                {
                    //as difficulty increases the enemy will fire more often
                    Enemy.shotcooldown = 5 - Difficulty.difficulty / 2;
                    controller.Shoot(Enemy, particle, angle,sound);    
                }
                
            }
            else if(Vector2.Distance(enemyloc, playerloc) < 100)
            {
                if (Enemy.Scale > 0.5)
                {
                    //a negative angle means that the alien will move away from the player.
                    controller.Move(Enemy, angle, 1);
                }
                else controller.Move(Enemy, -angle, 2);
            }
        }
    }
    
}
