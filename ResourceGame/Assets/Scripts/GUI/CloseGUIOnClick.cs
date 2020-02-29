using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGUIOnClick : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The menu to close on clicking the close button.")]
    public GameObject menuToClose;

    public void OnClick()
    {
        menuToClose.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
