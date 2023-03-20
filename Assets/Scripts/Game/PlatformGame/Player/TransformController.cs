using System.Collections;
using UnityEngine;

namespace PlatformGame.Player
{
    public class TransformController : MonoBehaviour
    {
        [SerializeField] private float updateLookTime = 0.5f;

        private Transform _target;

        private void Start()
        {
            StartCoroutine(UpdateLook());
        }

        public void Flip()
        {
            var currentTransform = transform;
            currentTransform.Rotate(currentTransform.up, 180);
            var currentScale = currentTransform.localScale;
            currentTransform.localScale = new Vector3(currentScale.x, currentScale.y, -currentScale.z);
        }

        public void LookAt(Transform target)
        {
            this._target = target;
        }

        public void LookOneTimeAt(Transform target)
        {
            var currentTransform = transform;
            if (Vector3.Dot(target.position - currentTransform.position, currentTransform.right) < 0)
                Flip();
        }

        public void StopLooking()
        {
            LookAt(null);
        }

        private IEnumerator UpdateLook()
        {
            while (true)
            {
                yield return new WaitForSeconds(updateLookTime);

                if (CheckFlipCondition())
                    Flip();
            }
        }

        private bool CheckFlipCondition()
        {
            var currentTransform = transform;
            return _target != null && Vector3.Dot(_target.position - currentTransform.position, currentTransform.right) < 0;
        }


    }
}
