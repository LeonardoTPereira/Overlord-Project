using UnityEngine;

namespace Game.GameManager
{
    public class GameOver : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MainMenuButton()
        {
            GameManagerSingleton.Instance.MainMenu();
        }
    }
}
