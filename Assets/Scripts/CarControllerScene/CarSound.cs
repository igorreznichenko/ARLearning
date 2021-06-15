using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CarControllerScene
{
    public class CarSound : MonoBehaviour
    {
        public bool IsStart => _isStart;
        public AudioClip StartEngine;
        public AudioClip Drive;
        private AudioSource _audioSource;
        private bool _isStart;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _isStart = false;
        }

        public void StartCar()
        {
            StartCoroutine(startCar());
        }
        public void DriveCar()
        {
            if (_isStart && !_audioSource.isPlaying) {
                _audioSource.clip = Drive;
                _audioSource.Play();
            }
        }
        public void StopDriveCar()
        {
            _audioSource.Stop();
        }
       private IEnumerator startCar()
        {
            _audioSource.clip = StartEngine;
            _audioSource.Play();
            yield return new WaitForSeconds(_audioSource.clip.length);
            _audioSource.clip = Drive;
            _isStart = true;
        }
    }
}