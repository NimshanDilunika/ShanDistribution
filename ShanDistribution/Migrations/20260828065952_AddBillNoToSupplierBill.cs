using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShanDistribution.Migrations
{
    /// <inheritdoc />
    public partial class AddBillNoToSupplierBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillNo",
                table: "SupplierBills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNo",
                table: "SupplierBills");
        }
    }
}
