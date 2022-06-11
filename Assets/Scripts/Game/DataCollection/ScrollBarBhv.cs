using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class ScrollBarBhv : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(ResetScrollPos());
        }

        private IEnumerator ResetScrollPos()
        {
            yield return null; // Waiting just one frame is probably good enough, yield return null does that
            gameObject.GetComponent<Scrollbar>().value = 1;
        }
    }
}
