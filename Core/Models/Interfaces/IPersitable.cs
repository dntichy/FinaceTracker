using System.Collections.ObjectModel;

namespace Core.Models
{
    internal interface IPersitable<T>
    {
        void Add(T record);
         void Remove(T record);
         void Modify(T record);
    }
}