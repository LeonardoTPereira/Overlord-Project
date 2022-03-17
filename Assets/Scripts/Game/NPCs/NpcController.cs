using System.Linq;
using Fog.Dialogue;
using UnityEngine;

namespace Game.NPCs
{
    public class NpcController : MonoBehaviour, IInteractable {
        [SerializeField] private Dialogue dialogue;

        public void Reset() {
            var nColliders = GetComponents<Collider2D>().Length;
            if (nColliders == 1) {
                GetComponent<Collider2D>().isTrigger = true;
            } else if (nColliders > 0) {
                var hasTrigger = HasAtLeastOneTrigger();
                if (!hasTrigger) {
                    GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }

        private bool HasAtLeastOneTrigger()
        {
            return GetComponents<Collider2D>().Any(col => col.isTrigger);
        }

        public void OnInteractAttempt() {
            if (dialogue != null) {
                DialogueHandler.instance.StartDialogue(dialogue);
            }
        }

        public void OnTriggerEnter2D(Collider2D col) {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Add(this);
            }
        }

        public void OnTriggerExit2D(Collider2D col) {
            var agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Remove(this);
            }
        }
        
    }
}