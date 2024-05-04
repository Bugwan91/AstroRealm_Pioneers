using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Code.FarWorld
{
    public class FarWorldLocalLight: MonoBehaviour
    {
        public Transform sun;
        public Transform local;
        
        private void Update()
        {
            var sunPosition = sun.localPosition;
            var localPosition = local.localPosition;
            var lightDirection = (localPosition - sunPosition).normalized;
            transform.position = -10f * lightDirection + localPosition;
            transform.rotation = Quaternion.LookRotation(lightDirection, Vector3.up);
        }
    }
}