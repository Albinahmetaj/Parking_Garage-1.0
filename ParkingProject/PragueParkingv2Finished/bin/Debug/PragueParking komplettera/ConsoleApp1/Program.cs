using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Transaction
    {
        public decimal Amount { get; }
        public DateTime Date {get;}
        public string Notes { get; }
        public Transaction(decimal amount, DateTime date, string note)
        {
            this.Amount = amount;
            this.Date = date;
            this.Notes = note;
        }


    }
    public class BankAccount
    {
        public string Number { get; }
        public string Owner { get; set; }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var item in allTransactions)
                {

                    balance += item.Amount;
                }
                return balance;
            }
        }
        private static int accountNumberSeed = 1234567890;

        private List<Transaction> allTransactions = new List<Transaction>();

        public BankAccount(string name, decimal initialBalance)
        {
            
            this.Owner = name;
            
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;

        }
        public void MakeDeposit(decimal amout, DateTime date, string note)
        {

        }
        public void MakeWithdrawal(decimal amout, DateTime date, string note)
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var account = new BankAccount("Albin", 100000);
            Console.WriteLine($"Account {account.Number} was created for {account.Owner} with {account.Balance}.");
        }
    }
}
