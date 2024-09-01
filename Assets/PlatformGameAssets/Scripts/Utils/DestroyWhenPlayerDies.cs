using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Player;

namespace PlatformGame.Util
{
    public class DestroyWhenPlayerDies : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            PlayerHealth.PlayerDiedEvent += DestroyThisObject;
        }

        private void DestroyThisObject()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void OnDisable()
        {
            PlayerHealth.PlayerDiedEvent -= DestroyThisObject;
        }
    }
}