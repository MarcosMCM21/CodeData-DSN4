using CodeData_Connection.Areas.Identity.User;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using System.Diagnostics.Contracts;
using System.Reflection.Emit;

namespace CodeData_Connection.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    // Definir DbSets para as classes
    public DbSet<ApplicationUser> ApplicationUser {  get; set; }
    public DbSet<Equipamento> Equipamentos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<MovimentacaoEquipamento> MovimentacoesEquipamentos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<Solicitacao> Solicitacoes { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Contrato> Contratos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Configurar relações (Foreign Keys)
        builder.Entity<Equipamento>()
            .HasOne(e => e.Estoque)
            .WithMany()
            .HasForeignKey(e => e.EstoqueId);

        builder.Entity<Equipamento>()
            .HasOne(e => e.Documento)
                .WithMany()
                .HasForeignKey(e => e.DocumentoId);

        builder.Entity<Estoque>()
            .HasOne(e => e.Endereco)
            .WithMany()
            .HasForeignKey(e => e.EnderecoId);

        builder.Entity<MovimentacaoEquipamento>()
           .HasOne(me => me.Equipamento)
           .WithMany()
           .HasForeignKey(me => me.EquipamentoId);

        builder.Entity<MovimentacaoEquipamento>()
            .HasOne(me => me.Endereco)
            .WithMany()
            .HasForeignKey(me => me.EnderecoId);

        builder.Entity<MovimentacaoEquipamento>()
            .HasOne(me => me.Solicitacao)
            .WithMany()
            .HasForeignKey(me => me.SolicitacaoId);

        builder.Entity<MovimentacaoEquipamento>()
            .HasOne(me => me.Documento)
            .WithMany()
            .HasForeignKey(me => me.DocumentoId);

        builder.Entity<Solicitacao>(entity =>
        {
            entity.Property(s => s.Tipo).HasDefaultValue(0);

            entity
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);
        });

        builder.Entity<Solicitacao>()
            .HasOne(s => s.Cliente)
            .WithMany()
            .HasForeignKey(s => s.ClienteId);

        builder.Entity<Contrato>()
            .HasOne(c => c.Solicitacao)
            .WithMany()
            .HasForeignKey(c => c.SolicitacaoId);

        builder.Entity<Contrato>()
            .HasOne(c => c.Documento)
            .WithMany()
            .HasForeignKey(c => c.DocumentoId);


        builder.Entity<Notificacao>(entity =>
        {
            entity.Property(e => e.Visualizado).HasDefaultValue(0);
            entity.Property(e => e.DataHora).HasDefaultValueSql("NOW()");

            entity
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserID);
        });

        builder.Entity<ApplicationUser>()
            .HasOne(au => au.User)
            .WithMany()
            .HasForeignKey(au => au.GerenteID);
            
    }
}