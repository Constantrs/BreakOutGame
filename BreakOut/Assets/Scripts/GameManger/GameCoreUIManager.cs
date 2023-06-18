using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BreakoutProject.Utility;

namespace BreakoutProject
{
    namespace Manager
    {
        /// <Summary>
        /// ゲームコアUIマネージャー
        /// </Summary>
        public class GameScreenManager : MonoBehaviour
        {
            public enum InfoType
            {
                LEVELCLEAR = 0,
                GAMECLEAR,
                GAMEOVER,
            };

            [SerializeField] private Image _blackImage;
            [SerializeField] private Text _infoText;
            [SerializeField] private Color _infoColor;
            [SerializeField] private GameObject _pauseRoot;
            [SerializeField] private GameObject _infoRoot;

            [SerializeField, ReadOnlyAttribute] private bool _IsFading = false;
            [SerializeField, ReadOnlyAttribute] private bool _InfoDisplayed = false;

            private MasterManager _masterManager;

            // Start is called before the first frame update
            private void Awake()
            {
                GameObject masterManagerObj = GameObject.FindWithTag("MasterManager");
                _masterManager = masterManagerObj.GetComponent<MasterManager>();

                if (_masterManager == null)
                {
                    Debug.LogError("MasterManager does not exist!");
                }
            }

            public bool isFading()
            {
                return _IsFading;
            }

            public bool isInfoDisplayed()
            {
                return _InfoDisplayed;
            }

            public void SetPauseLyaerActive(bool pause)
            {
                if (_pauseRoot != null)
                {
                    _pauseRoot.SetActive(pause);
                }
            }


            public void StartColorFade(Color startColor, Color endColor, float frame)
            {
                _IsFading = true;
                StartCoroutine(coColorFade(startColor, endColor, frame));
            }

            public void StartAlphaFade(float startAlpha, float endAlpha, float frame)
            {
                _IsFading = true;
                StartCoroutine(coAlphaFade(startAlpha, endAlpha, frame));
            }

            public void SetInfoActive(InfoType type, float startAlpha, float endAlpha, float fadeFrame, float activeFrame)
            {
                _InfoDisplayed = true;
                _infoRoot.SetActive(true);
                _infoText.color = _infoColor;

                if (type == InfoType.LEVELCLEAR)
                {
                    _infoText.text = "LEVEL CLEAR";
                }
                else if (type == InfoType.GAMECLEAR)
                {
                    _infoText.text = "GAME CLEAR";
                }
                else if (type == InfoType.GAMEOVER)
                {
                    _infoText.text = "GAME OVER";
                }

                if (_infoRoot.activeSelf)
                {
                    StartCoroutine(coInfoActive(startAlpha, endAlpha, fadeFrame, activeFrame));
                }
            }

            IEnumerator coColorFade(Color startColor, Color endColor, float frame)
            {
                float totalFrame = frame;
                float currentFrame = 0.0f;
                Color fadeColor = startColor;

                _blackImage.color = startColor;
                _blackImage.gameObject.SetActive(true);

                while (currentFrame < totalFrame)
                {
                    float t = currentFrame / totalFrame;
                    fadeColor.r = Mathf.Lerp(startColor.r, endColor.r, t);
                    fadeColor.g = Mathf.Lerp(startColor.g, endColor.g, t);
                    fadeColor.b = Mathf.Lerp(startColor.b, endColor.b, t);
                    fadeColor.a = Mathf.Lerp(startColor.a, endColor.a, t);

                    _blackImage.color = fadeColor;

                    currentFrame += 1.0f * _masterManager.GetDeltaTimeScale();

                    yield return null;
                }

                if (endColor.a == 0.0f)
                {
                    _blackImage.gameObject.SetActive(false);
                }
                _IsFading = false;
                yield return null;
            }

            IEnumerator coAlphaFade(float startAlpha, float endAlpha, float frame)
            {
                float totalFrame = frame;
                float currentFrame = 0.0f;
                Color fadeColor = _blackImage.color;
                fadeColor.a = startAlpha;

                _blackImage.color = fadeColor;
                _blackImage.gameObject.SetActive(true);

                while (currentFrame < totalFrame)
                {
                    float t = currentFrame / totalFrame;
                    fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, t);

                    _blackImage.color = fadeColor;

                    currentFrame += 1.0f * _masterManager.GetDeltaTimeScale();

                    yield return null;
                }

                if (endAlpha == 0.0f)
                {
                    _blackImage.gameObject.SetActive(false);
                }
                _IsFading = false;
                yield return null;
            }

            IEnumerator coInfoActive(float startAlpha, float endAlpha, float fadeFrame, float activeFrame)
            {
                float totalFrame = fadeFrame;
                float currentFrame = 0.0f;
                Color color = _infoColor;
                color.a = startAlpha;

                _infoText.color = color;

                while (currentFrame < totalFrame)
                {
                    float t = currentFrame / totalFrame;
                    color.a = Mathf.Lerp(startAlpha, endAlpha, t);

                    _infoText.color = color;

                    currentFrame += 1.0f * _masterManager.GetDeltaTimeScale();

                    yield return null;
                }

                color.a = endAlpha;
                _infoText.color = color;
                totalFrame = activeFrame;
                currentFrame = 0.0f;

                while (currentFrame < totalFrame)
                {
                    currentFrame += 1.0f * _masterManager.GetDeltaTimeScale();
                    yield return null;
                }

                _infoRoot.SetActive(false);
                _InfoDisplayed = false;
                yield return null;
            }
        }
    }
}