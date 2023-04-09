using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalPreviewScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject finalComix;
    [SerializeField] TitlesScript titlesScript;
    [SerializeField] List<RawImage> blackSquares = new List<RawImage>();

    [Header("Variables")]
    [SerializeField] private int index = 0;
    [SerializeField] private float delayTime = 2f;
    [SerializeField] private float changeColorSpeed = 0.4f;
    [SerializeField] private float shoutDownComixSpeed = 0.4f;

    [Header("Trigger")]
    [SerializeField] GameObject trigger;


    public void Update()
    {
        trigger = GameObject.FindGameObjectWithTag("Trigger");
        if (trigger.activeSelf)
            PlayPreview();
    }

    public void PlayPreview()
    {
        finalComix.SetActive(true);
        StartCoroutine(ShowNextCoroutine());
    }

    public IEnumerator ShoutDownComixCoroutine()
    {
        float startAlpha = finalComix.GetComponent<RawImage>().color.a;
        float time = 0f;

        while (time < shoutDownComixSpeed)
        {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, time / shoutDownComixSpeed);
            Color color = finalComix.GetComponent<RawImage>().color;
            color.a = currentAlpha;
            finalComix.GetComponent<RawImage>().color = color;
            yield return null;
        }
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
                yield return null;
            }
        }
    }

    IEnumerator ShowNextCoroutine()
    {

        if (index == blackSquares.Count - 1)
        {
            StartCoroutine(ChangeColor(0, changeColorSpeed, index));
            while (blackSquares[index].color.a > 0)
            {
                yield return null;
            }
            StopAllCoroutines();
            StartCoroutine(ShoutDownComixCoroutine());
            StopAllCoroutines();
            titlesScript.ShowTitles();
        }
        else
        {
            StartCoroutine(ChangeColor(0, changeColorSpeed, index));
            while (blackSquares[index].color.a > 0)
            {
                yield return null;
            }
            if (index < blackSquares.Count - 1)
            {
                index++;
            }
            Debug.Log($"Index was increased to {index}");
            Debug.Log($"Waiting {delayTime} seconds before showing the next black square");
            yield return new WaitForSeconds(delayTime);
            yield return StartCoroutine(ShowNextCoroutine());
        }

    }

}
