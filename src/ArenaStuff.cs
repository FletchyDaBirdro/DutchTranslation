using System;
using UnityEngine;
using MoreSlugcats;
using RWCustom;

namespace DutchTranslation
{
    public class ArenaStuff
    {
        public static void ApplyHooks()
        {
            On.MoreSlugcats.ChallengeInformation.GetOffset += MoreSlugcats_ChallengeInformation_GetOffset;
            On.MoreSlugcats.ChallengeInformation.ctor += TranslateChallengeNames;
            On.Menu.LevelSelector.LevelItem.ctor += TranslateArenaMaps;
            On.ArenaBehaviors.StartBump.Update += StartBump_Update;
            On.Menu.PauseMenu.ctor += PauseMenu_ctor;
        }
        
        public static InGameTranslator IGT
        {
            get
            {
                return Custom.rainWorld.inGameTranslator;
            }
        }

        private static void TranslateArenaMaps(On.Menu.LevelSelector.LevelItem.orig_ctor orig, Menu.LevelSelector.LevelItem self, Menu.Menu menu, Menu.MenuObject owner, string name)
        {
            orig(self, menu, owner, name);

            if (self.menu.CurrLang == LangID.LanguageID.Dutch)
                try
                {
                    self.subObjects.Remove(self.label);

                    self.label = new Menu.MenuLabel(menu, self, menu.Translate(MultiplayerUnlocks.LevelDisplayName(name)), new Vector2(0.01f, 0.01f), new Vector2(self.size.x, 20f), false, null);

                    self.subObjects.Add(self.label);
                }
                catch (Exception ex)
                {
                    MainPlugIn.TransLogger.LogError(ex);
                    MainPlugIn.TransLogger.LogMessage("Translating Arenas failed!");
                }
        }

        private static void TranslateChallengeNames(On.MoreSlugcats.ChallengeInformation.orig_ctor orig, ChallengeInformation self, Menu.Menu menu, Menu.MenuObject owner, int challengeID)
        {
            orig(self, menu, owner, challengeID);

            if (self.unlocked)
            {
                try
                {
                    self.subObjects.RemoveAt(0);

                    Menu.RoundedRect roundedRect = new Menu.RoundedRect(menu, self, self.posOffset, new Vector2(660f, 200f), false);

                    string name = "";

                    if (self.meta.name != null)
                    {
                        name = self.meta.name;
                    }

                    Menu.MenuLabel challenge = new Menu.MenuLabel(menu, self, menu.Translate("Challenge #<X>").Replace("<X>", challengeID.ToString()) + ": " + menu.Translate(name), new Vector2(roundedRect.pos.x + roundedRect.size.x / 2f, roundedRect.pos.y + roundedRect.size.y - 85f), new Vector2(0f, 100f), true, null);

                    self.subObjects.Add(challenge);
                    self.subObjects.Add(roundedRect);

                }
                catch (Exception ex)
                {
                    MainPlugIn.TransLogger.LogError(ex);
                    MainPlugIn.TransLogger.LogMessage("Translating challenge names failed!");
                }
            }
        }
        
        private static void PauseMenu_ctor(On.Menu.PauseMenu.orig_ctor orig, Menu.PauseMenu self, ProcessManager manager, RainWorldGame game)
        {
            orig(self, manager, game);
        }

        
        //this is supposed to translate the pop-up that appears when starting a challenge or entering a level
        //currently causes both the untranslated and the translated version to appear
        //needs to be fixed later
        private static void StartBump_Update(On.ArenaBehaviors.StartBump.orig_Update orig, ArenaBehaviors.StartBump self)
        {
            orig(self);

            MainPlugIn.TransLogger.LogDebug("ArenaStuff: 0");
            if (IGT.currentLanguage == LangID.LanguageID.Dutch)               
            try
            {               
                if (self.startGameCounter == 0)
                {                    
                    MainPlugIn.TransLogger.LogDebug("ArenaStuff: 1");
                    self.game.cameras[0].room.PlaySound(SoundID.UI_Multiplayer_Game_Start, 0f, 1f, 1f);                    

                    if (ModManager.MSC && self.gameSession.arenaSitting.gameTypeSetup.gameType == MoreSlugcatsEnums.GameTypeID.Challenge)
                    {
                        MainPlugIn.TransLogger.LogDebug("ArenaStuff: 2");
                        self.game.cameras[0].hud.textPrompt.AddMessage(IGT.Translate(self.gameSession.arenaSitting.gameTypeSetup.challengeMeta.name), 20, 160, false, true);
                        MainPlugIn.TransLogger.LogDebug("ArenaStuff: 3");
                    }
                    else if (self.gameSession.arenaSitting.ShowLevelName)
                    {
                        MainPlugIn.TransLogger.LogDebug("ArenaStuff: 4");
                        self.game.cameras[0].hud.textPrompt.AddMessage(IGT.Translate(MultiplayerUnlocks.LevelDisplayName(self.gameSession.arenaSitting.GetCurrentLevel)), 20, 160, false, true);
                        MainPlugIn.TransLogger.LogDebug("ArenaStuff: 5");
                    }
                    self.Destroy();
                }                
            }                
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Translating StartBump message failed!");
            }            
        }

        private static void MoreSlugcats_ChallengeInformation_GetOffset(On.MoreSlugcats.ChallengeInformation.orig_GetOffset orig, ChallengeInformation self, ref float creatureOffset, ref float itemOffset)
        {
            orig(self, ref creatureOffset, ref itemOffset);

            try
            {
                if (self.menu.CurrLang == LangID.LanguageID.Dutch)
                {
                    itemOffset = 90f;
                    return;
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Items didn't offset.");
            }
        }
    }
}
