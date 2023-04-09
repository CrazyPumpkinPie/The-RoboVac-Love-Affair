using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlesScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text titles;
    [SerializeField] private RectTransform target;
    [SerializeField] private GameObject finalLabel;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RawImage blackScreen;
    [SerializeField] private Canvas canvas;

    [Header("Variables")]
    [SerializeField] private float speedOfTitles = 0.2f;
    [SerializeField] private float blackScreenAppearSpeed = 2f;
    [SerializeField] private float finalLabelAppearSpeed = 1f;
    [SerializeField] private int mainMenuSceneBuildIndex = 0;
    void Start()
    {
        rectTransform = titles.GetComponent<RectTransform>();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowTitles()
    {
        rectTransform.position = Vector3.MoveTowards(rectTransform.position, 
            target.position, speedOfTitles);

        if (rectTransform.position == target.position)
        {
            StartCoroutine(ShowBlackScreenCoroutine());
        }
    }

    IEnumerator ShowBlackScreenCoroutine()
    {
        float startAlpha = blackScreen.GetComponent<RawImage>().color.a;
        float time = 0f;

        while (time < blackScreenAppearSpeed)
        {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 1f, time / blackScreenAppearSpeed);
            Color color = blackScreen.GetComponent<RawImage>().color;
            color.a = currentAlpha;
            blackScreen.GetComponent<RawImage>().color = color;
            yield return null;
        }

        StartCoroutine(ShowFinalLabelCoroutine());
    }

    IEnumerator ShowFinalLabelCoroutine()
    {
        float startAlpha = finalLabel.GetComponent<Text>().color.a;
        float time = 0f;
        finalLabel.SetActive(true);
        while (time < finalLabelAppearSpeed)
        {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 1f, time / blackScreenAppearSpeed);
            Color color = finalLabel.GetComponent<Text>().color;
            color.a = currentAlpha;
            finalLabel.GetComponent<Text>().color = color;
            yield return null;
        }
        if (Input.anyKey && finalLabel.gameObject.activeSelf)
        {
            SceneManager.LoadScene(mainMenuSceneBuildIndex);
        }
    }


}
