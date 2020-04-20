namespace Intranet.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entradas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Contenido = c.String(nullable: false),
                        UsuarioCreacion = c.String(nullable: false, maxLength: 50),
                        FechaCreacion = c.DateTime(nullable: false),
                        UsuarioModificacion = c.String(maxLength: 50),
                        FechaUltimaModificacion = c.DateTime(),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Etiquetas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        HexColor = c.String(),
                        UsuarioCreacion = c.String(nullable: false, maxLength: 50),
                        FechaCreacion = c.DateTime(nullable: false),
                        UsuarioModificacion = c.String(maxLength: 50),
                        FechaUltimaModificacion = c.DateTime(),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.EntradaEtiquetas",
                c => new
                    {
                        Entrada_Id = c.Guid(nullable: false),
                        Etiqueta_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Entrada_Id, t.Etiqueta_Id })
                .ForeignKey("dbo.Entradas", t => t.Entrada_Id, cascadeDelete: true)
                .ForeignKey("dbo.Etiquetas", t => t.Etiqueta_Id, cascadeDelete: true)
                .Index(t => t.Entrada_Id)
                .Index(t => t.Etiqueta_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntradaEtiquetas", "Etiqueta_Id", "dbo.Etiquetas");
            DropForeignKey("dbo.EntradaEtiquetas", "Entrada_Id", "dbo.Entradas");
            DropIndex("dbo.EntradaEtiquetas", new[] { "Etiqueta_Id" });
            DropIndex("dbo.EntradaEtiquetas", new[] { "Entrada_Id" });
            DropIndex("dbo.Etiquetas", new[] { "Nombre" });
            DropTable("dbo.EntradaEtiquetas");
            DropTable("dbo.Etiquetas");
            DropTable("dbo.Entradas");
        }
    }
}
