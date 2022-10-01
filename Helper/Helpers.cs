using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SimuladorVotos.Helper
{
    public static class Helpers
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, int itemsInList)
        {
            List<T> listT = items.ToList();
            int index = 0;
            while (listT.Any())
            {
                var slicedPart = listT.GetRange(0, listT.Count >= itemsInList ? itemsInList : listT.Count);
                index = slicedPart.Count;

                listT.RemoveRange(0, index);

                yield return slicedPart;
            }
        }

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target, int initial, int final)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }

    }

    public static class Mapper
    {
        public static TDestinationEntity Map<TDestinationEntity>(this object TSourceEntity) where TDestinationEntity : class, new()
        {
            var destinationEntity = new TDestinationEntity();

            PropertyInfo[] propertyInfosOrigin = TSourceEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo pInfo in destinationEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string nameToCompare = pInfo.Name;

                Attribute attrb = pInfo.GetCustomAttribute(typeof(MapperNameAttribute), true);
                if (attrb is MapperNameAttribute)
                {
                    MapperNameAttribute attr = (MapperNameAttribute)attrb;
                    nameToCompare = attr.GetName();
                }

                PropertyInfo propToMap = propertyInfosOrigin.Where(x => x.Name.Equals(nameToCompare, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                SetValue(TSourceEntity, destinationEntity, propToMap, pInfo);
            }
            return destinationEntity;
        }

        private static void SetValue<TDestinationEntity>(object source, TDestinationEntity destination, PropertyInfo propOrigin, PropertyInfo propDesdination)
        {
            if (propOrigin != null)
            {
                object value = GetPropertieValue(source, propOrigin);

                propDesdination.SetValue(destination, Convert.ChangeType(value, propDesdination.PropertyType, CultureInfo.InvariantCulture), null);
            }
        }

        private static object GetPropertieValue<TEntity>(TEntity source, PropertyInfo pInfo)
        {
            return source.GetType().GetProperty(pInfo.Name).GetValue(source, null);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MapperNameAttribute : Attribute
    {
        private string name;
        /// <summary>
        /// Nome que será pesquisado na classe base do mapeamento
        /// </summary>
        /// <param name="name"></param>
        public MapperNameAttribute(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}