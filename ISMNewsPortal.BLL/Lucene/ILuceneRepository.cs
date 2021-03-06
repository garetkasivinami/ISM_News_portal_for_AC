﻿using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;

namespace ISMNewsPortal.BLL.Lucene
{
    public interface ILuceneRepository<T> where T : Model
    {
        string Directory { get; }
        void SaveOrUpdate(IEnumerable<T> items);
        void SaveOrUpdate(T item);
        void Delete(IEnumerable<T> items);
        void Delete(IEnumerable<int> ids);
        void Delete(T item);
        void Delete(int id);
        bool DeleteAll();
        void Optimize();
        IEnumerable<T> Search(Options options);
    }
}
