using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RCTechs.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool ContainsKey<TValue, T>(this Dictionary<string, TValue> dictionary, Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
                return false;
            return dictionary.ContainsKey(memberExpression.Member.Name);
        }

        public static TValue ItemAt<TValue, T>(this Dictionary<string, TValue> dictionary, Expression<Func<T>> propertyExpression)
        {
            if (!dictionary.ContainsKey(propertyExpression))
                throw new ArgumentException("The provided dictionary doesn't contain this key.");
            var memberExpression = propertyExpression.Body as MemberExpression;
            return dictionary[memberExpression.Member.Name];
        }
    }
}
