using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        //----------------------------------------------------------------------
        // On Screen Dancer
        //----------------------------------------------------------------------
        public class Dancer : ScreenSprite
        {
            string[] sprites;
            public void LoadSprites(string spriteData)
            {
                sprites = spriteData.Split(',');
                Data = sprites[0];
                spriteIndex = 0;
                spriteStep = 1;
            }
            int spriteIndex = 0;
            int spriteStep = 1;
            int maxSpriteIndex = 3;
            public float Combo { get { return maxSpriteIndex/sprites.Length; } set { maxSpriteIndex = (int)Math.Ceiling((float)sprites.Length*value); if (maxSpriteIndex < 3) maxSpriteIndex = 3; } }
            public void NextSprite()
            {
                spriteIndex += spriteStep;
                if (spriteIndex >= sprites.Length)
                {
                    spriteIndex = sprites.Length - 1;
                    spriteStep = -1;
                }
                else if (spriteIndex < 0)
                {
                    spriteIndex = 0;
                    spriteStep = 1;
                }
                Data = sprites[spriteIndex];
            }
            public Dancer(Vector2 position) : base(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, Color.White, "Monospace", "", TextAlignment.CENTER, SpriteType.TEXT)
            {
                GameSprites.LoadDancerSprites(this);
            }
        }
    }
}
