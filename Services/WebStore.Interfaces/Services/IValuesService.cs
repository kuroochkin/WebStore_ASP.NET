using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces.Services
{
    public interface IValuesService
    {
        IEnumerable<string> GetValues();

        int Count();

        string? GetById(int Id);

        void Add(string value);

        void Edit(int Id, string value);    

        bool Delete(int Id);


    }
}
