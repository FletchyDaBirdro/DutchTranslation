using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil.Cil;

namespace DutchTranslation
{
    public class GeneralStuff
    {
        public static void ApplyHooks()
        {            
            On.MoreSlugcats.ChatlogData.DecryptResult += ChatlogData_DecryptResult;
            On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.GetSliderWidth += ColorSlider_GetSliderWidth;
        }

        public static void ApplyEarlyHooks()
        {
            IL.InGameTranslator.LoadShortStrings += new ILContext.Manipulator(InGameTranslator_LoadShortStrings);
        }
      
        private static void InGameTranslator_LoadShortStrings(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            //ILLabel label = c.DefineLabel();
            int target = -1;

            try
            {
                c.GotoNext(
                    x => x.MatchCallOrCallvirt(typeof(InGameTranslator).GetMethod("get_currentLanguage")),
                    x => x.MatchCallOrCallvirt(out _),
                    x => x.MatchStloc(out target),
                    x => x.MatchLdloc(out _)
                    );

                c.Index += 3;
                c.Emit(OpCodes.Ldloca, 0); //put one item on the stack
                c.Emit<GeneralStuff>(OpCodes.Call, nameof(LoadStrings));

                MainPlugIn.TransLogger.LogDebug(il.ToString());

                /*c.GotoNext(                    
                    x => x.MatchLdarg(0), //27
                    x => x.MatchCallOrCallvirt(typeof(InGameTranslator).GetMethod("get_currentLanguage")), //28
                    x => x.MatchLdsfld(typeof(InGameTranslator.LanguageID).GetField(nameof(InGameTranslator.LanguageID.English))), //29
                    x => x.MatchCallOrCallvirt(typeof(ExtEnum<InGameTranslator.LanguageID>).GetMethod("op_Equality")), //30
                    x => x.MatchBrfalse(out label) //31
                    );
                
                c.Emit(OpCodes.Ldarg_0);                                              
                c.Emit(OpCodes.Brfalse_S, label);                
                
                MainPlugIn.TransLogger.LogDebug(il.ToString());*/
                /*c.GotoNext(
                 x => x.MatchCallOrCallvirt(typeof(InGameTranslator).GetMethod("get_currentLanguage")), //36
                 x => x.MatchCallOrCallvirt(out _), //37
                 x => x.MatchStloc(out target), //38
                 x => x.MatchLdloc(out _), //39
                 x => x.MatchLdcI4(out _), //40
                 x => x.MatchLdelemRef(), //41
                 x => x.MatchCallOrCallvirt(typeof(System.IO.File), "Exists"), //42
                 x => x.MatchBrtrue(out _), //43
                 x => x.MatchRet()); //44
                {
                    //index starts at 36
                    c.Index += 3;
                    c.RemoveRange(6);
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloca, target);
                    c.Emit<GeneralStuff>(Mono.Cecil.Cil.OpCodes.Call, nameof(LoadStrings));
                }*/
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
    }
}
