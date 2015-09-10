using EnvDTE;

namespace ORM.VSPackage.Helper
{
    public static class CodeModelHelper
    {
        /// <summary>
        /// Returns the namespace element from file code.
        /// </summary>
        /// <param name="fileCodeModel"></param>
        /// <returns></returns>
        public static CodeNamespace GetNameSpaceFromFileCode(FileCodeModel fileCodeModel)
        {
            var codeElements = fileCodeModel.CodeElements;
            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                {
                    return (CodeNamespace)codeElement;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Returns the code class from the file code model.
        /// </summary>
        /// <param name="fileCodeModel"></param>
        public static CodeClass GetCodeClassFromFileCode(FileCodeModel fileCodeModel)
        {
            var codeNameSpace = GetNameSpaceFromFileCode(fileCodeModel);
            foreach (CodeElement namespaceChild in codeNameSpace.Children)
            {
                if (namespaceChild.Kind == vsCMElement.vsCMElementClass)
                {
                    return (CodeClass)namespaceChild;
                }
            }

            return null;
        }
    }
}
