using System.Collections;
using UnityEngine;

namespace Game.GameManager
{
    public class MinimapCameraController : MonoBehaviour
    {
        private const float CameraSpeed = 70f;
        public IEnumerator SmoothlyMoveCameraToRoom(Vector3 destination)
        {
            var step = CameraSpeed * Time.deltaTime;
            while (Vector3.Distance(transform.position, destination) > 1f)
            {
                yield return transform.position = Vector3.MoveTowards(transform.position, destination, step);
            }
        }
    }
} 