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
        public class ArrowColumn : IScreenSpriteProvider
        {
            float beatHeight = 0.5f;
            ScreenSprite target;
            ScreenSprite pressed;
            ScreenSprite hit;
            ScreenSprite hitRating;
            Vector2 hitOffset = new Vector2(0f,-15f);
            Vector2 hitRatingOffset = new Vector2(0f, 30f);
            List<ScreenSprite> arrows = new List<ScreenSprite>();
            List<float> arrowPositions = new List<float>();
            Screen screen;
            Color color = Color.White;
            char direction;
            int hitStep = 0;
            int hitStepDelay = 0;
            int hitStepDelayThreshold = 5;
            int pressDelay = 0;
            int pressDelayThreshold = 10;
            int missDelay = 0;
            int missDelayThreshold = 10;
            bool miss = false;
            PlayerScoreDisplay scoreDisplay;
            public Vector2 Position { get { return target.Position; } set { target.Position = value; pressed.Position = value; hit.Position = value + hitOffset; } }
            public bool Visible { get { return target.Visible; } set { target.Visible = value; pressed.Visible = value; hit.Visible = value; hitRating.Visible = value; foreach (ScreenSprite arrow in arrows) arrow.Visible = value; } }
            public ArrowColumn(char dir, Vector2 position, Color color, PlayerScoreDisplay scoreDisplay)
            {
                direction = dir;
                target = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, Color.White, "Monospace", GameSprites.arrows[dir], TextAlignment.CENTER, SpriteType.TEXT);
                pressed = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, color, "Monospace", GameSprites.pressed[dir], TextAlignment.CENTER, SpriteType.TEXT);
                hit = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position + hitOffset, 0.1f, Vector2.Zero, Color.White, "Monospace", GameSprites.hits[dir][hitStep], TextAlignment.CENTER, SpriteType.TEXT);
                hitRating = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position + hitRatingOffset, 0.075f, Vector2.Zero, Color.White, "Monospace", "", TextAlignment.CENTER, SpriteType.TEXT);
                this.color = color;
                pressed.Visible = false;
                hit.Visible = false;
                this.scoreDisplay = scoreDisplay;
            }
            public void ClearArrows()
            {
                foreach (ScreenSprite arrow in arrows)
                {
                    screen.RemoveSprite(arrow);
                }
                arrows.Clear();
                arrowPositions.Clear();
            }
            // add an arrow to the column
            public void AddArrow(float percent)
            {
                ScreenSprite arrow = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(target.Position.X, target.Position.Y + (screen.Size.Y*beatHeight*DanceGame.songLength * percent)), 0.1f, Vector2.Zero, color, "Monospace", target.Data, TextAlignment.CENTER, SpriteType.TEXT);
                arrows.Add(arrow);
                arrowPositions.Add(percent);
                screen.AddSprite(arrow);
                arrow.Visible = Visible;
            }
            public void AddArrowOnBeat(int beat)
            {
                AddArrow((float)beat * DanceGame.OneBeatIsPercentOfSong);
            }
            public void Scroll(float percent)
            {
                for (int i = 0; i < arrows.Count; i++)
                {
                    arrows[i].Position = new Vector2(arrows[i].Position.X, target.Position.Y + (screen.Size.Y * beatHeight * DanceGame.songLength * (arrowPositions[i] - percent)));
                    if (arrows[i].Position.Y < target.Position.Y - hitRange)
                    {
                        screen.RemoveSprite(arrows[i]);
                        arrows.RemoveAt(i);
                        arrowPositions.RemoveAt(i);
                        i--;
                        hitRating.Data = GameSprites.combo["miss"];
                        screen.RemoveSprite(hitRating);
                        screen.AddSprite(hitRating);
                        missDelay = missDelayThreshold;
                        miss = true;
                        scoreDisplay.Combo *= 0.95f;
                    }
                }
                if (hit.Visible && hitStepDelay-- < 0)
                {
                    hitStep++;
                    if (hitStep >= GameSprites.hits[direction].Length)
                    {
                        hitStep = 0;
                        hit.Visible = false;
                        screen.RemoveSprite(hit);
                        screen.RemoveSprite(hitRating);
                    }
                    else
                    {
                        hit.Data = GameSprites.hits[direction][hitStep];
                        hitStepDelay = hitStepDelayThreshold;
                    }
                }
                if (pressed.Visible && pressDelay-- < 0)
                {
                    pressed.Visible = false;
                }
                if (miss && hitRating.Visible && missDelay-- < 0)
                {
                    screen.RemoveSprite(hitRating);
                    miss = false;
                }
            }
            float hitRange = 75f;
            float perfectHitRange = 0.1f; // percent of hit range
            float greatHitRange = 0.35f;
            float goodHitRange = 1f;
            public void Press()
            {
                pressed.Visible = true;
                pressDelay = pressDelayThreshold;
                // see if one of the first 5 arrows is in the hit zone
                for (int i = 0; i < arrows.Count && i < 5; i++)
                {
                    float  dist = Math.Abs(Vector2.Distance(arrows[i].Position, target.Position));
                    if (dist < hitRange)
                    {
                        // only shoe effect if it's a on ok hit or better

                        if (dist < hitRange * goodHitRange)
                        {
                            hitStep = 0;
                            hit.Data = GameSprites.hits[direction][hitStep];
                            if (dist < hitRange * perfectHitRange)
                            {
                                // perfect hit
                                hit.Color = Color.Gold;
                                hitRating.Data = GameSprites.combo["perfect"];
                                scoreDisplay.Combo += 0.1f;
                                scoreDisplay.Score += (long)(1000000 * scoreDisplay.Combo) +99;
                            }
                            else if (dist < hitRange * greatHitRange)
                            {
                                // great hit
                                hit.Color = Color.LimeGreen;
                                hitRating.Data = GameSprites.combo["great"];
                                scoreDisplay.Combo += 0.05f;
                                scoreDisplay.Score += (long)(100000 * scoreDisplay.Combo) + 66;
                            }
                            else
                            {
                                // good hit
                                hit.Color = Color.LightCyan;
                                hitRating.Data = GameSprites.combo["good"];
                                scoreDisplay.Combo += 0.025f;
                                scoreDisplay.Score += (long)(10000 * scoreDisplay.Combo) + 33;
                            }
                            hit.Visible = Visible;
                            screen.RemoveSprite(hit);
                            screen.AddSprite(hit);
                            hitStepDelay = hitStepDelayThreshold;
                        }
                        screen.RemoveSprite(hitRating);
                        screen.AddSprite(hitRating);
                        screen.RemoveSprite(arrows[i]);
                        arrows.RemoveAt(i);
                        arrowPositions.RemoveAt(i);
                        break;
                    }
                }
            }
            public void AddToScreen(Screen screen)
            {
                this.screen = screen;
                screen.AddSprite(target);
                screen.AddSprite(pressed);
                screen.AddSprite(hit);
                foreach (ScreenSprite arrow in arrows)
                {
                    screen.AddSprite(arrow);
                }
            }
            public void RemoveFromScreen(Screen screen)
            {
                screen.RemoveSprite(target);
                foreach (ScreenSprite arrow in arrows)
                {
                    screen.RemoveSprite(arrow);
                }
            }
        }
    }
}
