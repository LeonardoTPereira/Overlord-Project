using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Util
{
    public class EdgeTester : MonoBehaviour
    {
        public bool IsOnGround = true;
        [HideInInspector] public UnityEvent<Collider2D> edgeTesterOnTriggerExit;

        private void OnTriggerExit2D(Collider2D collision)
        {
            edgeTesterOnTriggerExit?.Invoke(collision);
        }
    }
}
