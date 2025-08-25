using UnityEngine;

namespace LK.Runtime.Setters
{
    public class TimeScaleSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private float timeScale = 1f;
    
        public void Apply()
        {
            Time.timeScale = timeScale;
        }
    }
}
