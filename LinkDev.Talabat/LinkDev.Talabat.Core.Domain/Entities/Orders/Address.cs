﻿using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    [Owned]
    public class Address
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
