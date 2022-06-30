using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using L7Games.Movement;
using UnityEngine.UI;

namespace L7Games
{
    public class EnvAudioManager : MonoBehaviour
    {

        public PlayerController playerReference;
        private StudioEventEmitter soundEmitter;

        FMOD.Studio.Bus Music;

        private string deathParameter = "IsDead";
        private IEnumerator coDeathWarble;

        private void Awake() 
        {
            Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");          
            Music.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            playerReference = playerReference ?? FindObjectOfType<PlayerController>();
        }

        private void OnEnable() 
        {
            if(!playerReference)
            {
                return;
            }

            playerReference.onWipeout += PlayerWipeOut;
            playerReference.onRespawn += PlayerRespawn;
        }

        private void OnDisable()
        {
            if(!playerReference)
            {
                return;
            }

            playerReference.onWipeout -= PlayerWipeOut;
            playerReference.onRespawn -= PlayerRespawn;
        }

        // Start is called before the first frame update
        void Start()
        {
            soundEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
            soundEmitter.SetParameter(deathParameter, 0);
            coDeathWarble = Co_DeathWarble();
        }

        public void PlayerWipeOut(Vector3 death)
        {
            StopWarble();
            StartWarble();
        }

        public void PlayerRespawn()
        {
            StopWarble();
            soundEmitter.SetParameter(deathParameter, 0);
        }

        private void StopWarble()
        {
            if(coDeathWarble != null)
            {
                StopCoroutine(coDeathWarble);
                coDeathWarble = null;
            }
        }

        private void StartWarble()
        {
            if(coDeathWarble == null)
            {
                coDeathWarble = Co_DeathWarble();
            }

            StartCoroutine(coDeathWarble);
        }

        private IEnumerator Co_DeathWarble()
        {
            for(float t = 0; t < 4; t += Time.deltaTime)
            {
                soundEmitter.SetParameter(deathParameter, t / 4);
                yield return null;
            }
        }
    }
}
