using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prost.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261114035205_CreatePasswordCredentialsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PasswordCredentials",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PasswordCredentials", x => x.Id);
                table.ForeignKey(
                    name: "FK_PasswordCredentials_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PasswordCredentials_AccountId",
            table: "PasswordCredentials",
            column: "AccountId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PasswordCredentials");
    }
}
