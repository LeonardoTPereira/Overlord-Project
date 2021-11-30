using System;
using Util;

namespace Game.LevelManager
{
    public static class DefaultRoomCreator
    {
        public static void CreateRoomOfType(DungeonRoom room, int roomType)
        {
            var roomTypeEnum = (Enums.RoomPatterns) roomType;
            switch (roomTypeEnum)
            {
                case Enums.RoomPatterns.Empty:
                {
                    CreateEmptyRoom(room);
                    break;
                }
                case Enums.RoomPatterns.CheckerBoard:
                {
                    CreateCheckerBoardRoom(room);
                    break;
                }
                case Enums.RoomPatterns.HorizontalLines:
                {
                    CreateHorizontalLinesRoom(room);
                    break;
                }
                case Enums.RoomPatterns.VerticalLines:
                {
                    CreateVerticalLinesRoom(room);
                    break;
                }
                case Enums.RoomPatterns.Cross:
                {
                    CreateCrossRoom(room);
                    break;
                }
                default:
                    throw new ArgumentException($"$There is no such room type (Value = {roomType} )in the RoomTypes Enum");
            }
        }

        private static void CreateEmptyRoom(DungeonRoom room)
        {
            for (var x = 1; x < room.Dimensions.Width - 1; x++)
            {
                for (var y = 1; y < room.Dimensions.Height - 1; y++)
                {
                    room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                }
            }
        }
        
        private static void CreateCheckerBoardRoom(DungeonRoom room)
        {
            for (var x = 1; x < room.Dimensions.Width - 1; x++)
            {
                for (var y = 1; y < room.Dimensions.Height - 1; y++)
                {
                    if (x % 3 == 0 && y % 3 == 0)
                    {
                        room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                    }
                    else
                    {
                        room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                    }
                }
            }
        }
        
        private static void CreateVerticalLinesRoom(DungeonRoom room)
        {
            for (var x = 1; x < room.Dimensions.Width - 1; x++)
            {
                for (var y = 1; y < room.Dimensions.Height - 1; y++)
                {
                    if (x % 3 == 0)
                    {
                        if (1 < y && y < (room.Dimensions.Height / 2 - 1))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                        else if (y < room.Dimensions.Height - 2 && y > (room.Dimensions.Height / 2 + 1))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                        else
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                        }
                    }
                    else
                    {
                        room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                    }
                }
            }
        }
        
        private static void CreateHorizontalLinesRoom(DungeonRoom room)
        {
            for (var x = 1; x < room.Dimensions.Width - 1; x++)
            {
                for (var y = 1; y < room.Dimensions.Height - 1; y++)
                {
                    if (y % 3 == 0)
                    {
                        if (1 < x && x < (room.Dimensions.Width / 2 - 1))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                        else if (x < room.Dimensions.Width - 2 && x > (room.Dimensions.Width / 2))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                        else
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                        }
                    }
                    else
                    {
                        room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                    }
                }
            }
        }
        
        private static void CreateCrossRoom(DungeonRoom room)
        {
            for (var x = 1; x < room.Dimensions.Width - 1; x++)
            {
                for (var y = 1; y < room.Dimensions.Height - 1; y++)
                {
                    if (x > (room.Dimensions.Width/2 - 2) && x < (room.Dimensions.Width / 2 + 2))
                    {
                        if (y > 1 && y < (room.Dimensions.Height - 2))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                    }
                    else if (y > room.Dimensions.Height / 2 - 2 && y < (room.Dimensions.Height / 2 + 2))
                    {
                        if (x > 1 && x < (room.Dimensions.Width - 2))
                        {
                            room.Tiles[x, y] = (int) Enums.TileTypes.Block;
                        }
                    }
                    else
                    {
                        room.Tiles[x, y] = (int) Enums.TileTypes.Floor;
                    }
                }
            }
        }
    }
}
