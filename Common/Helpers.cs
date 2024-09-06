using Terraria;
using Terraria.ID;

namespace ReforgeQTE.Common;

public static class Helpers {
	public static string GetMoneyString(int input) {
		string text2 = "";
		int platinum = 0;
		int gold = 0;
		int silver = 0;
		int copper = 0;
		int amount = input;
		if (amount < 1)
		{
			amount = 1;
		}
		if (amount >= 1000000)
		{
			platinum = amount / 1000000;
			amount -= platinum * 1000000;
		}
		if (amount >= 10000)
		{
			gold = amount / 10000;
			amount -= gold * 10000;
		}
		if (amount >= 100)
		{
			silver = amount / 100;
			amount -= silver * 100;
		}
		if (amount >= 1)
		{
			copper = amount;
		}
		if (platinum > 0)
		{
			text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + platinum + " " + Lang.inter[15].Value + "] ";
		}
		if (gold > 0)
		{
			text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + gold + " " + Lang.inter[16].Value + "] ";
		}
		if (silver > 0)
		{
			text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + silver + " " + Lang.inter[17].Value + "] ";
		}
		if (copper > 0)
		{
			text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + copper + " " + Lang.inter[18].Value + "] ";
		}
		return text2;
	}
}