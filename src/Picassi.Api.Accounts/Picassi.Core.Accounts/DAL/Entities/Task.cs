namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Task : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}