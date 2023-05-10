namespace YCNRefine.Core.Entities;

public partial class Chat
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserIdentifier { get; set; }

    public int DatasetId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Dataset Dataset { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
