using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhilippWebsite.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastCommitSha = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    IsDirectory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileNodes_FileNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FileNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileNodes_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileContents",
                columns: table => new
                {
                    NodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContents", x => x.NodeId);
                    table.ForeignKey(
                        name: "FK_FileContents_FileNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "FileNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileNodes_ParentId",
                table: "FileNodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileNodes_RepositoryId",
                table: "FileNodes",
                column: "RepositoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileContents");

            migrationBuilder.DropTable(
                name: "FileNodes");

            migrationBuilder.DropTable(
                name: "Repositories");
        }
    }
}
