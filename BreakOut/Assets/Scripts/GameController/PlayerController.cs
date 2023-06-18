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
        /// プレイヤーコントローラー
        /// </Summary>
        public class PlayerController : MonoBehaviour
        {
            [SerializeField] private float _movedForce = 30.0f;

            [SerializeField, ReadOnlyAttribute] private Vector2 _direction;
            [SerializeField, ReadOnlyAttribute] private Vector2 _velocity;

            private Rigidbody2D _rigidbody2D;
            private BallController _ballController;
            private GameCoreManager _coreManager;

            // Start is called before the first frame update
            private void Awake()
            {
                _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            }

            // Update is called once per frame
            private void Update()
            {
                if (_coreManager == null)
                {
                    return;
                }
                
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    _direction = Vector2.left;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    _direction = Vector2.right;
                }
                else
                {
                    _direction = Vector2.zero;
                }

                if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
                {
                    Vector2 balldirection = new Vector2(_direction.x, 1.0f).normalized;
                    _ballController.BootBall(balldirection);
                }

                _velocity = _direction * _movedForce * _coreManager.GetSceneDeltaTimeScale();
            }

            private void FixedUpdate()
            {
                _rigidbody2D.AddForce(_velocity);

                if (_ballController != null)
                {
                    if (_ballController.isWaiting())
                    {
                        _ballController.TrackingObject(transform.position);
                    }
                }
            }

            public void InitPlayerController(GameCoreManager coreManager, GameObject ball)
            {
                _coreManager = coreManager;
                _ballController = ball.GetComponent<BallController>();
            }
        }
    }
}
