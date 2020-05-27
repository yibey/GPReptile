using Microsoft.EntityFrameworkCore.Migrations;

namespace GPReptile.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayTransact",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    day = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    tclose = table.Column<double>(nullable: false),
                    high = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false),
                    topen = table.Column<double>(nullable: false),
                    lclose = table.Column<double>(nullable: false),
                    chg = table.Column<double>(nullable: false),
                    pchg = table.Column<double>(nullable: false),
                    turnover = table.Column<double>(nullable: false),
                    voturnover = table.Column<long>(nullable: false),
                    vaturnover = table.Column<double>(nullable: false),
                    tcap = table.Column<double>(nullable: false),
                    mcap = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTransact", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayTransact");
        }
    }
}
