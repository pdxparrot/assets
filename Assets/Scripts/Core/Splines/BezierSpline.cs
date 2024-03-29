﻿using System;

using pdxpartyparrot.Core.Math;

using UnityEngine;

// https://catlikecoding.com/unity/tutorials/curves-and-splines/

namespace pdxpartyparrot.Core.Splines
{
    public class BezierSpline : MonoBehaviour
    {
        public enum Mode
        {
            Free,
            Aligned,
            Mirrored
        }

        [SerializeField]
        private Vector3[] _points;

        public int ControlPointCount => _points.Length;

        public int CurveCount => (ControlPointCount - 1) / 3;

        [SerializeField]
        private Mode[] _modes;

        [SerializeField]
        private bool _loop;

        public bool Loop
        {
            get => _loop;
            set
            {
                _loop = value;
                if(_loop) {
                    _modes[^1] = _modes[0];
                    SetControlPoint(0, _points[0]);
                }
            }
        }

        #region Control Points

        public Vector3 GetControlPoint(int index)
        {
            return _points[index];
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            if(index % 3 == 0) {
                Vector3 delta = point - _points[index];
                if(Loop) {
                    if(index == 0) {
                        _points[1] += delta;
                        _points[^2] += delta;
                        _points[^1] = point;
                    } else if(index == _points.Length - 1) {
                        _points[0] = point;
                        _points[1] += delta;
                        _points[index - 1] += delta;
                    } else {
                        _points[index - 1] += delta;
                        _points[index + 1] += delta;
                    }
                } else {
                    if(index > 0) {
                        _points[index - 1] += delta;
                    }

                    if(index + 1 < _points.Length) {
                        _points[index + 1] += delta;
                    }
                }
            }

            _points[index] = point;
            EnforceMode(index);
        }

        #endregion

        #region Control Point Modes

        public Mode GetControlPointMode(int index)
        {
            return _modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, Mode mode)
        {
            int modeIndex = (index + 1) / 3;
            _modes[modeIndex] = mode;

            if(Loop) {
                if(modeIndex == 0) {
                    _modes[^1] = mode;
                } else if(modeIndex == _modes.Length - 1) {
                    _modes[0] = mode;
                }
            }

            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;

            Mode mode = _modes[modeIndex];
            if(mode == Mode.Free || !Loop && (modeIndex == 0 || modeIndex == _modes.Length - 1)) {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if(index <= middleIndex) {
                fixedIndex = middleIndex - 1;
                if(fixedIndex < 0) {
                    fixedIndex = _points.Length - 2;
                }

                enforcedIndex = middleIndex + 1;
                if(enforcedIndex >= _points.Length) {
                    enforcedIndex = 1;
                }
            } else {
                fixedIndex = middleIndex + 1;
                if(fixedIndex >= _points.Length) {
                    fixedIndex = 1;
                }

                enforcedIndex = middleIndex - 1;
                if(enforcedIndex < 0) {
                    enforcedIndex = _points.Length - 2;
                }
            }

            Vector3 middle = _points[middleIndex];
            Vector3 enforcedTangent = middle - _points[fixedIndex];
            if(mode == Mode.Aligned) {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, _points[enforcedIndex]);
            }
            _points[enforcedIndex] = middle + enforcedTangent;
        }

        #endregion

        public Vector3 GetPoint(float t)
        {
            int i;
            if(t >= 1.0f) {
                t = 1.0f;
                i = _points.Length - 4;
            } else {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t));
        }

        public Vector3 GetVelocity(float t)
        {
            int i;
            if(t >= 1f) {
                t = 1f;
                i = _points.Length - 4;
            } else {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetFirstDerivative(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t)) - transform.position;
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public void AddCurve()
        {
            Vector3 point = _points[^1];
            Array.Resize(ref _points, _points.Length + 3);
            point.x += 1.0f;
            _points[^3] = point;
            point.x += 1.0f;
            _points[^2] = point;
            point.x += 1.0f;
            _points[^1] = point;

            Array.Resize(ref _modes, _modes.Length + 1);
            _modes[^1] = _modes[^2];
            EnforceMode(_points.Length - 4);

            if(Loop) {
                _points[^1] = _points[0];
                _modes[^1] = _modes[0];
                EnforceMode(0);
            }
        }

        public void ResetSpline()
        {
            _points = new[] {
                       Vector3.right,
                2.0f * Vector3.right,
                3.0f * Vector3.right,
                4.0f * Vector3.right
            };

            _modes = new[] {
                Mode.Free,
                Mode.Free
            };
        }

        public float EstimatedLength(int samples = 20)
        {
            float len = 0.0f;

            Vector3 prev = GetPoint(0.0f);
            for(int i = 1; i < samples; ++i) {
                Vector3 curr = GetPoint((float)i / (float)samples);
                len += (prev - curr).magnitude;
                prev = curr;
            }

            return len;
        }
    }
}
