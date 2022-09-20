using System;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public static class DefaultRoomCreator
    {
        public static void CreateRoomOfType(DungeonRoom room, int roomType)
        {
            var dimensions = room.Dimensions;
            var roomData = ScriptableObject.CreateInstance<RoomData>();
            roomData.Init(dimensions.Width, dimensions.Height);
            var roomTypeEnum = (Enums.RoomPatterns) roomType;
            CreateEmptyBorders(roomData, dimensions);
            switch (roomTypeEnum)
            {
                case Enums.RoomPatterns.Empty:
                {
                    CreateEmptyRoom(roomData, dimensions);
                    break;
                }
                case Enums.RoomPatterns.CheckerBoard:
                {
                    CreateCheckerBoardRoom(roomData, dimensions);
                    break;
                }
                case Enums.RoomPatterns.HorizontalLines:
                {
                    CreateHorizontalLinesRoom(roomData, dimensions);
                    break;
                }
                case Enums.RoomPatterns.VerticalLines:
                {
                    CreateVerticalLinesRoom(roomData, dimensions);
                    break;
                }
                case Enums.RoomPatterns.Cross:
                {
                    CreateCrossRoom(roomData, dimensions);
                    break;
                }
                default:
                    throw new ArgumentException(
                        $"$There is no such room type (Value = {roomType} )in the RoomTypes Enum");
            }

            room.Tiles = roomData;
        }

        private static void CreateEmptyBorders(RoomData roomData, Dimensions dimensions)
        {
            for (var x = 0; x < dimensions.Width; x++)
            {

                roomData[x, 0] = new Tile(Enums.TileTypes.Floor, new Vector2(x, 0));
                roomData[x, dimensions.Height-1] = new Tile(Enums.TileTypes.Floor, new Vector2(x, dimensions.Height-1));
            }
            for (var y = 0; y<dimensions.Height; y++)
            {
                roomData[0, y] = new Tile(Enums.TileTypes.Floor, new Vector2(0, y));
                roomData[dimensions.Width-1, y] = new Tile(Enums.TileTypes.Floor, new Vector2(dimensions.Width-1, y));
            }
        }

        private static void CreateEmptyRoom(RoomData room, Dimensions dimensions)
        {
            for (var x = 1; x < dimensions.Width - 1; x++)
            {
                for (var y = 1; y < dimensions.Height - 1; y++)
                {
                    room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                }
            }
        }
        
        private static void CreateCheckerBoardRoom(RoomData room, Dimensions dimensions)
        {
            for (var x = 1; x < dimensions.Width - 1; x++)
            {
                for (var y = 1; y < dimensions.Height - 1; y++)
                {
                    if (x % 3 == 0 && y % 3 == 0)
                    {
                        room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));

                    }
                    else
                    {
                        room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
        }
        
        private static void CreateVerticalLinesRoom(RoomData room, Dimensions dimensions)
        {
            for (var x = 1; x < dimensions.Width - 1; x++)
            {
                for (var y = 1; y < dimensions.Height - 1; y++)
                {
                    if (x % 3 == 0)
                    {
                        if (1 < y && y < (dimensions.Height / 2 - 1))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else if (y < dimensions.Height - 2 && y > (dimensions.Height / 2 + 1))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                        }
                    }
                    else
                    {
                        room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
        }
        
        private static void CreateHorizontalLinesRoom(RoomData room, Dimensions dimensions)
        {
            for (var x = 1; x < dimensions.Width - 1; x++)
            {
                for (var y = 1; y < dimensions.Height - 1; y++)
                {
                    if (y % 3 == 0)
                    {
                        if (1 < x && x < (dimensions.Width / 2 - 1))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else if (x < dimensions.Width - 2 && x > (dimensions.Width / 2))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                        }
                    }
                    else
                    {
                        room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
        }
        
        private static void CreateCrossRoom(RoomData room, Dimensions dimensions)
        {
            for (var x = 1; x < dimensions.Width - 1; x++)
            {
                for (var y = 1; y < dimensions.Height - 1; y++)
                {
                    if (x > (dimensions.Width/2 - 2) && x < (dimensions.Width / 2 + 2))
                    {
                        if (y > 1 && y < (dimensions.Height - 2))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                        }
                    }
                    else if (y > dimensions.Height / 2 - 2 && y < (dimensions.Height / 2 + 2))
                    {
                        if (x > 1 && x < (dimensions.Width - 2))
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else
                        {
                            room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                        }
                    }
                    else
                    {
                        room[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
        }
    }
}
