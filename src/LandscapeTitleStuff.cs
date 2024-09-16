using System;
using System.IO;
using UnityEngine;

namespace DutchTranslation
{
    public static class LandscapeTitleStuff
    {
        public static void ApplyHooks()
        {
            On.Menu.MenuIllustration.ctor += ReplaceLandscapeTitles;
            On.Menu.FastTravelScreen.AddWorldLoaderResultToLoadedWorlds += FastTravelScreen_AddWorldLoaderResultToLoadedWorlds;
            On.Menu.FastTravelScreen.FinalizeRegionSwitch += FastTravelScreen_FinalizeRegionSwitch;
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

        private static void FastTravelScreen_FinalizeRegionSwitch(On.Menu.FastTravelScreen.orig_FinalizeRegionSwitch orig, Menu.FastTravelScreen self, int newRegion)
        {
            orig(self, newRegion);

            try
            {
                if (IsDutchTitle)
                {                    
                    self.pages[1].RemoveSubObject(self.subtitleLabel);                    
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Something something display name failed!");
            }
        }
    }
}
