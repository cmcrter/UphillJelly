////////////////////////////////////////////////////////////
// File: AerialState.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/01/22
// Brief: The state the character is in when they're in the air
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;
using L7Games.Tricks;
using System.Collections;
using System.Collections.Generic;

namespace L7Games.Movement
{
    [Serializable]
    public class HAerialState : State
    {
        [NonSerialized] private HisGroundBelow groundedCondition = null;
        [NonSerialized] private HisNextToWallRun wallRideCondition = null;
        [NonSerialized] private HisOnGrind grindCondition = null;

        private PlayerHingeMovementController parentController;
        private Rigidbody movementRB;
        private PlayerInput pInput;
        private InputHandler inputHandler;

        private Transform playerTransform;

        [SerializeField]
        public float AerialDrag = 0.05f;
        [SerializeField]
        private float adjustGroundSmoothness = 4f;

        public HAerialState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB, HisGroundBelow groundBelow, HisNextToWallRun wallRunning, HisOnGrind grinding)
        {
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;

            grindCondition = grinding;
            groundedCondition = groundBelow;
            wallRideCondition = wallRunning;

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;
        }

        public void RegisterInputs()
        {
            //Register functions
            //inputHandler.trickPressed += Trick;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            //inputHandler.trickPressed -= Trick;
        }

        public override State returnCurrentState()
        {
            if(groundedCondition.isConditionTrue())
            {
                return parentController.groundedState;
            }

            if (grindCondition.isConditionTrue()) 
            {
                return parentController.grindingState;
            } 

            if(wallRideCondition.isConditionTrue()) 
            {
                return parentController.wallRideState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            parentController.AlignWheels();
            parentController.SmoothToGroundRotation(true, adjustGroundSmoothness, 1f, groundedCondition);
        }

        public override void PhysicsTick(float dT)
        {
            //Giving a minimum distance before the turning is effective
            if(groundedCondition.BackRightGroundHitLocalDown.distance > 5f && groundedCondition.FrontRightGroundHitLocalDown.distance > 5f)
            {
                if(Debug.isDebugBuild)
                {
                    //Debug.Log("Back Hit: " + groundedCondition.BackGroundHit.distance);
                    //Debug.Log("Front Hit: " + groundedCondition.FrontGroundHit.distance);
                }

                //Need some way of making the skateboard feel more stable in the air and just generally nicer
                //if(inputHandler.TurningAxis < 0)
                //{
                //    //Turn Left
                //    parentController.transform.Rotate(new Vector3(0, 1f, 0));
                //}

                //if(inputHandler.TurningAxis > 0)
                //{
                //    //Turn Right
                //    parentController.transform.Rotate(new Vector3(0, -1f, 0));
                //}
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Aerial");
            parentController.characterAnimator.SetBool("aerial", true);

            parentController.ResetWheelPos();
            parentController.AlignWheels();

            parentController.OverrideCamera(parentController.camBrain);

            movementRB.drag = AerialDrag;
            parentController.ModelRB.drag = AerialDrag;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            parentController.StopAirInfluenctCoroutine();
            parentController.characterAnimator.SetBool("aerial", false);

            if (parentController.trickBuffer.IsTricking)
            {
                if (parentController.trickBuffer.WithinInWipeOutTheshold)
                {
                    if(!parentController.bWipeOutLocked)
                    {
                        parentController.CallOnWipeout(movementRB.velocity);
                    }
                }
            }

            hasRan = false;
        }

        //private void Trick() 
        //{
        //    AnimationClip trick = trickClips[UnityEngine.Random.Range(0, trickClips.Length)];

        //    if(trickCombo.Count < 2)
        //    {
        //        if(trickPlaying == null)
        //        {
        //            //Start the tricking
        //            trickCombo.Add(trick);
        //            trickPlaying = parentController.StartCoroutine(Co_TrickPlayed(trick.length));
        //        }
        //        else
        //        {
        //            if(currentTrickPercent > 0.8f)
        //            {
        //                //Add to the combo for when the current animation is done (only if the trick is almost complete)
        //                trickCombo.Add(trick);
        //            }
        //        }
        //    }
        //}

        //private IEnumerator Co_TrickPlayed(float trickLength)
        //{
        //    wipeOutOnExit = true;
            
        //    while(trickCombo.Count > 0)
        //    {
        //        parentController.characterAnimator.Play(trickCombo[0].name);
        //        var state = parentController.characterAnimator.GetCurrentAnimatorStateInfo(0);
                
        //        trickLength = trickCombo[0].length * state.speed;
        //        currentTrickPercent = 0;

        //        float currentTrickTimer = 0;

        //        while(currentTrickTimer < trickLength)
        //        {
        //            currentTrickPercent = currentTrickTimer / trickLength;
        //            currentTrickTimer += Time.deltaTime;

        //            yield return null;
        //        }

        //        //Go to next trick if it's a combo
        //        trickCombo.RemoveAt(0);
                         
        //        yield return null;
        //    }

        //    wipeOutOnExit = false;
        //    parentController.characterAnimator.Play("Aerial");

        //    trickCombo.Clear();
        //    trickPlaying = null;
        //}
    }
}