using MyBox;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class SaveRoomExample : MonoBehaviour
    {
#if UNITY_EDITOR
        [ButtonMethod]
#endif
        public void CreateCheckerBoardRoom()
        {
            var rows = 10;
            var cols = 10;
            RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
            roomData.Init(rows, cols);
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    if (x % 3 == 0 && y % 3 == 0)
                    {
                        roomData[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                    }
                    else
                    {
                        roomData[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
#if UNITY_EDITOR
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/RoomGenerator/Checkered.asset");
            AssetDatabase.CreateAsset(roomData, uniquePath);
#endif
        }
#if UNITY_EDITOR
        [ButtonMethod]
#endif
        public void CreateVerticalLinesRoom()
        {
            var rows = 10;
            var cols = 10;
            RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
            roomData.Init(rows, cols);
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    if (x % 3 == 0)
                    {
                        if (1 < y && y < (cols / 2 - 1))
                        {
                            roomData[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else if (y < cols - 2 && y > (cols / 2 + 1))
                        {
                            roomData[x, y] = new Tile(Enums.TileTypes.Block, new Vector2(x, y));
                        }
                        else
                        {
                            roomData[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                        }
                    }
                    else
                    {
                        roomData[x, y] = new Tile(Enums.TileTypes.Floor, new Vector2(x, y));
                    }
                }
            }
#if UNITY_EDITOR
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/RoomGenerator/VerticalLines.asset");
            AssetDatabase.CreateAsset(roomData, uniquePath);
#endif
        }
    }
}