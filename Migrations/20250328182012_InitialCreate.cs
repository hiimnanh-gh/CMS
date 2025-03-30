using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cast_member",
                columns: table => new
                {
                    cast_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cast_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cast_member", x => x.cast_id);
                });

            migrationBuilder.CreateTable(
                name: "cinema",
                columns: table => new
                {
                    cinema_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cinema_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    cinema_address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cinema", x => x.cinema_id);
                });

            migrationBuilder.CreateTable(
                name: "director",
                columns: table => new
                {
                    director_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    director_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_director", x => x.director_id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "movie_status",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_status", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "seat_type",
                columns: table => new
                {
                    seat_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seat_type", x => x.seat_type_id);
                });

            migrationBuilder.CreateTable(
                name: "theatre",
                columns: table => new
                {
                    theatre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cinema_id = table.Column<int>(type: "int", nullable: false),
                    theatre_num = table.Column<int>(type: "int", nullable: false),
                    available_seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theatre", x => x.theatre_id);
                    table.ForeignKey(
                        name: "FK_theatre_cinema_cinema_id",
                        column: x => x.cinema_id,
                        principalTable: "cinema",
                        principalColumn: "cinema_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    director_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    age_rating = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    runtime_min = table.Column<int>(type: "int", nullable: true),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true),
                    trailer_link = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    banner_text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    poster_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    synopsis = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.movie_id);
                    table.ForeignKey(
                        name: "FK_movies_director_director_id",
                        column: x => x.director_id,
                        principalTable: "director",
                        principalColumn: "director_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movies_movie_status_status_id",
                        column: x => x.status_id,
                        principalTable: "movie_status",
                        principalColumn: "status_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    phone_num = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seat",
                columns: table => new
                {
                    seat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    theatre_id = table.Column<int>(type: "int", nullable: false),
                    seat_type_id = table.Column<int>(type: "int", nullable: false),
                    seat_row = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    seat_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seat", x => x.seat_id);
                    table.ForeignKey(
                        name: "FK_seat_seat_type_seat_type_id",
                        column: x => x.seat_type_id,
                        principalTable: "seat_type",
                        principalColumn: "seat_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seat_theatre_theatre_id",
                        column: x => x.theatre_id,
                        principalTable: "theatre",
                        principalColumn: "theatre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "showing_time",
                columns: table => new
                {
                    time_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    theatre_id = table.Column<int>(type: "int", nullable: false),
                    showing_datetime = table.Column<TimeSpan>(type: "time", nullable: false),
                    showing_date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_showing_time", x => x.time_id);
                    table.ForeignKey(
                        name: "FK_showing_time_theatre_theatre_id",
                        column: x => x.theatre_id,
                        principalTable: "theatre",
                        principalColumn: "theatre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_genre",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_genre", x => new { x.movie_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_movie_genre_genre_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genre",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_genre_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_theatre",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    theatre_id = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_theatre", x => new { x.movie_id, x.theatre_id });
                    table.ForeignKey(
                        name: "FK_movie_theatre_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_theatre_theatre_theatre_id",
                        column: x => x.theatre_id,
                        principalTable: "theatre",
                        principalColumn: "theatre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_location",
                columns: table => new
                {
                    cinema_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_work_location_cinema_cinema_id",
                        column: x => x.cinema_id,
                        principalTable: "cinema",
                        principalColumn: "cinema_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_work_location_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    booking_fee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    email_address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    created_datetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_booking_showing_time_time_id",
                        column: x => x.time_id,
                        principalTable: "showing_time",
                        principalColumn: "time_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_seat",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    seat_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_seat", x => new { x.booking_id, x.seat_id });
                    table.ForeignKey(
                        name: "FK_booking_seat_booking",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_booking_seat_seat",
                        column: x => x.seat_id,
                        principalTable: "seat",
                        principalColumn: "seat_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_time_id",
                table: "booking",
                column: "time_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_user_id",
                table: "booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "unique_seat_booking",
                table: "booking_seat",
                columns: new[] { "seat_id", "booking_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movie_genre_genre_id",
                table: "movie_genre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "UQ__movie_st__501B37536219D8F3",
                table: "movie_status",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movie_theatre_theatre_id",
                table: "movie_theatre",
                column: "theatre_id");

            migrationBuilder.CreateIndex(
                name: "IX_movies_director_id",
                table: "movies",
                column: "director_id");

            migrationBuilder.CreateIndex(
                name: "IX_movies_status_id",
                table: "movies",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "UQ__roles__783254B14599DBE3",
                table: "roles",
                column: "role_name",
                unique: true,
                filter: "[role_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_seat_seat_type_id",
                table: "seat",
                column: "seat_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_seat_theatre_id",
                table: "seat",
                column: "theatre_id");

            migrationBuilder.CreateIndex(
                name: "IX_showing_time_theatre_id",
                table: "showing_time",
                column: "theatre_id");

            migrationBuilder.CreateIndex(
                name: "IX_theatre_cinema_id",
                table: "theatre",
                column: "cinema_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164704975E0",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_work_location_cinema_id",
                table: "work_location",
                column: "cinema_id");

            migrationBuilder.CreateIndex(
                name: "unique_user_cinema",
                table: "work_location",
                columns: new[] { "user_id", "cinema_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking_seat");

            migrationBuilder.DropTable(
                name: "cast_member");

            migrationBuilder.DropTable(
                name: "movie_genre");

            migrationBuilder.DropTable(
                name: "movie_theatre");

            migrationBuilder.DropTable(
                name: "work_location");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "seat");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "showing_time");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "seat_type");

            migrationBuilder.DropTable(
                name: "director");

            migrationBuilder.DropTable(
                name: "movie_status");

            migrationBuilder.DropTable(
                name: "theatre");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "cinema");
        }
    }
}
