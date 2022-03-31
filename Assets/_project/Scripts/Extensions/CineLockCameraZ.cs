////////////////////////////////////////////////////////////
// File: CineLockCameraZ
// Author: Charles Carter, Matthew Mason
// Date Created: 01/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 30/03/22
// Brief: An Extension to lock the camera's orientation
//////////////////////////////////////////////////////////// 

using Cinemachine;
using UnityEngine;
using L7Games;
using L7Games.Movement;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CineLockCameraZ : CinemachineExtension
{
    [Tooltip("Lock the camera's Z rotation to this value")]
    public float m_ZRotation = 0;

    [SerializeField]
    private PlayerHingeMovementController movementController;
    private bool bWipeOutCamera = false;

    [SerializeField]
    CinemachineVirtualCamera normalCam;

    [SerializeField]
    CinemachineVirtualCamera wipeoutCam;
    //[SerializeField]
    //CinemachineTransposer wipeoutTransp;

    //[SerializeField]
    //private float WipeoutGroundThreshold = 4f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        movementController.onRespawn += SwitchOffWipeoutCam;
    }

    private void OnDisable()
    {
        movementController.onRespawn -= SwitchOffWipeoutCam;
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(enabled && stage == CinemachineCore.Stage.Finalize)
        {
            //Never roll the camera
            state.RawOrientation = Quaternion.Euler(state.RawOrientation.eulerAngles.x, state.RawOrientation.eulerAngles.y, m_ZRotation);
        }
    }

    //Has to run after the event
    public void SwitchOnWipeoutCam(Vector3 vel)
    {
        bWipeOutCamera = true;
        movementController.OverrideCamera(wipeoutCam);

        // If there is a root or main rigid body then take that into account, otherwise not a problem
        Rigidbody mainBody = movementController.currentRagdoll.GetComponent<Rigidbody>();
        if (mainBody != null)
        {
            wipeoutCam.LookAt = mainBody.transform;
            wipeoutCam.Follow = mainBody.transform;
        }
        else
        {
            Rigidbody[] boneBodies = movementController.currentRagdoll.GetComponentsInChildren<Rigidbody>();
            if (boneBodies.Length > 0)
            {
                wipeoutCam.LookAt = boneBodies[0].transform;
                wipeoutCam.Follow = boneBodies[0].transform;
            }
        }

        //if (movementController.currentRagdoll)
        //{
        //    wipeoutCam.Follow = movementController.currentRagdoll.transform;
        //    wipeoutCam.LookAt = movementController.currentRagdoll.transform;
        //}

        //wipeoutTransp = wipeoutCam.GetCinemachineComponent<CinemachineTransposer>();
        //wipeoutTransp.m_FollowOffset.y = WipeoutGroundThreshold;
    }

    public void SwitchOffWipeoutCam()
    {
        bWipeOutCamera = false;
        wipeoutCam.enabled = false;
        normalCam.enabled = true;
    }
}
