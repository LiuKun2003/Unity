using System.IO;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    public class RenderTextureToPNG : EditorWindow
    {
        private RenderTexture _renderTexture;
        private string _savePath;

        [MenuItem("Tools/RenderTexture To PNG")]
        public static void ShowWindow()
        {
            GetWindow<RenderTextureToPNG>("RenderTexture To PNG");
        }

        private void OnGUI()
        {
            _renderTexture = (RenderTexture)EditorGUILayout.ObjectField("RenderTexture", _renderTexture, typeof(RenderTexture), false);
            if (_renderTexture == null)
            {
                EditorGUILayout.HelpBox("No renderTexture selected.", MessageType.Warning);
            }
            
            var canSave = _renderTexture != null;
            EditorGUI.BeginDisabledGroup(!canSave);
            // 显示按钮
            if (GUILayout.Button("Generate PNG"))
            {
                // 弹出文件保存对话框
                _savePath = EditorUtility.SaveFilePanel("Save PNG", "", "RenderTexture", "png");
                if (!string.IsNullOrEmpty(_savePath))
                {
                    // 调用保存方法
                    SaveRenderTextureAsPNG(_renderTexture, _savePath);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void SaveRenderTextureAsPNG(RenderTexture rt, string path)
        {
            // 创建一个临时的Texture2D来存储RenderTexture的内容
            var currentActive = RenderTexture.active;
            RenderTexture.active = rt;
            var texture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();

            // 将Texture2D转换为PNG格式的字节数据
            var bytes = texture.EncodeToPNG();

            // 保存到指定路径
            File.WriteAllBytes(path, bytes);

            // 清理
            RenderTexture.active = currentActive;
            Debug.Log("RenderTexture saved to: " + path);
        }
    }
}