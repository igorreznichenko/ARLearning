using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CarControllerScene
{
    struct Wheel
    {
        public WheelPosition Position;
        public WheelCollider Collider;
        public Transform Transform;
    }
    enum WheelPosition
    {
        Real,
        Font
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
        }
        private void Move()
        {
            float vertical = _joystick.Vertical;
            if (Mathf.Abs(vertical) > 0.1) {
                foreach (var wheel in _wheels)
                {
                    wheel.Collider.motorTorque = vertical * MaxSpeed *Time.deltaTime;
                }
            }
        }
        private void Turn()
        {
            float horizontal = _joystick.Horizontal;
            if (Mathf.Abs(horizontal) > 0.1)
            {
                foreach (var wheel in _wheels)
                {
                    if (wheel.Position == WheelPosition.Font) { 
                        wheel.Collider.steerAngle = MaxTurnAngle * horizontal * Time.deltaTime;
                        wheel.Transform.rotation = wheel.Collider.transform.rotation;
                    }
                }
            }
        }


    }
}