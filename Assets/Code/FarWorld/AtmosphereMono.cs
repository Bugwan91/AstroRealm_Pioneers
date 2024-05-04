using Unity.Mathematics;
using UnityEngine;

namespace Code
{
    public class AtmosphereMono : MonoBehaviour
    {

        [SerializeField] private Transform sunPosition;

        private Material _material;
        private static readonly int SunDirection = Shader.PropertyToID("_sunDirection");
        private static readonly int Distance = Shader.PropertyToID("_sunDistance");

        private void Start() {
            _material = Instantiate(GetComponent<Renderer>().sharedMaterial);
            GetComponent<Renderer>().material = _material;
        }

        private void Update() {
            _material.SetVector(SunDirection, LightDirection());
            _material.SetFloat(Distance, SunDistance());
        }

        private Vector4 LightDirection() {
            var direction = (transform.position - sunPosition.position).normalized;
            return new Vector4(direction.x, direction.y, direction.z, 0);
        }

        private float SunDistance()
        {
            return math.length(transform.position - sunPosition.position);
        }
    }
}