using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike.Enemies
{
    public class Bunny : Enemy
    {
        public Bunny()
        {
            Name = "Bunny Rabbit";

            Stats = new Stats();
            Stats.HP = Stats.MaxHP = 5;
            Stats.MP = Stats.MaxMP = 0;
            Stats.ST = Stats.MaxST = 1;
        }
        public override void AttackDialoge(Player player, Dialogue dialogue, int turn)
        {
            // first turn
            if (turn == 0)
            {
                dialogue.Write("Bunny Rabbit Attacks!", ConsoleColor.Gray);
                dialogue.Write();
            }
            dialogue.Write("Ankle Nibble deals 2hp damage");
            player.Stats.HP -= 2;
        }
        public override void DeathDialogue(Player player, Dialogue dialogue, int turn)
        {
            dialogue.Write("The bunny gasps one final breath before collapsing into a pile of gore and fur.");
        }
    }
}
