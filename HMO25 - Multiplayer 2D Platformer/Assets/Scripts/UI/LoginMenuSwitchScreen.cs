using TMPro;
using UnityEngine;

public class LoginMenuSwitchScreen : MonoBehaviour
{
    public GameObject[] screens;
    public TMP_Dropdown screenDropdown;
    void Start()
    {
        if(PlayerPrefs.HasKey("LastScreenIndex"))
        {
            int lastScreenIndex = PlayerPrefs.GetInt("LastScreenIndex");
            SwitchScreen(lastScreenIndex);
        }
        else
        {
            SwitchScreen(0);
        }
    }

    public void SwitchScreen(int index)
    {
        Debug.Log("Switching to screen: " + index);
        transform.SetParent(screens[index].transform, false);
        PlayerPrefs.SetInt("LastScreenIndex", index);
    }

    public void OnDropdownValueChanged()
    {
        SwitchScreen(screenDropdown.value);
    }
}
