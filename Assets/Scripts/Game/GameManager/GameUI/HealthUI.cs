using System;
using Game.Events;
using Game.GameManager.Player;
using UnityEngine;

namespace Game.GameManager
{

    
    public class HealthUI : GridOfElementsUI
    {
        [SerializeField]
        protected Sprite fullHeartSprite, emptyHeartSprite;

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

        private void Start()
        {
            var maxSize = Player.DungeonPlayer.Instance.GetComponent<PlayerController>().GetMaxHealth();
            var spriteWidth = fullHeartSprite.rect.size.x;
            CreateGridOfImages(maxSize, spriteWidth, spriteWidth);
            ProcessImageList(maxSize);
        }

        private void ResetHealth(object sender, EventArgs eventArgs)
        {
            if (ImageList == null) return;
            for (var i = 0; i < Player.DungeonPlayer.Instance.GetComponent<PlayerController>().GetMaxHealth(); ++i)
            {
                ImageList[i].sprite = fullHeartSprite;
            }
        }

        private void OnDamage(object sender, PlayerIsDamagedEventArgs eventArgs)
        {

            if (eventArgs.PlayerHealth < 0)
                eventArgs.PlayerHealth = 0;
            for (var i = 0; i < eventArgs.PlayerHealth; ++i)
                ImageList[i].sprite = fullHeartSprite;
            for (var i = eventArgs.PlayerHealth; i < ImageList.Count; ++i)
                ImageList[i].sprite = emptyHeartSprite;
        }

        private void ProcessImageList(int currentHealth)
        {
            for (var heartIndex = 0; heartIndex < ImageList.Count; heartIndex++)
            {
                var image = ImageList[heartIndex];
                image.sprite = heartIndex <= currentHealth ? fullHeartSprite : emptyHeartSprite;
            }
        }
    }
}
