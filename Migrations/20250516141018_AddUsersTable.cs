using Microsoft.EntityFrameworkCore.Migrations;

namespace SupermarketWEB.Migrations
{
    public partial class AddUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users", // Nombre de la tabla en la base de datos
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Autoincremental para SQL Server. Ajusta según tu base de datos.
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    // Puedes agregar la propiedad Salt aquí si deseas que sea una columna en la tabla
                    // Salt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id); // Define 'Id' como clave primaria
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users"); // Esto elimina la tabla si se revierte la migración
        }
    }
}