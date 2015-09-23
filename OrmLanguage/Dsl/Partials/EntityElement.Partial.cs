namespace Company.OrmLanguage
{
    public partial class EntityElement
    {
        public string GetNameValue()
        {
            var index = SampleOrmModel.Elements.IndexOf(this);
            var result = "Entity_Name_" + index;
            return result;
        }
    }
}
