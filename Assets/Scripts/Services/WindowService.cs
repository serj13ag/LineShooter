using Enums;

namespace Services
{
    public interface IWindowService : IService
    {
        void ShowEndGameWindow(WindowType windowType);
    }

    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void ShowEndGameWindow(WindowType windowType)
        {
            _uiFactory.CreateEndGameWindow(windowType);
        }
    }
}