﻿using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.MicroDustMore)]
    public class MicroDustMoreUIEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            await ETTask.CompletedTask;
            string assetsName = $"Assets/Bundles/UI/MicroDust/Utility/{UIType.MicroDustMore}.prefab";
            GameObject bundleGameObject = await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.MicroDustMore, gameObject);
            ui.AddComponent<MicroDustMoreUIComponent>();

            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}
