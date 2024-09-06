using Microsoft.Xna.Framework;
using ReforgeQTE.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;

namespace ReforgeQTE.Common;

[Autoload(Side = ModSide.Client)]
public class ReforgeUISystem : ModSystem
{
	public UserInterface UserInterface;

	public ReforgeUI ReforgeUI;

	private GameTime lastUpdateUiGameTime;

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"ReforgeQTE : Goblin Tinkerer reforge UI",
				delegate {
					if (lastUpdateUiGameTime != null && UserInterface?.CurrentState != null) {
						UserInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
					}
					return true;
				},
				InterfaceScaleType.UI
			));
		}
	}

	public override void UpdateUI(GameTime gameTime) {
		lastUpdateUiGameTime = gameTime;
		if (UserInterface.CurrentState != null) {
			UserInterface.Update(gameTime);
			if (Main.LocalPlayer.TalkNPC == null) {
				Hide();
			}
		}
		if (!Main.playerInventory) {
			Hide();
		}
	}

	public override void Load() {
		UserInterface = new UserInterface();
		ReforgeUI = new ReforgeUI();
		ReforgeUI.Activate();
	}

	public override void Unload() {
		ReforgeUI = null;
	}

	public void Show() {
		UserInterface.SetState(ReforgeUI);
	}

	public void Hide() {
		UserInterface.SetState(null);
	}
}