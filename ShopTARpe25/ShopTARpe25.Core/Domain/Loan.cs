using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    internal class Loan
    {
        public Guid Id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
