using OSRoguelike.Enemies;
using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace OSRoguelike
{
    class Game
    {
        public static Player Player = new Player();
        public static DungeonMap Map;
        public static List<Enemy> Enemies = new List<Enemy>();        

        static void Main(string[] args)
        {
            Terminal.Init(95, 43, "caRL", 7, 9);
            GameStateManager.SetCurrentGameState(new MainGameState());
            
            var frameTimer = new Stopwatch();
            bool running = true;
            while (running)
            {
                // lock core game loop to 10 FPS
                var ms = (int)frameTimer.ElapsedMilliseconds;
                if (ms < 100) Thread.Sleep(100 - ms);
                frameTimer.Restart();

                GameStateManager.CurrentGameState.Update();    // logic update
                GameStateManager.CurrentGameState.Render();    // render update

                if (Terminal.IsKeyPressed(Keys.Escape)) running = false;
            }
        }


    }
}
