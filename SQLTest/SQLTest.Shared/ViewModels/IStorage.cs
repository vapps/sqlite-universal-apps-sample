using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
    interface IStorage<TItem>
    {
        Task<int> Insert(TItem item);
        Task<Customer> Get(int Id);
        Task Update(TItem item);
        Task Delete(int Id);
        Task<ObservableCollection<TItem>> GetAll();
    }
}
