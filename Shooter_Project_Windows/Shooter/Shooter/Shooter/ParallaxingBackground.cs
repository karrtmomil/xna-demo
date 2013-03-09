// ParallaxingBackground.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class ParallaxingBackground
    { 
        // The image representing the parallaxing background
        Texture2D texture;

        // An array of positions of the parallaxing bacground
        Vector2[] positions;

        // The speed which the background is moving
        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            // Load the background texture we will be using 
            texture = content.Load<Texture2D>(texturePath);

            // Set teh speed of the background
            this.speed = speed;

            // If we divide the screen with the texture widthe then we can determine the number of tiles we need.
            // We add 1 to it so that we won't have a gap in the tiling
            positions = new Vector2[screenWidth / texture.Width + 1];

            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                positions[i] = new Vector2(i * texture.Width, 0);
            }

        }

        public void Update()
        {

        }

        public void Draw()
        {

        }


    }
}
