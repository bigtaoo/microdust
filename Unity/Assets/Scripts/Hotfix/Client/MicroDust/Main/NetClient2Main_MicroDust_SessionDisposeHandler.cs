﻿namespace ET.Client
{
    [MessageHandler(SceneType.All)]
    public class NetClient2Main_MicroDust_SessionDisposeHandler : MessageHandler<Scene, NetClient2Main_MicroDust_SessionDispose>
    {
        protected override async ETTask Run(Scene entity, NetClient2Main_MicroDust_SessionDispose message)
        {
            Log.Error($"session dispose, error: {message.Error}");
            await ETTask.CompletedTask;
        }
    }
}
