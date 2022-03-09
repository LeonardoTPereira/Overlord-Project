using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private NpcDialogueSO dialogues;
        [SerializeField] private GameObject canvas, npcName, dialogue;
        [field: SerializeField] public NpcSO NpcSo { get; set; }

        public static event EventHandler DialogueOpenEventHandler;
        public static event EventHandler DialogueCloseEventHandler;

        // Start is called before the first frame update
        public void Start()
        {
            dialogue.GetComponent<TextMeshProUGUI>().text = dialogues.Dialogues[UnityEngine.Random.Range(0, 3)];
            npcName.GetComponent<TextMeshProUGUI>().text = NpcSo.NpcName;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("PlayerTrigger")) return;
            canvas.SetActive(true);
            DialogueOpenEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                canvas.SetActive(false);
                DialogueCloseEventHandler?.Invoke(null, EventArgs.Empty);
            }
        }

    }
}
