using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follow the player position 
namespace PlatformGame.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject _camera;
        [SerializeField] private GameObject _player; 
        [SerializeField] private Vector3 _initialPos;

        // Update is called once per frame
        void Update()
        {
            _camera.transform.position = _player.transform.position + _initialPos;
        }
    }
}
