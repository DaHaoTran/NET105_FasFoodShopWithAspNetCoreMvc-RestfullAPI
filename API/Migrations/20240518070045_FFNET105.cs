using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodShop.Migrations
{
    /// <inheritdoc />
    public partial class FFNET105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    AdminCode = table.Column<string>(type: "Char(5)", nullable: false),
                    Email = table.Column<string>(type: "Varchar(200)", nullable: false),
                    Password = table.Column<string>(type: "Varchar(100)", nullable: false),
                    IsOnl = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.AdminCode);
                });

            migrationBuilder.CreateTable(
                name: "foodCategory",
                columns: table => new
                {
                    FCategoryCode = table.Column<string>(type: "Char(4)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<string>(type: "Varchar(Max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foodCategory", x => x.FCategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "foodType",
                columns: table => new
                {
                    FTypeCode = table.Column<string>(type: "Char(4)", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foodType", x => x.FTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    Email = table.Column<string>(type: "Varchar(200)", nullable: false),
                    PassWord = table.Column<string>(type: "Varchar(100)", nullable: false),
                    UserName = table.Column<string>(type: "Varchar(300)", nullable: false),
                    AdminCode = table.Column<string>(type: "Char(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.Email);
                    table.ForeignKey(
                        name: "FK_customer_admins_AdminCode",
                        column: x => x.AdminCode,
                        principalTable: "admins",
                        principalColumn: "AdminCode");
                });

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    FoodCode = table.Column<string>(type: "Char(5)", nullable: false),
                    FoodName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CurrentPrice = table.Column<int>(type: "int", nullable: false),
                    PreviousPrice = table.Column<int>(type: "int", nullable: false),
                    Left = table.Column<int>(type: "int", nullable: false),
                    Sold = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "Varchar(Max)", nullable: false),
                    FTypeCode = table.Column<string>(type: "Char(4)", nullable: false),
                    FCategoryCode = table.Column<string>(type: "Char(4)", nullable: false),
                    AdminCode = table.Column<string>(type: "Char(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foods", x => x.FoodCode);
                    table.ForeignKey(
                        name: "FK_foods_admins_AdminCode",
                        column: x => x.AdminCode,
                        principalTable: "admins",
                        principalColumn: "AdminCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_foods_foodCategory_FCategoryCode",
                        column: x => x.FCategoryCode,
                        principalTable: "foodCategory",
                        principalColumn: "FCategoryCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_foods_foodType_FTypeCode",
                        column: x => x.FTypeCode,
                        principalTable: "foodType",
                        principalColumn: "FTypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerEmail = table.Column<string>(type: "Varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_cart_customer_CustomerEmail",
                        column: x => x.CustomerEmail,
                        principalTable: "customer",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customerInformation",
                columns: table => new
                {
                    CInforId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "Char(10)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "Varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerInformation", x => x.CInforId);
                    table.ForeignKey(
                        name: "FK_customerInformation_customer_CustomerEmail",
                        column: x => x.CustomerEmail,
                        principalTable: "customer",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "Nvarchar(Max)", nullable: true),
                    CInforId = table.Column<int>(type: "int", nullable: false),
                    CustomerEmail = table.Column<string>(type: "Varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_order_customer_CustomerEmail",
                        column: x => x.CustomerEmail,
                        principalTable: "customer",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    RateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    FoodCode = table.Column<string>(type: "Char(5)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "Varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating", x => x.RateId);
                    table.ForeignKey(
                        name: "FK_rating_customer_CustomerEmail",
                        column: x => x.CustomerEmail,
                        principalTable: "customer",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rating_foods_FoodCode",
                        column: x => x.FoodCode,
                        principalTable: "foods",
                        principalColumn: "FoodCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cartItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    FoodCode = table.Column<string>(type: "Char(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItem", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_cartItem_cart_CartId",
                        column: x => x.CartId,
                        principalTable: "cart",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartItem_foods_FoodCode",
                        column: x => x.FoodCode,
                        principalTable: "foods",
                        principalColumn: "FoodCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    FoodCode = table.Column<string>(type: "Char(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItem", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_orderItem_foods_FoodCode",
                        column: x => x.FoodCode,
                        principalTable: "foods",
                        principalColumn: "FoodCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderItem_order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_CustomerEmail",
                table: "cart",
                column: "CustomerEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cartItem_CartId",
                table: "cartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItem_FoodCode",
                table: "cartItem",
                column: "FoodCode");

            migrationBuilder.CreateIndex(
                name: "IX_customer_AdminCode",
                table: "customer",
                column: "AdminCode");

            migrationBuilder.CreateIndex(
                name: "IX_customerInformation_CustomerEmail",
                table: "customerInformation",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_foods_AdminCode",
                table: "foods",
                column: "AdminCode");

            migrationBuilder.CreateIndex(
                name: "IX_foods_FCategoryCode",
                table: "foods",
                column: "FCategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_foods_FTypeCode",
                table: "foods",
                column: "FTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_order_CustomerEmail",
                table: "order",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_FoodCode",
                table: "orderItem",
                column: "FoodCode");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_OrderId",
                table: "orderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_rating_CustomerEmail",
                table: "rating",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_rating_FoodCode",
                table: "rating",
                column: "FoodCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartItem");

            migrationBuilder.DropTable(
                name: "customerInformation");

            migrationBuilder.DropTable(
                name: "orderItem");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "foods");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "foodCategory");

            migrationBuilder.DropTable(
                name: "foodType");

            migrationBuilder.DropTable(
                name: "admins");
        }
    }
}
