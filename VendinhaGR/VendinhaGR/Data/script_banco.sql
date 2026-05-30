CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Clientes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Clientes" PRIMARY KEY AUTOINCREMENT,
    "Nome" TEXT NOT NULL,
    "CPF" TEXT NOT NULL,
    "DataNascimento" TEXT NOT NULL,
    "Email" TEXT NULL
);

CREATE TABLE "Dividas" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Dividas" PRIMARY KEY AUTOINCREMENT,
    "Valor" TEXT NOT NULL,
    "Paga" INTEGER NOT NULL,
    "DataCriacao" TEXT NOT NULL,
    "DataPagamento" TEXT NULL,
    "ClienteId" INTEGER NOT NULL,
    CONSTRAINT "FK_Dividas_Clientes_ClienteId" FOREIGN KEY ("ClienteId") REFERENCES "Clientes" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Clientes_CPF" ON "Clientes" ("CPF");

CREATE INDEX "IX_Dividas_ClienteId" ON "Dividas" ("ClienteId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260530071445_InicializacaoBanco', '10.0.8');

COMMIT;

