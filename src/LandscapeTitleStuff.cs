using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTranslation;
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
                if (menu.manager.rainWorld.inGameTranslator.currentLanguage == LangID.LanguageID.Dutch)
                {
                    bool isTitle = fileName.StartsWith("Title") && string.IsNullOrEmpty(folderName);

                    if (isTitle)
                    {
                        string[] array = fileName.Split(new char[] { '_' });

                        bool flag = array.Length >= 2;

                        if (flag)
                        {
                            bool isLandscapeScene = Region.GetRegionLandscapeScene(array[1]) != global::Menu.MenuScene.SceneID.Empty;

                            if (isLandscapeScene)
                            {
                                string title = string.Format("{0}_{1}", fileName, "dut");

                                bool fileExists = File.Exists(AssetManager.ResolveFilePath(string.Format("Illustrations{0}{1}.png", Path.DirectorySeparatorChar, title)));

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
                    self.pages[1].subObjects.Remove(self.subtitleLabel);
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
