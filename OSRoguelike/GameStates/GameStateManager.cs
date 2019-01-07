using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike
{
    public static class GameStateManager
    {
        public static List<GameState> GameStates = new List<GameState>();
        public static void SetCurrentGameState(GameState gameState)
        {
            GameStates.Insert(0, gameState);
            CurrentGameState = gameState;
        }
        public static void ExitCurrentGameState()
        {
            GameStates.RemoveAt(0);
            CurrentGameState = GameStates[0];
        }

        public static GameState CurrentGameState { get; private set; }
    }
}
