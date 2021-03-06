﻿using System.Collections.Generic;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IRepository<T> where T : Model
    {
        int Create(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWithOptions(object options);
        int Count();
    }
}
