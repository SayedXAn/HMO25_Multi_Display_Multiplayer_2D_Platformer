using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoginMenuSwitchScreen : MonoBehaviour
{
    public GameObject[] screens;
    public TMP_Dropdown screenDropdown;
    public GameObject restartSceneButton;
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

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            int currentIndex = screenDropdown.value;
            int nextIndex = (currentIndex + 1) % screens.Length;
            screenDropdown.value = nextIndex;
            SwitchScreen(nextIndex);

        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            int currentIndex = screenDropdown.value;
            int previousIndex = (currentIndex - 1 + screens.Length) % screens.Length;
            screenDropdown.value = previousIndex;
            SwitchScreen(previousIndex);
        }
    }

    public void SwitchScreen(int index)
    {
        Debug.Log("Switching to screen: " + index);
        transform.SetParent(screens[index].transform, false);
        restartSceneButton.transform.SetParent(screens[index].transform.GetChild(0).transform, false);
        PlayerPrefs.SetInt("LastScreenIndex", index);
    }

    public void OnDropdownValueChanged()
    {
        SwitchScreen(screenDropdown.value);
    }
}
