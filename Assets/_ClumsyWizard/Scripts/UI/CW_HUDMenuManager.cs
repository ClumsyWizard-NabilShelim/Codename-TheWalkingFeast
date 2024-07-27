using ClumsyWizard.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ClumsyWizard.UI
{
    public interface IHUDMenu
    {
        public void Close();
    }
    public class CW_HUDMenuManager : CW_Persistant<CW_HUDMenuManager>, ISceneLoadEvent
    {
        public Action OnAllMenusClosed;

        private List<IHUDMenu> menus = new List<IHUDMenu>();
        public bool IsMenuOpen => menus.Count > 0;

        private CursorLockMode lastLockMode = CursorLockMode.None;
        private bool lastVisible = true;

        private void Start()
        {
            CW_InputManager.Instance.OnUIInteruption += OnClose;
        }

        public void AddOpenMenu(IHUDMenu menu)
        {
            if (menus.Contains(menu))
                return;

            menus.Add(menu);

            if (Cursor.visible == false || Cursor.lockState == CursorLockMode.Locked)
            {
                lastLockMode = CursorLockMode.Locked;
                lastVisible = Cursor.visible;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        public void RemoveOpenMenu(IHUDMenu menu)
        {
            menus.Remove(menu);

            if (menus.Count == 0)
            {
                Cursor.lockState = lastLockMode;
                Cursor.visible = lastVisible;

                OnAllMenusClosed?.Invoke();
            }
        }

        private void OnClose()
        {
            menus[menus.Count - 1].Close();
        }

        //Clean Up
        public void OnSceneLoaded()
        {
        }

        public void OnSceneLoadTriggered(Action onComplete)
        {
            onComplete?.Invoke();
        }
    }
}