using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Events.Enemy
{
    public class EnemyDestroyerAnimationEvent : MonoBehaviour
    {
        public void DestroyEnemy()
        {
            var parentGameObject = gameObject.transform.parent.gameObject;
            Destroy(parentGameObject);
        }
    }
}