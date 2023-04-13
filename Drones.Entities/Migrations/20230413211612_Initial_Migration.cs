using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drones.Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    WeightLimit = table.Column<float>(type: "real", nullable: false),
                    BatteryCapacity = table.Column<float>(type: "real", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DroneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Drones_DroneId",
                        column: x => x.DroneId,
                        principalTable: "Drones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DroneId",
                table: "Medications",
                column: "DroneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Drones");
        }
    }
}
