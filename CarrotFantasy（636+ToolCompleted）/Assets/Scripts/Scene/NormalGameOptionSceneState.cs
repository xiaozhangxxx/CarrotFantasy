using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGameOptionSceneState : BaseSceneState
{
    public NormalGameOptionSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {

        base.ExitScene();
    }
}
