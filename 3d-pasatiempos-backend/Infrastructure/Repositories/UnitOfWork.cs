using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;
        private IDbContextTransaction? _Transaction;

        public UnitOfWork(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task BeginTransactionAsync()
        {
            _Transaction = await _Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_Transaction != null)
                await _Transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_Transaction != null)
                await _Transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
