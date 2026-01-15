using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.TaskService.Domain.TodoItems;
using TaskFlow.TaskService.Domain.TodoItems.Constraints;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Configurations;

internal sealed class TaskConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("tasks");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired()
            .HasConversion(
                taskId => taskId.Value,
                value => TodoId.FromGuid(value).Value);

        builder.Property(x => x.ProjectId)
            .HasColumnName("project_id")
            .IsRequired()
            .HasConversion(
                projectId => projectId.Value,
                value => TodoProjectId.FromGuid(value).Value)
            .IsRequired();

        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id")
            .IsRequired()
            .HasConversion(
                authorId => authorId.Value,
                value => TodoAuthorId.FromGuid(value).Value);
        
        builder.Property(x => x.PublicId)
            .HasColumnName("public_id")
            .IsRequired()
            .HasConversion(
                publicId => publicId.Value,
                value => TodoPublicId.FromString(value).Value);
        
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(TodoTitleConstraints.MaxLength)
            .HasConversion(
                title => title.Value, 
                value => TodoTitle.Create(value).Value);

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(TodoDescriptionConstraints.MaxLength)
            .HasConversion(
                description => description.Value, 
                value => TodoDescription.Create(value).Value);

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.DueDate)
            .HasColumnName("due_date");
        
        builder.Property(x => x.EstimatedCompletionTime)
            .HasColumnName("estimated_completion_time");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.ChangedAt)
            .HasColumnName("changed_at");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("status_idx");

        builder.HasIndex(x => x.Priority)
            .HasDatabaseName("priority_idx");
    }
}