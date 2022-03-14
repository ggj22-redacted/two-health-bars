using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Common.Entities;

public class StageMusicBehaviour : MonoBehaviour
{

    [Inject]
    private EntityRespawner _entityRespawner;
    [Inject]
    private EntityState _playerState;
    private float delayCounter;
    private AudioSource[] stageMusic;

    // Start is called before the first frame update
    void Start()
    {
        delayCounter = _entityRespawner.restartDelay;
        stageMusic = transform.parent.GetComponentsInChildren<AudioSource>();
        StartCoroutine(DelayStart());
    }

    void CallMusicStart(EntityRespawner _entityRespawner)
    {
        StartCoroutine(DelayStart());
    }

    void CallMusicStop(EntityState _playerState)
    {
        MusicStart();
    }

    void MusicStart()
    {
        for (int x = 0; x < stageMusic.Length; x++)
        {
            if (stageMusic[x].isPlaying)
            {
                stageMusic[x].Stop();
            }
            else
            {
                stageMusic[x].Play();
            }
        }
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayCounter);
        MusicStart();
    }

    private void OnEnable()
    {
        _entityRespawner.OnRespawn += CallMusicStart;
        _playerState.OnDied += CallMusicStop;
    }

    private void OnDisable()
    {
        _entityRespawner.OnRespawn -= CallMusicStart;
        _playerState.OnDied -= CallMusicStop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
