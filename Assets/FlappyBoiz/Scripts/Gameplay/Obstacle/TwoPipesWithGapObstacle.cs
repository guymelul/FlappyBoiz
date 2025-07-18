using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    [CreateAssetMenu(menuName = "FlappyBoiz/Obstacles/TwoPipesWithGap")]
    public class TwoPipesWithGapObstacle : BaseObstacle
    {
        [Header("Offset")] [SerializeField] private float offsetBoundTop = 2f;

        [SerializeField] private float offsetBoundBottom = -1.25f;

        [Header("Gap Distance")] [SerializeField]
        private float gapDistanceTop = 1.2f;

        [SerializeField] private float gapDistanceBottom = 1.1f;


        [SerializeField] private GameObject prefab;

        [SerializeField] private List<Sprite> variants;

        private void OnValidate()
        {
            if (prefab == null)
            {
                Debug.LogWarning("prefab is null");

                return;
            }

            var childTransforms = prefab.GetComponentsInChildren<Transform>();

            if (childTransforms.Length < 4)
            {
                Debug.LogWarning("Child transforms count is less than 4");
            }

            Assert.IsTrue(gapDistanceTop >= gapDistanceBottom);
            Assert.IsTrue(offsetBoundTop >= offsetBoundBottom);
        }

        public override GameObject Create()
        {
            return Instantiate(prefab);
        }

        public override void Randomize(GameObject obstacle, float variation, float difficulty)
        {
            Assert.IsTrue(variation is >= 0 and <= 1);
            Assert.IsTrue(difficulty is >= 0 and <= 1);

            // Normalize variation
            variation /= 2.0f;
            variation = (int)(variation * 100) % 2 == 0 ? variation + 0.5f : 0.5f - variation;

            // ignore the parent in [0]
            var topPipe = obstacle.transform.GetChild(0);
            var bottomPipe = obstacle.transform.GetChild(1);
            var collectable = obstacle.transform.GetChild(2);

            Assert.IsTrue(topPipe.CompareTag("Obstacle"));
            Assert.IsTrue(bottomPipe.CompareTag("Obstacle"));
            Assert.IsTrue(!collectable.CompareTag("Obstacle"));

            int chosenVariantIdx = Random.Range(0, variants.Count);
            var pipeSprite = variants[chosenVariantIdx];

            topPipe.gameObject.GetComponent<SpriteRenderer>().sprite = pipeSprite;
            bottomPipe.gameObject.GetComponent<SpriteRenderer>().sprite = pipeSprite;

            float offset = Mathf.Lerp(offsetBoundBottom, offsetBoundTop, variation);

            float gapDistance = Mathf.Lerp(gapDistanceTop, gapDistanceBottom, difficulty);

            float topPipeY = offset + gapDistance / 2;
            float bottomPipeY = offset - gapDistance / 2;
            float leftoverOffset = 0;

            if (bottomPipeY < offsetBoundBottom)
            {
                leftoverOffset = offsetBoundBottom - bottomPipeY;
                bottomPipeY = offsetBoundBottom;
                topPipeY += leftoverOffset;
            }
            else if (topPipeY > offsetBoundTop)
            {
                leftoverOffset = offsetBoundTop - topPipeY;
                topPipeY = offsetBoundTop;
                bottomPipeY += leftoverOffset;
            }

            collectable.localScale = new Vector3(collectable.localScale.x, gapDistance, 1);

            collectable.localPosition = new Vector3(collectable.localPosition.x, offset + leftoverOffset, 0);
            topPipe.localPosition = new Vector3(topPipe.localPosition.x, topPipeY, 0);
            bottomPipe.localPosition = new Vector3(bottomPipe.localPosition.x, bottomPipeY, 0);

            collectable.gameObject.SetActive(true);
        }
    }
}