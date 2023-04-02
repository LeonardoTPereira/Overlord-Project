using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Scenario;

namespace PlatformGame.Scenario
{
    public class Parallax : MonoBehaviour
    {
        enum BackgroundType
        {
            Farthest, Far, MiddleFar, Middle, MiddleClose, Close, Closest
        }

        [SerializeField] private BackgroundType _backgroundType;

        private float _backgroundSlotSize; // background sprite size in X axis
        private GameObject _camera;
        private float _parallaxFactorInX;
        private float _parallaxFactorInY;

        private float _xCentralPos;
        private float _yCentralPos;
        private float _temp;
        private float _xDistance;
        private float _yDistance;

        void Start()
        {
            GetParallaxConfigs();
            _xCentralPos = this.transform.position.x;
            _yCentralPos = this.transform.position.y;                    

            _backgroundSlotSize = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;    // has to have a gameobject with a spriterender used as background
        }

        private void GetParallaxConfigs()
        {
            ParallaxConfigs _parallaxConfigs = ParallaxConfigs.Instance;

            switch (_backgroundType)
            {
                case BackgroundType.Farthest:
                    _parallaxFactorInX = _parallaxConfigs.farthestX;
                    //_parallaxFactorInY = _parallaxConfigs.farthestY;
                    break;
                case BackgroundType.Far:
                    _parallaxFactorInX = _parallaxConfigs.farX;
                    //_parallaxFactorInY = _parallaxConfigs.farY;
                    break;
                case BackgroundType.MiddleFar:
                    _parallaxFactorInX = _parallaxConfigs.middleFarX;
                    //_parallaxFactorInY = _parallaxConfigs.middleFarY;
                    break;
                case BackgroundType.Middle:
                    _parallaxFactorInX = _parallaxConfigs.middleX;
                    //_parallaxFactorInY = _parallaxConfigs.middleY;
                    break;
                case BackgroundType.MiddleClose:
                    _parallaxFactorInX = _parallaxConfigs.middleCloseX;
                    //_parallaxFactorInY = _parallaxConfigs.middleCloseY;
                    break;
                case BackgroundType.Close:
                    _parallaxFactorInX = _parallaxConfigs.closeX;
                    //_parallaxFactorInY = _parallaxConfigs.closeY;
                    break;
                case BackgroundType.Closest:
                    _parallaxFactorInX = _parallaxConfigs.closestX;
                    //_parallaxFactorInY = _parallaxConfigs.closestY;
                    break;
            }
            _camera = _parallaxConfigs.mainCamera;
            _parallaxFactorInY = _parallaxFactorInX * _parallaxConfigs.yFactor;
            _backgroundSlotSize = _parallaxConfigs.chunkSize;
        }

        void Update()
        {
#if UNITY_EDITOR
            GetParallaxConfigs();
#endif
            _temp = _camera.transform.position.x * (1 - _parallaxFactorInX);
            _xDistance = _camera.transform.position.x * _parallaxFactorInX;
            _yDistance = (_camera.transform.position.y - _yCentralPos) * _parallaxFactorInY;

            transform.position = new Vector3(_xCentralPos + _xDistance, _yCentralPos + _yDistance, transform.position.z);

            // Repositioning of the background sprites
            if (_temp > _xCentralPos + _backgroundSlotSize * 2)
                _xCentralPos += _backgroundSlotSize * 3;
            else if (_temp < _xCentralPos - _backgroundSlotSize * 2)
                _xCentralPos -= _backgroundSlotSize * 3;
        }
    }
}