using UnityEngine;
using MyBox;
using Game.GameManager;
using TMPro;

namespace Game
{
    public class TranslatedText : MonoBehaviour
    {
        [field: SerializeField, MustBeAssigned] private TextMeshProUGUI textMesh;
        [SerializeField] private string _enText;
        [SerializeField] private string _ptText;

        private void Awake()
        {
            Translate();
        }

        public void Translate()
        {
            textMesh.text = GameManagerSingleton.Instance.IsInPortuguese ? _ptText : _enText;
        }
    }
}