using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil.Cil;
using RWCustom;

namespace DutchTranslation
{
    public class GeneralStuff
    {
        public static void ApplyHooks()
        {            
            On.MoreSlugcats.ChatlogData.DecryptResult += ChatlogData_DecryptResult;
            On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.GetSliderWidth += ColorSlider_GetSliderWidth;
            IL.Menu.SlugcatSelectMenu.AddColorInterface += new ILContext.Manipulator(SlugcatSelectMenu_AddColorInterface);
        }

        public static void ApplyILHook()
        {
            IL.InGameTranslator.LoadShortStrings += new ILContext.Manipulator(InGameTranslator_LoadShortStrings);
        }
      
        private static void InGameTranslator_LoadShortStrings(ILContext il)
        {
            ILCursor c = new ILCursor(il);            
            int target = -1;

            try
            {
                c.GotoNext(
                    x => x.MatchCallOrCallvirt(typeof(InGameTranslator).GetMethod("get_currentLanguage")),
                    x => x.MatchCallOrCallvirt(out _),
                    x => x.MatchStloc(out target)                    
                    );

                c.Index += 3;
                c.Emit(OpCodes.Ldloca, 0);
                c.Emit<GeneralStuff>(OpCodes.Call, nameof(LoadStrings));

                //MainPlugIn.TransLogger.LogDebug(il.ToString());                              
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Whoopsie doodles. The ILHook failed!");
            }
        }

        private static void LoadStrings(ref string[] files)
        {
            List<string> list = new List<string>(files);
            list.RemoveAll((string p) => !File.Exists(p));

            if (list.Count != files.Length)
            {
                files = list.ToArray();
            }
        }

        private static string ChatlogData_DecryptResult(On.MoreSlugcats.ChatlogData.orig_DecryptResult orig, string result, string path)
        {
            if (result[0] == '0')
            {
                return result;
            }
            return orig(result, path);
        } 
        
        private static float ColorSlider_GetSliderWidth(On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.orig_GetSliderWidth orig, InGameTranslator.LanguageID lang)
        {            
            if (lang == LangID.LanguageID.Dutch)
            {
                return 160f;
            }
            return orig(lang);
        }

        public InGameTranslator.LanguageID GetLang
        {
            get
            {
                return Custom.rainWorld.options.language;
            }
        }

        static bool IsItEspNed(bool isItEsp, GeneralStuff self) => isItEsp|| self.GetLang == LangID.LanguageID.Dutch;

        private static void SlugcatSelectMenu_AddColorInterface(ILContext il) 
        { 
            ILCursor c = new ILCursor(il);

            try
            {
                c.GotoNext(
                    MoveType.After,
                    x => x.MatchLdsfld(typeof(InGameTranslator.LanguageID).GetField("Spanish")),
                    x => x.MatchCallOrCallvirt(typeof(ExtEnum<InGameTranslator.LanguageID>).GetMethod("op_Equality"))
                    );

                c.MoveAfterLabels();
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(IsItEspNed);

                //MainPlugIn.TransLogger.LogDebug(il);
            }
            catch (Exception ex) 
            { 
                MainPlugIn.TransLogger.LogError(ex);
            }
        }

        public static void UnapplyHooks()
        {
            On.MoreSlugcats.ChatlogData.DecryptResult -= ChatlogData_DecryptResult;
            On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.GetSliderWidth -= ColorSlider_GetSliderWidth;
            IL.InGameTranslator.LoadShortStrings -= new ILContext.Manipulator(InGameTranslator_LoadShortStrings);
            IL.Menu.SlugcatSelectMenu.AddColorInterface -= new ILContext.Manipulator(SlugcatSelectMenu_AddColorInterface);
        }
    }
}
