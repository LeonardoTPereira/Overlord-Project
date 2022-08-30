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

    void Awake()
    {
        PlayerHealth.InitializePlayerHealthEvent += UI_SetMaxLife;
        PlayerHealth.PlayerTakeDamageEvent += UI_TakeDamage;
        _root = _UIDocument.rootVisualElement;
        _firstLine = _root.Q<GroupBox>("first-line");
        _secondLine = _root.Q<GroupBox>("second-line");
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

        if (_hearts.Count <= HEARTS_TO_SECOND_LINE)
        {
            _currentHeartLine = FIRST_LINE;
        }
    }

    // Creates a 'VisualElement' and add it to the life bar
    private void CreateHeart()
    {

        // Create an Heart with its right icon and uss
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
        for (int i = 0; i < maxLife; i += 2)
        {
            CreateHeart();
        }
        _currentHeartState = FULL_HEART;
        _currentHeartIndex = maxLife % HEARTS_PER_LINE * 2 - 1;
        if (_currentHeartIndex == -1)
            _currentHeartIndex = 9;
    }

    // Updates the lifebar UI accordly to the damage taken by the player
    // OBS: 1 Damage equals to half heart, and 2 is a full heart of damage
    private void UI_TakeDamage(int damage)
    {
        int numberOfHeartsDestroyed = damage / 2; // One heart stores 2 hitpoints, so it convert damagepoits to hearts destroyed
        int currentIndexInListOfHearts = _currentHeartIndex + HEARTS_PER_LINE * _currentHeartLine;

        ClearHeart(currentIndexInListOfHearts - numberOfHeartsDestroyed, currentIndexInListOfHearts + 1);
        _currentHeartIndex -= numberOfHeartsDestroyed;

        if (_currentHeartIndex >= 0 && damage % 2 == 1)
        {
            switch (_currentHeartState)
            {
                case HALF_HEART:
                    _hearts[_currentHeartIndex].UpdateIcon(_emptyHeart);
                    _currentHeartIndex--;
                    _currentHeartState = FULL_HEART;

                    if (_hearts.Count <= HEARTS_TO_SECOND_LINE)
                    {
                        _currentHeartLine = FIRST_LINE;
                    }
                    break;
                case FULL_HEART:
                    _hearts[_currentHeartIndex].UpdateIcon(_halfHeart);
                    break;
            }
        }
    }
}