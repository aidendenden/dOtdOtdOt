#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Rendering;
using UnityEditor.Build;

namespace Abiogenesis3d
{
    class ShaderPreprocessor : IPreprocessShaders
    {
        public int callbackOrder { get { return 0; } }

        public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            if (shader.name.Contains("Abiogenesis3d"))
            {
                for (int i = 0; i < data.Count; ++i)
                {
                    #if UNITY_PIPELINE_URP
                    var keyword = new ShaderKeyword("UNITY_PIPELINE_URP");
                    if (!data[i].shaderKeywordSet.IsEnabled(keyword))
                        data[i].shaderKeywordSet.Enable(keyword);
                    #endif
                }
            }
        }
    }
}
#endif
