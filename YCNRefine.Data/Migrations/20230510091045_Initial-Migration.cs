using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YCNRefine.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dataset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dataset__3214EC07087AB91B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatasetId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chat__3214EC077D6070AF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dataset_Chat",
                        column: x => x.DatasetId,
                        principalTable: "Dataset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GenerativeSample",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatasetId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Generati__3214EC07F257DC9E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dataset_GenerativeSample",
                        column: x => x.DatasetId,
                        principalTable: "Dataset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OriginalSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatasetId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Original__3214EC073881AC6A", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dataset_OriginalSource",
                        column: x => x.DatasetId,
                        principalTable: "Dataset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Message__3214EC07B7DF2459", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_Message",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalSourceId = table.Column<int>(type: "int", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__3214EC07A87916FC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalSource_QuestionAnswer",
                        column: x => x.OriginalSourceId,
                        principalTable: "OriginalSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_DatasetId",
                table: "Chat",
                column: "DatasetId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerativeSample_DatasetId",
                table: "GenerativeSample",
                column: "DatasetId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalSource_DatasetId",
                table: "OriginalSource",
                column: "DatasetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_OriginalSourceId",
                table: "QuestionAnswer",
                column: "OriginalSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerativeSample");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "OriginalSource");

            migrationBuilder.DropTable(
                name: "Dataset");
        }
    }
}
