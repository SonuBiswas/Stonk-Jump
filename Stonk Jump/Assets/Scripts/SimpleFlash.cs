using System.Collections;

using UnityEngine;

namespace BarthaSzabolcs.Tutorial_SpriteFlash
{
    public class SimpleFlash : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        [SerializeField] private Material flashMaterial;

        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration;

        #endregion
        #region Private Fields

        // The SpriteRenderer that should flash.
        public MeshRenderer meshRenderer;
        public SkinnedMeshRenderer skinMesh;
        // The material that was in use, when the script started.
        private Material originalMaterial;
        public Material originalSkinMaterial;

        // The currently running coroutine.
        private Coroutine flashRoutine;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            // Get the SpriteRenderer to be used,
            // alternatively you could set it from the inspector.
            if (meshRenderer == null)
            {
               var mesh = GetComponent<MeshRenderer>();
                if (mesh == null) return;
                meshRenderer = mesh;

                // Get the material that the SpriteRenderer uses, 
                // so we can switch back to it after the flash ended.
                originalMaterial = mesh.material;
            }
        }

        #endregion

        
        public void Flash()
        {
            if (meshRenderer == null) return;
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine());
        }

        public void Multipleflash(int number,float intervel)
        {

            StartCoroutine(flashMultiple(number, intervel));
        }
        private IEnumerator FlashRoutine()
        {
            // Swap to the flashMaterial.
            meshRenderer.material = flashMaterial;

            // Pause the execution of this function for "duration" seconds.
            yield return new WaitForSeconds(duration);

            // After the pause, swap back to the original material.
            meshRenderer.material = originalMaterial;

            // Set the routine to null, signaling that it's finished.
            flashRoutine = null;
        }
        public void FlashSkin()
        {
            if (skinMesh == null) return;
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutineSkin());
        }
        private IEnumerator FlashRoutineSkin()
        {
            // Swap to the flashMaterial.
            skinMesh.material = flashMaterial;

            // Pause the execution of this function for "duration" seconds.
            yield return new WaitForSeconds(duration);

            // After the pause, swap back to the original material.
            skinMesh.material = originalSkinMaterial;

            // Set the routine to null, signaling that it's finished.
            flashRoutine = null;
        }

        private IEnumerator flashMultiple(int _number, float _intervel)
        {
            int x = 0;
            while (x < _number)
            {
                Flash();
                x++;
                yield return new WaitForSeconds(_intervel);
            }
            
        }
        #endregion


    }



}