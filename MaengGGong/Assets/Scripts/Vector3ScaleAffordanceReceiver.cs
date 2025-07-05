using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class Vector3ScaleAffordanceReceiver : Vector3AffordanceReceiver
{
        [SerializeField]
        [Tooltip("The transform to apply the scale value to.")]
        Transform m_TargetTransform;

        /// <inheritdoc />
        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_TargetTransform == null)
                m_TargetTransform = transform;
        }

        /// <inheritdoc />
        protected override void OnAffordanceValueUpdated(float3 newValue)
        {
            base.OnAffordanceValueUpdated(newValue);
            m_TargetTransform.localScale = newValue;
        }
}
