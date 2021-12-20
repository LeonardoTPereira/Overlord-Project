using UnityEngine;

namespace Game.GameManager
{
    public class DestroyParticle : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Destroy(gameObject, 0.7f);
        }
    }
}
