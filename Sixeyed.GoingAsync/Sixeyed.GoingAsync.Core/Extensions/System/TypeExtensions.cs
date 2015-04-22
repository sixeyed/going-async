
namespace System
{
    public static class TypeExtensions
    {
        public static string GetMessageTypeName(this Type type)
        {
            return type.Name;
        }
    }
}