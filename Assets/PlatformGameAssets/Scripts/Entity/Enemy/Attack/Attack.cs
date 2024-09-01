using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public abstract class Attack : MonoBehaviour
    {
        
        [SerializeField] protected GameObject bullet;
        [SerializeField] private float fireRate;
        [SerializeField] private float particleDelay;
        [SerializeField] private Animator animator;
        [SerializeField] protected Transform mouth;
        [SerializeField] private float shootOffSet = 0.5f;
        [SerializeField] private ParticleSystem salivaParticle;

        //private TransformController transformController;
        
        private void Start()
        {
            Initialize();

            //transformController = GetComponent<TransformController>();
            //transformController.LookAt(GameObject.FindWithTag("Player").transform);
        
            InvokeRepeating("AnimateShoot", 0f, fireRate);
            InvokeRepeating("Shoot", shootOffSet, fireRate);
            InvokeRepeating("PlayParticle", 0f, fireRate + particleDelay);
        }

        protected abstract void Initialize();

        private void AnimateShoot()
        {
            animator.SetTrigger("Attack");
        }

        protected abstract void Shoot();

        private void PlayParticle()
        {
            salivaParticle.Play();
        }

    }

    
}