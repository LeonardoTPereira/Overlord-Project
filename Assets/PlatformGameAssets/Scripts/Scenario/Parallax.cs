using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Scenario
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private float _backgroundSlotSize;
        [SerializeField] private GameObject _camera;
        [SerializeField] private float _parallaxFactor;
        private float _startPos;

        void Start()
        {
            _startPos = this.transform.position.x;
            //_backgroundSlotSize = GetComponent<SpriteRenderer>().bounds.size.x;
            Debug.Log(_backgroundSlotSize);
            _backgroundSlotSize = _backgroundSlotSize;
        }

        void Update()
        {
            float temp = _camera.transform.position.x * (1 - _parallaxFactor);
            float distance = _camera.transform.position.x * _parallaxFactor;

            transform.position = new Vector3(_startPos + distance, transform.position.y, transform.position.z);

            // Repositioning of the background sprites
            if (temp > _startPos + _backgroundSlotSize)
                _startPos += _backgroundSlotSize;
            else if (temp < _startPos - _backgroundSlotSize)
                _startPos -= _backgroundSlotSize;
        }
    }
}
