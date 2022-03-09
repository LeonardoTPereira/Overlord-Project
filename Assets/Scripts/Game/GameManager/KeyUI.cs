using System;
using System.Collections.Generic;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Game.GameManager
{
    public class KeyUI : MonoBehaviour
    {
        private List<Image> keyList;
        // Start is called before the first frame update

        [SerializeField]
        protected Sprite keySprite;
        private List<int> playerKeys;

        private void Awake()
        {
            playerKeys = new List<int>();
            keyList = new List<Image>();
        }

        private void OnEnable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler += ResetKeyGUI;
            KeyBhv.KeyCollectEventHandler += CreateKeyImage;
        }

        private void OnDisable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler -= ResetKeyGUI;
            KeyBhv.KeyCollectEventHandler -= CreateKeyImage;
        }


        private void CreateKeyImage(object sender, KeyCollectEventArgs eventArgs)
        {
            var row = 0;
            var col = 0;
            var colMax = 1;

            ClearKeyImages();

            keyList = new List<Image>();

            var rowColSize = keySprite.rect.size.x * 2.0f;

            playerKeys.Add(eventArgs.KeyIndex);

            var currentKeys = playerKeys.Count;

            var keyAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);
            var keyGameObject = new GameObject("Key", typeof(Image));
            for (var i = 0; i < currentKeys; i++)
            {
                // Set as child of this transform
                keyGameObject.transform.SetParent(transform, false);
                keyGameObject.transform.localPosition = Vector3.zero;
                keyGameObject.transform.localScale = new Vector3(3, 3, 1);

                // Locate and Size heart
                keyGameObject.GetComponent<RectTransform>().anchoredPosition = keyAnchoredPosition;
                keyGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);

                // Set heart sprite
                var keyImageUI = keyGameObject.GetComponent<Image>();
                keyImageUI.sprite = keySprite;
                keyImageUI.color = Constants.colorId[playerKeys[i] - 1];


                keyList.Add(keyImageUI);

                col++;
                if (col < colMax) continue;
                row++;
                col = 0;
            }

        }
        private void ResetKeyGUI(object sender, EventArgs eventArgs)
        {
            playerKeys.Clear();
            ClearKeyImages();

        }

        private void ClearKeyImages()
        {
            for (var i = keyList.Count - 1; i > -1; --i)
            {
                Destroy(keyList[i].gameObject);
            }
            keyList.Clear();
        }
    }
}
