using System;
using Company.OrmLanguage.Window;

namespace Company.OrmLanguage
{
    internal partial class OrmLanguageDocView
    {
        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
            var entityShape = PrimarySelection as EntityShape;
            if (entityShape == null)
            {
                return;
            }

            var showSimpleOrmWindow = ShowSimpleOrmWindowSingleton.Instance();
            if (showSimpleOrmWindow == null)
            {
                return;
            }

            var windowFrame = showSimpleOrmWindow.GetWindowFrame();
            if (windowFrame.IsVisible() == Microsoft.VisualStudio.VSConstants.S_FALSE)
            {
                return;
            }

            var ormMappingWindow = showSimpleOrmWindow.GetOrmMappingWindow();
            var modelElement = entityShape.ModelElement as EntityElement;
            ormMappingWindow.EntityElement = modelElement;
        }
    }
}
