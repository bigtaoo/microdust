﻿using System;
using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.MicroDustGameUtility)]
    public class MicroDustGameUtilityUIEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            Fiber fiber = uiComponent.Fiber();
            try
            {
                var uiType = UIType.MicroDustGameUtility;
                ResourcesComponent resourcesComponent = fiber.Root.GetComponent<ResourcesComponent>();
                await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAsync(resourcesComponent.StringToAB(uiType));
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(resourcesComponent.StringToAB(uiType), uiType);
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
                UI ui = uiComponent.AddChild<UI, string, GameObject>(uiType, gameObject);

                ui.AddComponent<MicroDustGameUtilityUIComponent>();
                return ui;
            }
            catch (Exception e)
            {
                fiber.Error(e);
                return null;
            }
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}