using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReforgeQTE.Common;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ReforgeQTE.Content;

public class ReforgeUI : UIState {
	public UIPanel MainPanel;

	public ReforgeQTE.Common.UIItemSlot ReforgeSlot;

	public UIImageButton NormalReforgeButton;

	public UIImageButton MinigameButton;

	public override void OnInitialize() {
		MainPanel = new UIPanel();
		MainPanel.Width.Set(165, 0);
		MainPanel.Height.Set(64, 0);
		MainPanel.Left.Set(10, 0);
		MainPanel.Top.Set(270, 0);
		MainPanel.OnUpdate += element => {
			if (element.IsMouseHovering) {
				Main.LocalPlayer.mouseInterface = true;
			}
		};
		this.Append(MainPanel);

		ReforgeSlot = new ReforgeQTE.Common.UIItemSlot(
			ItemSlot.Context.PrefixItem,
			.8f
		);
		MainPanel.Append(ReforgeSlot);

		NormalReforgeButton = new UIImageButton(TextureAssets.Reforge[1]);
		NormalReforgeButton.Left.Set(55, 0);
		NormalReforgeButton.Top.Set(6, 0);
		NormalReforgeButton.OnLeftClick += VanillaReforge;
		NormalReforgeButton.OnUpdate += element => {
			if (element.IsMouseHovering) {
				string mousetext = Language.GetTextValue("Mods.ReforgeQTE.VanillaReforge");

				Item reforgeItem = ReforgeSlot.Item;
				int reforgePrice = reforgeItem.value;
				reforgePrice *= reforgeItem.stack; // #StackablePrefixWeapons: scale with current stack size

				bool canApplyDiscount = true;
				
				if (ItemLoader.ReforgePrice(reforgeItem, ref reforgePrice, ref canApplyDiscount)) {
					if (canApplyDiscount && Main.LocalPlayer.discountAvailable) {
						reforgePrice = (int) (reforgePrice* 0.8);
					}
					reforgePrice = (int) (reforgePrice* Main.LocalPlayer.currentShoppingSettings.PriceAdjustment);
					reforgePrice /= 3;
				}
				string text2 = "";
				if (reforgePrice != 0) {
					text2 = Helpers.GetMoneyString(reforgePrice);
				}
				Main.instance.MouseText(mousetext + "\n" + text2);
			}
		};
		MainPanel.Append(NormalReforgeButton);

		MinigameButton = new UIImageButton(TextureAssets.Item[ItemID.IronAnvil]);
		MinigameButton.Left.Set(100, 0);
		MinigameButton.Top.Set(12, 0);
		MinigameButton.OnLeftClick += LaunchMinigame;
		MinigameButton.OnUpdate += element => {
			if (element.IsMouseHovering) {
				string mousetext = Language.GetTextValue("Mods.ReforgeQTE.StartMinigame");
				Main.instance.MouseText(mousetext);
			}
		};
		MainPanel.Append(MinigameButton);
	}

	public override void OnDeactivate() {
		if (ReforgeSlot.Item.type == ItemID.None) {
			return;
		}
		Player player = Main.LocalPlayer;
		int itemWhoAmI = Item.NewItem(new EntitySource_OverfullInventory(player), player.position, ReforgeSlot.Item.width, ReforgeSlot.Item.height, ReforgeSlot.Item, noBroadcast: false, noGrabDelay: true);
		Main.item[itemWhoAmI].newAndShiny = false;
		if (Main.netMode == 1)
		{
			NetMessage.SendData(21, -1, -1, null, itemWhoAmI, 1f);
		}
		ReforgeSlot.Item = new Item();
	}

	public void VanillaReforge(UIMouseEvent evt, UIElement listeningElement) {
		if (ReforgeSlot.Item.type == ItemID.None) {
			return;
		}
		Item reforgeItem = ReforgeSlot.Item;
		int reforgePrice = reforgeItem.value;
		reforgePrice *= reforgeItem.stack; // #StackablePrefixWeapons: scale with current stack size

		bool canApplyDiscount = true;
		
		if (ItemLoader.ReforgePrice(reforgeItem, ref reforgePrice, ref canApplyDiscount)) {
			if (canApplyDiscount && Main.LocalPlayer.discountAvailable) {
				reforgePrice = (int) (reforgePrice* 0.8);
			}
			reforgePrice = (int) (reforgePrice* Main.LocalPlayer.currentShoppingSettings.PriceAdjustment);
			reforgePrice /= 3;
		}
	
		if (Main.LocalPlayer.CanAfford(reforgePrice) && ItemLoader.CanReforge(reforgeItem)) {
			Main.LocalPlayer.BuyItem(reforgePrice);
			ItemLoader.PreReforge(reforgeItem); // After BuyItem just in case

			reforgeItem.ResetPrefix();
			reforgeItem.Prefix(-2);
			reforgeItem.position.X = Main.LocalPlayer.position.X + Main.LocalPlayer.width / 2 - reforgeItem.width / 2;
			reforgeItem.position.Y = Main.LocalPlayer.position.Y + Main.LocalPlayer.height / 2 - reforgeItem.height / 2;
			ItemLoader.PostReforge(reforgeItem);

			PopupText.NewText(PopupTextContext.ItemReforge, reforgeItem, reforgeItem.stack, noStack: true);
			SoundEngine.PlaySound(SoundID.Item37);
		}
	}

	public void LaunchMinigame(UIMouseEvent evt, UIElement listeningElement) {
		if (ReforgeSlot.Item.type == ItemID.None) {
			return;
		}
		ReforgeMinigameSystem ReforgeMinigameSystem = ModContent.GetInstance<ReforgeMinigameSystem>();
		ReforgeMinigameSystem.Show();
		ReforgeMinigameSystem.ReforgeMinigame.MinigameSlot.Item = ReforgeSlot.Item;
		ReforgeSlot.Item = new Item();
		Main.playerInventory = false;
		ModContent.GetInstance<ReforgeUISystem>().Hide();
	}
}
