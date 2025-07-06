using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField]
        private IntVariable score;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            score.Value++;
            
            gameObject.SetActive(false);
        }
    }
}