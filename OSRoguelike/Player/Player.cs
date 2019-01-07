
using OSRoguelike.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike
{
    public class Player : Actor
    {
        public Player()
        {
            Awareness = 15;
            Name = "Rogue";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;

            Stats = new Stats();
            Stats.HP = Stats.MaxHP = 20;
            Stats.MP = Stats.MaxMP = 10;
            Stats.ST = Stats.MaxST = 10;
            Stats.GP = 0;
            Stats.Level = 1;
            Stats.Strength = 3;
            Stats.Intelligence = 2;
            Stats.Piety = 1;
            Stats.Vitality = 3;
            Stats.Dexterity = 1;
            Stats.Speed = 2;
            Stats.Personality = 1;
            Stats.Luck = 1;
        }        

        public void AttackDialoge(Enemy enemy, Dialogue dialogue, int turn)
        {
            dialogue.Write($"You strike the {enemy.Name} inflicting 3hp damage", hilight: ConsoleColor.Green);
            enemy.Stats.HP -= 3;
        }
    }
}