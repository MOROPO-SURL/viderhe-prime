using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Migrations
{
    public partial class InitialSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_core_security_common");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_middleware_identity");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_middleware_media");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_middleware_region");

            migrationBuilder.EnsureSchema(
                name: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.CreateTable(
                name: "extensions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    version = table.Column<string>(nullable: false),
                    enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_extensions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "traces",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(nullable: false),
                    author = table.Column<string>(nullable: true),
                    registered = table.Column<DateTime>(nullable: false),
                    entityname = table.Column<string>(nullable: false),
                    beforedata = table.Column<string>(nullable: false),
                    afterdata = table.Column<string>(nullable: true),
                    nextid = table.Column<long>(nullable: false),
                    signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_traces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "viderhe_sapressc_core_security_common",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                name: "emailcontactitems",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    emailaddress = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emailcontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phonecontactitems",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    phonenumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phonecontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "socialcontactitems",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    socialid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_socialcontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "websitecontactitems",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_websitecontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "thumbnails",
                schema: "viderhe_sapressc_middleware_media",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    contenttype = table.Column<string>(nullable: false),
                    length = table.Column<long>(nullable: false),
                    about = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_thumbnails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                schema: "viderhe_sapressc_middleware_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    isocode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "viderhe_sapressc_middleware_security_essential",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    passwordchangerequired = table.Column<bool>(nullable: false),
                    hasfullaccess = table.Column<bool>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    lastsessiondate = table.Column<DateTime>(nullable: false),
                    failauthenticationcount = table.Column<int>(nullable: false),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "overlays",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    extensionid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_overlays", x => x.id);
                    table.ForeignKey(
                        name: "fk_overlays_extensions_extensionid",
                        column: x => x.extensionid,
                        principalTable: "extensions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    extensionid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_templates_extensions_extensionid",
                        column: x => x.extensionid,
                        principalTable: "extensions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transitions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    extensionid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transitions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transitions_extensions_extensionid",
                        column: x => x.extensionid,
                        principalTable: "extensions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "images",
                schema: "viderhe_sapressc_middleware_media",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createat = table.Column<DateTime>(nullable: false),
                    contenttype = table.Column<string>(nullable: false),
                    length = table.Column<long>(nullable: false),
                    about = table.Column<string>(maxLength: 255, nullable: true),
                    height = table.Column<int>(nullable: false),
                    width = table.Column<int>(nullable: false),
                    thumbnailid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_thumbnails_thumbnailid",
                        column: x => x.thumbnailid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "thumbnails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "videos",
                schema: "viderhe_sapressc_middleware_media",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createat = table.Column<DateTime>(nullable: false),
                    extension = table.Column<string>(nullable: false),
                    length = table.Column<long>(nullable: false),
                    about = table.Column<string>(maxLength: 255, nullable: true),
                    duration = table.Column<TimeSpan>(nullable: false),
                    format = table.Column<string>(nullable: false),
                    framesize = table.Column<string>(nullable: false),
                    colormodel = table.Column<string>(nullable: false),
                    fps = table.Column<double>(nullable: false),
                    audioformat = table.Column<string>(nullable: false),
                    audiochanneloutput = table.Column<string>(nullable: false),
                    thumbnailid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_videos", x => x.id);
                    table.ForeignKey(
                        name: "fk_videos_thumbnails_thumbnailid",
                        column: x => x.thumbnailid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "thumbnails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "states",
                schema: "viderhe_sapressc_middleware_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    countryid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_states", x => x.id);
                    table.ForeignKey(
                        name: "fk_states_countries_countryid",
                        column: x => x.countryid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    primarycolor = table.Column<string>(nullable: false),
                    secoundarycolor = table.Column<string>(nullable: false),
                    maxplayercount = table.Column<int>(nullable: false),
                    logoid = table.Column<Guid>(nullable: false),
                    horizontalsplashid = table.Column<Guid>(nullable: false),
                    verticalsplashid = table.Column<Guid>(nullable: false),
                    defaultoverlayid = table.Column<Guid>(nullable: true),
                    serializedoverlaycontext = table.Column<string>(nullable: true),
                    createat = table.Column<DateTime>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounts_overlays_defaultoverlayid",
                        column: x => x.defaultoverlayid,
                        principalTable: "overlays",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_accounts_images_horizontalsplashid",
                        column: x => x.horizontalsplashid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounts_images_logoid",
                        column: x => x.logoid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounts_images_verticalsplashid",
                        column: x => x.verticalsplashid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationidentities",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    borndate = table.Column<DateTime>(nullable: true),
                    about = table.Column<string>(nullable: true),
                    ownerid = table.Column<Guid>(nullable: true),
                    imageid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationidentities", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizationidentities_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_organizationidentities_organizationidentities_ownerid",
                        column: x => x.ownerid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "personidentities",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    borndate = table.Column<DateTime>(nullable: true),
                    about = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    alias = table.Column<string>(nullable: true),
                    sex = table.Column<int>(nullable: false),
                    civilstatus = table.Column<int>(nullable: false),
                    imageid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personidentities", x => x.id);
                    table.ForeignKey(
                        name: "fk_personidentities_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "municipalities",
                schema: "viderhe_sapressc_middleware_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    stateid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_municipalities", x => x.id);
                    table.ForeignKey(
                        name: "fk_municipalities_states_stateid",
                        column: x => x.stateid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountextensionbridges",
                columns: table => new
                {
                    accountid = table.Column<Guid>(nullable: false),
                    extensionid = table.Column<Guid>(nullable: false),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountextensionbridges", x => new { x.accountid, x.extensionid });
                    table.ForeignKey(
                        name: "fk_accountextensionbridges_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accountextensionbridges_extensions_extensionid",
                        column: x => x.extensionid,
                        principalTable: "extensions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountimagebridges",
                columns: table => new
                {
                    accountid = table.Column<Guid>(nullable: false),
                    imageid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountimagebridges", x => new { x.accountid, x.imageid });
                    table.ForeignKey(
                        name: "fk_accountimagebridges_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accountimagebridges_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountvideobridges",
                columns: table => new
                {
                    accountid = table.Column<Guid>(nullable: false),
                    videoid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountvideobridges", x => new { x.accountid, x.videoid });
                    table.ForeignKey(
                        name: "fk_accountvideobridges_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accountvideobridges_videos_videoid",
                        column: x => x.videoid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "placards",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createat = table.Column<DateTime>(nullable: false),
                    accountid = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: false),
                    templateid = table.Column<Guid>(nullable: false),
                    playerorientationsupportkind = table.Column<int>(nullable: true),
                    beginat = table.Column<DateTime>(nullable: true),
                    endat = table.Column<DateTime>(nullable: true),
                    serializedcontext = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placards", x => x.id);
                    table.ForeignKey(
                        name: "fk_placards_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_placards_templates_templateid",
                        column: x => x.templateid,
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "screenplays",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    about = table.Column<string>(nullable: true),
                    accountid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_screenplays", x => x.id);
                    table.ForeignKey(
                        name: "fk_screenplays_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationemailcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    organizationid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationemailcontactitembridges", x => new { x.organizationid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_organizationemailcontactitembridges_emailcontactitems_contac~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "emailcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationemailcontactitembridges_organizationidentities_o~",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationphonecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    organizationid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationphonecontactitembridges", x => new { x.organizationid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_organizationphonecontactitembridges_phonecontactitems_contac~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "phonecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationphonecontactitembridges_organizationidentities_o~",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationsocialcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    organizationid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationsocialcontactitembridges", x => new { x.organizationid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_organizationsocialcontactitembridges_socialcontactitems_cont~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "socialcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationsocialcontactitembridges_organizationidentities_~",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationwebsitecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    organizationid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationwebsitecontactitembridges", x => new { x.organizationid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_organizationwebsitecontactitembridges_websitecontactitems_co~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "websitecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationwebsitecontactitembridges_organizationidentities~",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "moderators",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    identityid = table.Column<Guid>(nullable: false),
                    userid = table.Column<string>(nullable: true),
                    moderationkind = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_moderators", x => x.id);
                    table.ForeignKey(
                        name: "fk_moderators_personidentities_identityid",
                        column: x => x.identityid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_moderators_users_userid",
                        column: x => x.userid,
                        principalSchema: "viderhe_sapressc_middleware_security_essential",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "memberships",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    organizationid = table.Column<Guid>(nullable: false),
                    role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_memberships", x => new { x.organizationid, x.personid });
                    table.ForeignKey(
                        name: "fk_memberships_organizationidentities_organizationid",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_memberships_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personemailcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personemailcontactitembridges", x => new { x.personid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_personemailcontactitembridges_emailcontactitems_contactitemid",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "emailcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personemailcontactitembridges_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personphonecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personphonecontactitembridges", x => new { x.personid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_personphonecontactitembridges_phonecontactitems_contactitemid",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "phonecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personphonecontactitembridges_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personsocialcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personsocialcontactitembridges", x => new { x.personid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_personsocialcontactitembridges_socialcontactitems_contactite~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "socialcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personsocialcontactitembridges_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personwebsitecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personwebsitecontactitembridges", x => new { x.personid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_personwebsitecontactitembridges_websitecontactitems_contacti~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "websitecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personwebsitecontactitembridges_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "localities",
                schema: "viderhe_sapressc_middleware_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    municipalityid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localities", x => x.id);
                    table.ForeignKey(
                        name: "fk_localities_municipalities_municipalityid",
                        column: x => x.municipalityid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "municipalities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "placardimagebridges",
                columns: table => new
                {
                    placardid = table.Column<Guid>(nullable: false),
                    imageid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placardimagebridges", x => new { x.placardid, x.imageid });
                    table.ForeignKey(
                        name: "fk_placardimagebridges_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_placardimagebridges_placards_placardid",
                        column: x => x.placardid,
                        principalTable: "placards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "placardvideobridges",
                columns: table => new
                {
                    placardid = table.Column<Guid>(nullable: false),
                    videoid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placardvideobridges", x => new { x.placardid, x.videoid });
                    table.ForeignKey(
                        name: "fk_placardvideobridges_placards_placardid",
                        column: x => x.placardid,
                        principalTable: "placards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_placardvideobridges_videos_videoid",
                        column: x => x.videoid,
                        principalSchema: "viderhe_sapressc_middleware_media",
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "frames",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    screenplayid = table.Column<Guid>(nullable: false),
                    placardid = table.Column<Guid>(nullable: false),
                    priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_frames", x => x.id);
                    table.ForeignKey(
                        name: "fk_frames_placards_placardid",
                        column: x => x.placardid,
                        principalTable: "placards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_frames_screenplays_screenplayid",
                        column: x => x.screenplayid,
                        principalTable: "screenplays",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    accountid = table.Column<Guid>(nullable: false),
                    screenplayid = table.Column<Guid>(nullable: true),
                    overlayid = table.Column<Guid>(nullable: true),
                    serializedoverlaycontext = table.Column<string>(nullable: true),
                    about = table.Column<string>(nullable: true),
                    signature = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                    table.ForeignKey(
                        name: "fk_players_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_players_overlays_overlayid",
                        column: x => x.overlayid,
                        principalTable: "overlays",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_players_screenplays_screenplayid",
                        column: x => x.screenplayid,
                        principalTable: "screenplays",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "accountmoderatorbridges",
                columns: table => new
                {
                    accountid = table.Column<Guid>(nullable: false),
                    moderatorid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountmoderatorbridges", x => new { x.accountid, x.moderatorid });
                    table.ForeignKey(
                        name: "fk_accountmoderatorbridges_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accountmoderatorbridges_moderators_moderatorid",
                        column: x => x.moderatorid,
                        principalTable: "moderators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                schema: "viderhe_sapressc_middleware_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    discriminator = table.Column<string>(nullable: false),
                    countryid = table.Column<Guid>(nullable: true),
                    localityid = table.Column<Guid>(nullable: true),
                    municipalityid = table.Column<Guid>(nullable: true),
                    stateid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regions", x => x.id);
                    table.ForeignKey(
                        name: "fk_regions_countries_countryid",
                        column: x => x.countryid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_regions_localities_localityid",
                        column: x => x.localityid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "localities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_regions_municipalities_municipalityid",
                        column: x => x.municipalityid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "municipalities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_regions_states_stateid",
                        column: x => x.stateid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "scheduledrelaunches",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    playerid = table.Column<string>(nullable: false),
                    time = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scheduledrelaunches", x => x.id);
                    table.ForeignKey(
                        name: "fk_scheduledrelaunches_players_playerid",
                        column: x => x.playerid,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "postalcontactitems",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: false),
                    regionid = table.Column<Guid>(nullable: false),
                    zipcode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_postalcontactitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_postalcontactitems_regions_regionid",
                        column: x => x.regionid,
                        principalSchema: "viderhe_sapressc_middleware_region",
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationpostalcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    organizationid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationpostalcontactitembridges", x => new { x.organizationid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_organizationpostalcontactitembridges_postalcontactitems_cont~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "postalcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationpostalcontactitembridges_organizationidentities_~",
                        column: x => x.organizationid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personpostalcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity",
                columns: table => new
                {
                    personid = table.Column<Guid>(nullable: false),
                    contactitemid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personpostalcontactitembridges", x => new { x.personid, x.contactitemid });
                    table.ForeignKey(
                        name: "fk_personpostalcontactitembridges_postalcontactitems_contactite~",
                        column: x => x.contactitemid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "postalcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personpostalcontactitembridges_personidentities_personid",
                        column: x => x.personid,
                        principalSchema: "viderhe_sapressc_middleware_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accountextensionbridges_extensionid",
                table: "accountextensionbridges",
                column: "extensionid");

            migrationBuilder.CreateIndex(
                name: "ix_accountimagebridges_imageid",
                table: "accountimagebridges",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_accountmoderatorbridges_moderatorid",
                table: "accountmoderatorbridges",
                column: "moderatorid");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_defaultoverlayid",
                table: "accounts",
                column: "defaultoverlayid");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_horizontalsplashid",
                table: "accounts",
                column: "horizontalsplashid");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_logoid",
                table: "accounts",
                column: "logoid");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_verticalsplashid",
                table: "accounts",
                column: "verticalsplashid");

            migrationBuilder.CreateIndex(
                name: "ix_accountvideobridges_videoid",
                table: "accountvideobridges",
                column: "videoid");

            migrationBuilder.CreateIndex(
                name: "ix_frames_placardid",
                table: "frames",
                column: "placardid");

            migrationBuilder.CreateIndex(
                name: "ix_frames_screenplayid",
                table: "frames",
                column: "screenplayid");

            migrationBuilder.CreateIndex(
                name: "ix_moderators_identityid",
                table: "moderators",
                column: "identityid");

            migrationBuilder.CreateIndex(
                name: "ix_moderators_userid",
                table: "moderators",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_overlays_extensionid",
                table: "overlays",
                column: "extensionid");

            migrationBuilder.CreateIndex(
                name: "ix_placardimagebridges_imageid",
                table: "placardimagebridges",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_placards_accountid",
                table: "placards",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "ix_placards_templateid",
                table: "placards",
                column: "templateid");

            migrationBuilder.CreateIndex(
                name: "ix_placardvideobridges_videoid",
                table: "placardvideobridges",
                column: "videoid");

            migrationBuilder.CreateIndex(
                name: "ix_players_accountid",
                table: "players",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "ix_players_overlayid",
                table: "players",
                column: "overlayid");

            migrationBuilder.CreateIndex(
                name: "ix_players_screenplayid",
                table: "players",
                column: "screenplayid");

            migrationBuilder.CreateIndex(
                name: "ix_scheduledrelaunches_playerid",
                table: "scheduledrelaunches",
                column: "playerid");

            migrationBuilder.CreateIndex(
                name: "ix_screenplays_accountid",
                table: "screenplays",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "ix_templates_extensionid",
                table: "templates",
                column: "extensionid");

            migrationBuilder.CreateIndex(
                name: "ix_transitions_extensionid",
                table: "transitions",
                column: "extensionid");

            migrationBuilder.CreateIndex(
                name: "ix_rolepermissionbridges_permissionid",
                schema: "viderhe_sapressc_core_security_common",
                table: "rolepermissionbridges",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "ix_memberships_personid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "memberships",
                column: "personid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationemailcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationemailcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationidentities_imageid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationidentities",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationidentities_ownerid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationidentities",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationphonecontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationphonecontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationpostalcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationpostalcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationsocialcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationsocialcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationwebsitecontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "organizationwebsitecontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_personemailcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personemailcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_personidentities_imageid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personidentities",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_personphonecontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personphonecontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_personpostalcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personpostalcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_personsocialcontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personsocialcontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_personwebsitecontactitembridges_contactitemid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "personwebsitecontactitembridges",
                column: "contactitemid");

            migrationBuilder.CreateIndex(
                name: "ix_postalcontactitems_regionid",
                schema: "viderhe_sapressc_middleware_identity",
                table: "postalcontactitems",
                column: "regionid");

            migrationBuilder.CreateIndex(
                name: "ix_images_thumbnailid",
                schema: "viderhe_sapressc_middleware_media",
                table: "images",
                column: "thumbnailid");

            migrationBuilder.CreateIndex(
                name: "ix_videos_thumbnailid",
                schema: "viderhe_sapressc_middleware_media",
                table: "videos",
                column: "thumbnailid");

            migrationBuilder.CreateIndex(
                name: "ix_countries_isocode",
                schema: "viderhe_sapressc_middleware_region",
                table: "countries",
                column: "isocode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_countries_name",
                schema: "viderhe_sapressc_middleware_region",
                table: "countries",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_localities_municipalityid",
                schema: "viderhe_sapressc_middleware_region",
                table: "localities",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "ix_localities_name_municipalityid",
                schema: "viderhe_sapressc_middleware_region",
                table: "localities",
                columns: new[] { "name", "municipalityid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_municipalities_stateid",
                schema: "viderhe_sapressc_middleware_region",
                table: "municipalities",
                column: "stateid");

            migrationBuilder.CreateIndex(
                name: "ix_municipalities_name_stateid",
                schema: "viderhe_sapressc_middleware_region",
                table: "municipalities",
                columns: new[] { "name", "stateid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_regions_countryid",
                schema: "viderhe_sapressc_middleware_region",
                table: "regions",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "ix_regions_localityid",
                schema: "viderhe_sapressc_middleware_region",
                table: "regions",
                column: "localityid");

            migrationBuilder.CreateIndex(
                name: "ix_regions_municipalityid",
                schema: "viderhe_sapressc_middleware_region",
                table: "regions",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "ix_regions_stateid",
                schema: "viderhe_sapressc_middleware_region",
                table: "regions",
                column: "stateid");

            migrationBuilder.CreateIndex(
                name: "ix_states_countryid",
                schema: "viderhe_sapressc_middleware_region",
                table: "states",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "ix_userrolebridges_roleid",
                schema: "viderhe_sapressc_middleware_security_essential",
                table: "userrolebridges",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountextensionbridges");

            migrationBuilder.DropTable(
                name: "accountimagebridges");

            migrationBuilder.DropTable(
                name: "accountmoderatorbridges");

            migrationBuilder.DropTable(
                name: "accountvideobridges");

            migrationBuilder.DropTable(
                name: "frames");

            migrationBuilder.DropTable(
                name: "placardimagebridges");

            migrationBuilder.DropTable(
                name: "placardvideobridges");

            migrationBuilder.DropTable(
                name: "scheduledrelaunches");

            migrationBuilder.DropTable(
                name: "traces");

            migrationBuilder.DropTable(
                name: "transitions");

            migrationBuilder.DropTable(
                name: "rolepermissionbridges",
                schema: "viderhe_sapressc_core_security_common");

            migrationBuilder.DropTable(
                name: "memberships",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "organizationemailcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "organizationphonecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "organizationpostalcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "organizationsocialcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "organizationwebsitecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "personemailcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "personphonecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "personpostalcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "personsocialcontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "personwebsitecontactitembridges",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "userrolebridges",
                schema: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.DropTable(
                name: "moderators");

            migrationBuilder.DropTable(
                name: "placards");

            migrationBuilder.DropTable(
                name: "videos",
                schema: "viderhe_sapressc_middleware_media");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "viderhe_sapressc_core_security_common");

            migrationBuilder.DropTable(
                name: "organizationidentities",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "emailcontactitems",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "phonecontactitems",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "postalcontactitems",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "socialcontactitems",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "websitecontactitems",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "viderhe_sapressc_core_security_common");

            migrationBuilder.DropTable(
                name: "personidentities",
                schema: "viderhe_sapressc_middleware_identity");

            migrationBuilder.DropTable(
                name: "users",
                schema: "viderhe_sapressc_middleware_security_essential");

            migrationBuilder.DropTable(
                name: "templates");

            migrationBuilder.DropTable(
                name: "screenplays");

            migrationBuilder.DropTable(
                name: "regions",
                schema: "viderhe_sapressc_middleware_region");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "localities",
                schema: "viderhe_sapressc_middleware_region");

            migrationBuilder.DropTable(
                name: "overlays");

            migrationBuilder.DropTable(
                name: "images",
                schema: "viderhe_sapressc_middleware_media");

            migrationBuilder.DropTable(
                name: "municipalities",
                schema: "viderhe_sapressc_middleware_region");

            migrationBuilder.DropTable(
                name: "extensions");

            migrationBuilder.DropTable(
                name: "thumbnails",
                schema: "viderhe_sapressc_middleware_media");

            migrationBuilder.DropTable(
                name: "states",
                schema: "viderhe_sapressc_middleware_region");

            migrationBuilder.DropTable(
                name: "countries",
                schema: "viderhe_sapressc_middleware_region");
        }
    }
}
