using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ARComplexTask
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private DanceController _controller;
		[SerializeField] private Button _leftButton;
		[SerializeField] private Button _rightButton;
		[SerializeField] private Button _controll;
		[SerializeField] private AudioClip[] _playlist;
		[SerializeField] private TextMeshProUGUI _musicName;
		[SerializeField] private Slider _musicScrollPlay;
		[SerializeField] private TextMeshProUGUI _playTime;
		private AudioSource _audioSource;
		private int _playlistPosition = 0;
		private bool _isPlaying = false;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			UpdateScroll();
			SetPlayTime();
		}

		private void OnEnable()
		{
			SubscribeEvents();
		}

		private void OnDisable()
		{
			UnsubscribeEvents();
		}

		public void OnClick()
		{
			TextMeshProUGUI text = _leftButton.GetComponentInChildren<TextMeshProUGUI>();
			if (_isPlaying)
			{
				text.text = "\u25BA";
				_controller.Pause();
				_audioSource.Pause();
			}
			else
			{
				text.text = "||";
				_controller.Resume();
				_audioSource.Play();
			}
			_isPlaying = !_isPlaying;
		}

		public void InitState()
		{
			CheckRange();
			SetUpAudio();
			SetCurrentDance(_playlistPosition);
		}

		public void ClearState()
		{
			TextMeshProUGUI text = _controll.GetComponentInChildren<TextMeshProUGUI>();
			text.text = "||";
			_audioSource.Stop();
			if (!_isPlaying)
			{
				_isPlaying = true;
				_controller.Resume();
			}
		}

		private void UpdateScroll()
		{
			_musicScrollPlay.SetValueWithoutNotify(_audioSource.time);
			if (_audioSource.time == _audioSource.clip.length)
			{
				if (_playlistPosition != _playlist.Length - 1)
					RightClick();
				else
				{
					_controll.onClick.Invoke();
					_audioSource.time = 0;
				}
			}
		}

		private void SubscribeEvents()
		{
			_controll.onClick.AddListener(OnClick);
			_musicScrollPlay.onValueChanged.AddListener(ScrollMusic);
			_leftButton.onClick.AddListener(LeftClick);
			_rightButton.onClick.AddListener(RightClick);
		}

		private void UnsubscribeEvents()
		{
			_controll.onClick.RemoveListener(OnClick);
			_musicScrollPlay.onValueChanged.RemoveListener(ScrollMusic);
			_leftButton.onClick.RemoveListener(LeftClick);
			_rightButton.onClick.RemoveListener(RightClick);
		}

		private void SetPlayTime()
		{
			int time = (int)_audioSource.clip.length;
			int curentTime = (int)_audioSource.time;
			_playTime.text = string.Format("{0:00}:{1:00}/{2:00}:{3:00}", curentTime / 60, curentTime % 60, time / 60, time % 60);
		}

		private void ScrollMusic(float time)
		{
			_audioSource.time = time;
		}

		private void LeftClick()
		{
			_playlistPosition--;
			SetCurrentState();
		}

		private void RightClick()
		{
			_playlistPosition++;
			SetCurrentState();
		}

		private void SetCurrentDance(int position)
		{
			DanceStyle danceStyle = (DanceStyle)position;
			_controller.SetCurrentDance(danceStyle);
		}

		private void SetCurrentState()
		{
			_audioSource.Stop();
			CheckRange();
			ClearState();
			SetCurrentDance(_playlistPosition);
			SetUpAudio();
		}

		private void SetUpAudio()
		{
			_audioSource.clip = _playlist[_playlistPosition];
			_musicName.text = _audioSource.clip.name;
			_musicScrollPlay.maxValue = _audioSource.clip.length;
			_audioSource.Play();
			_audioSource.time = 0;
		}

		private void CheckRange()
		{
			if (_playlist.Length == 0)
				_leftButton.interactable = _rightButton.interactable = false;
			else
			{
				if (_playlistPosition == 0)
				{
					_leftButton.interactable = false;
				}

				if (_playlistPosition == _playlist.Length - 1)
				{
					_rightButton.interactable = false;
				}

				if (_playlistPosition > 0 && _playlistPosition < _playlist.Length - 1)
				{
					_leftButton.interactable = _rightButton.interactable = true;
				}
			}
		}
	}
}