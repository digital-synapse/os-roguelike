using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike.Enemies
{
    public class Snake : Enemy
    {
        public Snake()
        {
            Name = "Garter Snake";

            Stats = new Stats();
            Stats.HP = Stats.MaxHP = 7;
            Stats.MP = Stats.MaxMP = 1;
            Stats.ST = Stats.MaxST = 2;
        }
        public override void AttackDialoge(Player player, Dialogue dialogue, int turn)
        {
            // first turn
            if (turn == 0)
            {
                dialogue.Write("Garter Snake Strikes!", ConsoleColor.Gray);
                dialogue.Write();
            }
            dialogue.Write("Ankle Nibble deals 3hp damage");
            player.Stats.HP -= 3;
        }
        public override void DeathDialogue(Player player, Dialogue dialogue, int turn)
        {
            dialogue.Write("The snake coils up into a ball and dies.");
        }
    }
}
