using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ARComplexTask
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] DanceController _controller;
		[SerializeField] Button _leftButton;
		[SerializeField] Button _rightButton;
		[SerializeField] Button _controll;
		[SerializeField] AudioClip[] _playlist;
		[SerializeField] TextMeshProUGUI _musicName;
		[SerializeField] Slider _musicScrollPlay;
		[SerializeField] TextMeshProUGUI _playTime;
		private AudioSource _audioSource;
		private int _playlistPosition = 0;
		private bool _isPlaying = false;
		private UnityAction _controllAction;
		private UnityAction _leftButtonAction;
		private UnityAction _rightButtonAction;
		private UnityAction<float> _musicScrollAction;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
			_controllAction = () => OnClick(_controll);
			_leftButtonAction = () => LeftClick();
			_rightButtonAction = () => RightClick();
			_musicScrollAction = (time) => ScrollMusic(time);
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

		public void OnClick(Button button)
		{
			TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
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
			_controll.onClick.AddListener(_controllAction);
			_musicScrollPlay.onValueChanged.AddListener(_musicScrollAction);
			_leftButton.onClick.AddListener(_leftButtonAction);
			_rightButton.onClick.AddListener(_rightButtonAction);
		}

		private void UnsubscribeEvents()
		{
			_controll.onClick.RemoveListener(_controllAction);
			_musicScrollPlay.onValueChanged.RemoveListener(_musicScrollAction);
			_leftButton.onClick.RemoveListener(_leftButtonAction);
			_rightButton.onClick.RemoveListener(_rightButtonAction);
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