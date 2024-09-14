using System;
using UnityEngine;
using MoreSlugcats;
using RWCustom;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;

namespace DutchTranslation
{
    public class ArenaStuff
    {
        public static void ApplyHooks()
        {
            On.MoreSlugcats.ChallengeInformation.GetOffset += MoreSlugcats_ChallengeInformation_GetOffset;
            On.MoreSlugcats.ChallengeInformation.ctor += TranslateChallengeNames;
            On.Menu.LevelSelector.LevelItem.ctor += TranslateArenaMaps;            
            IL.ArenaBehaviors.StartBump.Update += new ILContext.Manipulator(ArenaBehaviors_StartBump_Update);
            On.Menu.PauseMenu.ctor += PauseMenu_ctor;
        }       
        public string Translate(string s)
        {
            return Custom.rainWorld.inGameTranslator.Translate(s);
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
        public static void ArenaBehaviors_StartBump_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            
            try
            {                              
                c.GotoNext(
                    MoveType.Before,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdfld(out _)
                    );

                c.MoveAfterLabels();                
                c.Emit(OpCodes.Ldarg, 0);
                c.Index += 6;
                c.Emit(OpCodes.Callvirt, typeof(ArenaStuff).GetMethod(nameof(Translate)));
                //c.Emit(OpCodes.Stloc_0);
                
                MainPlugIn.TransLogger.LogMessage("ArenaStuff: ILHook succeeded!");
                MainPlugIn.TransLogger.LogDebug(il.ToString());
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("ArenaStuff: ILHook failed!");
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
