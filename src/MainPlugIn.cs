using BepInEx;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MonoMod.Cil;
using System.Reflection.Emit;
using Mono.Cecil.Cil;
using Mono.Cecil;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;
using System.CodeDom;
using RWCustom;
using MoreSlugcats;
using IL.Menu;
using Menu;
using BepInEx.Logging;

namespace DutchTranslation
{
    [BepInPlugin("fdb.dutchrw", "Dutch Translation", "0.5.0")]
    public class MainPlugIn : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "fdb.dutchrw";
        public const string PLUGIN_NAME = "Dutch Translation";

        public static new ManualLogSource TransLogger { get; private set; } = null!;

        static bool _Initialized;        
        
        public void LogInfo(object ex)
        {
            TransLogger.LogInfo(ex);
        }   
        
        public void Awake()
        {
            LangID.RegisterValues();
        }

        public void OnEnable()
        {
            TransLogger = base.Logger;

            On.RainWorld.OnModsInit += RainWorld_OnOnModsInit;
            On.RainWorld.OnModsDisabled += RainWorld_OnModsDisabled;

            GeneralStuff.ApplyHooks();
        }

        public void RainWorld_OnOnModsInit(On.RainWorld.orig_OnModsInit orig, global::RainWorld self)
        {
            orig(self);

            try
            {
                if (_Initialized) return;
                _Initialized = true;

                TransLogger.LogInfo("Mod initialized!");
                
                LandscapeTitleStuff.ApplyHooks();
                ArenaStuff.ApplyHooks();
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Uh oh, stinky.");
            }
            finally
            {
                orig.Invoke(self);
            }
        } 
                                                                                                   
        public void RainWorld_OnModsDisabled(On.RainWorld.orig_OnModsDisabled orig, global::RainWorld self, global::ModManager.Mod[] newlyDisabledMods)
        {
            orig(self, newlyDisabledMods);            
        }

        public void OnDestroy()
        {
            LangID.UnregisterValues();
        }
    }
}            