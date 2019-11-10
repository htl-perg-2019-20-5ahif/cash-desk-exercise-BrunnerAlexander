using CashDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk
{
    /// <inheritdoc />
    public class DataAccess : IDataAccess
    {
        private CashDeskDataContext dataContext;

        /// <inheritdoc />
        public Task InitializeDatabaseAsync()
        {
            if (dataContext != null)
            {
                throw new InvalidOperationException("Context already initialized");
            }
            dataContext = new CashDeskDataContext();
            return Task.CompletedTask;
            
        }

        /// <inheritdoc />
        public async Task<int> AddMemberAsync(string firstName, string lastName, DateTime birthday)
        {
            if (String.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("First name cannot be null");
            }
            if (String.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Last name cannot be null");
            }
            if(birthday == null)
            {
                throw new ArgumentException("Birthday cannot be null");
            }

            Member m = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Birthday = birthday
            };

            if(dataContext.Member.Any(m => m.LastName.Equals(lastName)))
            {
                throw new DuplicateNameException("LastName is already in use");
            }

            dataContext.Add(m);

            await dataContext.SaveChangesAsync();

            return m.MemberNumber;
        }

        /// <inheritdoc />
        public async Task DeleteMemberAsync(int memberNumber)
        {
            Member m = dataContext.Member.FirstOrDefault(m => m.MemberNumber == memberNumber);

            if(m == null)
            {
                throw new ArgumentException("The provided number does not exist");
            }

            dataContext.Member.Remove(m);

            await dataContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IMembership> JoinMemberAsync(int memberNumber)
        {
            Member m = dataContext.Member.FirstOrDefault(m => m.MemberNumber == memberNumber);

            if (m == null)
            {
                throw new ArgumentException("The provided number does not exist");
            }

            if (dataContext.Membership.Any(m => m.Member.MemberNumber == memberNumber && DateTime.Now < m.End))
            {
                throw new AlreadyMemberException("This person is already a member");
            }

            Membership ms = new Membership
            {
                Member = m,
                Begin = DateTime.Now,
                End = DateTime.MaxValue
            };
            dataContext.Membership.Add(ms);
            await dataContext.SaveChangesAsync();

            return ms;
        }

        /// <inheritdoc />
        public async Task<IMembership> CancelMembershipAsync(int memberNumber)
        {
            Member m = dataContext.Member.FirstOrDefault(m => m.MemberNumber == memberNumber);

            if (m == null)
            {
                throw new ArgumentException("The provided number does not exist");
            }

            Membership ms = dataContext.Membership.FirstOrDefault(ms => ms.Member.MemberNumber == memberNumber && DateTime.Now < ms.End);
            if(ms == null)
            {
                throw new NoMemberException();
            }

            ms.End = DateTime.Now;
            await dataContext.SaveChangesAsync();
            return ms;

        }

        /// <inheritdoc />
        public async Task DepositAsync(int memberNumber, decimal amount)
        {
            Member m = dataContext.Member.FirstOrDefault(m => m.MemberNumber == memberNumber);

            if (m == null || amount <= 0)
            {
                throw new ArgumentException();
            }

            Membership ms = dataContext.Membership.FirstOrDefault(ms => ms.Member.MemberNumber == memberNumber && DateTime.Now < ms.End);
            if(ms == null)
            {
                throw new NoMemberException();
            }

            Deposit d = new Deposit
            {
                Membership = ms,
                Amount = amount
            };
            dataContext.Deposit.Add(d);
            await dataContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task<IEnumerable<IDepositStatistics>> GetDepositStatisticsAsync() 
            => throw new NotImplementedException();

        /// <inheritdoc />
        public void Dispose()
        {
            if(dataContext != null)
            {
                dataContext.Dispose();
                dataContext = null;
            }
        }
    }
}
