using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prost.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatePublishedPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublishedPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOrPublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, computedColumnSql: "COALESCE(\"UpdatedAt\", \"PublishedAt\")", stored: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    Body = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    Slug = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedPosts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublishedPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPosts_AccountId",
                table: "PublishedPosts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPosts_PostId",
                table: "PublishedPosts",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPosts_UpdatedOrPublishedAt_Id",
                table: "PublishedPosts",
                columns: new[] { "UpdatedOrPublishedAt", "Id" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPosts_Slug",
                table: "PublishedPosts",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublishedPosts");
        }
    }
}
