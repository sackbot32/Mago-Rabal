using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof (Camera))]
    [AddComponentMenu("")]
    public class ImageEffectBase : MonoBehaviour
    {
        /// Provides a shader property that is set in the inspector
        /// and a material instantiated from the shader
        public Shader shader;

        private Material m_Material;


        protected virtual void Start()
        {
            enabled = false;
            return;
        }


        protected Material material
        {
            get
            {
                if (m_Material == null)
                {
                    m_Material = new Material(shader);
                    m_Material.hideFlags = HideFlags.HideAndDontSave;
                }
                return m_Material;
            }
        }


        protected virtual void OnDisable()
        {
            if (m_Material)
            {
                DestroyImmediate(m_Material);
            }
        }
    }
}
