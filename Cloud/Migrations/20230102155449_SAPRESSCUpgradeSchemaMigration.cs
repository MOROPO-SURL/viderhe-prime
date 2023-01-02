using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartSolucionesCuba.Viderhe.Cloud.Migrations
{
    public partial class SAPRESSCUpgradeSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grouprolebridges",
                schema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.DropTable(
                name: "usergroupbridges",
                schema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_core_security_common");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "viderhe_sapressc_core_securitylocal",
                newName: "users",
                newSchema: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.AlterColumn<DateTime>(
                name: "borndate",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personidentities",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "borndate",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationidentities",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "viderhe_sapressc_core_security_common",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    code = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    family = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "viderhe_sapressc_core_security_common",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userrolebridges",
                schema: "viderhe_sapressc_middleware_security_essential",
                columns: table => new
                {
                    userid = table.Column<string>(nullable: false),
                    roleid = table.Column<Guid>(nullable: false),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrolebridges", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "fk_userrolebridges_roles_roleid",
                        column: x => x.roleid,
                        principalSchema: "viderhe_sapressc_core_security_common",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_userrolebridges_users_userid",
                        column: x => x.userid,
                        principalSchema: "viderhe_sapressc_middleware_security_essential",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rolepermissionbridges",
                schema: "viderhe_sapressc_core_security_common",
                columns: table => new
                {
                    roleid = table.Column<Guid>(nullable: false),
                    permissionid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rolepermissionbridges", x => new { x.roleid, x.permissionid });
                    table.ForeignKey(
                        name: "fk_rolepermissionbridges_permissions_permissionid",
                        column: x => x.permissionid,
                        principalSchema: "viderhe_sapressc_core_security_common",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rolepermissionbridges_roles_roleid",
                        column: x => x.roleid,
                        principalSchema: "viderhe_sapressc_core_security_common",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rolepermissionbridges_permissionid",
                schema: "viderhe_sapressc_core_security_common",
                table: "rolepermissionbridges",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "ix_userrolebridges_roleid",
                schema: "viderhe_sapressc_middleware_security_essential",
                table: "userrolebridges",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rolepermissionbridges",
                schema: "viderhe_sapressc_core_security_common");

            migrationBuilder.DropTable(
                name: "userrolebridges",
                schema: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "viderhe_sapressc_core_security_common");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "viderhe_sapressc_middleware_security_essential",
                newName: "users",
                newSchema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.RenameTable(
                name: "roles",
                schema: "viderhe_sapressc_core_security_common",
                newName: "roles",
                newSchema: "viderhe_sapressc_core_securitylocal");

            migrationBuilder.AlterColumn<DateTime>(
                name: "borndate",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personidentities",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "borndate",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationidentities",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "viderhe_sapressc_core_securitylocal",
                table: "roles",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "viderhe_sapressc_core_securitylocal",
                table: "roles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "viderhe_sapressc_core_securitylocal",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grouprolebridges",
                schema: "viderhe_sapressc_core_securitylocal",
                columns: table => new
                {
                    groupid = table.Column<Guid>(nullable: false),
                    roleid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grouprolebridges", x => new { x.groupid, x.roleid });
                    table.ForeignKey(
                        name: "fk_grouprolebridges_groups_groupid",
                        column: x => x.groupid,
                        principalSchema: "viderhe_sapressc_core_securitylocal",
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grouprolebridges_roles_roleid",
                        column: x => x.roleid,
                        principalSchema: "viderhe_sapressc_core_securitylocal",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usergroupbridges",
                schema: "viderhe_sapressc_core_securitylocal",
                columns: table => new
                {
                    userid = table.Column<string>(nullable: false),
                    groupid = table.Column<Guid>(nullable: false),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usergroupbridges", x => new { x.userid, x.groupid });
                    table.ForeignKey(
                        name: "fk_usergroupbridges_groups_groupid",
                        column: x => x.groupid,
                        principalSchema: "viderhe_sapressc_core_securitylocal",
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usergroupbridges_users_userid",
                        column: x => x.userid,
                        principalSchema: "viderhe_sapressc_core_securitylocal",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_grouprolebridges_roleid",
                schema: "viderhe_sapressc_core_securitylocal",
                table: "grouprolebridges",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "ix_usergroupbridges_groupid",
                schema: "viderhe_sapressc_core_securitylocal",
                table: "usergroupbridges",
                column: "groupid");
        }
    }
}
