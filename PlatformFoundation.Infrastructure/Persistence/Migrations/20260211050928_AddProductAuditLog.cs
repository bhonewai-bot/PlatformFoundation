using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformFoundation.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_audit_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_audit_logs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_audit_logs_ProductId",
                table: "product_audit_logs",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_audit_logs");
        }
    }
}
