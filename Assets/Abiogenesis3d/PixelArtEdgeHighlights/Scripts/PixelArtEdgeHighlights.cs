using UnityEngine;
#if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
using System.Collections.Generic;
#endif

namespace Abiogenesis3d
{
    [ExecuteInEditMode]
    public class PixelArtEdgeHighlights : MonoBehaviour
    {
    #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
    #else
        public Camera cam;
        Camera lastCam;
        public MirrorOnRenderImage mirrorOnRenderImage;
    #endif

        Material material;

        [Range(0, 1)] public float convexHighlight = 0.5f;
        [Range(0, 1)] public float outlineShadow = 0.5f;
        [Range(0, 1)] public float concaveShadow = 1;
        [Range(0, 10)] public int debugEffect;
        [HideInInspector] public Vector4 test1;

    #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
    #else
        public Shader shader;
    #endif

        // in URP effect is a renderer feature and not a blit
    #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
        public List<PixelArtEdgeHighlightsFeature> rendererFeatures;
    #endif

        void UpdateMaterialProperties()
        {
            material.SetFloat("_ConvexHighlight", convexHighlight);
            material.SetFloat("_OutlineShadow", outlineShadow);
            material.SetFloat("_ConcaveShadow", concaveShadow);

            material.SetInt("_DebugEffect", debugEffect);
            material.SetVector("_Test1", test1);
        }

        void Update()
        {
        #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
        #if UNITY_EDITOR
            rendererFeatures = SetupRenderFeatures.AddAndGetRendererFeatures<PixelArtEdgeHighlightsFeature>();
        #endif

            if (rendererFeatures.Count == 0)
            {
                Debug.Log("Renderer Features could not be added.");
                return;
            }

            foreach (var feature in rendererFeatures)
            {
                if (!feature) continue;

                var isDirty = false;
                if (feature.settings.convexHighlight != convexHighlight) isDirty = true;
                if (feature.settings.outlineShadow != outlineShadow) isDirty = true;
                if (feature.settings.concaveShadow != concaveShadow) isDirty = true;
                if (feature.settings.debugEffect != (int)debugEffect) isDirty = true;

                feature.settings.convexHighlight = convexHighlight;
                feature.settings.outlineShadow = outlineShadow;
                feature.settings.concaveShadow = concaveShadow;
                feature.settings.debugEffect = (int)debugEffect;

                if (isDirty) feature.UpdateMaterialProperties();

                feature.SetActive(true);
            }
            #else

            HandleBuiltinCam();
            #endif
        }

    #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
    #else
        void HandleBuiltinCam()
        {
            if (!cam) cam = GetComponent<Camera>();
            if (!cam) cam = Camera.main;

            if (lastCam == cam && mirrorOnRenderImage) return;

            RemoveRenderImageCallback();

            mirrorOnRenderImage = cam.GetComponent<MirrorOnRenderImage>();
            if (!mirrorOnRenderImage)
                mirrorOnRenderImage = cam.gameObject.AddComponent<MirrorOnRenderImage>();

            mirrorOnRenderImage.renderImageCallback += RenderImage;

            cam.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;

            lastCam = cam;
        }

        void RemoveRenderImageCallback()
        {
            if (mirrorOnRenderImage) mirrorOnRenderImage.renderImageCallback -= RenderImage;
        }

        private void RenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!shader) shader = Shader.Find("Abiogenesis3d/PixelArtEdgeHighlights");
            if (!material) material = new Material(shader);

            UpdateMaterialProperties();

            material.SetTexture("_MainTex", source);
            Graphics.Blit(source, destination, material);
        }

    #endif

        void OnDisable()
        {
        #if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
            foreach (var feature in rendererFeatures)
            {
                if (!feature) continue;
                feature.SetActive(false);
            }
        #else
            RemoveRenderImageCallback();
            mirrorOnRenderImage = null;
            lastCam = null;
        #endif
        }
    }
}
