using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogPlatform.Domain.Migrations
{
    public partial class RatingValueDataTypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Account",
                newName: "Nickname");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Rating",
                nullable: false,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "Account",
                newName: "NickName");

            migrationBuilder.AlterColumn<byte>(
                name: "Value",
                table: "Rating",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
