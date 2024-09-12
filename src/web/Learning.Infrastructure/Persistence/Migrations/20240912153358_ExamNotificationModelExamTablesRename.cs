using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExamNotificationModelExamTablesRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamAnswerConfiguration_Attachments_AnswerImageId",
                table: "ModelExamAnswerConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamAnswerConfiguration_ModelExamQuestionConfiguration~",
                table: "ModelExamAnswerConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfiguration_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfiguration_ExamNotifications_ExamNotificationId",
                table: "ModelExamConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfiguration_ExamConfigId",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamQuestionConfiguration_Attachments_QuestionImageId",
                table: "ModelExamQuestionConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamQuestionConfiguration_ModelExamConfiguration_ExamC~",
                table: "ModelExamQuestionConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResult_ModelExamConfiguration_ExamConfigId",
                table: "ModelExamResult");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamAnswerConfiguration_Selected~",
                table: "ModelExamResultDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamQuestionConfiguration_Questi~",
                table: "ModelExamResultDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamResult_ModelExamResultId",
                table: "ModelExamResultDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamResultDetail",
                table: "ModelExamResultDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamResult",
                table: "ModelExamResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamQuestionConfiguration",
                table: "ModelExamQuestionConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamConfiguration",
                table: "ModelExamConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamAnswerConfiguration",
                table: "ModelExamAnswerConfiguration");

            migrationBuilder.RenameTable(
                name: "ModelExamResultDetail",
                newName: "ModelExamResultDetails");

            migrationBuilder.RenameTable(
                name: "ModelExamResult",
                newName: "ModelExamResults");

            migrationBuilder.RenameTable(
                name: "ModelExamQuestionConfiguration",
                newName: "ModelExamQuestionConfigurations");

            migrationBuilder.RenameTable(
                name: "ModelExamConfiguration",
                newName: "ModelExamConfigurations");

            migrationBuilder.RenameTable(
                name: "ModelExamAnswerConfiguration",
                newName: "ModelExamAnswerConfigurations");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetail_SelectedAnswerId",
                table: "ModelExamResultDetails",
                newName: "IX_ModelExamResultDetails_SelectedAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetail_QuestionId",
                table: "ModelExamResultDetails",
                newName: "IX_ModelExamResultDetails_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetail_ModelExamResultId",
                table: "ModelExamResultDetails",
                newName: "IX_ModelExamResultDetails_ModelExamResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResult_ExamConfigId",
                table: "ModelExamResults",
                newName: "IX_ModelExamResults_ExamConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamQuestionConfiguration_QuestionImageId",
                table: "ModelExamQuestionConfigurations",
                newName: "IX_ModelExamQuestionConfigurations_QuestionImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamQuestionConfiguration_ExamConfigId",
                table: "ModelExamQuestionConfigurations",
                newName: "IX_ModelExamQuestionConfigurations_ExamConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamConfiguration_ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                newName: "IX_ModelExamConfigurations_ExamSolutionVideoId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamConfiguration_ExamNotificationId",
                table: "ModelExamConfigurations",
                newName: "IX_ModelExamConfigurations_ExamNotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamAnswerConfiguration_QuestionId",
                table: "ModelExamAnswerConfigurations",
                newName: "IX_ModelExamAnswerConfigurations_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamAnswerConfiguration_AnswerImageId",
                table: "ModelExamAnswerConfigurations",
                newName: "IX_ModelExamAnswerConfigurations_AnswerImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamResultDetails",
                table: "ModelExamResultDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamResults",
                table: "ModelExamResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamQuestionConfigurations",
                table: "ModelExamQuestionConfigurations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamConfigurations",
                table: "ModelExamConfigurations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamAnswerConfigurations",
                table: "ModelExamAnswerConfigurations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamAnswerConfigurations_Attachments_AnswerImageId",
                table: "ModelExamAnswerConfigurations",
                column: "AnswerImageId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamAnswerConfigurations_ModelExamQuestionConfiguratio~",
                table: "ModelExamAnswerConfigurations",
                column: "QuestionId",
                principalTable: "ModelExamQuestionConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations",
                column: "ExamSolutionVideoId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfigurations_ExamNotifications_ExamNotificationId",
                table: "ModelExamConfigurations",
                column: "ExamNotificationId",
                principalTable: "ExamNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfigurations_ExamConfig~",
                table: "ModelExamPurchaseHistory",
                column: "ExamConfigId",
                principalTable: "ModelExamConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamQuestionConfigurations_Attachments_QuestionImageId",
                table: "ModelExamQuestionConfigurations",
                column: "QuestionImageId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamQuestionConfigurations_ModelExamConfigurations_Exa~",
                table: "ModelExamQuestionConfigurations",
                column: "ExamConfigId",
                principalTable: "ModelExamConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamAnswerConfigurations_Select~",
                table: "ModelExamResultDetails",
                column: "SelectedAnswerId",
                principalTable: "ModelExamAnswerConfigurations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamQuestionConfigurations_Ques~",
                table: "ModelExamResultDetails",
                column: "QuestionId",
                principalTable: "ModelExamQuestionConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamResults_ModelExamResultId",
                table: "ModelExamResultDetails",
                column: "ModelExamResultId",
                principalTable: "ModelExamResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResults_ModelExamConfigurations_ExamConfigId",
                table: "ModelExamResults",
                column: "ExamConfigId",
                principalTable: "ModelExamConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamAnswerConfigurations_Attachments_AnswerImageId",
                table: "ModelExamAnswerConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamAnswerConfigurations_ModelExamQuestionConfiguratio~",
                table: "ModelExamAnswerConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfigurations_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamConfigurations_ExamNotifications_ExamNotificationId",
                table: "ModelExamConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfigurations_ExamConfig~",
                table: "ModelExamPurchaseHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamQuestionConfigurations_Attachments_QuestionImageId",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamQuestionConfigurations_ModelExamConfigurations_Exa~",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamAnswerConfigurations_Select~",
                table: "ModelExamResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamQuestionConfigurations_Ques~",
                table: "ModelExamResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResultDetails_ModelExamResults_ModelExamResultId",
                table: "ModelExamResultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelExamResults_ModelExamConfigurations_ExamConfigId",
                table: "ModelExamResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamResults",
                table: "ModelExamResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamResultDetails",
                table: "ModelExamResultDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamQuestionConfigurations",
                table: "ModelExamQuestionConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamConfigurations",
                table: "ModelExamConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelExamAnswerConfigurations",
                table: "ModelExamAnswerConfigurations");

            migrationBuilder.RenameTable(
                name: "ModelExamResults",
                newName: "ModelExamResult");

            migrationBuilder.RenameTable(
                name: "ModelExamResultDetails",
                newName: "ModelExamResultDetail");

            migrationBuilder.RenameTable(
                name: "ModelExamQuestionConfigurations",
                newName: "ModelExamQuestionConfiguration");

            migrationBuilder.RenameTable(
                name: "ModelExamConfigurations",
                newName: "ModelExamConfiguration");

            migrationBuilder.RenameTable(
                name: "ModelExamAnswerConfigurations",
                newName: "ModelExamAnswerConfiguration");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResults_ExamConfigId",
                table: "ModelExamResult",
                newName: "IX_ModelExamResult_ExamConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetails_SelectedAnswerId",
                table: "ModelExamResultDetail",
                newName: "IX_ModelExamResultDetail_SelectedAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetails_QuestionId",
                table: "ModelExamResultDetail",
                newName: "IX_ModelExamResultDetail_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamResultDetails_ModelExamResultId",
                table: "ModelExamResultDetail",
                newName: "IX_ModelExamResultDetail_ModelExamResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamQuestionConfigurations_QuestionImageId",
                table: "ModelExamQuestionConfiguration",
                newName: "IX_ModelExamQuestionConfiguration_QuestionImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamQuestionConfigurations_ExamConfigId",
                table: "ModelExamQuestionConfiguration",
                newName: "IX_ModelExamQuestionConfiguration_ExamConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamConfigurations_ExamSolutionVideoId",
                table: "ModelExamConfiguration",
                newName: "IX_ModelExamConfiguration_ExamSolutionVideoId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamConfigurations_ExamNotificationId",
                table: "ModelExamConfiguration",
                newName: "IX_ModelExamConfiguration_ExamNotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamAnswerConfigurations_QuestionId",
                table: "ModelExamAnswerConfiguration",
                newName: "IX_ModelExamAnswerConfiguration_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelExamAnswerConfigurations_AnswerImageId",
                table: "ModelExamAnswerConfiguration",
                newName: "IX_ModelExamAnswerConfiguration_AnswerImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamResult",
                table: "ModelExamResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamResultDetail",
                table: "ModelExamResultDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamQuestionConfiguration",
                table: "ModelExamQuestionConfiguration",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamConfiguration",
                table: "ModelExamConfiguration",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelExamAnswerConfiguration",
                table: "ModelExamAnswerConfiguration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamAnswerConfiguration_Attachments_AnswerImageId",
                table: "ModelExamAnswerConfiguration",
                column: "AnswerImageId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamAnswerConfiguration_ModelExamQuestionConfiguration~",
                table: "ModelExamAnswerConfiguration",
                column: "QuestionId",
                principalTable: "ModelExamQuestionConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfiguration_Attachments_ExamSolutionVideoId",
                table: "ModelExamConfiguration",
                column: "ExamSolutionVideoId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamConfiguration_ExamNotifications_ExamNotificationId",
                table: "ModelExamConfiguration",
                column: "ExamNotificationId",
                principalTable: "ExamNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamPurchaseHistory_ModelExamConfiguration_ExamConfigId",
                table: "ModelExamPurchaseHistory",
                column: "ExamConfigId",
                principalTable: "ModelExamConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamQuestionConfiguration_Attachments_QuestionImageId",
                table: "ModelExamQuestionConfiguration",
                column: "QuestionImageId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamQuestionConfiguration_ModelExamConfiguration_ExamC~",
                table: "ModelExamQuestionConfiguration",
                column: "ExamConfigId",
                principalTable: "ModelExamConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResult_ModelExamConfiguration_ExamConfigId",
                table: "ModelExamResult",
                column: "ExamConfigId",
                principalTable: "ModelExamConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamAnswerConfiguration_Selected~",
                table: "ModelExamResultDetail",
                column: "SelectedAnswerId",
                principalTable: "ModelExamAnswerConfiguration",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamQuestionConfiguration_Questi~",
                table: "ModelExamResultDetail",
                column: "QuestionId",
                principalTable: "ModelExamQuestionConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelExamResultDetail_ModelExamResult_ModelExamResultId",
                table: "ModelExamResultDetail",
                column: "ModelExamResultId",
                principalTable: "ModelExamResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
