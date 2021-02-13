using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
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
    partial class Program : MyGridProgram
    {


        List<IMyShipDrill> d = new List<IMyShipDrill>();
        IMyPistonBase piston;
        IMyMotorAdvancedStator rotor_up, rotor_down;

        String currentStep;
        public Program()
        { 
            IMyBlockGroup drills = GridTerminalSystem.GetBlockGroupWithName("drills");
            drills.GetBlocksOfType<IMyShipDrill>(d, drill => drill.Enabled = true);
            IMyMotorAdvancedStator rotor_up, rotor_down;
            
            piston = GridTerminalSystem.GetBlockWithName("p") as IMyPistonBase;
            piston.MaxLimit = 8.5f;
            rotor_up = GridTerminalSystem.GetBlockWithName("rotor oben") as IMyMotorAdvancedStator;
            rotor_down = GridTerminalSystem.GetBlockWithName("rotor unten") as IMyMotorAdvancedStator;

            currentStep = "STEP_1";

            if (piston.CurrentPosition < piston.MaxLimit)
                piston.Velocity = 1f;
        }

        public void Save()
        {

        }

        public void Main()
        {
            Echo("Current Stage: " + currentStep);

            switch (currentStep)
            {
                case "STEP_1":
                    if(rotor_up.IsAttached)
                    {
                        piston.Velocity = -0.01f;

                        if (piston.CurrentPosition == 0)
                            currentStep = "STEP_2";
                    }
                break;

                case "STEP_2":
                    if(rotor_down.IsAttached)
                    {
                        currentStep = "STEP_3";
                    }
                    else
                    {
                        rotor_down.Attach();
                    }
                break;
                
                case "STEP_3":
                    if(rotor_up.IsAttached)
                    {
                        rotor_up.Detach();
                    }
                    else
                    {
                        currentStep = "STEP_4";
                    }
                break;

                case "STEP_4":
                    if (piston.CurrentPosition < piston.MaxLimit)
                        piston.Velocity = 1f;
                    else
                        currentStep = "STEP_5";
                break;

                case "STEP_5":
                    if (rotor_up.IsAttached)
                    {
                        currentStep = "STEP_6";
                    }
                    else
                    {
                        rotor_up.Attach();
                    }

                break;

                case "STEP_6":
                    if (rotor_down.IsAttached)
                        rotor_down.Detach();
                    else
                        currentStep = "STEP_1";
                break;
            }
        }
    }
}
