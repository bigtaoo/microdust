﻿using System;
using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.MicroDustGameResource)]
    public class MicroDustGameResourceUIEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            await ETTask.CompletedTask;
            string assetsName = $"Assets/Bundles/UI/MicroDust/Game/{UIType.MicroDustGameResource}.prefab";
            GameObject bundleGameObject = await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.MicroDustGameResource, gameObject);
            ui.AddComponent<MicroDustGameResourceUIComponent>();

            return ui;

            //Fiber fiber = uiComponent.Fiber();
            //try
            //{
            //    var uiType = UIType.MicroDustGameResource;
            //    ResourcesComponent resourcesComponent = fiber.Root.GetComponent<ResourcesComponent>();
            //    await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAsync(resourcesComponent.StringToAB(uiType));
            //    GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(resourcesComponent.StringToAB(uiType), uiType);
            //    GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            //    UI ui = uiComponent.AddChild<UI, string, GameObject>(uiType, gameObject);

            //    ui.AddComponent<MicroDustGameResourceUIComponent>();
            //    return ui;
            //}
            //catch (Exception e)
            //{
            //    fiber.Error(e);
            //    return null;
            //}
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}
