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
        public class FloorArrow : Screen
        {
            ScreenSprite arrow;
            Color color;
            public FloorArrow(IMyTextSurface drawingSurface,Color color,string sprite) : base(drawingSurface)
            {
                BackgroundColor = Color.Black;
                this.color = color;
                arrow = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter,new Vector2(0f,10f),0.5f,Vector2.Zero,color,"Monospace",sprite,TextAlignment.CENTER,SpriteType.TEXT);
                AddSprite(arrow);
            }
            public bool Active 
            { 
                set 
                {
                    if(value) arrow.Color = color;
                    else arrow.Color = Color.Gray;
                } 
            }
            override public void Draw()
            {
                base.Draw();
            }
        }
    }
}
