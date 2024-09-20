using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSkyBlock;

internal class ShimmerGlobalNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        var config = ModContent.GetInstance<OneSkyBlockConfig>();

        if (!config.ShimmerChallenge)
        {
            return;
        }

        switch (npc.type)
        {
            case NPCID.Angler:
                npcLoot.Add(ItemDropRule.Common(ItemID.FrostDaggerfish, 1, 50, 100));
                break;
        }
    }
}
