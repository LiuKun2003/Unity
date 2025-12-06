using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class IntervalProcedure : Procedure
    {
        [SerializeField] private float intervalTime;
        
        private float _timer;
        
        public override void OnBegin()
        {
            base.OnBegin();
            _timer = 0;
        }

        public override void OnUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= intervalTime)
            {
                Complete();
            }
        }
    }
}
