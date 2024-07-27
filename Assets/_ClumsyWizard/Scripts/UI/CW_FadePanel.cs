using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.UI
{
    public class CW_FadePanel : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Fade(bool fade)
        {
            animator.SetBool("Fade", fade);
        }
    }
}