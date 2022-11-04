namespace Game.LevelManager.DungeonLoader
{
    public static class SoRoomLoader
    {
        public static void CreateRoom(DungeonRoom room, RoomGeneratorInput roomGeneratorInput)
        {
            var newRoom = RandomRoomGenerator.CreateNewRoom(roomGeneratorInput);
            room.Tiles = newRoom;
        }
    }
}

