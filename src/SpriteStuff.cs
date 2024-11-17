using System.IO;
using UnityEngine;

namespace DutchTranslation
{
    public class SpriteStuff
    {
        public static void InitExpSprites()
        {
            if (!Futile.atlasManager.DoesContainElementWithName("expeditiontitle_dut"))
            {
                Texture2D expTitle = new Texture2D(0, 0);
                expTitle.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/expeditiontitle_dut.png")));
                expTitle.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("expeditiontitle_dut", expTitle, false);
            }
            if (!Futile.atlasManager.DoesContainElementWithName("expeditionshadow_dut"))
            {
                Texture2D expShadow = new Texture2D(0, 0);
                expShadow.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/expeditionshadow_dut.png")));
                expShadow.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("expeditionshadow_dut", expShadow, false);
            }
            if (!Futile.atlasManager.DoesContainElementWithName("expeditionpage_dut"))
            {
                Texture2D expPage = new Texture2D(0, 0);
                expPage.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/expeditionpage_dut.png")));
                expPage.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("expeditionpage_dut", expPage, false);
            }            
            if (!Futile.atlasManager.DoesContainElementWithName("challengeselect_dut"))
            {
                Texture2D chalSelect = new Texture2D(0, 0);
                chalSelect.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/challengeselect_dut.png")));
                chalSelect.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("challengeselect_dut", chalSelect, false);
            }
            if (!Futile.atlasManager.DoesContainElementWithName("unlockables_dut"))
            {
                Texture2D unlocks = new Texture2D(0, 0);
                unlocks.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/unlockables_dut.png")));
                unlocks.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("unlockables_dut", unlocks, false);
            }
            if (!Futile.atlasManager.DoesContainElementWithName("progression_dut"))
            {
                Texture2D progress = new Texture2D(0, 0);
                progress.LoadImage(File.ReadAllBytes(AssetManager.ResolveFilePath("illustrations/progression_dut.png")));
                progress.filterMode = FilterMode.Point;
                Futile.atlasManager.LoadAtlasFromTexture("progression_dut", progress, false);
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
            if (Futile.atlasManager.DoesContainElementWithName("challengeselect_dut"))
            {
                Futile.atlasManager.UnloadAtlas("challengeselect_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("unlockables_dut"))
            {
                Futile.atlasManager.UnloadAtlas("unlockables_dut");
            }
            if (Futile.atlasManager.DoesContainElementWithName("progression_dut"))
            {
                Futile.atlasManager.UnloadAtlas("progression_dut");
            }            
        }
    }
}
