using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    public class CalculateSpawnPoints : MonoBehaviour
    {
        private int _startPosX, _startPosY;
        private Vector3 _roomLossyScale;
        private int _rowNum, _colNum;
        private float _xOffset, _yOffset;
        private bool[,] _backtrackingPath;
        private List<Vector3> _spawnPoints;

        public CalculateSpawnPoints(int rowNum, int colNum, float xOffset, float yOffset, Vector3 roomLossyScale)
        {
            _rowNum = rowNum;
            _colNum = colNum;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _spawnPoints = new List<Vector3>();
            _backtrackingPath = new bool[rowNum, colNum];
            _roomLossyScale = roomLossyScale;

            for (int i = 0; i < rowNum; i++)
                for (int j = 0; j < colNum; j++)
                    _backtrackingPath[i, j] = false;
        }

        public void SetStartPosition(char[,] roomModel, List<int> northDoor, List<int> eastDoor, List<int> southDoor)
        {
            if (northDoor != null)
            {
                _startPosX = (roomModel.GetLength(1) - 1) / 2;
                _startPosY = (roomModel.GetLength(0) - 2);
            }
            else if (eastDoor != null)
            {
                _startPosX = (roomModel.GetLength(1) - 1);
                _startPosY = (roomModel.GetLength(0) - 1) / 2;
            }
            else if (southDoor != null)
            {
                _startPosX = (roomModel.GetLength(1) - 1) / 2;
                _startPosY = 0;
            }
            else
            {
                _startPosX = 0;
                _startPosY = (roomModel.GetLength(0) - 1) / 2;
            }
        }

        public List<Vector3> GetSpawnPoints(char[,] roomModel)
        {
            // OBS: i controls matrix rows (Y pos) and j matrix columns (X pos)
            SpawnBacktracking(_startPosY, _startPosX, roomModel);
            return _spawnPoints;
        }

        private void SpawnBacktracking(int i, int j, char[,] roomModel)
        {
            // Check if it is a ground block
            if (roomModel[i, j] == '#')
            {
                return;
            }

            //Check if the upper block is a ground block or upper wall limit
            if (i + 1 < _rowNum && roomModel[i + 1, j] == '#')
            {
                _backtrackingPath[i + 1, j] = true;
                return;
            }

            // Test if the block bellow is a walking block
            if (i - 1 > 0 && roomModel[i - 1, j] == '#')
            {
                _backtrackingPath[i - 1, j] = true;
                _spawnPoints.Add(new Vector3(_xOffset + j * _roomLossyScale.x + 0.5f, _yOffset + i * _roomLossyScale.y + 0.5f, 0));
            }
            // Test the default ground block around the map
            if (i - 1 == 0 && roomModel[i - 1, j] != '#')
            {
                _spawnPoints.Add(new Vector3(_xOffset + j * _roomLossyScale.x + 0.5f, _yOffset + (i - 1) * _roomLossyScale.y + 0.5f, 0));
            }

            // go UP
            if (i + 1 < _rowNum && !_backtrackingPath[i + 1, j])
            {
                _backtrackingPath[i + 1, j] = true;
                SpawnBacktracking(i + 1, j, roomModel);
            }

            // go RIGHT
            if (j + 1 < _colNum && !_backtrackingPath[i, j + 1])
            {
                _backtrackingPath[i, j + 1] = true;
                SpawnBacktracking(i, j + 1,  roomModel);
            }
            // go DOWN
            if (i - 1 >= 0 && !_backtrackingPath[i - 1, j])
            {
                _backtrackingPath[i - 1, j] = true;
                SpawnBacktracking(i - 1, j, roomModel);
            }
            // go LEFT
            if (j - 1 >= 0 && !_backtrackingPath[i, j - 1])
            {
                _backtrackingPath[i, j - 1] = true;
                SpawnBacktracking(i, j - 1, roomModel);
            }
        }
    }
}
