using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CinemaBookingCore.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAccount",
                columns: table => new
                {
                    adminId = table.Column<string>(maxLength: 255, nullable: false),
                    adminPassword = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccount", x => x.adminId);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    cardNumber = table.Column<string>(maxLength: 50, nullable: false),
                    BankId = table.Column<int>(nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    expDate = table.Column<DateTime>(type: "date", nullable: true),
                    ownerName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.cardNumber);
                });

            migrationBuilder.CreateTable(
                name: "BankList",
                columns: table => new
                {
                    bankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bankName = table.Column<string>(maxLength: 255, nullable: true),
                    imgLogo = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankList", x => x.bankId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true),
                    userId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "DigitalType",
                columns: table => new
                {
                    digTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalType", x => x.digTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    filmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    actorList = table.Column<string>(maxLength: 255, nullable: true),
                    additionPicture = table.Column<string>(maxLength: 1000, nullable: true),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    countries = table.Column<string>(maxLength: 255, nullable: true),
                    dateRelease = table.Column<DateTime>(type: "datetime", nullable: false),
                    digTypeId = table.Column<string>(maxLength: 50, nullable: true),
                    filmContent = table.Column<string>(maxLength: 1000, nullable: true),
                    filmLength = table.Column<int>(nullable: true),
                    filmStatus = table.Column<int>(nullable: true),
                    imdb = table.Column<double>(nullable: true),
                    movieGenre = table.Column<string>(maxLength: 50, nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    posterPicture = table.Column<string>(maxLength: 255, nullable: true),
                    restricted = table.Column<int>(nullable: true),
                    trailerLink = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.filmId);
                });

            migrationBuilder.CreateTable(
                name: "GroupCinema",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    logoImg = table.Column<string>(maxLength: 50, nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCinema", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "ShowTime",
                columns: table => new
                {
                    timeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    endTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    startTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTime", x => x.timeId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    userId = table.Column<string>(maxLength: 255, nullable: false),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true),
                    userPassword = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "BookingTicket",
                columns: table => new
                {
                    bookingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bookingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    customerId = table.Column<int>(nullable: true),
                    paymentMethodId = table.Column<int>(nullable: true),
                    quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTicket", x => x.bookingId);
                    table.ForeignKey(
                        name: "FKBookingTicketCustomer001",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    newsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    filmId = table.Column<int>(nullable: true),
                    urlDocument = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.newsId);
                    table.ForeignKey(
                        name: "FKNewsFilm001",
                        column: x => x.filmId,
                        principalTable: "Film",
                        principalColumn: "filmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cinema",
                columns: table => new
                {
                    cinemaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cinemaAddress = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 200, nullable: true),
                    groupId = table.Column<int>(nullable: true),
                    introduction = table.Column<string>(maxLength: 1000, nullable: true),
                    openTime = table.Column<string>(maxLength: 200, nullable: true),
                    phone = table.Column<string>(maxLength: 200, nullable: true),
                    profilePicture = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema", x => x.cinemaId);
                    table.ForeignKey(
                        name: "FKCinemaGroupCinema001",
                        column: x => x.groupId,
                        principalTable: "GroupCinema",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartnerAccount",
                columns: table => new
                {
                    partnerId = table.Column<string>(maxLength: 255, nullable: false),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    groupOfCinemaId = table.Column<int>(nullable: true),
                    partnerPassword = table.Column<string>(maxLength: 255, nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerAccount", x => x.partnerId);
                    table.ForeignKey(
                        name: "FKPartnerAccountGroupCinema001",
                        column: x => x.groupOfCinemaId,
                        principalTable: "GroupCinema",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfSeat",
                columns: table => new
                {
                    typeSeatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    capacity = table.Column<int>(nullable: true),
                    groupId = table.Column<int>(nullable: true),
                    price = table.Column<double>(nullable: true),
                    typeName = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfSeat", x => x.typeSeatId);
                    table.ForeignKey(
                        name: "FKTypeOfSeatGroupCinema001",
                        column: x => x.groupId,
                        principalTable: "GroupCinema",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CinemaManager",
                columns: table => new
                {
                    managerId = table.Column<string>(maxLength: 255, nullable: false),
                    cinemaId = table.Column<int>(nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    managerPassword = table.Column<string>(maxLength: 255, nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaManager", x => x.managerId);
                    table.ForeignKey(
                        name: "FKCinemaManagerCinema001",
                        column: x => x.cinemaId,
                        principalTable: "Cinema",
                        principalColumn: "cinemaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    promotionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cinemaId = table.Column<int>(nullable: true),
                    urlDocument = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.promotionId);
                    table.ForeignKey(
                        name: "FKPromotionCinema001",
                        column: x => x.cinemaId,
                        principalTable: "Cinema",
                        principalColumn: "cinemaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    roomId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    capacity = table.Column<int>(nullable: true),
                    cinemaId = table.Column<int>(nullable: true),
                    digTypeId = table.Column<int>(nullable: true),
                    name = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.roomId);
                    table.ForeignKey(
                        name: "FKRoomCinema001",
                        column: x => x.cinemaId,
                        principalTable: "Cinema",
                        principalColumn: "cinemaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKRoomDigitalType001",
                        column: x => x.digTypeId,
                        principalTable: "DigitalType",
                        principalColumn: "digTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieSchedule",
                columns: table => new
                {
                    scheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    filmId = table.Column<int>(nullable: true),
                    roomId = table.Column<int>(nullable: true),
                    timeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSchedule", x => x.scheduleId);
                    table.ForeignKey(
                        name: "FKMovieScheduleFilm001",
                        column: x => x.filmId,
                        principalTable: "Film",
                        principalColumn: "filmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKMovieScheduleRoom001",
                        column: x => x.roomId,
                        principalTable: "Room",
                        principalColumn: "roomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKMovieScheduleShowTime001",
                        column: x => x.timeId,
                        principalTable: "ShowTime",
                        principalColumn: "timeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    seatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    px = table.Column<int>(nullable: true),
                    py = table.Column<int>(nullable: true),
                    roomId = table.Column<int>(nullable: true),
                    typeSeatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.seatId);
                    table.ForeignKey(
                        name: "FKSeatRoom001",
                        column: x => x.roomId,
                        principalTable: "Room",
                        principalColumn: "roomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKSeatTypeOfSeat001",
                        column: x => x.typeSeatId,
                        principalTable: "TypeOfSeat",
                        principalColumn: "typeSeatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ticketId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    paymentCode = table.Column<string>(maxLength: 50, nullable: true),
                    price = table.Column<double>(nullable: true),
                    qrCode = table.Column<int>(nullable: true),
                    scheduleId = table.Column<int>(nullable: true),
                    seatId = table.Column<int>(nullable: true),
                    ticketStatus = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ticketId);
                    table.ForeignKey(
                        name: "FKTicketMovieSchedule001",
                        column: x => x.scheduleId,
                        principalTable: "MovieSchedule",
                        principalColumn: "scheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKTicketSeat001",
                        column: x => x.seatId,
                        principalTable: "Seat",
                        principalColumn: "seatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetail",
                columns: table => new
                {
                    bookingDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bookingId = table.Column<int>(nullable: true),
                    ticketId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetail", x => x.bookingDetailId);
                    table.ForeignKey(
                        name: "FKBookingDetailBookingTicket001",
                        column: x => x.bookingId,
                        principalTable: "BookingTicket",
                        principalColumn: "bookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKBookingDetailTicket001",
                        column: x => x.ticketId,
                        principalTable: "Ticket",
                        principalColumn: "ticketId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetail_bookingId",
                table: "BookingDetail",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetail_ticketId",
                table: "BookingDetail",
                column: "ticketId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTicket_customerId",
                table: "BookingTicket",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cinema_groupId",
                table: "Cinema",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaManager_cinemaId",
                table: "CinemaManager",
                column: "cinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedule_filmId",
                table: "MovieSchedule",
                column: "filmId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedule_roomId",
                table: "MovieSchedule",
                column: "roomId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedule_timeId",
                table: "MovieSchedule",
                column: "timeId");

            migrationBuilder.CreateIndex(
                name: "IX_News_filmId",
                table: "News",
                column: "filmId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerAccount_groupOfCinemaId",
                table: "PartnerAccount",
                column: "groupOfCinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_cinemaId",
                table: "Promotion",
                column: "cinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_cinemaId",
                table: "Room",
                column: "cinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_digTypeId",
                table: "Room",
                column: "digTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_roomId",
                table: "Seat",
                column: "roomId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_typeSeatId",
                table: "Seat",
                column: "typeSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_scheduleId",
                table: "Ticket",
                column: "scheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_seatId",
                table: "Ticket",
                column: "seatId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfSeat_groupId",
                table: "TypeOfSeat",
                column: "groupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccount");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropTable(
                name: "BankList");

            migrationBuilder.DropTable(
                name: "BookingDetail");

            migrationBuilder.DropTable(
                name: "CinemaManager");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PartnerAccount");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "BookingTicket");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "MovieSchedule");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "ShowTime");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "TypeOfSeat");

            migrationBuilder.DropTable(
                name: "Cinema");

            migrationBuilder.DropTable(
                name: "DigitalType");

            migrationBuilder.DropTable(
                name: "GroupCinema");
        }
    }
}
