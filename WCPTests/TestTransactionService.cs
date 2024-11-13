//using Microsoft.EntityFrameworkCore;
//using WCPShared.Interfaces;
//using WCPShared.Models;
//using WCPShared.Models.DTOs;
//using WCPShared.Models.Entities;
//using WCPShared.Models.Entities.Financial;
//using WCPShared.Models.Entities.UserModels;
//using WCPShared.Models.Enums;
//using WCPShared.Services;
//using WCPShared.Services.Converters;
//using WCPShared.Services.EntityFramework;

//namespace WCPTests
//{
//    [TestClass]
//    public class TestTransactionService
//    {
//        private TransactionService _transactionService;

//        [TestInitialize]
//        public async Task Init()
//        {
//            // Set up new DbContextOptions with an InMemory database.
//            var options = new DbContextOptionsBuilder<TestDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a new in-memory database for each test
//                .Options;

//            IWcpDbContext context = new TestDbContext(options);
//            _transactionService = new TransactionService(context);
//            context.Balances.Add(new Balance()
//            {
//                Available = 0,
//                Pending = 0,
//                Underway = 0,
//                LastUpdated = DateTime.MinValue,
//                AccountId = Guid.NewGuid().ToString(),
//            });
//            await context.SaveChangesAsync();
//        }
        
//        [TestMethod]
//        public async Task TestGetBalance()
//        {
//            Balance? balance = await _transactionService.GetBalance(1);
//            Assert.IsNotNull(balance);
//            Assert.AreEqual(0, balance.Available);
//        }

//        [TestMethod]
//        public async Task TestAddToBalance()
//        {
//            Transaction transaction = await _transactionService.AddToBalance(1, 500, "Projektløn", TransactionType.IncomingTransfer);
//            Assert.IsNotNull(transaction);
//            Assert.AreEqual(500, transaction.Amount);

//            Balance? balance = await _transactionService.GetBalance(1);
//            Assert.IsNotNull(balance);
//            Assert.AreEqual(500, balance.Available);

//            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _transactionService.AddToBalance(2, 100, "Løn", TransactionType.IncomingTransfer));
            
//            // Limit testing
//            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _transactionService.AddToBalance(1, 0, "Løn", TransactionType.IncomingTransfer));
//            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _transactionService.AddToBalance(1, -1, "Løn", TransactionType.IncomingTransfer));
//            await _transactionService.AddToBalance(1, 1, "Løn", TransactionType.IncomingTransfer);
//        }

//        [TestMethod]
//        public async Task TestTakeFromBalance()
//        {
//            await _transactionService.AddToBalance(1, 500, "Projektløn", TransactionType.IncomingTransfer);

//            Balance? balance = await _transactionService.GetBalance(1);
//            Assert.IsNotNull(balance);
//            Assert.AreEqual(500, balance.Available);

//            await _transactionService.TakeFromBalance(1, 100, "Udbetaling", TransactionType.Payout);
//            Assert.AreEqual(400, balance.Available);

//            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _transactionService.TakeFromBalance(2, 100, "Løn", TransactionType.IncomingTransfer));
//            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _transactionService.TakeFromBalance(1, 999, "Udbetaling", TransactionType.Payout));

//            // Limit testing
//            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _transactionService.TakeFromBalance(1, 0, "Udbetaling", TransactionType.Payout));
//            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _transactionService.TakeFromBalance(1, -1, "Udbetaling", TransactionType.Payout));
//            await _transactionService.TakeFromBalance(1, 1, "Udbetaling", TransactionType.Payout);
//        }
//    }
//}
