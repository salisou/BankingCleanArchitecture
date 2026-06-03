using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filiali",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodiceFiliale = table.Column<string>(type: "TEXT", nullable: false),
                    NomeFiliale = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Indirizzo = table.Column<string>(type: "TEXT", nullable: false),
                    Citta = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CAP = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiali", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prestiti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodiceContratto = table.Column<string>(type: "TEXT", nullable: false),
                    ImportoErogato = table.Column<decimal>(type: "TEXT", nullable: false),
                    CapitaleResiduo = table.Column<decimal>(type: "TEXT", nullable: false),
                    TassoInteresse = table.Column<decimal>(type: "TEXT", nullable: false),
                    DurataMesi = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInizio = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StatoPrestito = table.Column<string>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestiti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clienti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodiceCliente = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    CodiceFiscale = table.Column<string>(type: "TEXT", nullable: false),
                    PartitaIva = table.Column<string>(type: "TEXT", nullable: true),
                    TipoCliente = table.Column<string>(type: "TEXT", nullable: false),
                    DataNascitaCostituzione = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: false),
                    IndirizzoResidenza = table.Column<string>(type: "TEXT", nullable: false),
                    FilialeRegistrazioneId = table.Column<int>(type: "INTEGER", nullable: false),
                    FilialeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clienti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clienti_Filiali_FilialeId",
                        column: x => x.FilialeId,
                        principalTable: "Filiali",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dipendenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Matricola = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    Ruolo = table.Column<string>(type: "TEXT", nullable: false),
                    FilialeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dipendenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dipendenti_Filiali_FilialeId",
                        column: x => x.FilialeId,
                        principalTable: "Filiali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatePrestito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrestitoId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroRata = table.Column<int>(type: "INTEGER", nullable: false),
                    DataScadenza = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ImportoRata = table.Column<decimal>(type: "TEXT", nullable: false),
                    StatoPagamento = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatePrestito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatePrestito_Prestiti_PrestitoId",
                        column: x => x.PrestitoId,
                        principalTable: "Prestiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContiCorrenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IBAN = table.Column<string>(type: "TEXT", nullable: false),
                    SaldoContabile = table.Column<decimal>(type: "TEXT", nullable: false),
                    SaldoDisponibile = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataApertura = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StatoConto = table.Column<string>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContiCorrenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContiCorrenti_Clienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DossierTitoli",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodiceDossier = table.Column<string>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataApertura = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ValoreTotalePortafoglio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossierTitoli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DossierTitoli_Clienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroCarta = table.Column<string>(type: "TEXT", nullable: false),
                    TipoCarta = table.Column<string>(type: "TEXT", nullable: false),
                    Circuito = table.Column<string>(type: "TEXT", nullable: false),
                    DataScadenza = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CVV = table.Column<string>(type: "TEXT", nullable: false),
                    PinHash = table.Column<string>(type: "TEXT", nullable: false),
                    PlafondMensile = table.Column<decimal>(type: "TEXT", nullable: false),
                    StatoCarta = table.Column<string>(type: "TEXT", nullable: false),
                    ContoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carte_ContiCorrenti_ContoId",
                        column: x => x.ContoId,
                        principalTable: "ContiCorrenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoTransazione = table.Column<string>(type: "TEXT", nullable: false),
                    Importo = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataOra = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descrizione = table.Column<string>(type: "TEXT", nullable: false),
                    IBANControparte = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transazioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transazioni_ContiCorrenti_ContoId",
                        column: x => x.ContoId,
                        principalTable: "ContiCorrenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortafoglioTitoli",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DossierId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ticker = table.Column<string>(type: "TEXT", nullable: false),
                    NomeStrumento = table.Column<string>(type: "TEXT", nullable: false),
                    Quantita = table.Column<int>(type: "INTEGER", nullable: false),
                    PrezzoMedioCarico = table.Column<decimal>(type: "TEXT", nullable: false),
                    DossierTitoliId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortafoglioTitoli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortafoglioTitoli_DossierTitoli_DossierTitoliId",
                        column: x => x.DossierTitoliId,
                        principalTable: "DossierTitoli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carte_ContoId",
                table: "Carte",
                column: "ContoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clienti_FilialeId",
                table: "Clienti",
                column: "FilialeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContiCorrenti_ClienteId",
                table: "ContiCorrenti",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Dipendenti_FilialeId",
                table: "Dipendenti",
                column: "FilialeId");

            migrationBuilder.CreateIndex(
                name: "IX_DossierTitoli_ClienteId",
                table: "DossierTitoli",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PortafoglioTitoli_DossierTitoliId",
                table: "PortafoglioTitoli",
                column: "DossierTitoliId");

            migrationBuilder.CreateIndex(
                name: "IX_RatePrestito_PrestitoId",
                table: "RatePrestito",
                column: "PrestitoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transazioni_ContoId",
                table: "Transazioni",
                column: "ContoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carte");

            migrationBuilder.DropTable(
                name: "Dipendenti");

            migrationBuilder.DropTable(
                name: "PortafoglioTitoli");

            migrationBuilder.DropTable(
                name: "RatePrestito");

            migrationBuilder.DropTable(
                name: "Transazioni");

            migrationBuilder.DropTable(
                name: "DossierTitoli");

            migrationBuilder.DropTable(
                name: "Prestiti");

            migrationBuilder.DropTable(
                name: "ContiCorrenti");

            migrationBuilder.DropTable(
                name: "Clienti");

            migrationBuilder.DropTable(
                name: "Filiali");
        }
    }
}
