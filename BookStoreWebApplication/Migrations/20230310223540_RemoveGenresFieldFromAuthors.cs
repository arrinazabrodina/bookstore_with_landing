using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGenresFieldFromAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Birth_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Short_Biography = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Publication_Year = table.Column<short>(type: "smallint", nullable: false),
                    Genre = table.Column<string>(type: "ntext", nullable: true),
                    Cover_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookstores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookstores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buyer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone_Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "ntext", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Birth_Date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorsBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Author_Id = table.Column<int>(type: "int", nullable: false),
                    Book_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorsBooks_Authors",
                        column: x => x.Author_Id,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthorsBooks_Books",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Book_Id = table.Column<int>(type: "int", nullable: false),
                    Bookstore_Id = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Availability_Books",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Availability_Bookstores",
                        column: x => x.Bookstore_Id,
                        principalTable: "Bookstores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birth_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Address = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Bookstore_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Bookstores",
                        column: x => x.Bookstore_Id,
                        principalTable: "Bookstores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuthorsGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Author_Id = table.Column<int>(type: "int", nullable: false),
                    Genre_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorsGenres_Authors",
                        column: x => x.Author_Id,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthorsGenres_Genres",
                        column: x => x.Genre_Id,
                        principalTable: "Genres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BooksGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Book_Id = table.Column<int>(type: "int", nullable: false),
                    Genre_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksGenres_Books",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BooksGenres_Genres",
                        column: x => x.Genre_Id,
                        principalTable: "Genres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Buyer_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Seller_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buys_Buyer",
                        column: x => x.Buyer_Id,
                        principalTable: "Buyer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Buys_Workers",
                        column: x => x.Seller_Id,
                        principalTable: "Workers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Book_Id = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Buy_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Books",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Buys",
                        column: x => x.Buy_Id,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsBooks_Author_Id",
                table: "AuthorsBooks",
                column: "Author_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsBooks_Book_Id",
                table: "AuthorsBooks",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsGenres_Author_Id",
                table: "AuthorsGenres",
                column: "Author_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsGenres_Genre_Id",
                table: "AuthorsGenres",
                column: "Genre_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Availability_Book_Id",
                table: "Availability",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Availability_Bookstore_Id",
                table: "Availability",
                column: "Bookstore_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BooksGenres_Book_Id",
                table: "BooksGenres",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BooksGenres_Genre_Id",
                table: "BooksGenres",
                column: "Genre_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Book_Id",
                table: "OrderItems",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Buy_Id",
                table: "OrderItems",
                column: "Buy_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Buyer_Id",
                table: "Orders",
                column: "Buyer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Seller_Id",
                table: "Orders",
                column: "Seller_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Bookstore_Id",
                table: "Workers",
                column: "Bookstore_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorsBooks");

            migrationBuilder.DropTable(
                name: "AuthorsGenres");

            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropTable(
                name: "BooksGenres");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Buyer");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Bookstores");
        }
    }
}
