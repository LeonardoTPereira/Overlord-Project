using UnityEngine;

namespace PlatformGame.GameManager
{
    public class QuitGameManager : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("PRESSED EXIT GAME");
        }

        public void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}