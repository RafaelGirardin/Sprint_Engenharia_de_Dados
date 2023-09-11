using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scraping_TJSP.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Relator",
                columns: table => new
                {
                    NomeRelatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relator", x => x.NomeRelatorId);
                });

            migrationBuilder.CreateTable(
                name: "Decisao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ementa = table.Column<string>(type: "varchar(7000)", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomeRelatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comarca = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisao", x => x.Id);
                    table.UniqueConstraint("Ementa", x => x.Ementa);
                    table.ForeignKey("NomeRelatorId", x => x.NomeRelatorId, "Relator");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Decisao");

            migrationBuilder.DropTable(
                name: "Relator");
        }
    }
}
