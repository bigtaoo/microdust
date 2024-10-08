﻿using System.Linq;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.MicroDustGate)]
    public class C2G_MicroDust_SkillsHandler :
        MessageSessionHandler<C2G_MicroDustSkills_Request, G2C_MicroDustSkills_Response>
    {
        protected override async ETTask Run(Session session, C2G_MicroDustSkills_Request request, G2C_MicroDustSkills_Response response)
        {
            var player = session.GetComponent<MicroDustSessionPlayerComponent>().Player;
            var db = session.Root().GetComponent<MicroDustDatabaseManagerComponent>().GetZoneDB(session.Zone());
            var skills = (await db.Query<MicroDustSkillComponent>(s => s.PlayerId == player.PlayerId,
                MicroDustCollections.Skills)).FirstOrDefault();
            skills ??= new MicroDustSkillComponent();
            response.Skills = skills.Skills.Select(h => ToSkillInfo(h)).ToList();
            await ETTask.CompletedTask;
        }

        private MicroDustSkillInfo ToSkillInfo(MicroDustSkill skill)
        {
            var result = MicroDustSkillInfo.Create();

            result.SkillConfigId = skill.ConfigId;
            result.SkillId = skill.SkillId;
            result.UsedByHeroId = skill.UsedByHeroId;

            return result;
        }
    }
}
