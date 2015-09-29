using Microsoft.VisualStudio.Shell.Interop;

namespace Company.OrmLanguage.Window
{
    public class ShowSimpleOrmWindowSingleton
    {
        private static ShowSimpleOrmWindowSingleton _instance;

        private readonly IVsWindowFrame _vsWindowFrame;

        private readonly SimpleOrmMappingWindow _simpleOrmMappingWindow;

        private ShowSimpleOrmWindowSingleton(
            IVsWindowFrame vsWindowFrame,
            SimpleOrmMappingWindow simpleOrmMappingWindow)
        {
            _vsWindowFrame = vsWindowFrame;
            _simpleOrmMappingWindow = simpleOrmMappingWindow;
        }

        public static void Instanciate(SimpleOrmMappingWindow ormMappingWindow)
        {
            if (_instance == null)
            {
                var windowFrame = (IVsWindowFrame)ormMappingWindow.Frame;
                _instance = new ShowSimpleOrmWindowSingleton(windowFrame, ormMappingWindow);
            }
        }

        public static ShowSimpleOrmWindowSingleton Instance()
        {
            return _instance;
        }

        public IVsWindowFrame GetWindowFrame()
        {
            return _vsWindowFrame;
        }

        public SimpleOrmMappingWindow GetOrmMappingWindow()
        {
            return _simpleOrmMappingWindow;
        }
    }
}
