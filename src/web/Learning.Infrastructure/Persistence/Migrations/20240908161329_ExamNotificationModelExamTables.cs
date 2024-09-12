using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExamNotificationModelExamTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExamNotifications",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "ModelExamConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsFree = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    DiscountedPrice = table.Column<float>(type: "real", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ExamSolutionVideoId = table.Column<long>(type: "bigint", nullable: false),
                    TotalTimeLimit = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ExamNotificationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamConfiguration_Attachments_ExamSolutionVideoId",
                        column: x => x.ExamSolutionVideoId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelExamConfiguration_ExamNotifications_ExamNotificationId",
                        column: x => x.ExamNotificationId,
                        principalTable: "ExamNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelExamPurchaseHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamConfigId = table.Column<int>(type: "integer", nullable: false),
                    PurchasedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamPurchaseHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamPurchaseHistory_ModelExamConfiguration_ExamConfigId",
                        column: x => x.ExamConfigId,
                        principalTable: "ModelExamConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelExamQuestionConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    QuestionImageId = table.Column<long>(type: "bigint", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ExamConfigId = table.Column<int>(type: "integer", nullable: false),
                    Mark = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamQuestionConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamQuestionConfiguration_Attachments_QuestionImageId",
                        column: x => x.QuestionImageId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelExamQuestionConfiguration_ModelExamConfiguration_ExamC~",
                        column: x => x.ExamConfigId,
                        principalTable: "ModelExamConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelExamResult",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamConfigId = table.Column<int>(type: "integer", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamResult_ModelExamConfiguration_ExamConfigId",
                        column: x => x.ExamConfigId,
                        principalTable: "ModelExamConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelExamAnswerConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerText = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AnswerImageId = table.Column<long>(type: "bigint", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    AnswerType = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    IsCorrectAnswer = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamAnswerConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamAnswerConfiguration_Attachments_AnswerImageId",
                        column: x => x.AnswerImageId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelExamAnswerConfiguration_ModelExamQuestionConfiguration~",
                        column: x => x.QuestionId,
                        principalTable: "ModelExamQuestionConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelExamResultDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelExamResultId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    SelectedAnswerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelExamResultDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelExamResultDetail_ModelExamAnswerConfiguration_Selected~",
                        column: x => x.SelectedAnswerId,
                        principalTable: "ModelExamAnswerConfiguration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelExamResultDetail_ModelExamQuestionConfiguration_Questi~",
                        column: x => x.QuestionId,
                        principalTable: "ModelExamQuestionConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelExamResultDetail_ModelExamResult_ModelExamResultId",
                        column: x => x.ModelExamResultId,
                        principalTable: "ModelExamResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamAnswerConfiguration_AnswerImageId",
                table: "ModelExamAnswerConfiguration",
                column: "AnswerImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamAnswerConfiguration_QuestionId",
                table: "ModelExamAnswerConfiguration",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamConfiguration_ExamNotificationId",
                table: "ModelExamConfiguration",
                column: "ExamNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamConfiguration_ExamSolutionVideoId",
                table: "ModelExamConfiguration",
                column: "ExamSolutionVideoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamPurchaseHistory_ExamConfigId",
                table: "ModelExamPurchaseHistory",
                column: "ExamConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamQuestionConfiguration_ExamConfigId",
                table: "ModelExamQuestionConfiguration",
                column: "ExamConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamQuestionConfiguration_QuestionImageId",
                table: "ModelExamQuestionConfiguration",
                column: "QuestionImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamResult_ExamConfigId",
                table: "ModelExamResult",
                column: "ExamConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamResultDetail_ModelExamResultId",
                table: "ModelExamResultDetail",
                column: "ModelExamResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamResultDetail_QuestionId",
                table: "ModelExamResultDetail",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelExamResultDetail_SelectedAnswerId",
                table: "ModelExamResultDetail",
                column: "SelectedAnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelExamPurchaseHistory");

            migrationBuilder.DropTable(
                name: "ModelExamResultDetail");

            migrationBuilder.DropTable(
                name: "ModelExamAnswerConfiguration");

            migrationBuilder.DropTable(
                name: "ModelExamResult");

            migrationBuilder.DropTable(
                name: "ModelExamQuestionConfiguration");

            migrationBuilder.DropTable(
                name: "ModelExamConfiguration");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExamNotifications");
        }
    }
}
