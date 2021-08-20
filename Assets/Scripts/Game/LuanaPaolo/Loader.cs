using UnityEngine;

public class Loader : MonoBehaviour
{
    private GameManager gameManager;

    LevelConfigSO levelConfig;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadNewLevel(levelConfig.fileName);
    }
}
