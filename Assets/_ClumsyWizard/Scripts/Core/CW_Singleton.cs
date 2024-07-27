using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.Core
{
	public abstract class CW_Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
		}

        //Used to remove reference and clear out Delegate functions
        protected abstract void CleanUpStaticData();
        protected virtual void OnDestroy()
        {
            CleanUpStaticData();
        }
    }

	public abstract class CW_Persistant<T> : CW_Singleton<T> where T : MonoBehaviour, ISceneLoadEvent
	{
		protected override void Awake()
		{
			base.Awake();
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
		}

        protected override void CleanUpStaticData()
        {
        }
    }
}