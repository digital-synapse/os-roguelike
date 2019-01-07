using OSRoguelike.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSRoguelike
{
    public class BattleGameState : GameState
    {
        public BattleGameState(Enemy attacker) {
            buffer = Terminal.CreateBuffer(81, 20);
            dialogue = new Dialogue(buffer);
            enemy = attacker;
            player = Game.Player;
            ui = new UI(player);

            buffer.DrawFrame(
                fg: ConsoleColor.DarkGray,
                topRight: FramePart.DividerCenter,
                right: FramePart.Vertical,
                bottomRight: FramePart.DividerBottom);
        }
        private Buffer buffer;
        private Dialogue dialogue;
        private Enemy enemy;
        private Player player;
        private UI ui;
        int turn = 0;

        public override void Render()
        {
            enemy.AttackDialoge(player, dialogue, turn);
            dialogue.Write();
            ui.DrawGameUI();
            buffer.Blit(0, 23);
            Terminal.WaitForAnyKey();

            player.AttackDialoge(enemy, dialogue, turn);
            dialogue.Write();
            ui.DrawGameUI();
            buffer.Blit(0, 23);

            if (enemy.Stats.HP <= 0)
            {
                enemy.DeathDialogue(player,dialogue, turn);
                enemy.Kill();
                ui.DrawGameUI();
                buffer.Blit(0, 23);
                Terminal.WaitForAnyKey();
                GameStateManager.ExitCurrentGameState();                
            }
            turn++;
        }

        public override bool Update()
        {            
            return true;    
        }
    }
}
