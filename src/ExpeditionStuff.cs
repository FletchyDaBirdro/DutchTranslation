using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace DutchTranslation
{
    public class ExpeditionStuff
    {
        public static void ApplyHooks()
        {
            On.Menu.ExpeditionWinScreen.ctor += ExpeditionWinScreen_ctor;
            On.Menu.CharacterSelectPage.ctor += CharacterSelectPage_ctor;
            On.Menu.ChallengeSelectPage.ctor += ChallengeSelectPage_ctor;
            On.Menu.UnlockDialog.ctor += UnlockDialog_ctor;
            On.Menu.ProgressionPage.ctor += ProgressionPage_ctor;
            IL.Menu.ChallengeSelectPage.GrafUpdate += new ILContext.Manipulator(ChallengeSelectPage_GrafUpdate);
        }

        private static void CharacterSelectPage_ctor(On.Menu.CharacterSelectPage.orig_ctor orig, Menu.CharacterSelectPage self, Menu.Menu menu, Menu.MenuObject owner, UnityEngine.Vector2 pos)
        {
            orig(self, menu, owner, pos);

            try
            {
                if (menu.CurrLang == LangID.LanguageID.Dutch)
                {
                    self.Container.RemoveChild(self.pageTitle);
                    self.subObjects.Remove(self.localizedSubtitle);

                    self.pageTitle = new FSprite("expeditionpage_dut", true);
                    self.pageTitle.SetAnchor(0.5f, 0f);
                    self.pageTitle.x = 680f;
                    self.pageTitle.y = 680f;
                    self.pageTitle.shader = menu.manager.rainWorld.Shaders["MenuText"];

                    self.Container.AddChild(self.pageTitle);

                    MainPlugIn.TransLogger.LogDebug("Replaced Expedition Page title!");
                }
            }
            catch (Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Replacing title failed!");
            }
        }

        private static void ChallengeSelectPage_ctor(On.Menu.ChallengeSelectPage.orig_ctor orig, Menu.ChallengeSelectPage self, Menu.Menu menu, Menu.MenuObject owner, UnityEngine.Vector2 pos)
        {
            orig(self, menu, owner, pos);

            try 
            {
                if (menu.CurrLang == LangID.LanguageID.Dutch)
                {
                    self.Container.RemoveChild(self.pageTitle);
                    self.subObjects.Remove(self.localizedSubtitle);

                    self.pageTitle = new FSprite("expchallengeselect_dut", true);
                    self.pageTitle.SetAnchor(0.5f, 0f);
                    self.pageTitle.x = 683f;
                    self.pageTitle.y = 680f;
                    self.pageTitle.shader = menu.manager.rainWorld.Shaders["MenuText"];                                        
                    
                    self.Container.AddChild(self.pageTitle);

                    MainPlugIn.TransLogger.LogDebug("Replaced Challenge Select title!");
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Replacing title failed!");
            }
        }
         
        private static void ChallengeSelectPage_GrafUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel label = il.DefineLabel();

            try
            {
                c.GotoNext(
                    MoveType.Before,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld(out _),
                    x => x.MatchCallOrCallvirt(out _),
                    x => x.MatchLdfld(out _),
                    x => x.MatchLdstr("challengeselect"),
                    x => x.MatchCallOrCallvirt(out _),
                    x => x.MatchBrfalse(out _)
                    );
                c.Index += 6;
                label = (ILLabel)c.Next.Operand;
                c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Action<Menu.ChallengeSelectPage>>((Menu.ChallengeSelectPage self) =>
                {
                    if (self.pageTitle.element.name != "expchallengeselect_dut")
                    {
                        self.pageTitle.SetElementByName("expchallengeselect_dut");                        
                    }
                });
                c.Emit(OpCodes.Br, label);                

                MainPlugIn.TransLogger.LogDebug(il.ToString());
            }
            catch (Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("ExpeditionStuff: ILHook failed!");
            }
        }

        private static void ProgressionPage_ctor(On.Menu.ProgressionPage.orig_ctor orig, Menu.ProgressionPage self, Menu.Menu menu, Menu.MenuObject owner, UnityEngine.Vector2 pos)
        {
            orig(self, menu, owner, pos);

            try 
            {
                if (menu.CurrLang == LangID.LanguageID.Dutch)
                {
                    self.Container.RemoveChild(self.pageTitle);
                    self.subObjects.Remove(self.localizedSubtitle);

                    self.pageTitle = new FSprite("progression_dut", true);
                    self.pageTitle.SetAnchor(0.5f, 0f);
                    self.pageTitle.x = 720f;
                    self.pageTitle.y = 680f;
                    self.pageTitle.shader = menu.manager.rainWorld.Shaders["MenuText"];

                    self.Container.AddChild(self.pageTitle);

                    MainPlugIn.TransLogger.LogDebug("Replaced Progression title!");
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Replacing title failed!");
            }
        }

        private static void ExpeditionWinScreen_ctor(On.Menu.ExpeditionWinScreen.orig_ctor orig, Menu.ExpeditionWinScreen self, ProcessManager manager)
        {
            orig(self, manager);

            try
            {
                if (self.CurrLang == LangID.LanguageID.Dutch)
                {
                    self.pages[0].Container.RemoveChild(self.title);
                    self.pages[0].Container.RemoveChild(self.shadow);

                    self.shadow = new FSprite("expeditionshadow_dut", true);
                    self.shadow.x = 10f;
                    self.shadow.y = 638f;
                    self.shadow.SetAnchor(0, 0);
                    self.shadow.shader = manager.rainWorld.Shaders["MenuText"];
                    self.pages[0].Container.AddChild(self.shadow);

                    self.title = new FSprite("expeditiontitle_dut", true);
                    self.title.x = 10f;
                    self.title.y = 638f;
                    self.title.SetAnchor(0, 0);
                    self.pages[0].Container.AddChild(self.title);

                    MainPlugIn.TransLogger.LogDebug("Replaced Expedition title!");
                }
            }
            catch (Exception ex) 
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Replacing title failed!");
            }
        }

        private static void UnlockDialog_ctor(On.Menu.UnlockDialog.orig_ctor orig, Menu.UnlockDialog self, ProcessManager manager, Menu.ChallengeSelectPage owner)
        {
            orig(self, manager, owner);

            try 
            {
                if (self.CurrLang == LangID.LanguageID.Dutch) 
                {
                    self.pages[0].Container.RemoveChild(self.pageTitle);
                    self.pages[0].subObjects.Remove(self.localizedSubtitle);

                    self.pageTitle = new FSprite("unlockables_dut", true);
                    self.pageTitle.SetAnchor(0.5f, 0f);
                    self.pageTitle.x = 720f;
                    self.pageTitle.y = 680f;                    

                    self.pages[0].Container.AddChild(self.pageTitle);

                    MainPlugIn.TransLogger.LogDebug("Replaced Unlockables title!");
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Replacing title failed!");
            }
        }

        public static void UnapplyHooks() 
        {
            On.Menu.ExpeditionWinScreen.ctor -= ExpeditionWinScreen_ctor;
            On.Menu.CharacterSelectPage.ctor -= CharacterSelectPage_ctor;
            On.Menu.ChallengeSelectPage.ctor -= ChallengeSelectPage_ctor;
            On.Menu.UnlockDialog.ctor -= UnlockDialog_ctor;
            On.Menu.ProgressionPage.ctor -= ProgressionPage_ctor;
            IL.Menu.ChallengeSelectPage.GrafUpdate -= new ILContext.Manipulator(ChallengeSelectPage_GrafUpdate);
        }
    }
}
