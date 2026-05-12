using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _3d_pasatiempos_backend.Migrations
{
    /// <inheritdoc />
    public partial class SyncDbChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("customer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    price_per_gram = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("material_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "printer",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    power_consumption_kwh = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    power_consumption_wh = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    cost_per_minute = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    useful_life_hours = table.Column<int>(type: "integer", nullable: false),
                    fail_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("printer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("project_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_customer",
                        column: x => x.customer_id,
                        principalSchema: "app",
                        principalTable: "customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quote",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    reject_reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("quote_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_quote_customer",
                        column: x => x.customer_id,
                        principalSchema: "app",
                        principalTable: "customer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quote_project",
                        column: x => x.project_id,
                        principalSchema: "app",
                        principalTable: "project",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quote_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_quote",
                        column: x => x.quote_id,
                        principalSchema: "app",
                        principalTable: "quote",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quote_item",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quote_id = table.Column<int>(type: "integer", nullable: false),
                    product_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    estimated_grams = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    estimated_hours = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    calculated_price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    price_per_gram_used = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    cost_per_kwh_used = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    machine_wear_cost_used = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    cost_overrun_failure = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValue: 0m),
                    profit_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("quote_item_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_quote_item_quote",
                        column: x => x.quote_id,
                        principalSchema: "app",
                        principalTable: "quote",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expense",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("expense_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_expense_order",
                        column: x => x.order_id,
                        principalSchema: "app",
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payment",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_order",
                        column: x => x.order_id,
                        principalSchema: "app",
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "production",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    grams_used = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    estimated_end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    time_used = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("production_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_production_order",
                        column: x => x.order_id,
                        principalSchema: "app",
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "idx_customer_name",
                schema: "app",
                table: "customer",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_expense_order_id",
                schema: "app",
                table: "expense",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "idx_orders_quote_id",
                schema: "app",
                table: "orders",
                column: "quote_id");

            migrationBuilder.CreateIndex(
                name: "orders_quote_id_key",
                schema: "app",
                table: "orders",
                column: "quote_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_order_id",
                schema: "app",
                table: "payment",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_production_order_id",
                schema: "app",
                table: "production",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "idx_project_customer_id",
                schema: "app",
                table: "project",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "idx_quote_customer_id",
                schema: "app",
                table: "quote",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "idx_quote_project_id",
                schema: "app",
                table: "quote",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "idx_quote_item_quote_id",
                schema: "app",
                table: "quote_item",
                column: "quote_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense",
                schema: "app");

            migrationBuilder.DropTable(
                name: "material",
                schema: "app");

            migrationBuilder.DropTable(
                name: "payment",
                schema: "app");

            migrationBuilder.DropTable(
                name: "printer",
                schema: "app");

            migrationBuilder.DropTable(
                name: "production",
                schema: "app");

            migrationBuilder.DropTable(
                name: "quote_item",
                schema: "app");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "quote",
                schema: "app");

            migrationBuilder.DropTable(
                name: "project",
                schema: "app");

            migrationBuilder.DropTable(
                name: "customer",
                schema: "app");
        }
    }
}
