using Microsoft.VisualStudio.Shell;

namespace Company.OrmLanguage
{

    [ProvideToolWindow(typeof(SimpleOrmMappingWindow))]
    internal sealed partial class OrmLanguagePackage
    {
        protected override void Initialize()
        {
            base.Initialize();
            SimpleOrmMappingWindowCommand.Initialize(this);
        }
    }
}
