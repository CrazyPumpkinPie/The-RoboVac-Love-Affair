using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayPreviewScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject startComix;
    [SerializeField] GameObject pressAnyKeyLabel;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] List<RawImage> blackSquares = new List<RawImage>();

    [Header("Variables")]
    [SerializeField] private int index = 0;
    [SerializeField] private float delayTime = 2f;
    [SerializeField] private float changeColorSpeed = 0.4f;

    public void Update()
    {
        if (Input.anyKey && pressAnyKeyLabel.gameObject.activeSelf)
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

    IEnumerator ChangeColor(float targetAlpha, float duration, int index)
    {
        if (index < blackSquares.Count)
        {
            float startAlpha = blackSquares[index].color.a;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                Color color = blackSquares[index].color;
                color.a = currentAlpha;
                blackSquares[index].color = color;
                Debug.Log($"Color was changed");
                yield return null;
            }
        }
    }

    IEnumerator ShowNext()
    {

        if (index == blackSquares.Count)
        {
            pressAnyKeyLabel.SetActive(true);
            StopAllCoroutines();
        }
        else
        {
            Color color = blackSquares[index].color;
            StartCoroutine(ChangeColor(0, changeColorSpeed, index));
            while (blackSquares[index].color.a > 0)
            {
                 yield return null;
            }
            if (index < blackSquares.Count)
            {
                index++;
            }
            Debug.Log($"Index was increased");
            Debug.Log($"Waiting {delayTime} seconds before showing the next black square");
            yield return new WaitForSeconds(delayTime);
            yield return StartCoroutine(ShowNext());
        }

    }

}
