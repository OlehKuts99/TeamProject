using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUpdater<T>
    {
        void Update(T item);
    }
}
