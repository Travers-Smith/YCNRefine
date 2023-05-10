namespace YCNRefine.Core.Entities;

public partial class QuestionAnswer
{
    public int Id { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public int OriginalSourceId { get; set; }

    public Guid UserIdentifier { get; set; }

    public bool CorrectAnswer { get; set; }

    public virtual OriginalSource OriginalSource { get; set; } = null!;
}
