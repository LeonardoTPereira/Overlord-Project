using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.DataCollection
{
    /// <summary>
    /// 
    /// </summary>
    public class Form : MonoBehaviour
    {
        public void GoToForm()
        {
            //TODO add correct link
            //Debug.Log("Open Form in Browser");
            //Application.OpenURL("http://unity3d.com/");
            SceneManager.LoadScene("LuanaPaolo");
        }
        /*public void Continue()
    {

        Debug.Log("Load New Batch");
        
        GameManager gm = GameManager.instance;
        gm.LoadNewBatch();
    }*/
        public void Quit()
        {
            Application.Quit();
        }
    }
}
