namespace YCNRefine.Core.Entities;

public partial class OriginalSource
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int DatasetId { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserIdentifier { get; set; }    

    public virtual Dataset Dataset { get; set; } = null!;

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();
}
