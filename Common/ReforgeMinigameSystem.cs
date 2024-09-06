using Microsoft.Xna.Framework;
using ReforgeQTE.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;

namespace ReforgeQTE.Common;

[Autoload(Side = ModSide.Client)]
public class ReforgeMinigameSystem : ModSystem
{
	public UserInterface UserInterface;

	public ReforgeMinigame ReforgeMinigame;

	private GameTime lastUpdateUiGameTime;

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"ReforgeQTE : Reforge minigame",
				delegate {
					if (lastUpdateUiGameTime != null && UserInterface?.CurrentState != null) {
						UserInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
					}
					return true;
				},
				InterfaceScaleType.None
			));
		}
	}

	public override void UpdateUI(GameTime gameTime) {
		lastUpdateUiGameTime = gameTime;
		if (UserInterface.CurrentState != null) {
			UserInterface.Update(gameTime);
		}
	}

	public override void Load() {
		UserInterface = new UserInterface();
		ReforgeMinigame = new ReforgeMinigame();
		ReforgeMinigame.Activate();
	}

	public override void Unload() {
		ReforgeMinigame = null;
	}

	public void Show() {
		UserInterface.SetState(ReforgeMinigame);
	}

	public void Hide() {
		UserInterface.SetState(null);
	}
}