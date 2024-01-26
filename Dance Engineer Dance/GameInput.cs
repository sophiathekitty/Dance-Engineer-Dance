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
        // GameInput
        //----------------------------------------------------------------------
        public class GameInput
        {
            IMyShipController controller;
            IMySensorBlock playerSensor;
            IMySensorBlock wSensor;
            IMySensorBlock aSensor;
            IMySensorBlock sSensor;
            IMySensorBlock dSensor;
            public GameInput(IMyShipController controller)
            {
                this.controller = controller;
            }
            public GameInput(IMyShipController controller, IMySensorBlock playerSensor, IMySensorBlock wSensor, IMySensorBlock aSensor, IMySensorBlock sSensor, IMySensorBlock dSensor)
            {
                this.controller = controller;
                this.playerSensor = playerSensor;
                this.wSensor = wSensor;
                this.aSensor = aSensor;
                this.sSensor = sSensor;
                this.dSensor = dSensor;
            }
            // buttons pressed
            public bool W { get { 
                    if(controller.IsUnderControl) return controller.MoveIndicator.Z < 0;
                    if(playerSensor != null && wSensor != null) return playerSensor.IsActive && wSensor.IsActive;
                    return false;
                } }
            public bool S { get {
                    if (controller.IsUnderControl) return controller.MoveIndicator.Z > 0;
                    if (playerSensor != null && sSensor != null) return playerSensor.IsActive && sSensor.IsActive;
                    return false;
                } }
            public bool A { get {
                    if (controller.IsUnderControl) return controller.MoveIndicator.X < 0;
                    if (playerSensor != null && aSensor != null) return playerSensor.IsActive && aSensor.IsActive;
                    return false;
                } }
            public bool D { get {
                    if (controller.IsUnderControl) return controller.MoveIndicator.X > 0;
                    if (playerSensor != null && dSensor != null) return playerSensor.IsActive && dSensor.IsActive;
                    return false;
                } }
            public bool Wpad { get { return wSensor.IsActive; } }
            public bool Apad { get { return aSensor.IsActive; } }
            public bool Spad { get { return sSensor.IsActive; } }
            public bool Dpad { get { return dSensor.IsActive; } }
            public bool Space { get { return controller.MoveIndicator.Y > 0; } }
            public bool C { get { return controller.MoveIndicator.Y < 0; } }
            public bool E { get { return controller.RollIndicator > 0; } }
            public bool Q { get { return controller.RollIndicator < 0; } }
            public bool LookLeft { get { return controller.RotationIndicator.Y < 0; } }
            public bool LookRight { get { return controller.RotationIndicator.Y > 0; } }
            public bool LookUp { get { return controller.RotationIndicator.X < 0; } }
            public bool LookDown { get { return controller.RotationIndicator.X > 0; } }
            public Vector2 WASD { get { return new Vector2(controller.MoveIndicator.X, controller.MoveIndicator.Z); } }
            public Vector2 Mouse { get { return new Vector2(controller.RotationIndicator.Y, controller.RotationIndicator.X); } }
            int playerJoined = 0;
            int playerJoinedThreshold = 30;
            public bool PlayerPresent 
            { 
                get 
                { 
                    if(controller.IsUnderControl) return true;
                    if(playerSensor.IsActive && !W && !A && !S && !D) playerJoined++; 
                    else if(playerSensor.IsActive && playerJoined == 0) playerJoined = 1;
                    else if(!playerSensor.IsActive) playerJoined--;
                    playerJoined = Math.Max(0, playerJoined);
                    playerJoined = Math.Min(playerJoinedThreshold, playerJoined);
                    return (playerJoined >= playerJoinedThreshold);
                }
            }
            public bool PlayerJoining { get { return playerJoined > 0 && playerSensor.IsActive; } }
            public bool PlayerLeaving { get { return playerJoined > 0 && !playerSensor.IsActive; } }
            public IMyTextSurface GetSurface(int index)
            {
                IMyTextSurfaceProvider provider = controller as IMyTextSurfaceProvider;
                if (provider != null)
                {
                    return provider.GetSurface(index);
                }
                return null;
            }
            bool lastW = false;
            bool lastA = false;
            bool lastS = false;
            bool lastD = false;
            public bool WPressed { get { bool pressed = W && !lastW; lastW = W; return pressed; } }
            public bool APressed { get { bool pressed = A && !lastA; lastA = A; return pressed; } }
            public bool SPressed { get { bool pressed = S && !lastS; lastS = S; return pressed; } }
            public bool DPressed { get { bool pressed = D && !lastD; lastD = D; return pressed; } }
        }
        //----------------------------------------------------------------------
    }
}
