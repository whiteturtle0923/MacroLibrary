using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MacroLibrary.Content
{
    public class MacroGlobalNPC: GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.HallowBoss;

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            npc.lifeRegen -= 2000;
            damage = 1000;
        }
    }
}