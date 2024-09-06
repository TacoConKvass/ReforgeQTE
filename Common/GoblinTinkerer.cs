using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ReforgeQTE.Common;

public class GoblinTinkerer : GlobalNPC {
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.GoblinTinkerer;

	public override bool PreChatButtonClicked(NPC npc, bool firstButton) {
		if (firstButton) {
			return base.PreChatButtonClicked(npc, firstButton);
		}

		Main.npcChatText = "";
		Main.playerInventory = true;
		ModContent.GetInstance<ReforgeUISystem>().Show();
		SoundEngine.PlaySound(SoundID.MenuOpen);
		return false;
	}
}