using System;
using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
    public class InventoryUI : GridOfElementsUI
    {

        private Dictionary<ItemSo, int> _itemAmountDictionary;
        private List<GameObject> _textList;
        private const int TextWidth = 7;

        protected void OnEnable()
        {
            TreasureController.TreasureCollectEventHandler += AddItem;
            NpcController.ItemTradeEventHandler += TradeItems;
            NpcController.ItemGiveEventHandler += GiveItem;
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetTreasure;

        }

        private void GiveItem(object sender, ItemGiveEventArgs eventArgs)
        {
            RemoveItem(eventArgs.GivenItem);
            UpdateUI(eventArgs.GivenItem);
        }

        protected void OnDisable()
        {
            TreasureController.TreasureCollectEventHandler -= AddItem;
            NpcController.ItemTradeEventHandler -= TradeItems;
            NpcController.ItemGiveEventHandler -= GiveItem;
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetTreasure;
        }

        private new void Awake()
        {
            base.Awake();
            _itemAmountDictionary = new Dictionary<ItemSo, int>();
            _textList = new List<GameObject>();
        }

        private void AddItem(object sender, TreasureCollectEventArgs eventArgs)
        {
            AddItemToDictionary(eventArgs.Item);
            UpdateUI(eventArgs.Item);
        }

        private void UpdateUI(ItemSo itemSo)
        {
            var currentItems = _itemAmountDictionary.Count;
            var spriteWidth = itemSo.sprite.rect.size.x;
            ClearTreasureImages();
            CreateGridOfImages(currentItems, spriteWidth, spriteWidth*6);
            ProcessImageList();
        }

        private void AddItemToDictionary(ItemSo itemSo)
        {
            if (!_itemAmountDictionary.ContainsKey(itemSo))
            {
                _itemAmountDictionary.Add(itemSo, 0);
            }
            _itemAmountDictionary[itemSo]++;
        }

        private void TradeItems(object sender, ItemTradeEventArgs eventArgs)
        {
            foreach (var items in eventArgs.GivenItems)
            {
                foreach (var unused in items.Value.QuestIds)
                {
                    RemoveItem(items.Key);
                }
            }
            AddItemToDictionary(eventArgs.Item);
            UpdateUI(eventArgs.GivenItems.First().Key);
        }
        
        private void RemoveItem(ItemSo item)
        {
            if (!_itemAmountDictionary.ContainsKey(item)) return;
            _itemAmountDictionary[item]--;
            if (_itemAmountDictionary[item] == 0)
            {
                _itemAmountDictionary.Remove(item);
            }
        }


        private void ResetTreasure(object sender, EventArgs eventArgs)
        {
            _itemAmountDictionary.Clear();
            ClearTreasureImages();
        }
        
        private void ProcessImageList()
        {
            var itemList = _itemAmountDictionary.Keys.ToList();
            for (var imageIndex = 0; imageIndex < ImageList.Count; imageIndex++)
            {
                var keyImage = ImageList[imageIndex];
                var itemSo = itemList[imageIndex];
                keyImage.sprite = itemSo.sprite;
                var amountText = new GameObject("AmountText", typeof(TextMeshProUGUI));
                amountText.transform.SetParent(transform, false);
                amountText.transform.localPosition = Vector3.zero;
                var rectTransform = amountText.GetComponent<RectTransform>();
                var itemAnchoredPosition = keyImage.GetComponent<RectTransform>().anchoredPosition;
                rectTransform.anchoredPosition = itemAnchoredPosition + new Vector2(keyImage.sprite.rect.size.x*TextWidth, 0);
                amountText.GetComponent<TextMeshProUGUI>().text = " x"+_itemAmountDictionary[itemSo];
                _textList.Add(amountText);
            }
        }

        private void ClearTreasureImages()
        {
            if (ImageList == null) return;
            for (var i = ImageList.Count - 1; i > -1; --i)
            {
                Destroy(_textList[i].gameObject);
                Destroy(ImageList[i].gameObject);
            }
            ImageList.Clear();
            _textList.Clear();
        }
    }
}