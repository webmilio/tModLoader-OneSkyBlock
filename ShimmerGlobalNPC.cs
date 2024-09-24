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
                npcLoot.Add(ItemDropRule.Common(ItemID.FrostDaggerfish, 1, 100, 200));
                break;
        }
    }

    public override void ModifyShop(NPCShop shop)
    {
        var config = ModContent.GetInstance<OneSkyBlockConfig>();

        if (!config.ShimmerChallenge)
        {
            return;
        }

        switch (shop.NpcType)
        {
            case NPCID.Merchant:
                if (config.MerchantWorkbench)
                {
                    shop.Add(new NPCShop.Entry(ItemID.WorkBench));
                }

                break;
        }
    }
}
