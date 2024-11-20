using System;
using UnityEngine;
using MoreSlugcats;
using RWCustom;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace DutchTranslation
{
    public class ArenaStuff
    {
        public static void ApplyHooks()
        {
            On.MoreSlugcats.ChallengeInformation.GetOffset += MoreSlugcats_ChallengeInformation_GetOffset;
            On.Menu.LevelSelector.LevelItem.ctor += Translate_ArenaMaps;            
            IL.ArenaBehaviors.StartBump.Update += new ILContext.Manipulator(ArenaBehaviors_StartBump_Update);
            IL.Menu.PauseMenu.ctor += new ILContext.Manipulator(PauseMenu_ctor);
            IL.MoreSlugcats.ChallengeInformation.ctor += new ILContext.Manipulator(Translate_ChallengeNames);
        }

        public string Translate(string s)
        {
            return Custom.rainWorld.inGameTranslator.Translate(s);
        }

        private static void Translate_ArenaMaps(On.Menu.LevelSelector.LevelItem.orig_ctor orig, Menu.LevelSelector.LevelItem self, Menu.Menu menu, Menu.MenuObject owner, string name)
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

        private static void Translate_ChallengeNames(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            try
            {
                c.GotoNext(
                    MoveType.After,                    
                    x => x.MatchLdstr(": ")
                    );
                
                c.Emit(OpCodes.Ldarg, 0);
                c.Index += 3;
                c.Emit(OpCodes.Call, typeof(ArenaStuff).GetMethod(nameof(Translate)));
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Translating challenge names failed!");
            }
        } 
        
        public static void ArenaBehaviors_StartBump_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            
            try
            {
                //Translates challenge names
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

                //Translates arena names
                c.GotoNext(
                    MoveType.Before,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdfld(out _),                    
                    x => x.MatchCallvirt(out _),
                    x => x.MatchCall(out _)
                    );

                c.MoveAfterLabels();
                c.Emit(OpCodes.Ldarg, 0);
                c.Index += 5;
                c.Emit(OpCodes.Callvirt, typeof(ArenaStuff).GetMethod(nameof(Translate)));                                
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("StartBump_Update: ILHook failed!");
            }
        }       

        public static void PauseMenu_ctor(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            try 
            { 
                c.GotoNext(
                    MoveType.After,                    
                    x => x.MatchLdstr(": ")
                    );
                
                c.Emit(OpCodes.Ldarg, 2);
                c.Index += 4;
                c.Emit(OpCodes.Call, typeof(ArenaStuff).GetMethod(nameof(Translate)));
            }
            catch(Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("PauseMenu_ctor: ILHook failed!");
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

        public static void UnapplyHooks() 
        {
            On.MoreSlugcats.ChallengeInformation.GetOffset -= MoreSlugcats_ChallengeInformation_GetOffset;
            On.Menu.LevelSelector.LevelItem.ctor -= Translate_ArenaMaps;
            IL.ArenaBehaviors.StartBump.Update -= new ILContext.Manipulator(ArenaBehaviors_StartBump_Update);
            IL.Menu.PauseMenu.ctor -= new ILContext.Manipulator(PauseMenu_ctor);
            IL.MoreSlugcats.ChallengeInformation.ctor -= new ILContext.Manipulator(Translate_ChallengeNames);
        }
    }
}
