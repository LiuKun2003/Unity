using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LK.Editor.MenuItems
{
    public static class FitColliderToMesh
    {
        [MenuItem("GameObject/Fit Collider", false, 21)]
        private static void FitSelectedCollider()
        {
            var transforms = Selection.GetTransforms(SelectionMode.Editable);
            if (!transforms.Any())
            {
                Debug.LogWarning("Please select at least one editable object.");
                return;
            }

            foreach (var transform in transforms)
            {
                var collider = transform.GetComponent<Collider>();
                if (collider == null) continue;
                FitCollider(collider, transform);
            }
        }

        /// <summary>
        /// 自动调整Collider大小以包含目标Transform及其所有活动子物体的可见网格
        /// </summary>
        /// <param name="collider">要调整的Collider</param>
        /// <param name="targetTransform">目标Transform（包含子物体）</param>
        private static void FitCollider(Collider collider, Transform targetTransform)
        {
            // 计算目标Transform及其所有子物体的世界空间边界
            var totalBounds = CalculateTotalBounds(targetTransform);

            if (totalBounds.size == Vector3.zero)
            {
                Debug.LogWarning($"未在{targetTransform.name}及其子物体中找到有效的Mesh");
                return;
            }

            // 根据不同的Collider类型进行适配
            switch (collider)
            {
                case BoxCollider boxCollider:
                    FitBoxCollider(boxCollider, totalBounds, collider.transform);
                    break;

                case SphereCollider sphereCollider:
                    FitSphereCollider(sphereCollider, totalBounds, collider.transform);
                    break;

                case CapsuleCollider capsuleCollider:
                    FitCapsuleCollider(capsuleCollider, totalBounds, collider.transform);
                    break;

                default:
                    Debug.LogWarning($"不支持的Collider类型: {collider.GetType().Name}");
                    break;
            }
        }

        private static void FitBoxCollider(BoxCollider boxCollider, Bounds worldBounds, Transform colliderTransform)
        {
            // 将世界边界转换到Collider的本地空间
            var worldToLocal = colliderTransform.worldToLocalMatrix;

            var corners = GetCorners(worldBounds);
            for (var i = 0; i < corners.Length; i++)
            {
                corners[i] = worldToLocal.MultiplyPoint3x4(corners[i]);
            }

            var localBounds = GetBoundsFromPoints(corners);

            boxCollider.center = localBounds.center;
            boxCollider.size = localBounds.size;
        }

        private static void FitSphereCollider(SphereCollider sphereCollider, Bounds worldBounds,
            Transform colliderTransform)
        {
            // 球体半径取边界框最大维度的一半
            var radius = worldBounds.extents.magnitude;

            // 中心点转换到本地空间
            var localCenter = colliderTransform.InverseTransformPoint(worldBounds.center);

            sphereCollider.center = localCenter;
            sphereCollider.radius = radius;
        }

        private static void FitCapsuleCollider(CapsuleCollider capsuleCollider, Bounds worldBounds,
            Transform colliderTransform)
        {
            var size = worldBounds.size;
            var extends = worldBounds.extents;

            // 确定胶囊体的方向（沿最长的轴）
            var direction = 1; // Y轴
            var height = size.y;
            var radius = Mathf.Max(size.x, size.z) * 0.5f;

            if (size.x > size.y && size.x > size.z)
            {
                direction = 0; // X轴
                height = size.x;
                radius = Mathf.Max(size.y, size.z) * 0.5f;
            }
            else if (size.z > size.y && size.z > size.x)
            {
                direction = 2; // Z轴
                height = size.z;
                radius = Mathf.Max(size.x, size.y) * 0.5f;
            }

            var localCenter = colliderTransform.InverseTransformPoint(worldBounds.center);

            capsuleCollider.center = localCenter;
            capsuleCollider.height = height;
            capsuleCollider.radius = radius;
            capsuleCollider.direction = direction;
        }

        /// <summary>
        /// 计算Transform及其所有活动子物体的总边界框
        /// </summary>
        private static Bounds CalculateTotalBounds(Transform targetTransform)
        {
            var totalBounds = new Bounds();
            var hasBounds = false;

            // 获取所有MeshFilter（不包括非活动物体）
            var meshFilters = targetTransform.GetComponentsInChildren<MeshFilter>();

            foreach (var meshFilter in meshFilters)
            {
                var mesh = meshFilter.sharedMesh;
                if (mesh == null) continue;

                // 获取网格的边界框并转换到世界空间
                var meshBounds = mesh.bounds;
                var meshTransform = meshFilter.transform;

                // 由于旋转可能导致min/max不准确，需要计算所有顶点
                var corners = GetCorners(meshBounds);
                for (var i = 0; i < corners.Length; i++)
                {
                    corners[i] = meshTransform.TransformPoint(corners[i]);
                }

                var worldBounds = GetBoundsFromPoints(corners);

                // 合并到总边界
                if (!hasBounds)
                {
                    totalBounds = worldBounds;
                    hasBounds = true;
                }
                else
                {
                    totalBounds.Encapsulate(worldBounds);
                }
            }

            return hasBounds ? totalBounds : new Bounds(Vector3.zero, Vector3.zero);
        }

        /// <summary>
        /// 获取边界框的8个角点
        /// </summary>
        private static Vector3[] GetCorners(Bounds bounds)
        {
            var corners = new Vector3[8];
            corners[0] = bounds.min;
            corners[1] = bounds.max;
            corners[2] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            corners[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            return corners;
        }

        /// <summary>
        /// 从点集计算边界框
        /// </summary>
        private static Bounds GetBoundsFromPoints(Vector3[] points)
        {
            if (points.Length == 0)
                return new Bounds();

            var min = points[0];
            var max = points[0];

            foreach (var point in points)
            {
                min = Vector3.Min(min, point);
                max = Vector3.Max(max, point);
            }

            return new Bounds((min + max) * 0.5f, max - min);
        }
    }
}
