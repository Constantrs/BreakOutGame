using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BreakoutProject.Utility;
using BreakoutProject.Manager;

namespace BreakoutProject
{
    namespace Controller
    {
        /// <Summary>
        /// ブロックコントローラー
        /// </Summary>
        public class BlockController : MonoBehaviour
        {

            [SerializeField, ReadOnlyAttribute] private bool _destroyed = false;

            private GameCoreManager _coreManager;

            private void Hit()
            {
                if (_coreManager != null)
                {
                    _coreManager.DestroyBlock(100);
                }
                gameObject.SetActive(false);
                _destroyed = true;
            }

            private void OnCollisionEnter2D(Collision2D collision)
            {
                if(_destroyed)
                {
                    return;
                }

                Hit();
            }


            public void InitBlockController(GameCoreManager coreManager)
            {
                _coreManager = coreManager;
            }
        }
    }
}
