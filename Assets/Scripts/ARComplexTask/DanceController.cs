using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ARComplexTask
{
    public class DanceController : MonoBehaviour
    {

        public ParticleSystem[] Lights;
        private Animator _animator;
        private int _idleState;
        private int _hipHopDance;
        private int _sillyDance;
        private int _breakDance;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _idleState = Animator.StringToHash("Stop");
            _hipHopDance = Animator.StringToHash("Hip-hop");
            _sillyDance = Animator.StringToHash("Silly");
            _breakDance = Animator.StringToHash("Breakdance");
        }
        public void SetCurrentDance(DanceStyle danceStyle)
        {
            switch (danceStyle)
            {
                case DanceStyle.HipHop:
                    {
                        _animator.SetTrigger(_hipHopDance);
                        SetLights(true);
                        break;
                    }
                case DanceStyle.Silly:
                    {
                        _animator.SetTrigger(_sillyDance);
                        SetLights(true);
                        break;
                    }
                case DanceStyle.BreakDance:
                    {
                        _animator.SetTrigger(_breakDance);
                        SetLights(true);
                        break;
                    }
                case DanceStyle.None:
                    {
                        _animator.SetTrigger(_idleState);
                        SetLights(false);
                        break;
                    }
            }
        }

        private void SetLights(bool state)
        {
            foreach (var light in Lights)
            {
                if (state)
                    light.Play();
                else
                    light.Stop();
            }
        }
    }
    public enum DanceStyle
    {
        None = -1,
        HipHop,
        Silly,
        BreakDance
    }
}