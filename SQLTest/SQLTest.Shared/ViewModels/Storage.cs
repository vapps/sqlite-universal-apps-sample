using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
    class Storage
    {

        public async static Task<long> Insert(string customerName, string customerCity, string customerContact)
        {
            var db = App.conn;
            try
            {
                await Task.Run(() =>
                    {
                        using (var custstmt = db.Prepare("INSERT INTO Customer (Name, City, Contact) VALUES (?, ?, ?)"))
                        {
                            custstmt.Bind(1, customerName);
                            custstmt.Bind(2, customerCity);
                            custstmt.Bind(3, customerContact);
                            custstmt.Step();
                        }
                    });
            }
            catch (Exception ex)
            {
                return 0;
            }
            using (var idstmt = db.Prepare("SELECT last_insert_rowid()"))
            {
                idstmt.Step();
                return (long)idstmt[0];
            }
        }
        public async static Task<Customer> GetCustomer(long customerId)
        {
            Customer customer = null;
            await Task.Run(() =>
            {
                using (var statement = App.conn.Prepare("SELECT Id, Name, City, Contact FROM Customer WHERE Id = ?"))
                {
                    statement.Bind(1, customerId);
                    var a = statement.Step();
                    if (SQLiteResult.DONE == a || SQLiteResult.ROW == a)
                    {
                        customer = new Customer()
                        {
                            Id = (long)statement[0],
                            Name = (string)statement[1],
                            City = (string)statement[2],
                            Contact = (string)statement[3]
                        };
                    }
                }
            });
            return customer;
        }
        public async static Task Update(Customer customer)
        {
            var existingCustomer = await GetCustomer(customer.Id);
            if (existingCustomer != null)
            {
                await Task.Run(() =>
                {
                    using (var custstmt = App.conn.Prepare("UPDATE Customer SET Name = ?, City =?, Contact = ? WHERE Id=?"))
                    {
                        custstmt.Bind(1, customer.Name);
                        custstmt.Bind(2, customer.City);
                        custstmt.Bind(3, customer.Contact);
                        custstmt.Bind(4, customer.Id);

                        custstmt.Step();
                    }
                });
            }
        }
        public async static Task DeleteCustomer(long customerId)
        {
            await Task.Run(() =>
                    {
                        using (var statement = App.conn.Prepare("DELETE FROM Customer WHERE Id = ? "))
                        {
                            statement.Bind(1, customerId);
                            statement.Step();
                        }
                    });
        }

        internal static ObservableCollection<Customer> GetAllCustomers()
        {
            var items = new ObservableCollection<Customer>();
            Customer customer = null;
            using (var statement = App.conn.Prepare("SELECT Id, Name, City, Contact FROM Customer"))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Customer cs = CreateItem(statement);
                    items.Add(cs);
                }
                return items;
            }
        }

        private static Customer CreateItem(ISQLiteStatement statement)
        {
            var customer = new Customer()
            {
                Id = (long)statement[0],
                Name = (string)statement[1],
                City = (string)statement[2],
                Contact = (string)statement[3]
            };

            return customer;
        }
    }
}
