using System.Collections.Generic;

namespace WebStore.Interfaces.TestAPI
{
    public interface IValueClient
    {
        IEnumerable<string> GetAll();
        int Count();
        void Add(string value);
        string GetById(int id);
        void Edit(int id, string value);
        bool Delete(int id);
    }
}
