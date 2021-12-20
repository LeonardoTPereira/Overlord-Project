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
        List<int> playerKeys;

        private void Awake()
        {
            playerKeys = new List<int>();
            keyList = new List<Image>();
        }
        void Start()
        {
            //CreateHeartImage();
        }

        private void OnEnable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler += ResetKeyGUI;
            KeyBHV.KeyCollectEventHandler += CreateKeyImage;
        }

        private void OnDisable()
        {
            GameManagerSingleton.NewLevelLoadedEventHandler -= ResetKeyGUI;
            KeyBHV.KeyCollectEventHandler -= CreateKeyImage;
        }


        private void CreateKeyImage(object sender, KeyCollectEventArgs eventArgs)
        {
            int row = 0;
            int col = 0;
            int colMax = 1;

            clearKeyImages();

            keyList = new List<Image>();

            float rowColSize = keySprite.rect.size.x * 2.0f;

            playerKeys.Add(eventArgs.KeyIndex);

            int currentKeys = playerKeys.Count;

            for (int i = 0; i < currentKeys; i++)
            {

                Vector2 keyAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);

                GameObject keyGameObject = new GameObject("Key", typeof(Image));

                // Set as child of this transform
                keyGameObject.transform.SetParent(transform, false);
                keyGameObject.transform.localPosition = Vector3.zero;
                keyGameObject.transform.localScale = new Vector3(3, 3, 1);

                // Locate and Size heart
                keyGameObject.GetComponent<RectTransform>().anchoredPosition = keyAnchoredPosition;
                keyGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);

                // Set heart sprite
                Image keyImageUI = keyGameObject.GetComponent<Image>();
                keyImageUI.sprite = keySprite;
                keyImageUI.color = Constants.colorId[playerKeys[i] - 1];


                keyList.Add(keyImageUI);

                col++;
                if (col >= colMax)
                {
                    row++;
                    col = 0;
                }
            }

        }
        private void ResetKeyGUI(object sender, EventArgs eventArgs)
        {
            playerKeys.Clear();
            clearKeyImages();

        }

        private void clearKeyImages()
        {
            for (int i = keyList.Count - 1; i > -1; --i)
            {
                Destroy(keyList[i].gameObject);
            }
            keyList.Clear();
        }
    }
}
