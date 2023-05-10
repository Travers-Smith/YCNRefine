namespace YCNRefine.Core.Entities;

public partial class Dataset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CreatedByUser { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<GenerativeSample> GenerativeSamples { get; set; } = new List<GenerativeSample>();

    public virtual ICollection<OriginalSource> OriginalSources { get; set; } = new List<OriginalSource>();
}
