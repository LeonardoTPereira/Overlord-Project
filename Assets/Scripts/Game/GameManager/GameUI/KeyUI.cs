using System;
using System.Collections.Generic;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using UnityEngine;
using Util;

namespace Game.GameManager
{
    public class KeyUI : GridOfElementsUI
    {
        
        [SerializeField]
        protected Sprite keySprite;
        private List<int> _playerKeys;

        private new void Awake()
        {
            base.Awake();
            _playerKeys = new List<int>();
        }

        private void OnEnable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetKeyGUI;
            KeyBhv.KeyCollectEventHandler += CreateKeyImage;
        }

        private void OnDisable()
        {
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetKeyGUI;
            KeyBhv.KeyCollectEventHandler -= CreateKeyImage;
        }


        private void CreateKeyImage(object sender, KeyCollectEventArgs eventArgs)
        {
            _playerKeys.Add(eventArgs.KeyIndex);
            
            var currentKeys = _playerKeys.Count;
            var spriteWidth = keySprite.rect.size.x;

            CreateGridOfImages(currentKeys, spriteWidth, spriteWidth);
            ProcessImageList();
        }
        private void ResetKeyGUI(object sender, EventArgs eventArgs)
        {
            _playerKeys.Clear();
            ClearImages();
        }



        private void ProcessImageList()
        {
            var keyIndex = 0;
            foreach (var keyImage in ImageList)
            {
                keyImage.sprite = keySprite;
                keyImage.color = Constants.ColorId[_playerKeys[keyIndex++] - 1];
            }
        }
    }
}
