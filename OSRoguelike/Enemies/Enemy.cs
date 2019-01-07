using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RogueSharp;

namespace OSRoguelike.Enemies
{
    public abstract class Enemy : Actor
    {
        public Enemy()
        {
            Awareness = 10;
            Name = "Enemy";
            Color = Colors.Enemy;
            Symbol = '@';
            X = 10;
            Y = 10;
        }

        public abstract void AttackDialoge(Player player, Dialogue dialogue, int turn);
        public abstract void DeathDialogue(Player player, Dialogue dialogue, int turn);

        public void Kill()
        {
            Game.Enemies.Remove(this);
        }

        public bool Update(Player player, DungeonMap map)
        {
            if (!Awake)
            {
                if (map.IsInFov(X, Y))
                {
                    Awake = true;
                }
            }

            if (Awake)
            {
                // move toward player
                var p = new PathToPlayer(player, map);
                if (p.CreateFrom(X, Y))
                {
                    var px = p.FirstCell.X;
                    var py = p.FirstCell.Y;
                    map.SetActorPosition(this, px, py);
                    if (px == player.X && py == player.Y)
                    {
                        NextToPlayer = true;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Awake { get; private set; }
        public bool NextToPlayer { get; private set; }
    }

    public class PathToPlayer
    {
        private readonly Player _player;
        private readonly DungeonMap _map;
        private readonly PathFinder _pathFinder;
        private IEnumerable<ICell> _cells;

        public PathToPlayer(Player player, DungeonMap map)
        {
            _player = player;
            _map = map;
            _pathFinder = new PathFinder(map);
        }
        public ICell FirstCell { get; private set; }

        public bool CreateFrom(int x, int y)
        {
            try
            {
                // in order for pathfinding to work the end nodes must be walkable
                _map.SetIsWalkable(x, y, true);
                _map.SetIsWalkable(_player.X, _player.Y, true);
                var origin = _map.GetCell(x, y);
                var destination = _map.GetCell(_player.X, _player.Y);
                var path = _pathFinder.TryFindShortestPath(origin, destination);
                //_map.SetIsWalkable(_player.X, _player.Y, false);
                //_map.SetIsWalkable(x, y, false);

                if (path != null)
                {
                    _cells = path.Steps;
                    if (path.Length > 1)
                    {
                        FirstCell = path.Steps.ElementAt(1);// StepForward();
                        return true;
                    }
                }
            }
            catch (PathNotFoundException ex) { Debugger.Break(); }
            catch (NoMoreStepsException ex) { Debugger.Break(); }
            return false;
        }

    }
}
