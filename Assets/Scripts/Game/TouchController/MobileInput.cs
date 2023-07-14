using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.TouchController
{
    public sealed class MobileInput : IInput
    {
        private readonly float _duplicationTapDelta = 5f;
        private readonly Camera _camera;
        private readonly int _maxDuplicatedTapsCount = 3;
        private readonly HashSet<int> _ignoredFingersId = new HashSet<int>();

        private int _touchId = -1;
        private int _duplicatedTapsCount;
        private float _lastDuplicatedTapTime;
        private bool _isHold;

        public event Action<Vector2> Clicked;

        public MobileInput() => _camera = Camera.main;

        public void Update()
        {
            if (Input.touchCount > 0 && !_isHold)
            {
                if (Input.touchCount > 1)
                {
                    var availableTouches = Input.touches.Where(t => !_ignoredFingersId.Contains(t.fingerId)
                                                                    && t.phase == TouchPhase.Began).ToArray();
                    if (availableTouches.Length > 0)
                        _touchId = availableTouches[0].fingerId;
                }
                else if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
                {
                    _touchId = Input.touches[0].fingerId;
                }
            }
            else if (_touchId >= 0 && Input.touchCount == 0)
            {
                _isHold = false;
                _touchId = -1;
            }

            var touchIndex = Array.FindIndex(Input.touches, t => t.fingerId == _touchId);
            if (touchIndex < 0)
            {
                _touchId = -1;
                return;
            }

            var activeTouch = Input.touches[touchIndex];

            if (Input.touchCount > 1 && _isHold)
            {
                foreach (var duplicatedTouch in Input.touches)
                {
                    if (duplicatedTouch.fingerId == _touchId || duplicatedTouch.phase != TouchPhase.Began)
                        continue;

                    if (Time.time - _lastDuplicatedTapTime > _duplicationTapDelta)
                        _duplicatedTapsCount = 0;

                    _duplicatedTapsCount++;
                    _lastDuplicatedTapTime = Time.time;

                    if (_duplicatedTapsCount < _maxDuplicatedTapsCount)
                        continue;

                    _ignoredFingersId.Add(duplicatedTouch.fingerId);
                    _duplicatedTapsCount = 0;
                    _isHold = false;
                    activeTouch = duplicatedTouch;
                    _touchId = activeTouch.fingerId;
                }
            }

            foreach (var touch in Input.touches)
            {
                if (touch.fingerId == activeTouch.fingerId)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            var position = _camera.ScreenToWorldPoint(activeTouch.position);
                            Clicked?.Invoke(position);
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        {
                            _ignoredFingersId.Remove(touch.fingerId);
                            _isHold = false;
                            _touchId = -1;
                            break;
                        }
                    }
                }
                else
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        {
                            _ignoredFingersId.Remove(touch.fingerId);
                            break;
                        }
                    }
                }
            }
        }
    }
}