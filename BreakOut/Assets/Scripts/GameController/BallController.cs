using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BreakoutProject.Utility;
using UnityEngine.Playables;
using BreakoutProject.Manager;

namespace BreakoutProject
{
    namespace Controller
    {

        /// <Summary>
        /// ボールコントローラー
        /// </Summary>
        public class BallController : MonoBehaviour
        {
            [SerializeField] private float _movedForce = 140.0f;

            [SerializeField, ReadOnlyAttribute] private Vector3 _offset = Vector3.zero;
            [SerializeField, ReadOnlyAttribute] private float _maxBounceAngle = 75.0f;
            [SerializeField, ReadOnlyAttribute] private float _maxBounceSpeed = 2.2f;
            [SerializeField, ReadOnlyAttribute] private int _randomBounceAngel = 8;

            private bool _waiting = true;
            private Rigidbody2D _rigidbody2D;

            // Start is called before the first frame update
            private void Awake()
            {
                _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                _rigidbody2D.isKinematic = true;
            }

            public void BootBall(Vector2 direction)
            {
                if (_waiting)
                {
                    _rigidbody2D.isKinematic = false;
                    _rigidbody2D.AddForce(direction * _movedForce);
                    _waiting = false;
                }
            }

            public bool isWaiting()
            { 
                return _waiting; 
            }

            public void SetOffset(Vector3 offset)
            {
                _offset = offset;
            }

            public void TrackingObject(Vector3 position)
            {
                Vector3 pos = _offset + position;
                transform.position = pos;
            }

            public void ResetBallController()
            {
                _rigidbody2D.isKinematic = true;
                _waiting = true;
            }


            private void OnCollisionEnter2D(Collision2D collision)
            {
                if(_waiting)
                {
                    return;
                }

                var layer = collision.gameObject.layer;
                // プレイヤー
                if (layer == 9)
                {
                    Vector2 paddlePosition = collision.gameObject.transform.position;
                    Vector2 contactPoint = collision.GetContact(0).point;

                    float offset = paddlePosition.x - contactPoint.x;
                    float maxOffset = collision.collider.bounds.size.x / 2;

                    float currentAngle = Vector2.SignedAngle(Vector2.up, _rigidbody2D.velocity);
                    float bounceAngle = (offset / maxOffset) * _maxBounceAngle;
                    float randomAngle = (float)Random.Range(-_randomBounceAngel, _randomBounceAngel);
                    float newAngle = Mathf.Clamp(currentAngle + bounceAngle + randomAngle, -_maxBounceAngle, _maxBounceAngle);

                    Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
                    float speed = Mathf.Max(_rigidbody2D.velocity.magnitude, _maxBounceSpeed);
                    _rigidbody2D.velocity = rotation * Vector2.up * speed;
                    //Debug.Log("Hit Paddle");
                }
                // ブロック
                else if (layer == 11)
                {
                    //Debug.Log("Hit Block");
                }
                // 壁
                else if (layer == 12)
                {
                    //Debug.Log("Hit Wall");
                }
                // デッドゾーン
                else if (layer == 13)
                {
                    //Debug.Log("Hit Deadzone");
                }
            }
        }
    }
}
