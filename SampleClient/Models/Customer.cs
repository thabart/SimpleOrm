using System;

namespace SampleClient.Models
{
    public class Customer
    {
        private Guid _id { get; set; }

        public Guid Id
        {
            get
            {
                return _id;
            } set
            {
                _id = value;
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
