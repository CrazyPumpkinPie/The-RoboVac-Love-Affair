using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayPreviewScript : MonoBehaviour
{
    [SerializeField] GameObject startComix;
    [SerializeField] GameObject pressAnyKeyLabel;
    [SerializeField] List<GameObject> blackSquares = new List<GameObject>();
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] private int index = 0;
    [SerializeField] private float delayTime = 2f;

    public void Update()
    {
        if (Input.anyKey && pressAnyKeyLabel.gameObject.active)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void PlayPreview()
    {
        startComix.SetActive(true);
        StartCoroutine(ShowNext());
    }

    IEnumerator ShowNext()
    {

        if (index > blackSquares.Count)
        {
            startComix.SetActive(false);
            pressAnyKeyLabel.SetActive(true);
            playButton.SetActive(false);
            exitButton.SetActive(false);
            StopAllCoroutines();
        }
        else
        {
            blackSquares[index].SetActive(false);
            index++;
            Debug.Log("Index was increased.");
            Debug.Log($"Waiting {delayTime} seconds before showing the next black square");
            yield return new WaitForSeconds(delayTime);
            yield return StartCoroutine(ShowNext());
        }

    }

}
