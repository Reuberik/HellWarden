﻿using System;
using Bit.Core.Utilities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bit.PostgresMigrations.Migrations
{
    public partial class SelfHostF4E : Migration
    {
        private const string _scriptLocationTemplate = "Scripts.2022-03-01_00_{0}_MigrateOrganizationApiKeys.psql";

        private static string SqlContents(string dir)
        {
            return CoreHelpers.GetEmbeddedResourceContentsAsync(string.Format(_scriptLocationTemplate, dir));
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesRenewedWithoutValidation",
                table: "OrganizationSponsorship");

            migrationBuilder.CreateTable(
                name: "OrganizationApiKey",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    ApiKey = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationApiKey", x => new { x.OrganizationId, x.Type });
                    table.ForeignKey(
                        name: "FK_OrganizationApiKey_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(SqlContents("Up"));

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Organization");

            migrationBuilder.RenameColumn(
                name: "SponsorshipLapsedDate",
                table: "OrganizationSponsorship",
                newName: "ValidUntil");

            migrationBuilder.RenameColumn(
                name: "CloudSponsor",
                table: "OrganizationSponsorship",
                newName: "ToDelete");



            migrationBuilder.CreateTable(
                name: "OrganizationConnection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Config = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationConnection_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationConnection_OrganizationId",
                table: "OrganizationConnection",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Organization",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.Sql(SqlContents("Down"));

            migrationBuilder.DropTable(
                name: "OrganizationApiKey");

            migrationBuilder.DropTable(
                name: "OrganizationConnection");

            migrationBuilder.RenameColumn(
                name: "ValidUntil",
                table: "OrganizationSponsorship",
                newName: "SponsorshipLapsedDate");

            migrationBuilder.RenameColumn(
                name: "ToDelete",
                table: "OrganizationSponsorship",
                newName: "CloudSponsor");

            migrationBuilder.AddColumn<byte>(
                name: "TimesRenewedWithoutValidation",
                table: "OrganizationSponsorship",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}