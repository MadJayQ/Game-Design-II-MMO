using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class ExampleMovingPlatform : BaseMoverController
    {
        public Vector3 TranslationAxis = Vector3.right;
        public float TranslationPeriod = 10;
        public float TranslationSpeed = 1;
        public Vector3 RotationAxis = Vector3.up;
        public float RotSpeed = 10;
        public Vector3 OscillationAxis = Vector3.zero;
        public float OscillationPeriod = 0.5f;
        public float OscillationSpeed = 10;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private Vector3 _goalPosition;
        private float m_flOscillationTime = 0f;

        public void Start()
        {
            _originalPosition = Mover.Rigidbody.position;
            _originalRotation = Mover.Rigidbody.rotation;
            _goalPosition = _originalPosition;
        }

        public void FixedUpdate() {
            _goalPosition = (_originalPosition + (TranslationAxis.normalized * Mathf.Sin(Time.time * TranslationSpeed) * TranslationPeriod));
        }

        public override void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            m_flOscillationTime += deltaTime;
            goalPosition = (_originalPosition + (TranslationAxis.normalized * Mathf.Sin(m_flOscillationTime * TranslationSpeed) * TranslationPeriod));

            Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(Time.time * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(RotationAxis * RotSpeed * Time.time) * targetRotForOscillation;
        }
    }
}