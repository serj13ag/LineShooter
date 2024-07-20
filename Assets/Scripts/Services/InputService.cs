using UnityEngine;

namespace Services
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
    }

    public class InputService : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        public Vector2 Axis => new(Input.GetAxis(HorizontalAxisName), Input.GetAxis(VerticalAxisName));
    }
}