using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class NineSpliceSprite : UIComponent
    {
        SpriteBatch spriteBatch;
        Texture2D texture;

        public enum ExpandMode { STRECH, TILE }
        ExpandMode horizontalExpand;
        ExpandMode verticalExpand;
        ExpandMode centerExpand;

        NineSpliceSpriteSection[] sections = new NineSpliceSpriteSection[9];

        public Rectangle Destination { get; private set; }
        public int SpliceWidth { get; private set; }
        public int SpliceHeight { get; private set; }
        public int HorizontalWidth { get; private set; }
        public int VerticalHeight { get; private set; }

        public NineSpliceSprite(UIEntity _owner) : base(_owner)
        {// Add in parameters for stretch
            spriteBatch = owner.SpriteBatch;
        }

        public void Initialize(Texture2D _texture, Rectangle _destination)
        {
            Destination = new Rectangle(_destination.X + owner.DestinationRect.X, _destination.Y + owner.DestinationRect.Y, _destination.Width, _destination.Height);
            texture = _texture;

            SpliceWidth = texture.Width / 3;
            SpliceHeight = texture.Height / 3;

            HorizontalWidth = _destination.Width - (SpliceWidth * 2);
            VerticalHeight = _destination.Height - (SpliceHeight * 2);

            SetSections();
        }

        public override void Draw(float deltaTime)
        {
            if (!IsVisible)
                return;
                
            //TODO: Right now this only works for stretched expand, for tiled expand it will be a bit more complicated.
            foreach (NineSpliceSpriteSection section in sections)
            {
                spriteBatch.Draw(texture, section.DestinationRect, section.SourceRect, Color.White);

                /*
                Check to see the expand type.
                if tiled then don't just stretch it to the destination rect but run a for loop with a length set as tiled amount
                this will need to be done for both directions depending on section.
                It is possible most of this can be handled from the section class
                */
            }
        }

        ///<summary>
        /// Splits the texture into 9 equal parts, then creates the sections based upon the source rectangles of the texture
        ///</summary>
        private NineSpliceSpriteSection[] SetSections()
        {
            NineSpliceSpriteSection[] tempSections = new NineSpliceSpriteSection[9];

            int currentRect = 0;

            for (int y = 0; y < 3; ++y)
            {
                for (int x = 0; x < 3; ++x)
                {
                    Rectangle sourceRect = new Rectangle(x * SpliceWidth, y * SpliceHeight, SpliceWidth, SpliceHeight);
                    sections[currentRect] = new NineSpliceSpriteSection(this, currentRect, sourceRect);
                    currentRect++;
                }
            }

            return tempSections;
        }

        public class NineSpliceSpriteSection
        {
            public enum Section { TOP_LEFT, TOP, TOP_RIGHT, LEFT, CENTER, RIGHT, BOTTOM_LEFT, BOTTOM, BOTTOM_RIGHT }
            public Section SelectedSection { get; set; }

            NineSpliceSprite parent;
            public Rectangle SourceRect { get; set; }
            public Rectangle DestinationRect { get; set; }

            public NineSpliceSpriteSection(NineSpliceSprite _parent, int _currentRect, Rectangle _sourceRect)
            {
                parent = _parent;
                SourceRect = _sourceRect;
                SelectedSection = (Section)_currentRect;
                DestinationRect = SetDestinationRect();

            }

            ///<summary>
            /// Sets the DestinationRect of the section based upon section location (top_left, top_right, etc) and the NineSpliceSprites final destination
            ///</summary>
            private Rectangle SetDestinationRect()
            {
                int startingPosX = 0;
                int startingPosY = 0;
                int width = 0;
                int height = 0;

                if (SelectedSection == Section.TOP_LEFT)
                {
                    startingPosX = parent.Destination.X;
                    startingPosY = parent.Destination.Y;
                    width = parent.SpliceWidth;
                    height = parent.SpliceHeight;
                }

                else if (SelectedSection == Section.TOP)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth;
                    startingPosY = parent.Destination.Y;
                    width = parent.HorizontalWidth;
                    height = parent.SpliceHeight;
                }

                else if (SelectedSection == Section.TOP_RIGHT)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth + parent.HorizontalWidth;
                    startingPosY = parent.Destination.Y;
                    width = parent.SpliceWidth;
                    height = parent.SpliceHeight;
                }

                else if (SelectedSection == Section.LEFT)
                {
                    startingPosX = parent.Destination.X;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight;
                    width = parent.SpliceWidth;
                    height = parent.VerticalHeight;
                }

                else if (SelectedSection == Section.CENTER)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight;
                    width = parent.HorizontalWidth;
                    height = parent.VerticalHeight;
                }

                else if (SelectedSection == Section.RIGHT)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth + parent.HorizontalWidth;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight;
                    width = parent.SpliceWidth;
                    height = parent.VerticalHeight;
                }
                
                else if (SelectedSection == Section.BOTTOM_LEFT)
                {
                    startingPosX = parent.Destination.X;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight + parent.VerticalHeight;
                    width = parent.SpliceWidth;
                    height = parent.SpliceHeight;
                }

                else if (SelectedSection == Section.BOTTOM)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight + parent.VerticalHeight;
                    width = parent.HorizontalWidth;
                    height = parent.SpliceHeight;
                }

                else if (SelectedSection == Section.BOTTOM_RIGHT)
                {
                    startingPosX = parent.Destination.X + parent.SpliceWidth + parent.HorizontalWidth;
                    startingPosY = parent.Destination.Y + parent.SpliceHeight + parent.VerticalHeight;
                    width = parent.SpliceWidth;
                    height = parent.SpliceHeight;
                }

                Rectangle tempDestinationRect = new Rectangle(startingPosX, startingPosY, width, height);
                return tempDestinationRect;
            }
        }
    }
}