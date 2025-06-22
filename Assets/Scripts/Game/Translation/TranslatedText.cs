using UnityEngine;
using MyBox;
using Game.GameManager;

namespace Game
{
    public class TranslatedText : MonoBehaviour
    {
        [field: SerializeField, MustBeAssigned] private TextMesh textMesh;
        [SerializeField] private string _enText;
        [SerializeField] private string _ptText;

        private void Awake()
        {
            textMesh.text = GameManagerSingleton.Instance.IsInPortuguese ? _ptText : _enText;
        }
    }
}