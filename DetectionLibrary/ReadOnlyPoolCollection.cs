using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace Drivelens.DetectionLibrary
{
    /// <summary>
    /// 表示一个包含对象池的集合。集合内的所有元素全部存在于对象池中。使用单一的标识符来确定对象。
    /// </summary>
    /// <typeparam name="TObject">集合包含的对象类型。</typeparam>
    /// <typeparam name="TIdentifier">用于确定对象的标识符。</typeparam>
    public class ReadOnlyPoolCollection<TObject, TIdentifier>
        : ReadOnlyCollection<TObject>
        where TObject : IIdentifiable<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        private static List<TObject> pool = new List<TObject>();

        public static TObject GetElement(TIdentifier identifier)
        {
            return pool.FirstOrDefault(obj => obj.Identifier.Equals(identifier));
        }

        public static TObject GetOrCreate(TIdentifier identifier, Func<TObject> creator)
        {
            TObject element = pool.FirstOrDefault(obj => obj.Identifier.Equals(identifier));
            if(element == null)
            {
                element = creator();
                pool.Add(element);
            }
            return element;
        }

        public static void AddToPool(TObject obj)
        {
            if (!pool.Contains(obj))
            {
                pool.Add(obj);
            }
        }

        public static ReadOnlyPoolCollection<TObject, TIdentifier> Pool
        {
            get
            {
                return new ReadOnlyPoolCollection<TObject, TIdentifier>(pool);
            }
        }

        public ReadOnlyPoolCollection(List<TObject> objects) : base(objects)
        {
            for (int objIndex = 0; objIndex < objects.Count; objIndex++)
            {
                TObject obj = objects[objIndex];
                int index = pool.IndexOf(obj);
                if (index != -1)
                {
                    objects[objIndex] = pool[index];
                }
                else
                {
                    pool.Add(obj);
                }
            }
        }
    }
}
