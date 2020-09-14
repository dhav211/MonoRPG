using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class SpriteRenderer : Component
    {
        public Texture2D Texture { get; private set; }
        SpriteBatch spriteBatch;
        Transform transform;

        public Point Size { get; private set; }
        public Rectangle SourceRect { get; set; }

        public SpriteRenderer(Entity _owner, SpriteBatch _spriteBatch, Texture2D _texture, Point _size) : base(_owner)
        {
            Texture = _texture;
            spriteBatch = _spriteBatch;
            Size = _size;

            owner.AddComponent<SpriteRenderer>(this);
        }

        public override void Initialize()
        {
            CheckRequiredComponents();
            transform = owner.GetComponent<Transform>() as Transform;
            SourceRect = new Rectangle(0,0, Size.X, Size.Y);
        }

        public override void Draw(float deltaTime)
        {
            Rectangle destinationRect = new Rectangle((int)transform.Position.X, (int)transform.Position.Y, Size.X, Size.Y);
            spriteBatch.Draw(Texture, destinationRect, SourceRect, Color.White);
        }

        public void SetTextureFrame(int _x, int _y)
        {
            SourceRect = new Rectangle(_x, _y, Size.X, Size.Y);
        }

        protected override void CheckRequiredComponents()
        {
            List<Type> requiredComponents = new List<Type>();
            requiredComponents.Add(typeof(Transform));

            foreach(Type requiredComponent in requiredComponents)
            {
                // TODO finish this later
            }
            //Console.Error.WriteLine(requiredTransform.Name + " was not found!");
        }
    }
}