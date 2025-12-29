using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LK.Editor.MenuItems
{
    public static class NormalizeMeshSize
    {
        [UnityEditor.MenuItem("GameObject/Normalize Mesh Size", false, 20)]
        private static void NormalizeSelectedMeshes()
        {
            var transforms = Selection.GetTransforms(SelectionMode.Editable);
            if (!transforms.Any())
            {
                Debug.LogWarning("Please select at least one editable object.");
                return;
            }
            NormalizeMesh(transforms);
        }

        private static void NormalizeMesh(Transform[] transforms)
        {
            // ReSharper disable CoVariantArrayConversion
            var so = new SerializedObject(transforms);
            // ReSharper restore CoVariantArrayConversion

            var scaleProp = so.FindProperty("m_LocalScale");
            if(scaleProp == null) return;
            
            foreach (var transform in transforms)
            {
                Mesh mesh = null;

                // 尝试从MeshFilter获取
                var meshFilter = transform.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    mesh = meshFilter.sharedMesh;
                }
        
                // 如果MeshFilter没有，尝试从SkinnedMeshRenderer获取
                if (mesh == null)
                {
                    var skinnedRenderer = transform.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedRenderer != null)
                    {
                        mesh = skinnedRenderer.sharedMesh;
                    }
                }

                if (mesh == null || mesh.vertices.Length == 0)
                {
                    Debug.LogWarning($"{transform.name} has no MeshFilter or SkinnedMeshRenderer.");
                    continue;
                }
                
                var bounds = mesh.bounds;
                var currentSize = bounds.size.magnitude;

                if (currentSize == 0)
                {
                    Debug.LogWarning("The mesh size is too small to normalize.");
                    continue;
                }
                
                scaleProp.vector3Value = Vector3.one * (1 / currentSize);
                scaleProp.Next(true);
            }
            so.ApplyModifiedProperties();
        }
    }
}