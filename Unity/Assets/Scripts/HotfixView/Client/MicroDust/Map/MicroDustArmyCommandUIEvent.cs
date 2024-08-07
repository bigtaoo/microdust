﻿using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.MicroDustArmyCommand)]
    public class MicroDustArmyCommandUIEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            var uiType = UIType.MicroDustArmyCommand;
            var resourcesComponent = uiComponent.Root().GetComponent<ResourcesComponent>();
            await resourcesComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAsync(resourcesComponent.StringToAB(uiType));

            var bundleGameObject = (GameObject)resourcesComponent.GetAsset(resourcesComponent.StringToAB(uiType), uiType);
            var uiLayerGameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            var ui = uiComponent.AddChild<UI, string, GameObject>(uiType, uiLayerGameObject);
            ui.AddComponent<MicroDustArmyCommandUIComponent>();

            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}