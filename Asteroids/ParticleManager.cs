using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class ParticleManager
    {
        public Vector2 EmitterLocation { get; set; }
        public float Rangle;
        public List<Particle> particles;
        private Texture2D texture;
        private List<Texture2D> textures;

        public ParticleManager(Texture2D texture, Vector2 location,float rangle)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle>();
        }
        public ParticleManager(List<Texture2D> textures, Vector2 location, float rangle)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
        }
        public void NewParticle()
        {
            particles.Add(GenerateNewParticle());
        }
        public void NewParticle(int id)
        {
            particles.Add(GenerateNewParticle(id));
        }
        public Collision GetCollider(int i)
        {
            return particles[i].Collider;
        }
        public void Kill(int i)
        {
            particles[i].TTL = 0;
        }
        public void Update()
        {        
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = this.texture;
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(0, -10);
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(Rangle));       
            float angle = 0;
            float angularVelocity = 0;
            Color color = Color.White;
            float size = 1f;
            int ttl = 50;
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        private Particle GenerateNewParticle(int id)
        {
            Texture2D texture;
            if (id == 0) 
            {
                 texture = this.textures[1];
            }
            else  texture = this.textures[0];


            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(0, -0.5f);
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(Rangle));
            float angle = 0;
            float angularVelocity = 0.01f;
            Color color = Color.White;
            float size = 1f;
            int ttl = 50;
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            
        }
    }
}
