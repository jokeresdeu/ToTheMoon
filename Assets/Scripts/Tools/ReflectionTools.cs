using System.Reflection;

namespace Tools
{
    public static class ReflectionTools
    {
        public static T GetPrivateField<T>(this object objectWithField, string fieldName)
        {
            var field = objectWithField.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (T)field?.GetValue(objectWithField);
        }

        public static void SetPrivateField<T>(this object objectWithField, string fieldName, T value)
        {
            var field = objectWithField.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(objectWithField, value);
        }

        public static void InvokePrivateMethod(this object objectWithMethod, string methodName, object[] parameters)
        {
            var method = objectWithMethod.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(objectWithMethod, parameters);
        }
    }
}