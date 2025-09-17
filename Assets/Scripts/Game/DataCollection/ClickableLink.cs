using UnityEngine;

public class ClickableLink : MonoBehaviour
{
    [SerializeField] private string selectedLink = "";
    public void OnPointerClick()
    {
        if (selectedLink != "")
        {
            Debug.LogFormat("Open link {0}", selectedLink);
            Application.OpenURL(selectedLink);
        }
    }
}