using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class GridOfElementsUI : MonoBehaviour
    {
        protected List<Image> ImageList { get; set; }

        private const float Multiplier = 1f;
        private const int Scale = 2;

        protected void Awake()
        {
            ImageList = new List<Image>();
        }

        protected void CreateGridOfImages(int maxSize, float spriteWidth, float distance)
        {
            var row = 0;
            var col = 0;
            const int colMax = 10;

            ClearImages();

            var rowColSize = distance;

            for (var i = 0; i < maxSize; i++)
            {
                var addedImage = CreateAndPlaceItemImage(col, rowColSize, row, spriteWidth);
                ImageList.Add(addedImage);
                col++;
                if (col < colMax) continue;
                row++;
                col = 0;
            }
        }

        private Image CreateAndPlaceItemImage(int col, float rowColSize, int row, float spriteWidth)
        {
            var itemAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);

            var itemGameObject = new GameObject("Item", typeof(Image));

            itemGameObject.transform.SetParent(transform, false);
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.transform.localScale = new Vector3(Scale, Scale, 1);

            itemGameObject.GetComponent<RectTransform>().anchoredPosition = itemAnchoredPosition;
            itemGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteWidth, spriteWidth);

            var itemImageUI = itemGameObject.GetComponent<Image>();
            return itemImageUI;
        }
        
        protected void ClearImages()
        {
            if (ImageList == null) return;
            for (var i = ImageList.Count - 1; i > -1; --i)
            {
                Destroy(ImageList[i].gameObject);
            }
            ImageList.Clear();
        }
    }
}