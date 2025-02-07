using System.Collections.Generic;

namespace AppFramework.Admin
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            int num = 0;
            EqualityComparer<T> @default = EqualityComparer<T>.Default;
            foreach (T item in source)
            {
                if (@default.Equals(item, value))
                    return num;
                num++;
            }
            return -1;
        }
    }
}
