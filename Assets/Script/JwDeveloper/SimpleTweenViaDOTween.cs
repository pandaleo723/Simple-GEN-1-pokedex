using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace JwDeveloper.Utility
{
    public class SimpleTweenViaDOTween : MonoBehaviour
    {
        public string tweenName;
        [Space(10)]
        public bool tweenOnStart = false;
        public bool tweenOnEnable = false;
        //[Tooltip("Overide all the tween duration to this float value." +
        //    "\nSet this value to negative to disable this feature.")]
        [HorizontalGroup("overrideTweenDuration", 0.1f), HideLabel]
        public bool overrideTweenDuration = false;
        [HorizontalGroup("overrideTweenDuration"), LabelText("Override Tween Duration")]
        [EnableIf("overrideTweenDuration")]
        public float tweenDuration = 0f;
        [Space(10)]
        public bool disableObjectOnTweenEnd = false;
        [Space(10)]

        // -------------------------- Local Move Vairable ---------------------------//

        #region DOTween Local Move Vairable
        public bool doLocalMove = false;
        [BoxGroup("LocalMove")]
        [ShowIfGroup("LocalMove/doLocalMove"), ToggleLeft]
        [HorizontalGroup("LocalMove/doLocalMove/Horizontal")]
        public bool useCustomStartLocalPosition = false;
        [ShowIfGroup("LocalMove/doLocalMove"), Button("$getPosToTargetName")]
        [HorizontalGroup("LocalMove/doLocalMove/Horizontal", Width = 120f)]
        public void GetCurrentLocalPosition()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this.transform, getPosToTargetName);
#endif
            targetLocalMove = transform.localPosition;
        }
        [ShowIfGroup("LocalMove/doLocalMove"), ShowIf("@!useRelativeDistance"), DisableIf("@!useCustomStartLocalPosition")]
        public Vector3 startLocalMove = default;
        [ShowIfGroup("LocalMove/doLocalMove"), ShowIf("@!useRelativeDistance")]
        public Vector3 targetLocalMove = default;
        [ShowIfGroup("LocalMove/doLocalMove"), ToggleLeft]
        [HorizontalGroup("LocalMove/doLocalMove/Horizontal_1")]
        public bool useRelativeDistance = false;
        [ShowIfGroup("LocalMove/doLocalMove"), Button("$getRelativeDistanceName")]
        [HorizontalGroup("LocalMove/doLocalMove/Horizontal_1", Width = 150f)]
        public void GetCurrentRelativePos()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this.transform, getRelativeDistanceName);
#endif
            relativeTargetDistance = targetLocalMove - transform.localPosition;
            targetLocalMove = Vector3.zero;
            useRelativeDistance = true;
        }
        [ShowIfGroup("LocalMove/doLocalMove"), ShowIf("useRelativeDistance"), DisableIf("@!useCustomStartLocalPosition")]
        public Vector3 relativeStartDistance = default;
        [ShowIfGroup("LocalMove/doLocalMove"), ShowIf("useRelativeDistance")]
        public Vector3 relativeTargetDistance = default;
        [ShowIfGroup("LocalMove/doLocalMove"), DisableIf("overrideTweenDuration")]
        public float moveDuration = 0f;


        [ShowIfGroup("LocalMove/doLocalMove")]
        public Ease moveEaseType = default;
        [ShowIfGroup("LocalMove/doLocalMove")]
        public UnityEvent onMoveComplete;
        #endregion //DOTween Local Move Vairable

        // -------------------------- Local Scale Vairable ---------------------------//

        #region DOTween Local Scale Variable 
        [Space(10)]
        public bool doLocalScale = false;
        [BoxGroup("LocalScale")]
        [ShowIfGroup("LocalScale/doLocalScale"), ToggleLeft]
        public bool useCustomStartLocalScale = false;
        [ShowIfGroup("LocalScale/doLocalScale"), EnableIf("useCustomStartLocalScale")]
        public Vector3 startLocalScale = Vector3.one;
        [ShowIfGroup("LocalScale/doLocalScale")]
        public Vector3 targetLocalScale = Vector3.one;
        [ShowIfGroup("LocalScale/doLocalScale"), DisableIf("overrideTweenDuration")]
        [HorizontalGroup("LocalScale/doLocalScale/Horizontal")]
        public float scaleDuration = 0f;

        [ShowIfGroup("LocalScale/doLocalScale"), Button("$getScaleToTargetName")]
        [HorizontalGroup("LocalScale/doLocalScale/Horizontal", Width = 120f)]
        public void GetCurrentLocalScale()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this.transform, getScaleToTargetName);
#endif
            targetLocalScale = transform.localScale;
        }
        [ShowIfGroup("LocalScale/doLocalScale")]
        public Ease scaleEaseType = default;
        [ShowIfGroup("LocalScale/doLocalScale")]
        public UnityEvent onScaleComplete;
        #endregion //DOTween Local Scale Variable

        // -------------------------- Local Rotation Vairable ---------------------------//

        #region DOTween Local Rotation Variable
        [Space(10)]
        public bool doLocalRotation = false;
        [BoxGroup("LocalRotation")]
        [ShowIfGroup("LocalRotation/doLocalRotation"), ToggleLeft]
        [HorizontalGroup("LocalRotation/doLocalRotation/Horizontal")]
        public bool useCustomStartLocalRotation = false;
        [ShowIfGroup("LocalRotation/doLocalRotation"), Button("$getRotationToTargetName")]
        [HorizontalGroup("LocalRotation/doLocalRotation/Horizontal", Width = 150)]
        public void GetCurrentLocalRotate()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this.transform, getRotationToTargetName);
#endif
            targetLocalRotate = transform.localEulerAngles;
        }
        [ShowIfGroup("LocalRotation/doLocalRotation"), EnableIf("useCustomStartLocalRotation")]
        public Vector3 startLocalRotate = Vector3.zero;
        [ShowIfGroup("LocalRotation/doLocalRotation")]
        public Vector3 targetLocalRotate = Vector3.zero;
        [ShowIfGroup("LocalRotation/doLocalRotation"), ToggleLeft]
        public bool useRelativeRotation = false;
        [ShowIfGroup("LocalRotation/doLocalRotation"), DisableIf("overrideTweenDuration")]
        public float rotateDuration = 0f;
        [ShowIfGroup("LocalRotation/doLocalRotation")]
        public Ease rotationEaseType = default;
        [ShowIfGroup("LocalRotation/doLocalRotation")]
        public bool rewindRotation = false;
        [ShowIfGroup("LocalRotation/doLocalRotation")]
        public UnityEvent onRotationComplete;
        #endregion //DOTween Local Rotation Variable

        // -----------------------------------------------------------------------//

        private bool isTweening = false;
        private string getPosToTargetName        = "GetPosToTarget";
        private string getRelativeDistanceName   = "GetRelativeDistance";
        private string getScaleToTargetName      = "GetScaleToTarget";
        private string getRotationToTargetName   = "GetRotationToTarget";

        private void Start()
        {
            if (tweenOnStart) Tween_Start();
        }

        private void OnEnable()
        {
            if (tweenOnEnable) Tween_Start();
        }

        //public void TweenAndDisable()
        //{
        //    Tween_Start();
            
        //}

        public void Tween_Start()
        {
            if (doLocalMove) TweenLocalPosition();
            if (doLocalScale) TweenLocalScale();
            if (doLocalRotation) TweenLocalRotation();
        }

        public void Tween_Backward()
        {
            if (doLocalRotation) TweenLocalRotation().PlayBackwards();
        }

        public void TweenLocalPosition()
        {
            if (!doLocalMove || isTweening) return;
            if (IsOverideTweenDurationEnabled()) moveDuration = tweenDuration;
            if (useRelativeDistance) targetLocalMove = transform.localPosition + relativeTargetDistance;
            if (useCustomStartLocalPosition)
            {
                if (useRelativeDistance)
                {
                    transform.localPosition -= relativeStartDistance;
                }
                else
                {
                    transform.localPosition = startLocalMove;
                }
            }

            transform.DOLocalMove(targetLocalMove, moveDuration)
                .SetEase(moveEaseType).OnComplete(_OnMoveComplete);
        }

        public void TweenLocalScale()
        {
            if (!doLocalScale || isTweening) return;
            if (IsOverideTweenDurationEnabled()) scaleDuration = tweenDuration;
            if (useCustomStartLocalScale) transform.localScale = startLocalScale;
            transform.DOScale(targetLocalScale, scaleDuration)
                .SetEase(scaleEaseType).OnComplete(_OnScaleComplete);
        }

        public Tween TweenLocalRotation()
        {
            if (!doLocalRotation || isTweening) return null;
            if (IsOverideTweenDurationEnabled()) rotateDuration = tweenDuration;
            if (useCustomStartLocalRotation)
            {
                if (useRelativeRotation)
                {
                    transform.localEulerAngles -= startLocalRotate;
                }
                else
                {
                    transform.localEulerAngles = startLocalRotate;
                }
            }
            return transform.DORotate(targetLocalRotate, rotateDuration)
                .SetEase(rotationEaseType).OnComplete(_OnRotationComplete)
                .SetRelative(useRelativeRotation);
        }

        private void _OnMoveComplete()
        {
            if (disableObjectOnTweenEnd) this.gameObject.SetActive(false);
            onMoveComplete?.Invoke();
        }

        private void _OnScaleComplete()
        {
            if (disableObjectOnTweenEnd) this.gameObject.SetActive(false);
            onScaleComplete?.Invoke();
        }

        private void _OnRotationComplete()
        {
            if (disableObjectOnTweenEnd) this.gameObject.SetActive(false);
            onRotationComplete?.Invoke();
        }

        private bool IsOverideTweenDurationEnabled()
        {
            return overrideTweenDuration;
        }


    }
}
