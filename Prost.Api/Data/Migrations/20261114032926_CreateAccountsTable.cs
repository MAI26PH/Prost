using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prost.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261114032926_CreateAccountsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Email",
            table: "Accounts",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Accounts");
    }
}
