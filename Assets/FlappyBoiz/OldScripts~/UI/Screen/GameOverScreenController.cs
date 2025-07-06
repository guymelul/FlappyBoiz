using System.Collections;
using LitMotion.Animation;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FlappyBoiz.Scripts.UI.Screen
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameScreen))]
    public class GameOverScreenController : MonoBehaviour
    {
        private static readonly WaitForSecondsRealtime LongDelay = new(0.8f);
        private static readonly WaitForSecondsRealtime ShortDelay = new(0.25f);
        private const float RunningNumberDuration = 0.15f;

        private GameScreen m_gameScreen;
        private int m_rageSkips;
        private InputAction m_action;

        #region Fields

        [SerializeField]
        private LitMotionAnimation titleAnimation;

        [SerializeField]
        private LitMotionAnimation scoreAnimation;

        [SerializeField]
        private LitMotionAnimation highScoreAnimation;

        [SerializeField]
        private LitMotionAnimation earnedCoinsAnimation;

        [SerializeField]
        private LitMotionAnimation buttonGroupAnimation;

        [SerializeField]
        private ParticleSystem newHighScoreEffect;

        private LitMotionAnimation m_newHighScoreAnimation;

        [SerializeField]
        private IntVariable score;

        [SerializeField]
        private IntVariable highScore;

        [SerializeField]
        private RunningNumberDisplay scoreDisplay;

        [SerializeField]
        private RunningNumberDisplay highScoreDisplay;

        [SerializeField]
        private RunningNumberDisplay earnedCoinsDisplay;

        #endregion

        private void Awake()
        {
            m_newHighScoreAnimation = newHighScoreEffect.gameObject.GetComponent<LitMotionAnimation>();
            m_rageSkips = -1;
            m_gameScreen = GetComponent<GameScreen>();
            m_action = InputSystem.actions.FindActionMap("Player").FindAction("Action");

            m_gameScreen.onGameScreenShown.AddListener(() => { StartCoroutine(StartSequence()); });
            m_gameScreen.onGameScreenHidden.AddListener(() =>
            {
                m_rageSkips = -1;
                newHighScoreEffect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                StopAllCoroutines();
            });
        }

        private void Update()
        {
            if (m_rageSkips < 0) return;

            if (m_action.WasPressedThisFrame()) m_rageSkips += 1;
        }

        private IEnumerator StepDelay
        {
            get
            {
                if (m_rageSkips == 0) return LongDelay;

                return m_rageSkips < 3 ? ShortDelay : null;
            }
        }

        private bool IsNewHighScore => score.Value == highScore.Value && score.Value > highScore.OldValue &&
                                       highScore.OldValue > 0;

        private IEnumerator StartSequence()
        {
            m_rageSkips = 0;
            titleAnimation.gameObject.SetActive(false);
            scoreAnimation.gameObject.SetActive(false);
            highScoreAnimation.gameObject.SetActive(false);
            buttonGroupAnimation.gameObject.SetActive(false);
            newHighScoreEffect.gameObject.SetActive(false);
            earnedCoinsAnimation.gameObject.SetActive(false);
            m_newHighScoreAnimation.Stop();
            newHighScoreEffect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

            yield return null;

            titleAnimation.Restart();
            titleAnimation.gameObject.SetActive(true);

            yield return StepDelay;

            scoreAnimation.Restart();
            scoreAnimation.gameObject.SetActive(true);
            scoreDisplay.Play(RunningNumberDuration);

            yield return StepDelay;

            highScoreAnimation.Restart();
            highScoreAnimation.gameObject.SetActive(true);

            if (IsNewHighScore)
            {
                float firstPartDuration =
                    Mathf.Lerp(0, RunningNumberDuration, (float)highScore.OldValue / highScore.Value);

                firstPartDuration = Mathf.Max(0.1f, firstPartDuration);

                highScoreDisplay.Play(firstPartDuration, 0, highScore.OldValue);

                yield return new WaitForSeconds(firstPartDuration);

                m_newHighScoreAnimation.Play();

                yield return ShortDelay;

                highScoreDisplay.Play(Mathf.Max(0.1f, RunningNumberDuration - firstPartDuration),
                    highScore.OldValue, highScore.Value);
            }
            else
            {
                highScoreDisplay.Play(RunningNumberDuration);
            }

            yield return StepDelay;

            earnedCoinsAnimation.Restart();
            earnedCoinsAnimation.gameObject.SetActive(true);

            earnedCoinsDisplay.Play(RunningNumberDuration);

            yield return StepDelay;

            buttonGroupAnimation.Restart();
            buttonGroupAnimation.gameObject.SetActive(true);

            if (!IsNewHighScore) yield break;

            yield return StepDelay;

            newHighScoreEffect.gameObject.SetActive(true);
            newHighScoreEffect.Play();

            m_rageSkips = -1;
        }
    }
}