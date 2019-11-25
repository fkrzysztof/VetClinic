﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Add_ScheduleBlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleBlocks",
                columns: table => new
                {
                    ScheduleBlockID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBlocks", x => x.ScheduleBlockID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleBlocks");
        }
    }
}
