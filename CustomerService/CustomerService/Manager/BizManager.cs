using CustomerService.Models;
using CustomerService.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Manager
{
    public class BizManager : IBizManager<Customer>
    {
        private static IList<Customer> _repository = new List<Customer>();
        public void Add(Customer entity)
        {
            Random rn = new Random();
            entity.Id = rn.Next(1000000, 9999999).ToString();
            _repository.Add(entity);
        }

        public bool DeleteById(string id)
        {
            var customer = _repository.Where(cust => cust.Id == id).FirstOrDefault();
            if(customer!=null)
            {
                _repository.Remove(customer);
                return true;
            }
            return false;
        }

        public IList<Customer> GetAll()
        {
            return _repository;
        }

        public Customer GetById(string id)
        {
            return _repository.Where(cust=> cust.Id==id).FirstOrDefault();
        }

        public bool UpdateById(string id, Customer entity)
        {
            var targetCustomer = _repository.FirstOrDefault(cust => cust.Id == id);
            if (targetCustomer != null)
            {
                targetCustomer.LastName = entity.LastName;
                targetCustomer.FirstName = entity.FirstName;
                targetCustomer.DOB = entity.DOB;
                //targetCustomer.SSN = entity.SSN;
                return true;
            }
            return false;
        }
    }
}
