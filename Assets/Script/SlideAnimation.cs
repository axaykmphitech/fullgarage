using UnityEngine;
using DG.Tweening;

public class SlideAnimation : MonoBehaviour
{
    public RectTransform targetRect;
    private float slideDistance = 50f;
    private float duration = 0.5f;
    public bool leftSlide = true;
    public bool rightSlide = false;
    public bool topSlide = false;
    public bool bottomSlide = false;
    private Ease ease = Ease.OutQuint;

    private Vector2 startPosition;

    void OnEnable()
    {
        targetRect = GetComponent<RectTransform>();
        startPosition = targetRect.anchoredPosition;

        if (leftSlide)
            SlideLeft();
        else if (rightSlide)
            SlideRight();
        else if (topSlide)
            SlideTop();
        else if (bottomSlide)
            SlideBottom();
    }

    void SlideLeft()
    {
        targetRect.anchoredPosition = new Vector2(startPosition.x - slideDistance, startPosition.y) ;
        targetRect.DOAnchorPosX(startPosition.x, duration).SetEase(ease).OnComplete(MoveBackToStart);
    }

    void SlideRight()
    {
        targetRect.anchoredPosition = new Vector2(startPosition.x + slideDistance, startPosition.y );
        targetRect.DOAnchorPosX(startPosition.x, duration).SetEase(ease).OnComplete(MoveBackToStart);
    }

    void SlideTop()
    {
        targetRect.anchoredPosition = new Vector2(startPosition.x, startPosition.y + slideDistance);
        targetRect.DOAnchorPosY(startPosition.y, duration).SetEase(ease).OnComplete(MoveBackToStart);
    }

    void SlideBottom()
    {
        targetRect.anchoredPosition = new Vector2(startPosition.x, startPosition.y - slideDistance);
        targetRect.DOAnchorPosY(startPosition.y, duration).SetEase(ease).OnComplete(MoveBackToStart);
    }

    void MoveBackToStart()
    {
        targetRect.DOAnchorPos(startPosition, duration).SetEase(ease);
    }
}
