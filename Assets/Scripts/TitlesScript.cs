using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlesScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text titles;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RawImage blackScreen;
    [SerializeField] private Canvas canvas;

    [Header("Variables")]
    [SerializeField] private float speed = 0.4f;
    [SerializeField] private float blackScreenAppearSpeed = 2f;
    [SerializeField] private float textHeight;
    [SerializeField] private float canvasHeight;
    [SerializeField] private bool isMoving = true;
    void Start()
    {
        rectTransform = titles.GetComponent<RectTransform>();
        textHeight = rectTransform.rect.height;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
    }

    public void ShowTitles()
    {
        StartCoroutine(MoveTitlesCoroutine());
    }

    IEnumerator MoveTitlesCoroutine()
    {
        var targetPosition = new Vector3(rectTransform.position.x, canvasHeight + textHeight / 2, rectTransform.position.z);
        
        while (isMoving)
        {
            float step = speed * Time.deltaTime;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, targetPosition, step);

            if (rectTransform.position == targetPosition)
            {
                isMoving = false;
                StartCoroutine(ShowBlackScreenCoroutine());
                Debug.Log($"Coroutine with titles is stop");
            }

            yield return null;
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
    }


}
