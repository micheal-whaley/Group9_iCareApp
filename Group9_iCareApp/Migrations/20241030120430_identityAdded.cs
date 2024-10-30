using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group9_iCareApp.Migrations
{
    /// <inheritdoc />
    public partial class identityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentMetadata",
                columns: table => new
                {
                    DocID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DateOfCreation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__3EF1888DDD7318E2", x => x.DocID);
                });

            migrationBuilder.CreateTable(
                name: "DrugsDictionary",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DrugsDic__3214EC270D13C263", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GeoCodes__3214EC27C87E4192", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkerRole",
                columns: table => new
                {
                    Profession = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserRole__3214EC27AC10000D", x => x.Profession);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "iCAREUser",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Lname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    locationID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__iCAREUse__3214EC275A117A47", x => x.ID);
                    table.ForeignKey(
                        name: "FK_iCAREUser_Location",
                        column: x => x.locationID,
                        principalTable: "Location",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientRecord",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Lname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    BloodGroup = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    BedID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TreatmentArea = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    locationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PatientR__3214EC27268C669B", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientRecord_Location",
                        column: x => x.locationID,
                        principalTable: "Location",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_iCAREUser_UserId",
                        column: x => x.UserId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_iCAREUser_UserId",
                        column: x => x.UserId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_iCAREUser_UserId",
                        column: x => x.UserId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_iCAREUser_UserId",
                        column: x => x.UserId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "iCAREAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iCAREAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_iCAREAdmins_iCAREUser_IdNavigationId",
                        column: x => x.IdNavigationId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "iCAREWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfessionNavigationProfession = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iCAREWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_iCAREWorkers_WorkerRole_ProfessionNavigationProfession",
                        column: x => x.ProfessionNavigationProfession,
                        principalTable: "WorkerRole",
                        principalColumn: "Profession",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iCAREWorkers_iCAREUser_IdNavigationId",
                        column: x => x.IdNavigationId,
                        principalTable: "iCAREUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModificationHistory",
                columns: table => new
                {
                    ModificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DocID = table.Column<int>(type: "int", nullable: true),
                    WorkerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modifica__A3FE5A12391D4EDB", x => x.ModificationID);
                    table.ForeignKey(
                        name: "FK__Modificat__DocID__4D94879B",
                        column: x => x.DocID,
                        principalTable: "DocumentMetadata",
                        principalColumn: "DocID");
                    table.ForeignKey(
                        name: "FK__Modificat__Worke__4E88ABD4",
                        column: x => x.WorkerID,
                        principalTable: "iCAREWorkers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TreatmentRecord",
                columns: table => new
                {
                    TreatmentID = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TreatmentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    WorkerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Treatmen__1A57B711ABC92DA2", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK__Treatment__Patie__47DBAE45",
                        column: x => x.PatientID,
                        principalTable: "PatientRecord",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Treatment__Worke__48CFD27E",
                        column: x => x.WorkerID,
                        principalTable: "iCAREWorkers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_iCAREAdmins_IdNavigationId",
                table: "iCAREAdmins",
                column: "IdNavigationId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "iCAREUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_iCAREUser_locationID",
                table: "iCAREUser",
                column: "locationID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "iCAREUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_iCAREWorkers_IdNavigationId",
                table: "iCAREWorkers",
                column: "IdNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_iCAREWorkers_ProfessionNavigationProfession",
                table: "iCAREWorkers",
                column: "ProfessionNavigationProfession");

            migrationBuilder.CreateIndex(
                name: "IX_ModificationHistory_DocID",
                table: "ModificationHistory",
                column: "DocID");

            migrationBuilder.CreateIndex(
                name: "IX_ModificationHistory_WorkerID",
                table: "ModificationHistory",
                column: "WorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecord_locationID",
                table: "PatientRecord",
                column: "locationID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecord_PatientID",
                table: "TreatmentRecord",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecord_WorkerID",
                table: "TreatmentRecord",
                column: "WorkerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DrugsDictionary");

            migrationBuilder.DropTable(
                name: "iCAREAdmins");

            migrationBuilder.DropTable(
                name: "ModificationHistory");

            migrationBuilder.DropTable(
                name: "TreatmentRecord");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DocumentMetadata");

            migrationBuilder.DropTable(
                name: "PatientRecord");

            migrationBuilder.DropTable(
                name: "iCAREWorkers");

            migrationBuilder.DropTable(
                name: "WorkerRole");

            migrationBuilder.DropTable(
                name: "iCAREUser");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
