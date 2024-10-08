﻿namespace ET.Server
{
    [FriendOfAttribute(typeof(ET.MicroDustNumericComponent))]
    public static class MicroDustResourceHelper
    {
        public static async ETTask UpdateResource(MicroDustPlayerComponent unit, MicroDustDatabaseComponent db)
        {
            var numericComponent = unit.GetComponent<MicroDustNumericComponent>();
            if (numericComponent == null)
            {
                return;
            }
            var time = TimeInfo.Instance.ServerNow();
            var times = (time - numericComponent.ResourceUpdateTime) / MicroDustConstants.ResourceDeltaSeconds;
            if (times == 0)
            {
                return;
            }
            MicroDustResource.UpdateResource(numericComponent, times);
            numericComponent.ResourceUpdateTime += times * MicroDustConstants.ResourceDeltaSeconds;

            await db.SaveWithComponents(unit, MicroDustCollections.UserInfo);
        }
    }
}
