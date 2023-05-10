namespace YCNRefine.Core.Entities;

public partial class Message
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsSystem { get; set; }

    public int ChatId { get; set; }

    public virtual Chat Chat { get; set; } = null!;
}
