using OSRoguelike.Enemies;
using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSRoguelike
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        // Generate a new map that is a simple open floor with walls around the outside
        public DungeonMap CreateMap()
        {
            // Initialize every cell in the map by
            // setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);

            IMapCreationStrategy<DungeonMap> mapCreationStrategy = new RandomRoomsMapCreationStrategy<DungeonMap>(_width, _height, 50, 15, 5);
            _map= mapCreationStrategy.CreateMap();

            //foreach (Cell cell in _map.GetAllCells())
            //    _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);

            /*
            foreach (Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }


            // Set the first and last rows in the map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            // Set the first and last columns in the map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            */

            return _map;
        }
    }

    // Our custom DungeonMap class extends the base RogueSharp Map class
    public class DungeonMap : Map
    {
        // This method will be called any time we move the player to update field-of-view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Compute the field-of-view based on the player's location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Mark all cells in field-of-view as having been explored
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public void MovePlayerToWalkable()
        {
            Player player = Game.Player;

            if (!GetCell(player.X, player.Y).IsWalkable)
            {
                foreach (Cell cell in GetAllCells())
                {
                    if (cell.IsWalkable)
                    {
                        player.X = cell.X;
                        player.Y = cell.Y;
                        return;
                    }
                }
            }
        }

        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(Buffer mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(Buffer console, Cell cell)
        {
            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            // When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, (char)219);
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, (char)219);
                }
            }
        }

        // Returns true when able to place the Actor on the cell or false otherwise
        public bool SetActorPosition(Actor actor, int x, int y)
        {
            // Only allow actor placement if the cell is walkable
            if (GetCell(x, y).IsWalkable)
            {
                // The cell the actor was previously on is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                // Update the actor's position
                actor.X = x;
                actor.Y = y;
                // The new cell the actor is on is now not walkable
                SetIsWalkable(actor.X, actor.Y, false);
                // Don't forget to update the field of view if we just repositioned the player
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        // A helper method for setting the IsWalkable property on a Cell
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            ICell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        /// <summary>
        /// make enemies to populate the dungeon
        /// </summary>
        public void GenerateEnemies(List<Enemy> enemies, int numOfEnemies, int dungeonLevel, int dungeonFloor)
        {
            // ensure we are starting with an empty list   
            enemies.Clear();
            var r = new DotNetRandom();

            // get every possible enemy type in the game
            var enemyTypes= typeof(Enemy).GetAllDerivedTypes();
            
            // order them by difficulty
            var enemyInstances = enemyTypes.Select(t => (Enemy)Activator.CreateInstance(t)).OrderBy(i=>i.Stats.Difficulty).ToList();

            // select 3 enemy types based on dungeon level
            var min = dungeonLevel * dungeonFloor -1;            
            enemyTypes = enemyInstances.Skip(min).Take(3).Select(i => i.GetType()).ToList();

            for (var i = 0; i < numOfEnemies; i++)
            {
                var t = enemyTypes[r.Next(enemyTypes.Count-1)];
                var e = (Enemy)Activator.CreateInstance(t);

                enemies.Add(e);
                var cell = GetRandomWalkableCell();
                e.X = cell.X;
                e.Y = cell.Y;
            }

        }

        public ICell GetRandomWalkableCell()
        {
            var r = new DotNetRandom();
            var walkable = GetAllCells().Where(x => x.IsWalkable);
            var index = r.Next(walkable.Count() - 1);
            var cell = walkable.ElementAt(index);
            return cell;
        }
    }
}
