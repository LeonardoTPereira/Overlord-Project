﻿using System;
using System.Collections.Generic;
using Game.LevelManager.DungeonLoader;
using TMPro;
using UnityEngine;

namespace Game.GameManager
{
    public class ComboUI : MonoBehaviour
    {
        protected int actualCombo;
        protected TextMeshProUGUI comboText;
        protected ComboStruct actualComboStruct;
        static readonly Dictionary<int, ComboStruct> COMBOMAP = new Dictionary<int, ComboStruct>
        {
            {0, new ComboStruct("NO", Color.gray, Color.black)},
            {3, new ComboStruct("OK", Color.white, Color.gray)},
            {6, new ComboStruct("SUPER", Color.white, Color.gray)},
            {9, new ComboStruct("HYPER", Color.white, Color.gray)},
            {12, new ComboStruct("BRUTAL", Color.white, Color.gray)},
            {15, new ComboStruct("MASTER", new Color(0.431f, 0.741f, 0.749f), Color.white)},
            {18, new ComboStruct("AWESOME",new Color(0.247f, 0.890f, 0.913f), new Color(0.431f, 0.741f, 0.749f))},
            {21, new ComboStruct("BLASTER", new Color(0.478f, 0.882f, 0.521f), Color.white)},
            {25, new ComboStruct("MONSTER", new Color(0.282f, 0.917f, 0.349f), new Color(0.478f, 0.882f, 0.521f))},
            {30, new ComboStruct("KING", new Color(0.949f, 0.886f, 0.149f), Color.white)},
            {35, new ComboStruct("KILLER", new Color(0.956f, 1.0f, 0.2f), new Color(0.949f, 0.83f, 0.1f))},
            {40, new ComboStruct("ULTRA", new Color(0.929f, 0.541f, 0.070f), new Color(0.882f, 0.341f, 0.019f))},
            {45, new ComboStruct("ULTIMATE", new Color(0.996f, 0.094f, 0.086f), new Color(0.5f, 0.0f, 0.019f))}
        };

        protected struct ComboStruct
        {
            public ComboStruct(string name, Color top, Color bottom)
            {
                this.name = name;
                topColor = top;
                bottomColor = bottom;
            }
            public readonly string name;
            public readonly Color topColor, bottomColor;
        };

        // Start is called before the first frame update
        void Awake()
        {
            comboText = GetComponent<TextMeshProUGUI>();
            actualCombo = 0;
        }

        void Start()
        {
            UpdateComboGUI();
        }

        protected void OnEnable()
        {
            ProjectileController.EnemyHitEventHandler += IncrementCombo;
            BombController.PlayerHitEventHandler += ResetCombo;
            ProjectileController.PlayerHitEventHandler += ResetCombo;
            EnemyController.PlayerHitEventHandler += ResetCombo;
            DungeonSceneManager.NewLevelLoadedEventHandler += ResetCombo;
        }

        protected void OnDisable()
        {
            ProjectileController.EnemyHitEventHandler -= IncrementCombo;
            BombController.PlayerHitEventHandler -= ResetCombo;
            ProjectileController.PlayerHitEventHandler -= ResetCombo;
            EnemyController.PlayerHitEventHandler -= ResetCombo;
            DungeonSceneManager.NewLevelLoadedEventHandler -= ResetCombo;
        }

        public void IncrementCombo(object sender, EventArgs eventArgs)
        {
            actualCombo++;
            UpdateComboGUI();
        }

        public void ResetCombo(object sender, EventArgs eventArgs)
        {
            actualCombo = 0;
            UpdateComboGUI();
        }

        public void UpdateComboGUI()
        {
            // When a program often has to try keys that turn out not to
            // be in the dictionary, TryGetValue can be a more efficient
            // way to retrieve values.
            if (COMBOMAP.TryGetValue(actualCombo, out actualComboStruct))
            {
                comboText.text = actualComboStruct.name;
                comboText.colorGradient = new VertexGradient(actualComboStruct.topColor, actualComboStruct.topColor, actualComboStruct.bottomColor, actualComboStruct.bottomColor);
            }
        }
    }
}
