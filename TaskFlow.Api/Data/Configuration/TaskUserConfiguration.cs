using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Data.Configuration;

public class TaskUserConfiguration : IEntityTypeConfiguration<TaskUser>
{
    public void Configure(EntityTypeBuilder<TaskUser> builder)
    {
        builder.ToTable("TasksUsers");

        builder.HasKey(x => new
        {
            x.TaskId,
            x.UserId
        });

        builder.HasOne(x => x.Task)
            .WithMany(x => x.TaskUsers)
            .HasForeignKey(x => x.TaskId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.TaskUsers)
            .HasForeignKey(x => x.UserId);
    }
}
