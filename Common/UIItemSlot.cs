using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;
using Terraria;

namespace ReforgeQTE.Common;

public class UIItemSlot : UIElement {
	public Item Item;
	public readonly int Context;
	public readonly float Scale;
	public Func<Item, bool> ValidItemFunc;
	public bool Interactable;

	public UIItemSlot(int context, float scale = 1f, bool interactable = true) {
		Interactable = interactable;
		Context = context;
		Scale = scale;
		Item = new Item();
		Item.SetDefaults(0);

		Width.Set(TextureAssets.InventoryBack9.Value.Width * scale, 0f);
		Height.Set(TextureAssets.InventoryBack9.Value.Height * scale, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch) {
		var oldScale = Main.inventoryScale;
		Main.inventoryScale = Scale;
		var rectangle = GetDimensions().ToRectangle();

		if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface && Interactable) {
			Main.LocalPlayer.mouseInterface = true;
			if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem)) {
				// Handle handles all the click and hover actions based on the context.
				ItemSlot.Handle(ref Item, Context);
			}
		}
		// Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
		ItemSlot.Draw(spriteBatch, ref Item, Context, rectangle.TopLeft());
		Main.inventoryScale = oldScale;
	}
}
