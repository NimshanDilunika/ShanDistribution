using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShanDistribution.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierBillPaymentsAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "SupplierBills",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "SupplierBills",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SupplierBills",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SupplierBillPayments",
                columns: table => new
                {
                    BillPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SBId = table.Column<int>(type: "int", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBillPayments", x => x.BillPaymentId);
                    table.ForeignKey(
                        name: "FK_SupplierBillPayments_SupplierBills_SBId",
                        column: x => x.SBId,
                        principalTable: "SupplierBills",
                        principalColumn: "SBId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierBillPayments_SBId",
                table: "SupplierBillPayments",
                column: "SBId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierBillPayments");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "SupplierBills");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "SupplierBills");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupplierBills");
        }
    }
}
