using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ReforgeQTE.Content;

public class ReforgeMinigame : UIState {
	public UIPanel MainPanel;

	public ReforgeQTE.Common.UIItemSlot MinigameSlot;

	public override void OnInitialize() {
		MainPanel = new UIPanel();
		MainPanel.Width.Set(165, 0);
		MainPanel.Height.Set(64, 0);
		MainPanel.Left.Set(Main.screenWidth / 2, 0);
		MainPanel.Top.Set(Main.screenHeight / 2, 0);
		MainPanel.OnUpdate += element => {
			if (element.IsMouseHovering) {
				Main.LocalPlayer.mouseInterface = true;
			}
		};
		this.Append(MainPanel);

		MinigameSlot = new ReforgeQTE.Common.UIItemSlot(
			ItemSlot.Context.PrefixItem,
			.8f,
			false
		);
		MainPanel.Append(MinigameSlot);
	}

	
}