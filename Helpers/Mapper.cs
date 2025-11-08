using System.Reflection;

namespace ComplianceAPI.Helpers
{
    public static class Mapper
    {
        public static void MapProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] destinationProperties = destinationType.GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = Array.Find(destinationProperties, p => p.Name == sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                {
                    object value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
        }
    }
}