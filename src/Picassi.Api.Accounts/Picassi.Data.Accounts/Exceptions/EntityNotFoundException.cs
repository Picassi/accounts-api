using System;

namespace Picassi.Data.Accounts.Exceptions
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
