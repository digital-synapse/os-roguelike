using OSRoguelike.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike
{
    public class MainGameState : GameState
    {
        public MainGameState( )
        {
            this.player = Game.Player;
            this.enemies = Game.Enemies;
            this.ui = new UI(player);

            mapBuffer = Terminal.CreateBuffer(80, 40);
            var mapGenerator = new MapGenerator(80, 40);
            dungeonMap = mapGenerator.CreateMap();
            dungeonMap.MovePlayerToWalkable();
            dungeonMap.UpdatePlayerFieldOfView();
            dungeonMap.GenerateEnemies(enemies, 10, 1,1);
            Game.Map = dungeonMap;
        }
        private Buffer mapBuffer;
        private DungeonMap dungeonMap;
        private Player player;
        private List<Enemy> enemies;
        private UI ui;

        public override bool Update()
        {
            // logic
            if (Terminal.IsKeyPressed(Keys.Up)) player.MovePlayer(Actor.Direction.Up);
            if (Terminal.IsKeyPressed(Keys.Right)) player.MovePlayer(Actor.Direction.Right);
            if (Terminal.IsKeyPressed(Keys.Down)) player.MovePlayer(Actor.Direction.Down);
            if (Terminal.IsKeyPressed(Keys.Left)) player.MovePlayer(Actor.Direction.Left);
            if (Terminal.IsKeyPressed(Keys.Escape)) return false;
            foreach (var e in enemies)
            {
                if (e.Update(player, dungeonMap))
                {
                    GameStateManager.SetCurrentGameState(new BattleGameState(e));
                    return true;
                }
            }
            return true;
        }

        public override void Render()
        {
            // render
            mapBuffer.Clear();
            dungeonMap.Draw(mapBuffer);
            foreach (var e in enemies)
            {
                e.Draw(mapBuffer, dungeonMap);
            }
            player.Draw(mapBuffer, dungeonMap);
            mapBuffer.Blit(0, 3);
            ui.DrawGameUI();
        }
    }

    public abstract class GameState
    {
        public abstract bool Update();
        public abstract void Render();
    }
}
