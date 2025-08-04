using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Procedures.Editor
{
    public class ProcedureCreateMenuEditor : MonoBehaviour
    {
        [MenuItem("GameObject/Procedure/Controller", false, -20)]
        public static void CreateProcedureController(MenuCommand menuCommand)
        {
            Create(typeof(ProcedureController), menuCommand);
        }

        [MenuItem("GameObject/Procedure/Group", false, -19)]
        public static void CreateProcedureGroup(MenuCommand menuCommand)
        {
            Create(typeof(ProcedureGroup), menuCommand);
        }

        [MenuItem("GameObject/Procedure/Queue", false, -18)]
        public static void CreateProcedureQueue(MenuCommand menuCommand)
        {
            Create(typeof(ProcedureQueue), menuCommand);
        }

        [MenuItem("GameObject/Procedure/Active", false, -17)]
        public static void CreateActiveProcedure(MenuCommand menuCommand)
        {
            Create(typeof(ActiveProcedure), menuCommand);
        }

        [MenuItem("GameObject/Procedure/Action", false, -16)]
        public static void CreateActionProcedure(MenuCommand menuCommand)
        {
            Create(typeof(ActionProcedure), menuCommand);
        }

        [MenuItem("GameObject/Procedure/Button", false, -15)]
        public static void CreateButtonProcedure(MenuCommand menuCommand)
        {
            Create(typeof(ButtonProcedure), menuCommand);
        }

        private static void Create(Type procedureType, MenuCommand menuCommand)
        {
            var go = new GameObject(procedureType.Name);
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(go);
            go.AddComponent(procedureType);
            Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
            Selection.activeObject = go;
        }
    }
}

