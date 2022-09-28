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

        private float _xStartPos;
        private float _yStartPos;
        private float _temp;
        private float _xDistance;
        private float _yDistance;

        void Start()
        {
            _xStartPos = this.transform.position.x;
            _yStartPos = this.transform.position.y;
            //Debug.Log(GetComponent<SpriteRenderer>().bounds.size.x);
        }

        void Update()
        {
            _temp = _camera.transform.position.x * (1 - _parallaxFactor);
            _xDistance = _camera.transform.position.x * _parallaxFactor;
            _yDistance = _camera.transform.position.y * _parallaxFactor/2;

            transform.position = new Vector3(_xStartPos + _xDistance, _yStartPos + _yDistance, transform.position.z);

            
            // Repositioning of the background sprites
            if (_temp > _xStartPos + _backgroundSlotSize * 2)
                _xStartPos += _backgroundSlotSize * 3;
            else if (_temp < _xStartPos - _backgroundSlotSize * 2)
                _xStartPos -= _backgroundSlotSize * 3;            
        }
    }
}
