using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    public class DataTableEditorWindow : EditorWindow
    {
        private static class ExceptionResource
        {
            public const string ColumnNameEmpty = "The column name can't be empty!";
            public const string DataTableNameEmpty = "The data table name can't be empty!";
        }
    
        // 存储二维数组数据
        private DataTable dataTable;
    
        // 存储插入/删除的行/列
        private readonly HashSet<int> insertRawSet = new();
        private readonly HashSet<int> deleteRawSet = new();
        private readonly HashSet<int> deleteColumnSet = new();
    
        // 滚动视图位置
        private Vector2 scrollPosition;
        // 单元格大小
        private const float DefaultCellWidth = 90f;     // 默认单元宽度
        private const float DefaultCellHeight = 30f;    // 默认单元高度
        private float cellWidth = DefaultCellWidth;     // 单元格宽度
        private float cellHeight = DefaultCellHeight;   // 单元格高度
        private const float MinCellWith = 48f;          // 单元格最小宽度
        private const float MinCellHeight = 20f;        // 单元格最小高度
        private float Space => cellWidth * 3 + 12f; // 标头预留空间，对齐单元格（Unity元素间隔为3，3个子元素，总宽为：3 * width + 3 * 4）
    

        /// <summary>
        /// 显示表格编辑器窗口
        /// </summary>
        [MenuItem("Tools/Data Table Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<DataTableEditorWindow>("Data Table Editor");
            window.minSize = new Vector2(400, 400);
        }

        private void OnEnable()
        {
            // 初始化数据（默认4x4）
            InitializeData(4, 4);
        }
    
        private void OnGUI()
        {
            // 完成标记的删除/插入的行/列
            UpdateInsertAndDelete();
        
            // 绘制工具栏
            DrawToolbar();
        
            // 绘制分隔线
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
            // 开始滚动视图
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
            // 绘制表格
            DrawTable();
        
            EditorGUILayout.EndScrollView();
        }
    
        // 绘制顶部工具栏
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        
            // 绘制打开按钮
            if (GUILayout.Button("Open File", EditorStyles.toolbarButton))
            {
                OpenFile();
            }
        
            // 绘制保存按钮
            if (GUILayout.Button("Save As", EditorStyles.toolbarButton))
            {
                SaveAs();
            }
        
            GUILayout.FlexibleSpace();
        
            var newTableName = EditorGUILayout.TextField("Table Name", dataTable.TableName, EditorStyles.toolbarTextField);
            if (string.IsNullOrWhiteSpace(newTableName))
            {
                Debug.LogWarning(ExceptionResource.DataTableNameEmpty);
            }
            else
            {
                dataTable.TableName = newTableName;
            }
        
            cellWidth = Mathf.Max(MinCellWith, EditorGUILayout.FloatField("Cell Width", cellWidth, EditorStyles.toolbarTextField));
            cellHeight = Mathf.Max(MinCellHeight, EditorGUILayout.FloatField("Cell Height", cellHeight, EditorStyles.toolbarTextField));
        
            GUILayout.FlexibleSpace();
        
            // 绘制添加行按钮
            if (GUILayout.Button("Add Row", EditorStyles.toolbarButton))
            {
                dataTable.Rows.Add();
            }
        
            // 绘制添加列按钮
            if (GUILayout.Button("Add Column", EditorStyles.toolbarButton))
            {
                dataTable.Columns.Add();
            }
        
            // 绘制重置单元格大小按钮
            if (GUILayout.Button("Reset Cell Size", EditorStyles.toolbarButton))
            {
                cellWidth = DefaultCellWidth;
                cellHeight = DefaultCellHeight;
            }
        
            // 绘制清除内容按钮
            if (GUILayout.Button("Clear All", EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Confirm", "Clear all data?", "Yes", "No"))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row.ItemArray = new object[dataTable.Columns.Count];
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    
        // 绘制表格
        private void DrawTable()
        {
            if (dataTable == null) return;
        
            // 绘制删除列按钮
            DrawSpaceRow(i =>
            {
                if (GUILayout.Button("Delete", GUILayout.Height(cellHeight), GUILayout.Width(cellWidth)))
                {
                    deleteColumnSet.Add(i);
                }
            });
        
            // 绘制列号
            DrawSpaceRow(i =>
            {
                EditorGUILayout.LabelField(GetColumnNumber(i + 1), GUILayout.Height(cellHeight), GUILayout.Width(cellWidth));
            });
        
            // 绘制列名称
            DrawSpaceRow(i =>
            {
                var col = dataTable.Columns[i];
                var newColumnName = EditorGUILayout.TextField(col.ColumnName, GUILayout.Height(cellHeight), GUILayout.Width(cellWidth));
                if (string.IsNullOrWhiteSpace(newColumnName))
                {
                    Debug.LogWarning(ExceptionResource.ColumnNameEmpty);
                }
                else
                {
                    col.ColumnName = newColumnName;
                }
            });
        
            // 绘制每一行
            foreach (var i in Enumerable.Range(0, dataTable.Rows.Count))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    var row = dataTable.Rows[i];
                    // 绘制插入行按钮
                    if (GUILayout.Button("Insert", GUILayout.Height(cellHeight), GUILayout.Width(cellWidth)))
                    {
                        insertRawSet.Add(i);
                    }

                    // 绘制删除行按钮
                    if (GUILayout.Button("Delete", GUILayout.Height(cellHeight), GUILayout.Width(cellWidth)))
                    {
                        deleteRawSet.Add(i);
                    }
                
                    // 绘制行索引
                    EditorGUILayout.LabelField($"{i + 1}", GUILayout.Height(cellHeight), GUILayout.Width(cellWidth));

                    // 绘制每个单元格
                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        row[j] = EditorGUILayout.TextField(row[j].ToString(), GUILayout.Height(cellHeight), GUILayout.Width(cellWidth));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    
        // 获取索引对应的列号（excel形式）
        private static string GetColumnNumber(int value)
        {
            var result = "";
            while (value > 0)
            {
                value--;
                var letter = (char)('A' + value % 26);
                result = letter + result;
                value /= 26;
            }
            return result;
        }

        private void DrawSpaceRow(Action<int> drawAction)
        {
            var colCount = dataTable.Columns.Count;
            EditorGUILayout.BeginHorizontal();
            { 
                GUILayout.Space(Space);
                for (var j = 0; j < colCount; j++)
                {
                    drawAction(j);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    
        // 完成标记的删除/插入的行/列
        private void UpdateInsertAndDelete()
        {
            foreach (var index in insertRawSet)
            {
                dataTable.Rows.InsertAt(dataTable.NewRow(), index);
            }
            insertRawSet.Clear();
        
            foreach (var index in deleteRawSet)
            {
                dataTable.Rows.RemoveAt(index);
            }
            deleteRawSet.Clear();
        
            foreach (var index in deleteColumnSet)
            {
                dataTable.Columns.RemoveAt(index);
            }
            deleteColumnSet.Clear();
        }
    
        // 初始化数据
        private void InitializeData(int rows, int cols)
        {
            dataTable = new DataTable("New Data Table");
            for (var i = 0; i < cols; i++)
            {
                dataTable.Columns.Add(new DataColumn($"Column{i + 1}"));
            }
            for (var i = 0; i < rows; i++)
            {
                dataTable.Rows.Add();
            }
        }
    
        // 打开文件
        private void OpenFile()
        {
            var openPath = EditorUtility.OpenFilePanel("Open Data Table File", "", "");
            if (string.IsNullOrEmpty(openPath)) return;
            var openedDataTable = new DataTable();
            openedDataTable.ReadXml(openPath);
            dataTable = openedDataTable;
            Debug.Log($"Open DataTable Success. [{DateTime.Now:F}] {openPath}");
        }
    
        // 保存表到文件
        private void SaveAs()
        {
            var savePath = EditorUtility.SaveFilePanel("Save Data Table File", "", "NewDataTable", "xml");
            if (string.IsNullOrEmpty(savePath)) return;
            var file = new FileInfo(savePath);
            using var writer = file.CreateText();
            dataTable.WriteXml(writer, XmlWriteMode.WriteSchema);
            AssetDatabase.Refresh();
            Debug.Log($"Save DataTable Success. [{DateTime.Now:F}] {savePath}");
        }
    }
}