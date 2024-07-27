using ClumsyWizard.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.UI
{
    public class CW_InputManager : CW_Persistant<CW_InputManager>, ISceneLoadEvent
    {
        public Action OnUIInteruption;
        public Action OnPause;

        public Action SkipDialogue;
        public Action ToggleCrouch;
        public Action<Vector2> OnMouseDownWorld;
        public Vector2 InputAxis;

        //Dragging
        public bool IsMouseDragging { get; private set; }
        public Action<Vector2> OnMouseDrag;
        private Vector2 dragStartPosition;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(CW_HUDMenuManager.Instance.IsMenuOpen)
                    OnUIInteruption?.Invoke();
                else
                    OnPause?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
                ToggleCrouch?.Invoke();

            InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if(Input.GetMouseButtonDown(0))
            {
                OnMouseDownWorld?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            if(Input.GetMouseButtonDown(2))
            {
                dragStartPosition = Input.mousePosition;
            }

            if(Input.GetMouseButton(2))
            {
                if (Input.GetAxisRaw("Mouse X") != 0.0f || Input.GetAxisRaw("Mouse Y") != 0.0f)
                {
                    IsMouseDragging = true;
                    OnMouseDrag?.Invoke((dragStartPosition - (Vector2)Input.mousePosition).normalized);
                }
                else
                {
                    dragStartPosition = Input.mousePosition;
                }
            }
            else
            {
                IsMouseDragging = false;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                SkipDialogue?.Invoke();
            }
        }

        public void OnSceneLoadTriggered(Action onComplete)
        {
            onComplete?.Invoke();
        }
        public void OnSceneLoaded()
        {
        }
    }
}