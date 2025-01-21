using UnityEngine;
using UnityEngine.EventSystems; // Required for event handling
using DG.Tweening; // Required for DOTween
using UnityEngine.UI; // Required for UI components

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Define the target scale factor
    //[SerializeField]

    [SerializeField]
    private Ease ease = Ease.OutExpo;

    private float hoverScaleFactor = 1.05f;

    // Define the animation duration
    //[SerializeField]
    private float animationDuration = 0.2f;

    // Define the shadow increase factor
    //[SerializeField]
    private Vector2 shadowIncrease = new Vector2(5f, -5f);

    // Store the original scale of the button
    private Vector3 originalScale;

    // Store the original shadow distance
    private Vector2 originalShadowDistance;

    // Reference to the Shadow component
    private Shadow shadowComponent;

    private void Start()
    {
        // Record the initial scale of the button
        originalScale = transform.localScale;

        // Get the Shadow component
        shadowComponent = GetComponent<Shadow>();
        if (shadowComponent != null)
        {
            // Record the initial shadow distance
            originalShadowDistance = shadowComponent.effectDistance;
        }
    }

    // Triggered when the mouse pointer enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Animate the button to the target scale
        transform.DOScale(originalScale * hoverScaleFactor, animationDuration).SetEase(ease);

        // Animate the shadow effect distance increase
        if (shadowComponent != null)
        {
            DOTween.To(() => shadowComponent.effectDistance,
                       x => shadowComponent.effectDistance = x,
                       originalShadowDistance + shadowIncrease,
                       animationDuration)
                   .SetEase(ease);
        }
    }

    // Triggered when the mouse pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Animate the button back to its original scale
        transform.DOScale(originalScale, animationDuration).SetEase(ease);

        // Animate the shadow effect distance back to original
        if (shadowComponent != null)
        {
            DOTween.To(() => shadowComponent.effectDistance,
                       x => shadowComponent.effectDistance = x,
                       originalShadowDistance,
                       animationDuration)
                   .SetEase(ease);
        }
    }
}
