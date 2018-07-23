using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    //I think I can use this to spawn all the different objects.
    class Spawner
    {
        public void NewItem(SpriteStripManager item, List<SpriteStripManager> itemlist,SpriteStrip texture,int itemid,Random ranGen)
        {
            item = new SpriteStripManager(1, false, ranGen, itemid);
            itemlist.Add(item);
            itemlist[itemlist.Count - 1].addAnimatedSpriteStrip(texture);
            itemlist[itemlist.Count - 1].Update();

        }
        public void SpawnChildren(SpriteStripManager item, List<SpriteStripManager> itemlist, SpriteStrip texture,int current,int amount, Random ranGen)
        {
            //spawns smaller versions of a given item.
            for (int i = 0; i < amount; i++)
            {
                texture = new SpriteStrip(itemlist[current].getTexture(), 0.1f, true);
                texture.setName("Idle");
                item = new SpriteStripManager(1, false, ranGen, itemlist[current].XPos, itemlist[current].YPos, itemlist[current].Scale / 2,itemlist[current].myID);
                itemlist.Add(item);
                itemlist[itemlist.Count - 1].addAnimatedSpriteStrip(texture);
                itemlist[itemlist.Count - 1].Update();

            }
          
        }
    }
}
