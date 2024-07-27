using ClumsyWizard.Core;
using ClumsyWizard.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace ClumsyWizard.Audio
{
    public class CW_AudioDataHub : CW_Persistant<CW_AudioDataHub>, ISceneLoadEvent
    {
        [field: SerializeField] public CW_Dictionary<SoundType, AudioMixerGroup> Mixers { get; private set; }

        protected override void CleanUpStaticData()
        {
        }

        //Clean up
        public void OnSceneLoaded()
        {
        }

        public void OnSceneLoadTriggered(Action onComplete)
        {
            onComplete?.Invoke();
        }
    }
}