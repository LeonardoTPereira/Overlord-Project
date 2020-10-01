using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private List<Image> heartList;
    // Start is called before the first frame update

    [SerializeField] 
    protected Sprite fullheartSprite, emptyheartSprite;
    protected float multiplier = 1.7f;
    protected int scale = 3;
    protected int delta = 35;

      
    private void Awake()
    {
    }
    void Start()
    {
        //CreateHeartImage();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void CreateHeartImage()
    {
        int row = 0;
        int col = 0;
        int colMax = 10;


        heartList = new List<Image>();

        float rowColSize = fullheartSprite.rect.size.x* multiplier;
        int actualHealth = Player.instance.GetComponent<HealthController>().GetHealth();

        for (int i = 0; i < Player.instance.GetComponent<PlayerController>().GetMaxHealth(); i++)
        {

            Vector2 heartAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);

            GameObject heartGameObject = new GameObject("Heart", typeof(Image));

            // Set as child of this transform
            heartGameObject.transform.SetParent(transform, false);
            heartGameObject.transform.localPosition = Vector3.zero;
            heartGameObject.transform.localScale = new Vector3(scale, scale, 1);

            // Locate and Size heart
            heartGameObject.GetComponent<RectTransform>().anchoredPosition = heartAnchoredPosition;
            heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(delta, delta);

            // Set heart sprite
            Image heartImageUI = heartGameObject.GetComponent<Image>();
            if (i <= actualHealth)
                heartImageUI.sprite = fullheartSprite;
            else
                heartImageUI.sprite = emptyheartSprite;

            heartList.Add(heartImageUI);

            col++;
            if ((col >= colMax) || ((heartGameObject.transform.position.x + delta + rowColSize) > Screen.width))
            {
                row++;
                col = 0;
            }
        }

    }

    public void OnDamage(int health)
    {
        if (health < 0)
            health = 0;
        for (int i = 0; i < health; ++i)
            heartList[i].sprite = fullheartSprite;
        for (int i = health; i < heartList.Count; ++i)
            heartList[i].sprite = emptyheartSprite;
    }
}
