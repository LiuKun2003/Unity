using UnityEngine;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(LineRenderer))]
    public class DrawLineSegment : MonoBehaviour
    {
        [SerializeField] [Min(2)] private int pointCount = 20;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        
        private LineRenderer _lineRenderer;
        
        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = pointCount;
        }

        private void Update()
        {
            var direction = endPoint.position - startPoint.position;
            for (var i = 0; i < pointCount; i++)
            {
                _lineRenderer.SetPosition(i, startPoint.position + direction * i / (pointCount - 1));
            }
        }
    }
}
