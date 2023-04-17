using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Enemy
{
    
    public class MoveTwoWay : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float maxHorizontalDistance = 1f;
        [SerializeField] private float maxVerticalDistance = 1f;
        [SerializeField] private float _flipCooldown = 1.0f;
        private bool _isInFlipCooldown = false;

        private Transform currentPoint;

        private TransformController transformController;
    
        private Rigidbody2D enemyRb;

        private void Awake()
        {
            transformController = GetComponent<TransformController>();
            enemyRb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            enemyRb.velocity = transform.right * speed + new Vector3(0, enemyRb.velocity.y, 0);
        
            if (!_isInFlipCooldown && CheckFlip())
            {
                StartCoroutine(StartFlipCooldown());
                transformController.Flip();
            }
        }

        IEnumerator StartFlipCooldown()
        {
            _isInFlipCooldown = true;
            yield return new WaitForSeconds(_flipCooldown);
            _isInFlipCooldown = false;
        }

        private bool CheckFlip()
        {
            var currentTransform = transform;

            bool hasNotGround = (Physics2D.Raycast(currentTransform.position + currentTransform.right * maxHorizontalDistance,
                -currentTransform.up,
                maxVerticalDistance, mask).collider == null);
            bool hasFrontBlock = (Physics2D.Raycast(currentTransform.position,
                currentTransform.position + currentTransform.right,
                maxVerticalDistance, mask).collider != null);

            Debug.Log("hasNotGround:" + hasNotGround + "hasFrontBlock:" + hasFrontBlock);


            return hasNotGround || hasFrontBlock;
        }

    }

    
}
