using TMPro;
using UnityEngine;

public class ActivateDisplays : MonoBehaviour
{
    //public TMP_Text[] sTexts;
    void Start()
    {
        // Activate all connected displays
        Debug.Log("Display.displays.Length   " + Display.displays.Length);
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            //sTexts[i].text = "Display " + (i) + " Activated";
        }
    }
}
