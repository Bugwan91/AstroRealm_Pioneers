using UnityEngine;

namespace Code
{
    public class AudioTest : MonoBehaviour
    {
        AudioSource m_MyAudioSource;

        public float fireRate = 4f;
        public int index = 0;
        public int count = 4;
        
        private float lastTime;
        private float delta;
        private float dellay;
        
        void Start()
        {
            //Fetch the AudioSource from the GameObject
            m_MyAudioSource = GetComponent<AudioSource>();
            delta = 1f / fireRate;
            dellay = (delta / count) * index;
            lastTime = Time.time + dellay;
        }
        
        void FixedUpdate()
        {
            if (Time.time > (lastTime + (1f / fireRate)))
            {
                lastTime = Time.time;
                m_MyAudioSource.Play();
            }
        }
    }
}