using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xTile_testProject
{
    public struct Sprite
    {
        private Vector2 origin;
        private Texture2D texture;

        public Sprite(Texture2D texture, Vector2 origin)
        {
            this.texture = texture;
            this.origin = origin;
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public static void Draw(SpriteBatch spriteBatch, Sprite sprite, Vector2 position, float rotation)
        {
            spriteBatch.Draw(sprite.texture, position, null, Color.White, rotation, sprite.origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
