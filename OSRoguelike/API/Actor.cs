using RogueSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OSRoguelike
{

    public interface IActor
    {
        string Name { get; set; }
        int Awareness { get; set; }
    }

    public interface IDrawable
    {
        ConsoleColor Color { get; set; }
        char Symbol { get; set; }
        int X { get; set; }
        int Y { get; set; }

        void Draw(Buffer buffer, IMap map);
    }

    public class Actor : IActor, IDrawable
    {
        public Stats Stats { get; protected set; }

        // IActor
        public string Name { get; set; }
        public int Awareness { get; set; }

        // IDrawable
        public ConsoleColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(Buffer buffer, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {                
                buffer.Draw(Symbol.ToString(), X, Y, Color, Colors.FloorBackgroundFov);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                buffer.Draw(".", X, Y, Colors.Floor, Colors.FloorBackground);
            }
        }

        public enum Direction
        {
            Up, Down, Left, Right
        }
        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            if (Game.Map.SetActorPosition(Game.Player, x, y))
            {
                return true;
            }

            return false;
        }
    }
}
