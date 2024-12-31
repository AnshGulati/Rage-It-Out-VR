using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowMenu : MonoBehaviour
{
    public GameObject menuScreen;
    public InputActionProperty menuButton;


    private bool isMenuVisible = false;
    private bool menuButtonPressed = false;

    private void Start() {
        menuScreen.SetActive(isMenuVisible);
    }

    // call menu button pressed when the menu button is pressed
    private void Update() {
        // use the occulus menu button to open the menu
        float menuButtonValue = menuButton.action.ReadValue<float>();
        if (menuButtonValue == 1 && !menuButtonPressed) {
            menuButtonPressed = true;
            MenuButtonPressed();
        } else if (menuButtonValue == 0) {
            menuButtonPressed = false;
        }
    }

    public void MenuButtonPressed()
    {
        isMenuVisible = !isMenuVisible;

        menuScreen.SetActive(isMenuVisible); // sets menu screen to active or inactive based on isMenuVisible variable

        if (isMenuVisible)
        {
            menuScreen.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.0f;
            // also set rotation
            menuScreen.transform.rotation = Camera.main.transform.rotation;
            // but make sure the menu y axis is facing up
            menuScreen.transform.rotation = Quaternion.Euler(menuScreen.transform.rotation.eulerAngles.x, menuScreen.transform.rotation.eulerAngles.y, 0);

        }
    }
}
