using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Extentions
    {
        public const float Duration = 0.3f;
        public const float Delay = 0.5f;

        public static void EnableGroup(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration);
        }

        public static void DelayedEnableGroup(this CanvasGroup canvasGroup, float duration = Duration, float delay = Delay)
        { 
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration).SetDelay(delay);
        }

        public static void DelayedDisableGroup(this CanvasGroup canvasGroup, float duration = Duration, float delay = Delay)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(0f, duration).SetDelay(delay);
        }

        public static void DisableGroup(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(0f, duration);
        }
    }
}
