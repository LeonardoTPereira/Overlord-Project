using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Scenario
{
    public class ParallaxConfigs : MonoBehaviour
    {
        public static ParallaxConfigs Instance;

        [Header("Size of background chunk")] // Put the background chunk sprite in it and get "gameObject.GetComponent<SpriteRenderer>().bounds.size.x" value
        public float chunkSize;

        [Header("Parallax Factor in X axis")]
        [Range(0.0f, 1.0f)]
        public float closestX;
        [Range(0.0f, 1.0f)]
        public float closeX;
        [Range(0.0f, 1.0f)]
        public float middleCloseX;
        [Range(0.0f, 1.0f)]
        public float middleX;
        [Range(0.0f, 1.0f)]
        public float middleFarX;
        [Range(0.0f, 1.0f)]
        public float farX;
        [Range(0.0f, 1.0f)]
        public float farthestX;


        [Header("Parallax Factor in X axis")]
        [Range(0.0f, 1.0f)]
        public float closestY;
        [Range(0.0f, 1.0f)]
        public float closeY;
        [Range(0.0f, 1.0f)]
        public float middleCloseY;
        [Range(0.0f, 1.0f)]
        public float middleY;
        [Range(0.0f, 1.0f)]
        public float middleFarY;
        [Range(0.0f, 1.0f)]
        public float farY;
        [Range(0.0f, 1.0f)]
        public float farthestY;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
    }
}