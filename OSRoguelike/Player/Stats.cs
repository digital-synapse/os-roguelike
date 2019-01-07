using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike
{
    public class Stats
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public int ST { get; set; }
        public int MaxST { get; set; }
        public int GP { get; set; }

        public int Level { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Piety { get; set; }
        public int Vitality { get; set; }
        public int Dexterity { get; set; }
        public int Speed { get; set; }
        public int Personality { get; set; }
        public int Luck { get; set; }

        // lazy method to determine the potential difficulty of an enemy - we just multiply all the stats togeather 
        // change this formula to affect the progression and probability of enemy spawns
        public long Difficulty =>
            1 + Dexterity *
            1 + HP *
            1 + Intelligence *
            1 + Level *
            1 + Luck *
            1 + MaxHP *
            1 + MaxMP *
            1 + MaxST *
            1 + MP *
            1 + Personality *
            1 + Piety *
            1 + Speed *
            1 + ST *
            1 + Strength *
            1 + Vitality;
    }
}
