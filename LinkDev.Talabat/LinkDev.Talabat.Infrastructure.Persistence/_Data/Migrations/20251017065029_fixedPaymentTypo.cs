using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedPaymentTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayementIntentId",
                table: "Orders",
                newName: "PaymentIntentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "Orders",
                newName: "PayementIntentId");
        }
    }
}
