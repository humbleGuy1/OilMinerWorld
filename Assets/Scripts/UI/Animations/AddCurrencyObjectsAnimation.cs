using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts
{
    public class AddCurrencyObjectsAnimation : MonoBehaviour
    {
        [SerializeField] private InputRoot _input;
        [SerializeField] private Image _leafTemplate;
        [SerializeField] private Image _starTemplate;
        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _endLeafPoint;
        [SerializeField] private RectTransform _endStarPoint;
        [Header("Animation Settings")]
        [SerializeField] private int _spawnCountStars = 1;
        [SerializeField] private int _spawnCountLeaves = 10;
        [SerializeField] private float _spawnRadius = 350;
        [SerializeField] private float _moveToOppositeDiractionDuration = 0.2f;
        [SerializeField] private float _moveToEndPositionDuration = 0.45f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void PlayLeaf(Vector3 fromPosition, System.Action onComplete = null)
        {
            Play(fromPosition, _spawnCountLeaves, _leafTemplate, _endLeafPoint, onComplete);
        }

        public void PlayStars(Vector3 fromPosition, System.Action onComplete = null)
        {
            Play(fromPosition, _spawnCountStars, _starTemplate, _endStarPoint, onComplete);
        }

        private void Play(Vector3 fromPosition, int spawnCount, Image template, RectTransform endPoint, System.Action onComplete = null)
        {
            bool onCompleteInvoked = false;
            float zoom = Mathf.Lerp(0.3f, 1f, _input.NormalizedCurrentZoom);
            var localContainerFromPosition = ConvertWorldToContainerSpacePosition(fromPosition);

            for (int i = 0; i < spawnCount; i++)
            {
                RectTransform spawnedCoin = Instantiate(template, _container).rectTransform;
                spawnedCoin.position = localContainerFromPosition + Vector2.right * Random.Range(-_spawnRadius, _spawnRadius) * zoom / 2;
                spawnedCoin.localScale *= zoom;

                spawnedCoin.DOMoveY(localContainerFromPosition.y + Random.Range(-_spawnRadius * 1.75f, 0) * zoom,
                    ConvertTime(_moveToOppositeDiractionDuration + Random.Range(0, _moveToOppositeDiractionDuration), zoom))
                        .OnComplete(() =>
                        {
                            float randomValue = Random.Range(0, _moveToEndPositionDuration);
                            spawnedCoin.DOScale(0.3f, ConvertTime(_moveToEndPositionDuration + randomValue, zoom));
                            spawnedCoin.DOMove(endPoint.position, ConvertTime(_moveToEndPositionDuration + randomValue, zoom))
                                .OnComplete(() =>
                                {
                                    Destroy(spawnedCoin.gameObject);

                                    if (onCompleteInvoked == false)
                                    {
                                        onComplete?.Invoke();
                                        onCompleteInvoked = true;
                                    }
                                });
                        });
            }
        }

        private float ConvertTime(float time, float zoom) => time * Time.timeScale + Mathf.Lerp(1f, 0f, zoom);

        private Vector2 ConvertWorldToContainerSpacePosition(Vector3 worldPosition)
        {
            Vector2 viewportPostion = _camera.WorldToViewportPoint(worldPosition);
            Vector2 containerSpacePosition = new Vector2(viewportPostion.x * _container.sizeDelta.x, viewportPostion.y * _container.sizeDelta.y);

            return containerSpacePosition;
        }
    }
}
