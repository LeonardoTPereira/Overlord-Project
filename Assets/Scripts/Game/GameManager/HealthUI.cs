using System;
using System.Collections.Generic;
using Game.Events;
using Game.GameManager.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class HealthUI : MonoBehaviour
    {
        private List<Image> _heartList;

        [SerializeField]
        protected Sprite fullHeartSprite, emptyHeartSprite;
        protected float Multiplier = 1.7f;
        protected int Scale = 3;
        protected int Delta = 35;

        private void OnEnable()
        {
            HealthController.PlayerIsDamagedEventHandler += OnDamage;
            PlayerController.ResetHealthEventHandler += ResetHealth;
        }

        private void OnDisable()
        {
            HealthController.PlayerIsDamagedEventHandler -= OnDamage;
            PlayerController.ResetHealthEventHandler -= ResetHealth;
        }

        void Start()
        {
            CreateHeartImage();
        }

        private void ResetHealth(object sender, EventArgs eventArgs)
        {
            if(_heartList != null)
            {
                for (int i = 0; i < Player.Player.Instance.GetComponent<PlayerController>().GetMaxHealth(); ++i)
                {
                    _heartList[i].sprite = fullHeartSprite;
                }
            }
        }


        public void CreateHeartImage()
        {
            int row = 0;
            int col = 0;
            int colMax = 10;


            _heartList = new List<Image>();

            float rowColSize = fullHeartSprite.rect.size.x * Multiplier;
            int actualHealth = Player.Player.Instance.GetComponent<HealthController>().GetHealth();

            for (int i = 0; i < Player.Player.Instance.GetComponent<PlayerController>().GetMaxHealth(); i++)
            {

                Vector2 heartAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);

                GameObject heartGameObject = new GameObject("Heart", typeof(Image));

                // Set as child of this transform
                heartGameObject.transform.SetParent(transform, false);
                heartGameObject.transform.localPosition = Vector3.zero;
                heartGameObject.transform.localScale = new Vector3(Scale, Scale, 1);

                // Locate and Size heart
                heartGameObject.GetComponent<RectTransform>().anchoredPosition = heartAnchoredPosition;
                heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Delta, Delta);

                var heartImageUI = heartGameObject.GetComponent<Image>();
                heartImageUI.sprite = i <= actualHealth ? fullHeartSprite : emptyHeartSprite;

                _heartList.Add(heartImageUI);

                col++;
                if ((col >= colMax) || ((heartGameObject.transform.position.x + Delta + rowColSize) > Screen.width))
                {
                    row++;
                    col = 0;
                }
            }

        }

        private void OnDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {

            if (eventArgs.PlayerHealth < 0)
                eventArgs.PlayerHealth = 0;
            for (int i = 0; i < eventArgs.PlayerHealth; ++i)
                _heartList[i].sprite = fullHeartSprite;
            for (int i = eventArgs.PlayerHealth; i < _heartList.Count; ++i)
                _heartList[i].sprite = emptyHeartSprite;
        }
    }
}
