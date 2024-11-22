using System;
using System.IO;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;


namespace DutchTranslation
{
    public class LandscapeTitleStuff
    {
        public static void ApplyHooks()
        {
            On.Menu.MenuIllustration.ctor += ReplaceLandscapeTitles;
            On.Menu.FastTravelScreen.AddWorldLoaderResultToLoadedWorlds += FastTravelScreen_AddWorldLoaderResultToLoadedWorlds;
            IL.Menu.FastTravelScreen.FinalizeRegionSwitch += new ILContext.Manipulator(Remove_Shadow);
            On.Menu.MultiplayerMenu.ClearGameTypeSpecificButtons += MultiplayerMenu_ClearGameTypeSpecificButtons;
        }

        public static bool IsDutchTitle;

        public static Menu.MultiplayerMenu MultiMenu;
        
        private static void ReplaceLandscapeTitles(On.Menu.MenuIllustration.orig_ctor orig, Menu.MenuIllustration self, Menu.Menu menu, Menu.MenuObject owner, string folderName, string fileName, Vector2 pos, bool crispPixels, bool anchorCenter)
        {
            try
            {
                if (menu.CurrLang == LangID.LanguageID.Dutch)
                {                                  
                    if (((ModManager.JollyCoop && fileName.StartsWith("jolly_title")) || (ModManager.MSC && fileName.StartsWith("Challenge")) || fileName.StartsWith("Competitive") || fileName.StartsWith("Title")) && string.IsNullOrEmpty(folderName) || (ModManager.JollyCoop || ModManager.Expedition) && fileName.StartsWith("manual"))
                    {
                        string[] array = fileName.Split(
                        [
                            '_' 
                        ]);                        

                        if (array.Length >= 2)
                        {
                            string title = fileName + "_" + LocalizationTranslator.LangShort(LangID.LanguageID.Dutch);

                            string path = "Illustrations" + Path.DirectorySeparatorChar + title;

                            if (File.Exists(AssetManager.ResolveFilePath(path + ".png")))
                            {
                                folderName = "Illustrations";
                                fileName = title;
                                IsDutchTitle = true;
                                //MainPlugIn.TransLogger.LogDebug(title + " has been loaded!");
                            }

                            if (Region.GetRegionLandscapeScene(array[1]) != global::Menu.MenuScene.SceneID.Empty)
                            {
                                title = fileName + "_" + LocalizationTranslator.LangShort(LangID.LanguageID.Dutch);

                                path = "Illustrations" + Path.DirectorySeparatorChar + title;

                                if (File.Exists(AssetManager.ResolveFilePath(path + ".png")))
                                {
                                    folderName = "Illustrations";
                                    fileName = title;
                                    IsDutchTitle = true;
                                    //MainPlugIn.TransLogger.LogDebug(title + " has been loaded!");
                                }
                            }                            
                        }
                        else
                        {
                            string title = fileName + "_" + LocalizationTranslator.LangShort(LangID.LanguageID.Dutch);
                            string path = "Illustrations" + Path.DirectorySeparatorChar + title;

                            if (File.Exists(AssetManager.ResolveFilePath(path + ".png")))
                            {
                                folderName = "Illustrations";
                                fileName = title;
                                //MainPlugIn.TransLogger.LogDebug(title + " has been loaded!");
                            }

                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed replacing the titles!");
            }
            orig(self, menu, owner, folderName, fileName, pos, crispPixels, anchorCenter);
        }       

        private static void FastTravelScreen_AddWorldLoaderResultToLoadedWorlds(On.Menu.FastTravelScreen.orig_AddWorldLoaderResultToLoadedWorlds orig, Menu.FastTravelScreen self, int reg)
        {
            IsDutchTitle = false;
            orig(self, reg);
        }

        private static void Remove_Shadow(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel label = c.DefineLabel();

            try
            {
                c.GotoNext(
                    MoveType.Before,
                    x => x.MatchRet()
                );

                label = c.MarkLabel();

                c.GotoPrev(
                    MoveType.After,                    
                    x => x.MatchLdsfld(typeof(SlugcatStats.Name).GetField(nameof(SlugcatStats.Name.White))),
                    x => x.MatchCallOrCallvirt(out _),
                    x => x.MatchCallOrCallvirt(typeof(System.String).GetMethod("op_Inequality")),
                    x => x.MatchBrfalse(out _)
                    );

                c.MoveAfterLabels();               
                c.Emit(OpCodes.Ldarg, 0);
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldsfld, typeof(LandscapeTitleStuff).GetField(nameof(IsDutchTitle)));
                c.Emit(OpCodes.Brtrue, label);
               
                //MainPlugIn.TransLogger.LogDebug(il.ToString());
            }
            catch (Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Removing shadow failed!");
            }
        }

        private static void MultiplayerMenu_ClearGameTypeSpecificButtons(On.Menu.MultiplayerMenu.orig_ClearGameTypeSpecificButtons orig, Menu.MultiplayerMenu self)
        {
            orig(self);

            for (int i = self.scene.flatIllustrations.Count - 1; i >= 0; i--) 
            {
                if (self.scene.flatIllustrations[i].fileName.ToLowerInvariant().EndsWith("dut"))
                {
                    self.scene.flatIllustrations[i].RemoveSprites();
                    self.scene.RemoveSubObject(self.scene.flatIllustrations[i]);
                }
            }
        }

        public static void UnapplyHooks() 
        {
            On.Menu.MenuIllustration.ctor -= ReplaceLandscapeTitles;
            On.Menu.FastTravelScreen.AddWorldLoaderResultToLoadedWorlds -= FastTravelScreen_AddWorldLoaderResultToLoadedWorlds;
            IL.Menu.FastTravelScreen.FinalizeRegionSwitch -= new ILContext.Manipulator(Remove_Shadow);
            On.Menu.MultiplayerMenu.ClearGameTypeSpecificButtons -= MultiplayerMenu_ClearGameTypeSpecificButtons;
        }
    }
}
