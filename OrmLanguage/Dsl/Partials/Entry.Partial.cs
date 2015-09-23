using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace Company.OrmLanguage
{
    public partial class Entry
    {		
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        protected Entry(DslModeling::Partition partition, DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            Guid = System.Guid.NewGuid();
        }
    }
}
