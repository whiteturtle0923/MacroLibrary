using Terraria;
using Terraria.ModLoader;

namespace MacroLibrary.Content.Items
{
    public class MacroGlobalItem: GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<SaveMacro>() || entity.type == ModContent.ItemType<StartMacro>();
        }
    }
}