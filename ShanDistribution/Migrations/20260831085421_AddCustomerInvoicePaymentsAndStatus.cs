using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShanDistribution.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerInvoicePaymentsAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "CustomerInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "CustomerInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CustomerInvoices",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CustomerInvoicePayments",
                columns: table => new
                {
                    InvoicePaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIId = table.Column<int>(type: "int", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoicePayments", x => x.InvoicePaymentId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoicePayments_CustomerInvoices_CIId",
                        column: x => x.CIId,
                        principalTable: "CustomerInvoices",
                        principalColumn: "CIId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoicePayments_CIId",
                table: "CustomerInvoicePayments",
                column: "CIId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInvoicePayments");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CustomerInvoices");
        }
    }
}
