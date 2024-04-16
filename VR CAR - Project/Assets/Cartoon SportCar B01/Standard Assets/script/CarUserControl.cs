using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    


    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public HingeJoint steeringWheel;
        public float maxTurnAngle = 180;

        public HingeJoint speedLever;
        public float maxSpeedAngle = 50;
        

        //Inputs to control car speed...
        public InputActionAsset inputActions;
        private InputAction _increase;
        private InputAction _decrease;
        public float v = 0f;


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        private void Start()
        {
            _increase = inputActions.FindActionMap("XRI RightHand").FindAction("Increase");
            _increase.Enable();
            _increase.performed += Accelerate;

            _decrease = inputActions.FindActionMap("XRI LeftHand").FindAction("Decrease");
            _decrease.Enable();
            _decrease.performed += Deccelerate;
        }

        private void OnDestroy()
        {
            _decrease.performed -= Deccelerate;
            _increase.performed -= Accelerate;
        }

        public void Accelerate(InputAction.CallbackContext context)
        {
            v = Mathf.Clamp(v + 0.1f, 0f, 1f); // Increase speed within range 0 to 1
        }

        public void Deccelerate(InputAction.CallbackContext context)
        {
            v = Mathf.Clamp(v *Time.deltaTime, -1f, 0f); // Decrease speed within range -1 to 0
        }
        
        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = Mathf.Clamp(steeringWheel.angle/maxTurnAngle,-1,1);
            // v = Mathf.Clamp(speedLever.angle / maxSpeedAngle, -1, 1);

            //if (Mathf.Abs(v) < 0.1f) v = 0;
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
