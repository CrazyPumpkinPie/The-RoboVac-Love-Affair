using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameScript : MonoBehaviour
{
    [SerializeField] GameObject startComix;
    [SerializeField] GameObject newGameButton;

    public void NewGamePreview()
    {
        startComix.SetActive(true);
        Invoke("ShowNewGameButton", 2f);
    }

    private void ShowNewGameButton()
    {
        newGameButton.SetActive(true);
    }
}
