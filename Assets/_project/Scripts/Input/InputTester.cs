//==================================================================================================================================================================================================================================================================================================================================
// File:                    Input Tester.cs
// Author:                  Matthew Mason
// Date Created:            28/10/21
// Last Edited By:          Matthew Mason
// Date Last Edited:        28/10/21
// Brief:                   A quick script for manually testing if the input handler works
//==================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Input
{
    public class InputTester : MonoBehaviour
    {
        [SerializeField]
        private InputHandler inputHandler;

        private void OnEnable()
        {
            inputHandler.brakeEnded         += InputHandler_brakeEnded;
            inputHandler.brakeStarted       += InputHandler_brakeStarted;
            inputHandler.breakUpdate        += InputHandler_breakUpdate;
            inputHandler.pressDownEnded     += InputHandler_pressDownEnded;
            inputHandler.pressDownStarted   += InputHandler_pressDownStarted;
            inputHandler.pressDownUpdate    += InputHandler_pressDownUpdate;
            inputHandler.pushEnded          += InputHandler_pushEnded;
            inputHandler.pushStarted        += InputHandler_pushStarted;
            inputHandler.pushUpdate         += InputHandler_pushUpdate;
            inputHandler.balanceUpdate      += InputHandler_balanceUpdate;
            inputHandler.turningUpdated     += InputHandler_turningUpdated;
            inputHandler.groundedJumpUpPerformed    += InputHandler_jumpUpPerformed;

            inputHandler.startGrindStarted += InputHandler_GrindButtonStart;
            inputHandler.startGrindUpdate += InputHandler_GrindButtonUpdate;
            inputHandler.startGrindEnded += InputHandler_GrindButtonEnded;
        }

        private void InputHandler_jumpUpPerformed()
        {
            Debug.Log("Jump Performed");
        }

        private void InputHandler_turningUpdated(float axisValue)
        {
            if (axisValue != 0)
            {
                Debug.Log("turning Updated: " + axisValue);
            }
        }

        private void InputHandler_balanceUpdate(float axisValue)
        {
            if (axisValue != 0)
            {
                Debug.Log("Balance Updated: " + axisValue);
            }
        }

        private void InputHandler_pushUpdate()
        {
            Debug.Log("Push Updated");
        }

        private void InputHandler_pushStarted()
        {
            Debug.Log("Push Started");
        }

        private void InputHandler_pushEnded()
        {
            Debug.Log("Push Ended");
        }

        private void InputHandler_pressDownUpdate()
        {
            Debug.Log("Press Down Held Down");
        }

        private void InputHandler_pressDownStarted()
        {
            Debug.Log("Press Down Started");
        }

        private void InputHandler_pressDownEnded()
        {
            Debug.Log("Press Down ended");
        }

        private void InputHandler_breakUpdate()
        {
            Debug.Log("Brake Held Down");
        }

        private void InputHandler_brakeStarted()
        {
            Debug.Log("Brake Started");
        }

        private void InputHandler_brakeEnded()
        {
            Debug.Log("Brake Ended");
        }

        private void InputHandler_GrindButtonStart()
        {
            Debug.Log("GrindButton Started");
        }

        private void InputHandler_GrindButtonUpdate()
        {
            Debug.Log("GrindButton Update");
        }

        private void InputHandler_GrindButtonEnded()
        {
            Debug.Log("GrindButton Ended");
        }

        private void OnDisable()
        {
            inputHandler.brakeEnded         -= InputHandler_brakeEnded;
            inputHandler.brakeStarted       -= InputHandler_brakeStarted;
            inputHandler.breakUpdate        -= InputHandler_breakUpdate;
            inputHandler.pressDownEnded     -= InputHandler_pressDownEnded;
            inputHandler.pressDownStarted   -= InputHandler_pressDownStarted;
            inputHandler.pressDownUpdate    -= InputHandler_pressDownUpdate;
            inputHandler.pushEnded          -= InputHandler_pushEnded;
            inputHandler.pushStarted        -= InputHandler_pushStarted;
            inputHandler.pushUpdate         -= InputHandler_pushUpdate;
            inputHandler.balanceUpdate      -= InputHandler_balanceUpdate;
            inputHandler.turningUpdated     -= InputHandler_turningUpdated;
            inputHandler.groundedJumpUpPerformed -= InputHandler_jumpUpPerformed;

            inputHandler.startGrindStarted -= InputHandler_GrindButtonStart;
            inputHandler.startGrindUpdate -= InputHandler_GrindButtonUpdate;
            inputHandler.startGrindEnded -= InputHandler_GrindButtonEnded;
        }
    }
}

