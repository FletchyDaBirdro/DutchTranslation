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
            //On.Menu.FastTravelScreen.FinalizeRegionSwitch += Remove_Subtitle;
            //IL.Menu.FastTravelScreen.FinalizeRegionSwitch += new ILContext.Manipulator(Remove_Shadow);
        }

        public static bool IsDutchTitle;
        private static void ReplaceLandscapeTitles(On.Menu.MenuIllustration.orig_ctor orig, Menu.MenuIllustration self, Menu.Menu menu, Menu.MenuObject owner, string folderName, string fileName, Vector2 pos, bool crispPixels, bool anchorCenter)
        {
            try
            {
                if (menu.CurrLang == LangID.LanguageID.Dutch)
                {
                    bool isTitle = fileName.StartsWith("Title") && string.IsNullOrEmpty(folderName);

                    if (isTitle)
                    {
                        string[] array = fileName.Split(
                        [
                            '_' 
                        ]);

                        bool isScene = array.Length >= 2;

                        if (isScene)
                        {
                            bool sceneNotReplaced = Region.GetRegionLandscapeScene(array[1]) != global::Menu.MenuScene.SceneID.Empty;

                            if (sceneNotReplaced)
                            {
                                string title = fileName + "_" + LocalizationTranslator.LangShort(LangID.LanguageID.Dutch);

                                string path = "Illustrations" + Path.DirectorySeparatorChar + title;

                                bool fileExists = File.Exists(AssetManager.ResolveFilePath(path + ".png"));

                                if (fileExists)
                                {
                                    folderName = "Illustrations";
                                    fileName = title;
                                    IsDutchTitle = true;
                                    MainPlugIn.TransLogger.LogDebug(title + " has been loaded!");
                                }
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

        /*private static void Remove_Shadow(ILContext il)
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
                    MoveType.Before,                    
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
               

                MainPlugIn.TransLogger.LogDebug(il.ToString());
            }
            catch (Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Removing shadow failed!");
            }
        }*/
    }
}
