using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ARComplexTask
{
    public class UIManager : MonoBehaviour
    {
        public DanceController Controller;
        public Button LeftButton;
        public Button RightButton;
        public Button Controll;
        public AudioClip[] _playlist;
        public TextMeshProUGUI MusicName;
        private AudioSource _audioSource;
        private int _playlistPosition = 0;
        private bool _isPlaying;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Controll.onClick.AddListener(() => { OnClick(Controll); });
            LeftButton.onClick.AddListener(() => LeftClick());
            RightButton.onClick.AddListener(()=> RightClick());
            _isPlaying = false;
        }
        public void OnClick(Button button)
        {
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (_isPlaying)
            {
                text.text = "\u25BA";
                SetCurrentDance(-1);
                _audioSource.Pause();
            }
            else
            {
                text.text = "||";
                SetCurrentDance(_playlistPosition);
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
            TextMeshProUGUI text = Controll.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "||";
            _isPlaying = true;
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
            Controller.SetCurrentDance(danceStyle);
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
            MusicName.text = _audioSource.clip.name;
            MusicName.color = Random.ColorHSV();
            _audioSource.Play();
        }
        private void CheckRange()
        {
            if (_playlist.Length == 0)
                LeftButton.interactable = RightButton.interactable = false;
            else
            {
                if (_playlistPosition == 0)
                {
                    LeftButton.interactable = false;
                }
                if (_playlistPosition == _playlist.Length - 1)
                {
                    RightButton.interactable = false;
                }
                if (_playlistPosition > 0 && _playlistPosition < _playlist.Length-1)
                {
                    LeftButton.interactable = RightButton.interactable = true;
                }
            }

        }

    }
}