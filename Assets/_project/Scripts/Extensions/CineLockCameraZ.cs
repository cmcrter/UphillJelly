////////////////////////////////////////////////////////////
// File: CineLockCameraZ
// Author: Charles Carter
// Date Created: 01/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 02/03/22
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
    [SerializeField]
    CinemachineTransposer wipeoutTransp;


    [SerializeField]
    private float WipeoutGroundThreshold = 4f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        movementController.onWipeout += SwitchOnWipeoutCam;
        movementController.onRespawn += SwitchOffWipeoutCam;
    }

    private void OnDisable()
    {
        movementController.onWipeout -= SwitchOnWipeoutCam;
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

    public void SwitchOnWipeoutCam(Vector3 vel)
    {
        bWipeOutCamera = true;
        wipeoutCam.enabled = true;
        normalCam.enabled = false;

        wipeoutCam.Follow = movementController.currentRagdoll.transform;
        wipeoutCam.LookAt = movementController.currentRagdoll.transform;
        wipeoutTransp = wipeoutCam.GetCinemachineComponent<CinemachineTransposer>();
        wipeoutTransp.m_FollowOffset.y = WipeoutGroundThreshold;
    }

    public void SwitchOffWipeoutCam()
    {
        bWipeOutCamera = false;
        wipeoutCam.enabled = false;
        normalCam.enabled = true;
    }
}
