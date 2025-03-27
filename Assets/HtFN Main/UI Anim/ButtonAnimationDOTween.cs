using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimationDOTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack);

        // ¬оспроизведение глобального звука наведени€
        AudioManager.Instance?.PlayHoverSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.InOutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 0.8f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => transform.DOScale(originalScale, 0.1f));

        // ¬оспроизведение глобального звука клика
        AudioManager.Instance?.PlayClickSound();
    }
}
