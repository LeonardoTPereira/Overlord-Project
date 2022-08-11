using UnityEngine;

namespace DialogueTrees
{
    public class TreeRunner : MonoBehaviour
    {
        public DialogueTree tree;

        private void Start()
        {
            tree = tree.Clone();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
