using UnityEngine;
using UnityEngine.SceneManagement;

public class Quantities : MonoBehaviour
{
    public int numberEnemies = 0, numberItens = 0, numberNpcs = 0;
    public bool gotSecret = false, gotTreasure = false;
    public bool isEnemy;

    public string goodLevel, badLevel;

    public QuestManager manager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Final")
        {
            /*if (isEnemy)
            {
                if (manager.totalEnemies <= numberEnemies && goodLevel != null) SceneManager.LoadScene(goodLevel);
                else if (badLevel != null) SceneManager.LoadScene(badLevel);
                else if (badLevel == "null" && goodLevel == "null") Application.Quit();
            }
            else
            {
                if (totalTreasures <= numberTreasures && goodLevel != null) SceneManager.LoadScene(goodLevel);
                else if (badLevel != null) SceneManager.LoadScene(badLevel);
                else if (badLevel == "null" && goodLevel == "null") Application.Quit();
            }*/
        }
    }

}
