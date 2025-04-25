using System;
using CoreSuit.Domain.Entities;
using CoreSuit.Infrastructure.TablesDefinition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSuit.Infrastructure.EntitiesConfiguration;

public class ExampleEntityConfiguration : IEntityTypeConfiguration<ExampleEntity>
{
    public void Configure(EntityTypeBuilder<ExampleEntity> builder)
    {
        TableInfo tableInfoExample = Tables.ExampleTable;

        builder.ToTable(tableInfoExample.Name, b => b.HasComment(tableInfoExample.Description));

        builder.HasKey(v => v.Id);

        builder.Property(d => d.Name).HasComment("Nome completo exemplo");
        builder.Property(d => d.Description).HasComment("Descrição completo exemplo");

    }
}
