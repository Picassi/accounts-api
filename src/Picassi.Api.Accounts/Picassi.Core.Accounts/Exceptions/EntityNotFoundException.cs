using System;

namespace Picassi.Core.Accounts.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public int Id { get; }

        public EntityNotFoundException(int id)
        {
            Id = id;
        }
    }
}
