using Microsoft.EntityFrameworkCore;
using YCNRefine.Core.Entities;

namespace YCNRefine.Data;

public partial class YcnrefineContext : DbContext
{
    public YcnrefineContext()
    {
    }

    public YcnrefineContext(DbContextOptions<YcnrefineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Dataset> Datasets { get; set; }

    public virtual DbSet<GenerativeSample> GenerativeSamples { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<OriginalSource> OriginalSources { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC077D6070AF");

            entity.ToTable("Chat");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Dataset).WithMany(p => p.Chats)
                .HasForeignKey(d => d.DatasetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dataset_Chat");
        });

        modelBuilder.Entity<Dataset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dataset__3214EC07087AB91B");

            entity.ToTable("Dataset");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<GenerativeSample>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Generati__3214EC07F257DC9E");

            entity.ToTable("GenerativeSample");

            entity.HasOne(d => d.Dataset).WithMany(p => p.GenerativeSamples)
                .HasForeignKey(d => d.DatasetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dataset_GenerativeSample");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC07B7DF2459");

            entity.ToTable("Message");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_Message");
        });

        modelBuilder.Entity<OriginalSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Original__3214EC073881AC6A");

            entity.ToTable("OriginalSource");

            entity.HasOne(d => d.Dataset).WithMany(p => p.OriginalSources)
                .HasForeignKey(d => d.DatasetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dataset_OriginalSource");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07A87916FC");

            entity.ToTable("QuestionAnswer");

            entity.HasOne(d => d.OriginalSource).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.OriginalSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OriginalSource_QuestionAnswer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
