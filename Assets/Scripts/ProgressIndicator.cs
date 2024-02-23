using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressIndicator : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Image progressCircle;

    private Slider progressSlider;
    private GameObject penguinInstance;

    void Start()
    {
        progressSlider = GetComponent<Slider>();
        progressSlider.minValue = 0;
        progressSlider.maxValue = 1;
        progressSlider.value = 0;
    }

    void Update()
    {
        if (GameManager.instance && !penguinInstance)
        {
            penguinInstance = GameManager.instance.GetPenguinInstance();
        }

        if (penguinInstance && startPoint && endPoint)
        {
            float totalDistance = endPoint.position.x - startPoint.position.x;
            float currentProgress = penguinInstance.transform.position.x - startPoint.position.x;
            progressSlider.value = Mathf.Clamp((currentProgress / totalDistance), 0f, 1f);
        }

        RectTransform sliderTransform = progressSlider.GetComponent<RectTransform>();
        Vector3 circlePosition = new Vector3(
            sliderTransform.anchoredPosition.x + (sliderTransform.sizeDelta.x * progressSlider.value),
            progressCircle.rectTransform.anchoredPosition.y,
            0
        );
        progressCircle.rectTransform.anchoredPosition = circlePosition;
    }
}