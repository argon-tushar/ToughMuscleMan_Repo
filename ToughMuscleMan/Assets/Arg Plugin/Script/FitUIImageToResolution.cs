using UnityEngine;
using UnityEngine.UI;

public class FitUIImageToResolution : MonoBehaviour
{
    [SerializeField] Image uiImage;
    [SerializeField] RectTransform canvasRect;

    private void Start()
    {
        RectTransform imageRect = uiImage.rectTransform;
        Sprite sprite = uiImage.sprite;

        float aspectRatio = sprite.rect.width / sprite.rect.height;
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float canvasAspectRatio = canvasWidth / canvasHeight;
        float imageWidth;
        float imageHeight;

        if (canvasAspectRatio > aspectRatio)
        {
            imageWidth = canvasWidth;
            imageHeight = canvasWidth / aspectRatio;
        }
        else
        {
            imageWidth = canvasHeight * aspectRatio;
            imageHeight = canvasHeight;
        }

        uiImage.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
        uiImage.rectTransform.anchoredPosition = Vector2.zero;
    }
}
