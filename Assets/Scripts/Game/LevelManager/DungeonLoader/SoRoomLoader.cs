namespace Game.LevelManager.DungeonLoader
{
    public static class SoRoomLoader
    {
        public static void CreateRoom(DungeonRoom room, RoomGeneratorInput roomGeneratorInput)
        {
            var newRoom = RandomRoomGenerator.CreateNewRoom(roomGeneratorInput); //Sua geração de salas
            for (var x = 0; x < room.Dimensions.Width; x++)
            {
                for (var y = 0; y < room.Dimensions.Height; y++)
                {
                    room.Tiles[x, y] = (int) newRoom.Room[x][y].TileType;
                }
            }
        }
    }
}

