using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DataCollection
{
    public class FormCheckboxBhv : MonoBehaviour
    {
        public Toggle[] toggles;
        public Text questionText;
        public Text descriptionText;
               
        void Awake()
        {
            toggles = GetComponentsInChildren<Toggle>().ToArray<Toggle>();
        }        
    }
}
