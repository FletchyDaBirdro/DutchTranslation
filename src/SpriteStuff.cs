using System.IO;
using UnityEngine;
using System;

namespace DutchTranslation
{
    public class SpriteStuff
    {
        public static void InitExpSprites()
        {
            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("expeditiontitle_dut"))
                {
                    Texture2D expTitle = new Texture2D(0, 0);
                    expTitle.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "expeditiontitle_dut.png")));
                    expTitle.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("expeditiontitle_dut", expTitle, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 1");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("expeditionshadow_dut"))
                {
                    Texture2D expShadow = new Texture2D(0, 0);
                    expShadow.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "expeditionshadow_dut.png")));
                    expShadow.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("expeditionshadow_dut", expShadow, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 2");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("expeditionpage_dut"))
                {
                    Texture2D expPage = new Texture2D(0, 0);
                    expPage.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "expeditionpage_dut.png")));
                    expPage.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("expeditionpage_dut", expPage, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 3");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("expchallengeselect_dut"))
                {
                    Texture2D chalSelect = new Texture2D(0, 0);
                    chalSelect.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "expchallengeselect_dut.png")));
                    chalSelect.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("expchallengeselect_dut", chalSelect, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 4");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("unlockables_dut"))
                {
                    Texture2D unlocks = new Texture2D(0, 0);
                    unlocks.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "unlockables_dut.png")));
                    unlocks.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("unlockables_dut", unlocks, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 5");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("progression_dut"))
                {
                    Texture2D progress = new Texture2D(0, 0);
                    progress.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "progression_dut.png")));
                    progress.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("progression_dut", progress, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 6");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("milestones_dut"))
                {
                    Texture2D mileStones = new Texture2D(0, 0);
                    mileStones.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "milestones_dut.png")));
                    mileStones.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("milestones_dut", mileStones, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 7");
            }

            try
            {
                if (!Futile.atlasManager.DoesContainElementWithName("mission_dut"))
                {
                    Texture2D mileStones = new Texture2D(0, 0);
                    mileStones.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations" + Path.DirectorySeparatorChar + "mission_dut.png")));
                    mileStones.filterMode = FilterMode.Point;
                    Futile.atlasManager.LoadAtlasFromTexture("mission_dut", mileStones, false);
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Failed initializing sprite! 7");
            }
        }

        public static void UnloadExpSprites()
        {
            if (Futile.atlasManager.DoesContainElementWithName("expeditiontitle_dut"))
            {
                Futile.atlasManager.UnloadAtlas("expeditiontitle_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("expeditionshadow_dut"))
            {
                Futile.atlasManager.UnloadAtlas("expeditionshadow_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("expeditionpage_dut"))
            {
                Futile.atlasManager.UnloadAtlas("expeditionpage_dut");
            }            
            if (Futile.atlasManager.DoesContainElementWithName("expchallengeselect_dut"))
            {
                Futile.atlasManager.UnloadAtlas("expchallengeselect_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("unlockables_dut"))
            {
                Futile.atlasManager.UnloadAtlas("unlockables_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("progression_dut"))
            {
                Futile.atlasManager.UnloadAtlas("progression_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("milestones_dut"))
            {
                Futile.atlasManager.UnloadAtlas("milestones_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("mission_dut"))
            {
                Futile.atlasManager.UnloadAtlas("mission_dut");
            }
        }
    }
}
