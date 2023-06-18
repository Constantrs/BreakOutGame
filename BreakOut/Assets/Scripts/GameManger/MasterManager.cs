using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BreakoutProject.Utility;
using static BreakoutProject.Manager.MasterManager;

namespace BreakoutProject
{
    namespace Manager
    {
        /// <Summary>
        /// マスターマネージャー(削除しない)
        /// </Summary>
        public class MasterManager : MonoBehaviour
        {
            public enum FrameRateType
            {
                VSYNC = 0,
                FPS_30,
                FPS_60,
                FPS_120,
            };

            [SerializeField, ReadOnlyAttribute] private FrameRateType _frameRate = FrameRateType.FPS_60;
            [SerializeField, ReadOnlyAttribute] private float _deltaTimeScale = 1.0f;

            private void Awake()
            {
                // 60FPS固定
                ChangeFrameRate(FrameRateType.FPS_60);

                DontDestroyOnLoad(this.gameObject);
            }

            public void ChangeFrameRate(FrameRateType type)
            {
                _frameRate = type;
                switch (_frameRate)
                {
                    case FrameRateType.VSYNC:
                        QualitySettings.vSyncCount = 1;
                        _deltaTimeScale = 1.0f;
                        break;
                    case FrameRateType.FPS_30:
                        QualitySettings.vSyncCount = 0;
                        Application.targetFrameRate = 30;
                        _deltaTimeScale = 2.0f;
                        break;
                    case FrameRateType.FPS_60:
                        QualitySettings.vSyncCount = 0;
                        Application.targetFrameRate = 60;
                        _deltaTimeScale = 1.0f;
                        break;
                    case FrameRateType.FPS_120:
                        QualitySettings.vSyncCount = 0;
                        Application.targetFrameRate = 120;
                        _deltaTimeScale = 0.5f;
                        break;
                }
            }
            public float GetDeltaTimeScale()
            {
                return _deltaTimeScale;
            }
        }
    }
}