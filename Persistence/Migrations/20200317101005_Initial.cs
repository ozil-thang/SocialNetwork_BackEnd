using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    GithubUserName = table.Column<string>(nullable: true),
                    Youtube = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Linkedin = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    AvatarId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Profile_Photo_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Photo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    School = table.Column<string>(nullable: true),
                    Degree = table.Column<string>(nullable: true),
                    FieldOfStudy = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Current = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Current = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experience_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    PhotoId = table.Column<string>(nullable: true),
                    VideoId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Photo_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserProfileUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skill_Profile_UserProfileUserId",
                        column: x => x.UserProfileUserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    PostId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PostId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_Like_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Like_Profile_UserId",
                        column: x => x.UserId,
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserId",
                table: "Education",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_UserId",
                table: "Experience",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                table: "Like",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PhotoId",
                table: "Post",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_VideoId",
                table: "Post",
                column: "VideoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_UserId",
                table: "Skill",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_UserProfileUserId",
                table: "Skill",
                column: "UserProfileUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
