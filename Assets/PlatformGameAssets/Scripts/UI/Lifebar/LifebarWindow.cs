using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Player;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class LifebarWindow : MonoBehaviour
{
    [SerializeField] private UIDocument _UIDocument;
    [SerializeField] private Sprite _emptyHeart;
    [SerializeField] private Sprite _halfHeart;
    [SerializeField] private Sprite _fullHeart;

    /* CONSTANTS */
    private const int EMPTY_HEART = 0;
    private const int HALF_HEART = 1;
    private const int FULL_HEART = 2;
    private const int HEARTS_PER_LINE = 10;
    private const int HEARTS_TO_SECOND_LINE = 10;
    private const int FIRST_LINE = 0;
    private const int SECOND_LINE = 1;

    private VisualElement _root;
    private VisualElement _firstLine;
    private VisualElement _secondLine;

    private List<Heart> _hearts = new List<Heart>();// Store a list of hearts, showed in the Lifebar_UI
    private int _currentHeartIndex = 0;             // The last non empty heart on the list of hearts
    private int _currentHeartLine = FIRST_LINE;     // The line of the last non-empty heart on the list of hearts
    private int _currentHeartState = EMPTY_HEART;   // The state of the last non-empty heart on the list of hearts

    int t;

    void Awake()
    {
        PlayerHealth.InitializePlayerHealthEvent += UI_SetMaxLife;
        PlayerHealth.PlayerTakeDamageEvent += UI_TakeDamage;
        _root = _UIDocument.rootVisualElement;
        _firstLine = _root.Q<GroupBox>("first-line");
        _secondLine = _root.Q<GroupBox>("second-line");
    }

     // Creates a 'VisualElement' Heart and add it to the life bar and the list of hearts
    private void CreateHeart()
    {
        // Create an Heart with its right icon and uss and add to the list of hearts
        Heart heart = new Heart(_fullHeart);
        _hearts.Add(heart);

        // Add the heart accordly to the line in the Lifebar UI
        switch (_currentHeartLine)
        {
            case FIRST_LINE:
                _firstLine.Add(heart);
                break;
            case SECOND_LINE:
                _secondLine.Add(heart);
                break;
        }

        // Update the heart if the maximum heart by line were achieved
        if (_hearts.Count >= HEARTS_TO_SECOND_LINE)
        {
            _currentHeartLine = SECOND_LINE;
        }
    }

    // Sets the max number of hearts to the life bar
    // OBS: 2 equals to 1 heart and 1 equals to half heart
    //      so even if the 'maxLife' is even, a full heart will be added
    public void UI_SetMaxLife(int maxLife)
    {
        int correctLife = maxLife;     
        for (int i = 0; i < correctLife; i += 2)
        {
            CreateHeart();
        }

        _currentHeartIndex = _hearts.Count - 1;
        if (correctLife % FULL_HEART == 1)
        {
            _currentHeartState = HALF_HEART;
            _hearts[_currentHeartIndex].UpdateIcon(_halfHeart);
        }
        else
        {
            _currentHeartState = FULL_HEART;
        }
        t = maxLife;
        Debug.Log(t);
    }

    // Clear hearts in the Lifebar_UI, between two given indexes
    // it also updates the currentHeartIndex and currentHeartLine
    private void ClearHeart(int indexFrom, int indexTo)
    {
        if (indexFrom < 0)
            indexFrom = 0;

        for (int i = indexFrom; i < indexTo; i++)
        {
            _hearts[i].UpdateIcon(_emptyHeart);
        }

    }

    // Updates the lifebar UI accordly to the damage taken by the player
    // OBS: 1 Damage equals to half heart, and 2 is a full heart of damage
    private void UI_TakeDamage(int damage)
    {
        int numberOfHeartsDestroyed = damage / 2; // One heart stores 2 hitpoints, so it convert damagepoits to hearts destroyed

        // Updates the Lifebar_UI accordly to the number of hearts damaged
        ClearHeart(_currentHeartIndex - numberOfHeartsDestroyed + 1, _currentHeartIndex + 1);

        _currentHeartIndex -= numberOfHeartsDestroyed;
                
        // After the number of full hearts were removed in the Lifebar_UI, it updates the heart at the current index
        // and tests if there is a damage (1 damage = HALF_HEART damaged) that needs to be treated
        if (_currentHeartIndex >= 0)
        {
            switch (_currentHeartState)
            {
                case HALF_HEART:
                    if (damage % FULL_HEART == 1)
                    {
                        _hearts[_currentHeartIndex].UpdateIcon(_emptyHeart);
                        _currentHeartIndex--;
                        _currentHeartState = FULL_HEART;

                        if (_hearts.Count <= HEARTS_TO_SECOND_LINE)
                        {
                            _currentHeartLine = FIRST_LINE;
                        }
                    }
                    else
                    {
                        _hearts[_currentHeartIndex].UpdateIcon(_halfHeart);
                    }
                    break;
                case FULL_HEART:
                    if (damage % FULL_HEART == 1)
                    {
                        _hearts[_currentHeartIndex].UpdateIcon(_halfHeart);
                        _currentHeartState = HALF_HEART;
                    }
                    break;
            }
        }

        // Updates the 'current heart' line
        if (_currentHeartIndex < HEARTS_TO_SECOND_LINE)
            _currentHeartLine = FIRST_LINE;

        t = t - damage;
        //Debug.Log(t);
    }
}