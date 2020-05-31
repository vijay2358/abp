﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public class EfCoreDatabaseBlobRepository : EfCoreRepository<IBlobStoringDbContext, DatabaseBlob, Guid>, IDatabaseBlobRepository
    {
        public EfCoreDatabaseBlobRepository(IDbContextProvider<IBlobStoringDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<DatabaseBlob> FindAsync(
            Guid containerId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(
                x => x.ContainerId == containerId && x.Name == name,
                GetCancellationToken(cancellationToken)
            );
        }

        public virtual async Task<bool> ExistsAsync(
            Guid containerId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(
                x => x.ContainerId == containerId && x.Name == name,
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> DeleteAsync(
            Guid containerId,
            string name,
            CancellationToken cancellationToken = default)
        {
            var blob = await FindAsync(containerId, name, cancellationToken);
            if (blob == null)
            {
                return false;
            }

            await base.DeleteAsync(blob.Id, cancellationToken: GetCancellationToken(cancellationToken));
            return true;
        }
    }
}