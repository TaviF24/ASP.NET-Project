namespace Project.Models.Base
{
	public interface IBaseEntity
	{
		public Guid Id { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}

