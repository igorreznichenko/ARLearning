using System;
using UnityEngine;
namespace CarControllerScene
{
    [Serializable]
    struct Wheel
    {
        public WheelPosition Position;
        public WheelCollider Collider;
        public Transform Transform;
    }
    enum WheelPosition
    {
        Real,
        Front
    }
    public class CarControllScript : MonoBehaviour
    {
        public float MaxSpeed;
        public float TurnSensitivity;
        public float MaxTurnAngle;
        [SerializeField] Joystick _joystick;
        [SerializeField] Wheel[] _wheels;

        private void Update()
        {
            Move();
            Turn();
            AnimateWheels();
        }

        private void AnimateWheels()
        {
            foreach (var wheel in _wheels)
            {

                Quaternion rotation;
                Vector3 position;
                wheel.Collider.GetWorldPose(out position, out rotation);
                wheel.Transform.rotation = rotation;
                wheel.Transform.position = position;
            }
        }

        private void Move()
        {
            float vertical = _joystick.Vertical;
            foreach (var wheel in _wheels)
            {
                wheel.Collider.motorTorque = vertical * MaxSpeed;
                wheel.Transform.position = wheel.Collider.transform.position;
            }
        }
        private void Turn()
        {
            float horizontal = _joystick.Horizontal;
            foreach (var wheel in _wheels)
            {
                if (wheel.Position == WheelPosition.Front)
                {
                    wheel.Collider.steerAngle = MaxTurnAngle * horizontal;
                }
            }
        }


    }
}