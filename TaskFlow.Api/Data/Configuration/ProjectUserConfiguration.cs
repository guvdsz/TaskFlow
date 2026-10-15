using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Data.Configuration;

public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.ToTable("ProjectsUsers");

        builder.HasKey(x => new
        {
            x.ProjectId,
            x.UserId
        });

        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProjectUsers)
            .HasForeignKey(x => x.ProjectId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ProjectUsers)
            .HasForeignKey(x => x.UserId);
    }
}
