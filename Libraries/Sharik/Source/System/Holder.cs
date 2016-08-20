using System;
using System.Threading;

namespace Sharik
{
    public class Holder<T> where T : class
    {
        protected T fObject;

        // Public

        public delegate T UpdatedObjectGetter(T originalObject);
        public delegate bool ConditionalExchangePredicate(T originalObject);

        public T Object
        {
            get { return fObject; }
            set { fObject = value; }
        }

        public Holder()
        {
        }

        public Holder(T obj)
        {
            fObject = obj;
        }

        public T Exchange(T updatedObject)
        {
            return Interlocked.Exchange(ref fObject, updatedObject);
        }

        public T CompareExchange(T updatedObject, T originalObject)
        {
            return Interlocked.CompareExchange(ref fObject, updatedObject, originalObject);
        }

        public bool ComparedExchanged(T updatedObject, T originalObject)
        {
            return Interlocked.CompareExchange(ref fObject, updatedObject, originalObject) == originalObject;
        }

        public bool ComparedExchanged(UpdatedObjectGetter updatedObjectGetter, int attempts)
        {
            while (attempts > 0)
            {
                T d = Object;
                T u = updatedObjectGetter(d);
                if (Interlocked.CompareExchange(ref fObject, u, d) == d)
                    break;
                else
                {
                    attempts -= 1;
                    Thread.Yield();
                }
            }
            return attempts > 0;
        }

        public bool ConditionalExchange(ConditionalExchangePredicate predicate, T updatedObject, int attempts)
        {
            while (attempts > 0)
            {
                T d = Object;
                if (predicate(d))
                {
                    if (Interlocked.CompareExchange(ref fObject, updatedObject, d) == d)
                        break;
                    else
                        attempts -= 1;
                }
            }
            return attempts > 0;
        }

        public bool ConditionalExchange(ConditionalExchangePredicate predicate, UpdatedObjectGetter updatedObjectGetter, int attempts)
        {
            while (attempts > 0)
            {
                T d = Object;
                if (predicate(d))
                {
                    T u = updatedObjectGetter(d);
                    if (Interlocked.CompareExchange(ref fObject, u, d) == d)
                        break;
                    else
                        attempts -= 1;
                }
            }
            return attempts > 0;
        }

        public override string ToString()
        {
            return fObject.ToString();
        }
    }
}
