using UnityEngine;

namespace ARComplexTask
{
	public class DanceController : MonoBehaviour
	{
		[SerializeField] ParticleSystem[] _lights;
		private Animator _animator;
		private int _hipHopDance = Animator.StringToHash("Hip-hop");
		private int _sillyDance = Animator.StringToHash("Silly");
		private int _breakDance = Animator.StringToHash("Breakdance");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public void Pause()
		{
			SetLights(false);
			_animator.speed = 0;
		}

		public void Resume()
		{
			SetLights(true);
			_animator.speed = 1;
		}

		public void SetCurrentDance(DanceStyle danceStyle)
		{
			switch (danceStyle)
			{
				case DanceStyle.HipHop:
					{
						_animator.SetTrigger(_hipHopDance);
						break;
					}
				case DanceStyle.Silly:
					{
						_animator.SetTrigger(_sillyDance);
						break;
					}
				case DanceStyle.BreakDance:
					{
						_animator.SetTrigger(_breakDance);
						break;
					}
			}
			SetLights(true);
		}

		private void SetLights(bool state)
		{
			foreach (var light in _lights)
			{
				if (state)
					light.Play();
				else
					light.Stop();
			}
		}
	}
}