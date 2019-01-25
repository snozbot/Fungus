﻿using UnityEngine;

namespace Fungus
{
    [AddComponentMenu("")]
    public class GenericCollection<T> : Collection
    {
        [SerializeField]
        protected System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>();

        protected virtual T Promote(object o)
        {
            if (o is T)
            {
                return (T)o;
            }
            else if (o is VariableBase<T>)
            {
                var oAs = o as VariableBase<T>;
                return (T)oAs.Value;
            }

            Debug.LogError("Collection cannot promote " + o.GetType().Name + " to " + typeof(T).Name);
            return default(T);
        }

        public override bool IsCompatible(object o)
        {
            return o is T || o is VariableBase<T>;
        }

        public override void Add(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                collection.Add(t);
            }
        }

        public override void AddUnique(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                if (!collection.Contains(t))
                {
                    collection.Add(t);
                }
            }
        }

        public override void Clear()
        {
            collection.Clear();
        }

        public override System.Type ContainedType()
        {
            return typeof(T);
        }

        public override object Get(int index)
        {
            return collection[index];
        }

        public override void Remove(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                collection.Remove(t);
            }
        }

        public override void RemoveAll(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                collection.RemoveAll(x => x.Equals(t));
            }
        }

        public override void RemoveAt(int index)
        {
            collection.RemoveAt(index);
        }

        public override void Set(int index, object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                collection[index] = t;
            }
        }

        public override int Count()
        {
            return collection.Count;
        }

        public override void Shuffle()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var tmp = collection[i];
                var rnd = UnityEngine.Random.Range(0, collection.Count);
                collection[i] = collection[rnd];
                collection[rnd] = tmp;
            }
        }

        public override void Reverse()
        {
            collection.Reverse();
        }

        public override bool Contains(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                return collection.Contains(t);
            }
            return false;
        }

        public override int IndexOf(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                return collection.IndexOf(t);
            }
            return -1;
        }

        public override int LastIndexOf(object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                return collection.LastIndexOf(t);
            }
            return -1;
        }

        public override void Get(int index, ref Variable variable)
        {
            if(variable is VariableBase<T>)
            {
                VariableBase<T> vt = variable as VariableBase<T>;
                vt.Value = collection[index];
            }
        }

        public override int Occurrences(object o)
        {
            int retval = 0;
            var t = Promote(o);
            if (t != null)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].Equals(t))
                        retval++;
                }
            }
            return retval;
        }

        public override void Insert(int index, object o)
        {
            var t = Promote(o);
            if (t != null)
            {
                collection.Insert(index, t);
            }
        }

        public override void Sort()
        {
            collection.Sort();
        }

        public override void Reserve(int count)
        {
            collection.Capacity = count;
        }

        public override void Resize(int count)
        {
            var toAdd = count - collection.Count;
            if(toAdd > 0)
                collection.AddRange(System.Linq.Enumerable.Repeat(default(T), toAdd));
        }

        public override int Capacity()
        {
            return collection.Capacity;
        }
    }
}