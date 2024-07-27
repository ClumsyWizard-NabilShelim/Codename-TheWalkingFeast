using ClumsyWizard.Core;
using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.Audio
{
    public class AudioVolumeLerpData
    {
        public float startVolume;
        public float targetVolume;
        public float elapsedTime;
        public AudioSource audioSource;

        public AudioVolumeLerpData(float startVolume, float targetVolume, AudioSource audioSource)
        {
            this.startVolume = startVolume;
            this.targetVolume = targetVolume;
            this.audioSource = audioSource;
            elapsedTime = 0.0f;
        }
    }

    [Serializable]
    public class AudioData
    {
        public CW_SoundDataSO Data;
        public bool Loop;
        public bool PlayOnAwake;
        public float Volume;
    }

    public class CW_AudioManager : MonoBehaviour, ISceneLoadEvent
    {
        [SerializeField] private CW_Dictionary<string, AudioData> audioDatas;
        private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        //Fade out audio
        private List<AudioVolumeLerpData> audiosToLerp = new List<AudioVolumeLerpData>();
        private float audioStopDuration = 1.0f;

        //One shot audio
        private AudioSource oneShotAudioSourcePrefab;
        private int currentPlayNextIndex;

        private void Awake()
        {
            currentPlayNextIndex = 0;
            foreach (string audioID in audioDatas.Keys)
            {
                if (audioDatas[audioID] == null)
                    continue;

                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = CW_AudioDataHub.Instance.Mixers[audioDatas[audioID].Data.Type];

                source.volume = audioDatas[audioID].Volume;

                source.loop = audioDatas[audioID].Loop;
                source.playOnAwake = audioDatas[audioID].PlayOnAwake;

                audioSources.Add(audioID, source);
                if (audioDatas[audioID].PlayOnAwake)
                    Play(audioID);
            }
        }

        private void Update()
        {
            if (audiosToLerp.Count == 0)
                return;

            for (int i = audiosToLerp.Count - 1; i >= 0; i--)
            {
                AudioVolumeLerpData lerpData = audiosToLerp[i];

                if (lerpData.elapsedTime > audioStopDuration)
                {
                    lerpData.audioSource.Stop();
                    audiosToLerp.RemoveAt(i);
                }
                else
                {
                    lerpData.elapsedTime += Time.deltaTime;
                    lerpData.audioSource.volume = Mathf.Lerp(lerpData.startVolume, lerpData.targetVolume, lerpData.elapsedTime / audioStopDuration);
                }
            }
        }

        public void Play(string id)
        {
            if (!audioSources.ContainsKey(id) || audioSources[id] == null || audioSources[id].isPlaying)
                return;

            audioSources[id].clip = audioDatas[id].Data.GetClip();
            audioSources[id].Play();
        }
        public void PlayOneShot(CW_SoundDataSO soundData, float volume)
        {
            if (soundData == null)
                return;

            AudioSource source = Instantiate(oneShotAudioSourcePrefab);

            source.outputAudioMixerGroup = CW_AudioDataHub.Instance.Mixers[soundData.Type];

            source.clip = soundData.GetClip();
            source.volume = volume;

            source.loop = false;
            source.Play();
        }
        public void Stop(string id)
        {
            audiosToLerp.Add(new AudioVolumeLerpData(audioDatas[id].Volume, 0.0f, audioSources[id]));
        }

        public void PlayNextAudioQueue()
        {
            if (currentPlayNextIndex >= audioDatas.Count)
                return;

            string audioKey = audioDatas.GetKeyAt(currentPlayNextIndex);

            Play(audioKey);

            currentPlayNextIndex++;
        }

        //Clean up
        private IEnumerator CompleteAudioStop(Action onComplete)
        {
            yield return new WaitForSeconds(audioStopDuration);
            onComplete?.Invoke();
        }

        public void OnSceneLoaded()
        {
        }

        public void OnSceneLoadTriggered(Action onComplete)
        {
            foreach (string audioID in audioDatas.Keys)
            {
                if (audioDatas[audioID] != null)
                    Stop(audioID);
            }

            StartCoroutine(CompleteAudioStop(onComplete));
        }
    }
}