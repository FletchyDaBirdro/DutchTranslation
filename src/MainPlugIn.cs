﻿using BepInEx;
using System;
using BepInEx.Logging;

namespace DutchTranslation
{
    [BepInPlugin("fdb.dutchrw", "Dutch Translation", "0.5.1")]
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
            GeneralStuff.ApplyILHook();            
        }        
        public void RainWorld_OnOnModsInit(On.RainWorld.orig_OnModsInit orig, global::RainWorld self)
        {
            orig(self);

            try
            {
                if (_Initialized) return;
                _Initialized = true;               
                
                GeneralStuff.ApplyHooks();
                ArenaStuff.ApplyHooks();
                LandscapeTitleStuff.ApplyHooks();
                ExpeditionStuff.ApplyHooks();
                TransLogger.LogInfo("Applied hooks!");
                
                SpriteStuff.InitExpSprites();
                TransLogger.LogInfo("Initialized sprites!");

                TransLogger.LogInfo("Mod initialized!");
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

        public void OnDestroy()
        {
            GeneralStuff.UnapplyHooks();
            ArenaStuff.UnapplyHooks();
            LandscapeTitleStuff.UnapplyHooks();
            ExpeditionStuff.UnapplyHooks();

            SpriteStuff.UnloadExpSprites();

            LangID.UnregisterValues();
        }
    }
}            