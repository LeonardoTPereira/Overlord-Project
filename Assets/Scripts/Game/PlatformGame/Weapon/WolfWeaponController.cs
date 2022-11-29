using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Weapons.Melee
{
    public class WolfWeaponController : MeleeWeaponController
    {
        [SerializeField] private float wolfTackleForce = 10f;
        private Rigidbody2D _wolfRigidBody2D;

        protected override void Awake()
        {
            base.Awake();
            _wolfRigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        protected override void OnUsing()
        {
            StartCoroutine(DoTackle());
        }

        private IEnumerator DoTackle()
        {
            var wolfTransform = transform;
            var force = (wolfTransform.up*2 + wolfTransform.right).normalized;
            force = force * wolfTackleForce;
            _wolfRigidBody2D.AddForce(force, ForceMode2D.Impulse);
            yield return new WaitForSeconds(attackDuration);
            _wolfRigidBody2D.velocity = Vector2.zero;
        }
    }
}