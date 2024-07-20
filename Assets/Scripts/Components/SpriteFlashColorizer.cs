using UnityEngine;

namespace Components
{
    public class SpriteFlashColorizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Color _color;
        [SerializeField] private float _timeToFade;

        private bool _isFlashing;
        private float _timeTillFade;

        private void Update()
        {
            if (!_isFlashing)
            {
                return;
            }

            if (_timeTillFade > 0)
            {
                _timeTillFade -= Time.deltaTime;
                var t = _timeTillFade / _timeToFade;
                _sprite.color = Color.Lerp(Color.white, _color, t);
            }
            else
            {
                _sprite.color = Color.white;
                _isFlashing = false;
            }
        }

        public void Flash()
        {
            _sprite.color = _color;
            _timeTillFade = _timeToFade;
            _isFlashing = true;
        }
    }
}