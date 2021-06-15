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
        private float _currentSpeed;
        public CarSound Sounds;
        [SerializeField] Joystick _joystick;
        [SerializeField] Wheel[] _wheels;
        private void Start()
        {
            Sounds.StartCar();
        }
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
            if (Sounds.IsStart)
            {
                float vertical = _joystick.Vertical;

                foreach (var wheel in _wheels)
                {
                    if (vertical > 0)
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed,MaxSpeed, 0.2f);
                    else
                        _currentSpeed = Mathf.InverseLerp(_currentSpeed, 0, 5f);
                    wheel.Collider.motorTorque = _currentSpeed;
                    wheel.Transform.position = wheel.Collider.transform.position;
                }

                if (vertical > 0)
                    Sounds.DriveCar();
                else
                    Sounds.StopDriveCar();
                Debug.Log(_currentSpeed);
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