using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Database
{
    internal class RepositoryManager
    {
        public static RepositoryManager Instance { get; } = new RepositoryManager();

        public CategoryRepository CategoryRepository { get; }
        public ItemRepository ItemRepository { get; }
        public DiscountRepository DiscountRepository { get; }
        public ReceiptRepository ReceiptRepository { get; }
        public ReceiptLineRepository ReceiptLineRepository { get; }
        public UserRepository UserRepository { get; }

        private RepositoryManager()
        {
            CategoryRepository  = new CategoryRepository();
            ItemRepository = new ItemRepository();
            DiscountRepository = new DiscountRepository();
            ReceiptRepository = new ReceiptRepository();
            ReceiptLineRepository = new ReceiptLineRepository();
            UserRepository = new UserRepository();
        }
    }
}
