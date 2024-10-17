-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: defaultdb
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('262c3dfa-e36b-4e2b-a4c8-4d1a058d917e','Gerente','GERENTE',NULL),('63625631-b053-4af1-886b-2d89ea2fa62e','Usuario','USUARIO',NULL),('b972dacc-2bc5-4c78-ab6a-12198c98a3db','Administrador','ADMINISTRADOR',NULL);
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('dabeb2c6-4598-472d-bcb4-fef7ea12fd70','262c3dfa-e36b-4e2b-a4c8-4d1a058d917e'),('e355beef-b062-4bf8-8337-96982b9df3f7','63625631-b053-4af1-886b-2d89ea2fa62e'),('35991d2f-c76d-4176-ad99-2e358538618e','b972dacc-2bc5-4c78-ab6a-12198c98a3db'),('45cc817c-b1f5-4f59-ae2b-b9874338f4ca','b972dacc-2bc5-4c78-ab6a-12198c98a3db');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `FirstName` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  `GerenteID` varchar(255) DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`(255)),
  KEY `EmailIndex` (`NormalizedEmail`(255)),
  KEY `IX_AspNetUsers_GerenteID` (`GerenteID`),
  CONSTRAINT `FK_AspNetUsers_AspNetUsers_GerenteID` FOREIGN KEY (`GerenteID`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('35991d2f-c76d-4176-ad99-2e358538618e','Administrador','CodeData','2024-09-27 11:54:53',NULL,NULL,'Admin','ADMIN','admin@codedata.com.br','ADMIN@CODEDATA.COM.BR',1,'AQAAAAIAAYagAAAAEBQwfEERims5riNZRumw2yA8ms1CUriSkrFsuXBMl4d8s4oadnqTML8yzv91v4HNyw==','IGNOEPEE4RRUTFJWKWULGYGRGYBZTLAD','e46e9b05-315e-44e5-8eb4-a1c3037974fa',NULL,0,0,NULL,1,0),('45cc817c-b1f5-4f59-ae2b-b9874338f4ca','Marcos','Melo','2024-09-26 00:55:54',NULL,NULL,'Marcos','MARCOS','marcos@gmail.com','MARCOS@GMAIL.COM',1,'AQAAAAIAAYagAAAAEJJWIBQbiSnm6I9hPbVnNU1TQnqMhJXGZkaMmP2UAbEfnpLpecYuUjoObbNB5vdpgw==','ZBIBOGLBY3ICPSZJW5I6H4UNS4DMTUX2','d925b2b5-4624-43d2-878c-e93a9af72091',NULL,0,0,NULL,1,0),('dabeb2c6-4598-472d-bcb4-fef7ea12fd70','Gerente','CodeData','2024-09-27 11:54:22',NULL,NULL,'Gerente','GERENTE','gerente@codedata.com.br','GERENTE@CODEDATA.COM.BR',1,'AQAAAAIAAYagAAAAEIkCY+FenX2JglZMvphadENgeTYbKKlvGRqi+WtlTF8NtSVaMFdDpVK9ND+Ty2BWEQ==','N3Q6HUJIM2VRFBQTUJTBU7WOZRU23QWY','ec39865f-916c-4248-822e-3eebef655f05',NULL,0,0,NULL,1,0),('e355beef-b062-4bf8-8337-96982b9df3f7','Usuario','CodeData','2024-09-27 11:53:50',NULL,'dabeb2c6-4598-472d-bcb4-fef7ea12fd70','Usuario','USUARIO','usuario@codedata.com.br','USUARIO@CODEDATA.COM.BR',1,'AQAAAAIAAYagAAAAEG09EgA/VmmqqIldscGRCEPD7Q5hvHPu+thkrIQsqgjX3EHREgkKpr3lf8aZKIciYA==','SVUEIEM7K2X6VPM2OS7WDTZPGC2CQDU5','2accbba2-c8d4-4bb0-9ef0-749cc53c60ec',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clientes`
--

DROP TABLE IF EXISTS `clientes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `CNPJ` varchar(20) NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  `EnderecoId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Clientes_EnderecoId` (`EnderecoId`),
  CONSTRAINT `FK_Clientes_Enderecos_EnderecoId` FOREIGN KEY (`EnderecoId`) REFERENCES `enderecos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--

LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES (1,'Cliente 1','00.000.000/0001-01','2024-09-27 11:50:49',NULL,3),(2,'Cliente 2','00.000.000/0001-02','2024-09-27 11:50:49',NULL,5),(3,'Cliente 3','00.000.000/0001-03','2024-09-27 11:50:49',NULL,6),(4,'Cliente 4','00.000.000/0001-04','2024-09-27 11:50:49',NULL,7),(5,'Cliente 5','00.000.000/0001-05','2024-09-27 11:50:49',NULL,8),(6,'Cliente 6','00.000.000/0001-06','2024-09-27 11:50:49',NULL,9),(7,'Cliente 7','00.000.000/0001-07','2024-09-27 11:50:49',NULL,10);
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contratos`
--

DROP TABLE IF EXISTS `contratos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contratos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SolicitacaoId` int NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  `DocumentoId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Contratos_DocumentoId` (`DocumentoId`),
  KEY `IX_Contratos_SolicitacaoId` (`SolicitacaoId`),
  CONSTRAINT `FK_Contratos_Documentos_DocumentoId` FOREIGN KEY (`DocumentoId`) REFERENCES `documentos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Contratos_Solicitacoes_SolicitacaoId` FOREIGN KEY (`SolicitacaoId`) REFERENCES `solicitacoes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contratos`
--

LOCK TABLES `contratos` WRITE;
/*!40000 ALTER TABLE `contratos` DISABLE KEYS */;
INSERT INTO `contratos` VALUES (1,1,'2024-09-27 12:05:19',NULL,6),(2,2,'2024-09-27 12:05:19',NULL,7),(3,3,'2024-09-27 12:05:19',NULL,8),(4,4,'2024-09-27 12:05:19',NULL,9),(5,5,'2024-09-27 12:05:19',NULL,10),(6,6,'2024-09-27 12:05:19',NULL,11),(7,7,'2024-09-27 12:05:19',NULL,12);
/*!40000 ALTER TABLE `contratos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `documentos`
--

DROP TABLE IF EXISTS `documentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `documentos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Numero` varchar(45) NOT NULL,
  `Nome` varchar(100) NOT NULL,
  `Anexo` mediumblob NOT NULL,
  `Tipo` varchar(20) NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documentos`
--

LOCK TABLES `documentos` WRITE;
/*!40000 ALTER TABLE `documentos` DISABLE KEYS */;
INSERT INTO `documentos` VALUES (1,'25698','NF NKR','','Sa√≠da','2024-09-27 11:44:15',NULL),(2,'35687','NF EQUIP','','Entrada','2024-09-27 11:44:15',NULL),(3,'45876','NF 7MZ','','Sa√≠da','2024-09-27 11:44:15',NULL),(4,'58964','NF MHRAP','','Entrada','2024-09-27 11:44:15',NULL),(5,'69875','NF VMZ','','Sa√≠da','2024-09-27 11:44:15',NULL),(6,'74589','Contrato 1','','Loca√ß√£o','2024-09-27 11:44:15',NULL),(7,'85764','Contrato 2','','Loca√ß√£o','2024-09-27 11:44:15',NULL),(8,'96874','Contrato 3','','Loca√ß√£o','2024-09-27 11:44:15',NULL),(9,'10345','Contrato 4','','Loca√ß√£o','2024-09-27 11:44:15',NULL),(10,'11458','Contrato 5','','Loca√ß√£o','2024-09-27 11:44:15',NULL),(11,'16548','Contrato 6','','Homologa√ß√£o','2024-09-27 12:03:48',NULL),(12,'12354','Contrato 7','','Homologa√ß√£o','2024-09-27 12:03:48',NULL);
/*!40000 ALTER TABLE `documentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `enderecos`
--

DROP TABLE IF EXISTS `enderecos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enderecos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CEP` varchar(9) NOT NULL,
  `Rua` varchar(255) NOT NULL,
  `Numero` int NOT NULL,
  `Bairro` varchar(255) NOT NULL,
  `Cidade` varchar(255) NOT NULL,
  `Estado` varchar(30) NOT NULL,
  `Complemento` varchar(500) NOT NULL,
  `Localizacao` point NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enderecos`
--

LOCK TABLES `enderecos` WRITE;
/*!40000 ALTER TABLE `enderecos` DISABLE KEYS */;
INSERT INTO `enderecos` VALUES (1,'12345-678','Rua das Flores',101,'Centro','S√£o Paulo','S√£o Paulo','Apto 12',_binary '\0\0\0\0\0\0\0>\ÀÛ\‡\Óå7¿ò°ÒDQG¿'),(2,'98765-432','Avenida Paulista',900,'Bela Vista','S√£o Paulo','S√£o Paulo','Cobertura',_binary '\0\0\0\0\0\0\0Å^∏saê7¿:ì6USG¿'),(3,'54321-987','Rua Santos Dumont',325,'Jardim das Am√©ricas','Curitiba','Paran√°','Casa',_binary '\0\0\0\0\0\0\0]\ﬁÆ\’r9¿Ûë\ÔRûH¿'),(4,'65432-109','Avenida Boa Viagem',205,'Boa Viagem','Recife','Pernambuco','Casa',_binary '\0\0\0\0\0\0\0y$^û\Œ= ¿àÚ-$pA¿'),(5,'87654-321','Rua Carlos Gomes',88,'Centro','Salvador','Bahia','Apto 301',_binary '\0\0\0\0\0\0\0ÜØØu©Ò)¿ıL/1ñAC¿'),(6,'11223-344','Avenida Brasil',700,'Centro','Rio de Janeiro','Rio de Janeiro','Cobertura',_binary '\0\0\0\0\0\0\0=c_≤Ò\Ë6¿ª%9`WóE¿'),(7,'77889-001','Rua Tiradentes',122,'Jardim Progresso','S√£o Paulo','S√£o Paulo','Loja',_binary '\0\0\0\0\0\0\0b¢A\nûÇ7¿\'›ñ\»]G¿'),(8,'66778-990','Avenida das Na√ß√µes Unidas',304,'Pinheiros','S√£o Paulo','S√£o Paulo','Escrit√≥rio',_binary '\0\0\0\0\0\0\0\ÿfc%\Êë7¿;Ú\œZG¿'),(9,'33221-556','Rua Professor Pedro Pinto',450,'Jardim Europa','Belo Horizonte','Minas Gerais','Casa',_binary '\0\0\0\0\0\0\0wº\…o\Ì3¿k) \Ì¯E¿'),(10,'99887-776','Rua Marechal Deodoro',250,'Centro','Porto Alegre','Rio Grande do Sul','Apto 401',_binary '\0\0\0\0\0\0\0mésõ>¿\Óy˛¥QùI¿');
/*!40000 ALTER TABLE `enderecos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `equipamentos`
--

DROP TABLE IF EXISTS `equipamentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `equipamentos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Codigo` varchar(45) NOT NULL,
  `Modelo` varchar(70) NOT NULL,
  `Descricao` text NOT NULL,
  `Marca` varchar(45) NOT NULL,
  `SerialNumber` varchar(50) NOT NULL,
  `PartNumber` varchar(50) NOT NULL,
  `Condicao` tinyint(1) NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  `EstoqueId` int NOT NULL,
  `DocumentoId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Equipamentos_DocumentoId` (`DocumentoId`),
  KEY `IX_Equipamentos_EstoqueId` (`EstoqueId`),
  CONSTRAINT `FK_Equipamentos_Documentos_DocumentoId` FOREIGN KEY (`DocumentoId`) REFERENCES `documentos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Equipamentos_Estoques_EstoqueId` FOREIGN KEY (`EstoqueId`) REFERENCES `estoques` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `equipamentos`
--

LOCK TABLES `equipamentos` WRITE;
/*!40000 ALTER TABLE `equipamentos` DISABLE KEYS */;
INSERT INTO `equipamentos` VALUES (1,'COL001','Zebra TC57','Coletor de dados m√≥vel com Android','Zebra','SN12345A','PNTC57001',1,'2024-09-27 11:49:09',NULL,1,3),(2,'COL002','Honeywell EDA51','Coletor de dados com leitor de c√≥digo de barras','Honeywell','SN98765B','PNEDA51002',1,'2024-09-27 11:49:09',NULL,2,2),(3,'COL003','Datalogic Memor 10','Coletor de dados robusto Android','Datalogic','SN65432C','PNMEMOR1003',1,'2024-09-27 11:49:09',NULL,1,4),(4,'COL004','Cognex MX-1502','Leitor de c√≥digo de barras com coletor de dados integrado','Cognex','SN87654D','PNMX1502004',1,'2024-09-27 11:49:09',NULL,1,4),(5,'COL005','Intermec CK3X','Coletor de dados com teclado f√≠sico','Intermec','SN23456E','PNCK3X005',1,'2024-09-27 11:49:09',NULL,2,4),(6,'COL006','Zebra MC3300','Coletor de dados com pistola de leitura','Zebra','SN34567F','PNMC330006',1,'2024-09-27 11:49:09',NULL,4,2),(7,'COL007','Honeywell Dolphin CT60','Coletor de dados 4G LTE Android','Honeywell','SN45678G','PNCT60007',1,'2024-09-27 11:49:09',NULL,2,2),(8,'COL008','Datalogic Falcon X4','Coletor de dados ergon√¥mico com teclado','Datalogic','SN56789H','PNFALCONX4008',1,'2024-09-27 11:49:09',NULL,2,2),(9,'COL009','Cognex MX-1000','Coletor de dados industrial','Cognex','SN67890I','PNMX1000009',1,'2024-09-27 11:49:09',NULL,4,2),(10,'COL010','Intermec CN51','Coletor de dados com tela sens√≠vel ao toque','Intermec','SN78901J','PNCN510010',1,'2024-09-27 11:49:09',NULL,4,2),(11,'COL011','Zebra TC52','Coletor de dados robusto com Android','Zebra','SN11223K','PNTC52011',1,'2024-09-27 11:49:09',NULL,1,4),(12,'COL012','Honeywell CK75','Coletor de dados com teclado alfanum√©rico','Honeywell','SN33445L','PNCK75012',1,'2024-09-27 11:49:09',NULL,1,4),(13,'COL013','Datalogic Skorpio X5','Coletor de dados ergon√¥mico com teclado','Datalogic','SN55667M','PNSKORPIOX5013',1,'2024-09-27 11:49:09',NULL,2,2),(14,'COL014','Cognex MX-1000','Coletor de dados industrial','Cognex','SN77889N','PNMX1000014',1,'2024-09-27 11:49:09',NULL,2,4),(15,'COL015','Intermec CN70','Coletor de dados com teclado f√≠sico','Intermec','SN99001O','PNCN70015',1,'2024-09-27 11:49:09',NULL,2,4),(16,'COL016','Zebra TC21','Coletor de dados compacto com Android','Zebra','SN23456P','PNTC21016',1,'2024-09-27 11:49:09',NULL,2,4),(17,'COL017','Honeywell EDA71','Tablet robusto com coletor de dados','Honeywell','SN34567Q','PNEDA71017',1,'2024-09-27 11:49:09',NULL,4,2),(18,'COL018','Datalogic Memor K','Coletor de dados leve e compacto','Datalogic','SN45678R','PNMEMORK018',1,'2024-09-27 11:49:09',NULL,4,2),(19,'COL019','Cognex MX-1502','Leitor de c√≥digo de barras industrial','Cognex','SN56789S','PNMX1502019',1,'2024-09-27 11:49:09',NULL,4,2),(20,'COL020','Intermec CK75','Coletor de dados robusto com teclado','Intermec','SN67890T','PNCK75020',1,'2024-09-27 11:49:09',NULL,2,2),(21,'COL021','Zebra MC9200','Coletor de dados robusto com teclado f√≠sico','Zebra','SN78901U','PNMC920021',1,'2024-09-27 11:49:09',NULL,4,2),(22,'COL022','Honeywell Dolphin 75e','Coletor de dados multiuso','Honeywell','SN89012V','PND75022',0,'2024-09-27 11:49:09',NULL,4,2),(23,'COL023','Datalogic Joya Touch','Coletor de dados ergon√¥mico com tela sens√≠vel ao toque','Datalogic','SN90123W','PNJOYATOUCH023',1,'2024-09-27 11:49:09',NULL,1,4),(24,'COL024','Cognex MX-1000','Coletor de dados com foco industrial','Cognex','SN01234X','PNMX1000024',1,'2024-09-27 11:49:09',NULL,1,2),(25,'COL025','Intermec CN70e','Coletor de dados robusto com teclado','Intermec','SN12345Y','PNCN70025',0,'2024-09-27 11:49:09',NULL,1,4),(26,'COL026','Zebra TC72','Coletor de dados com Android para ambientes industriais','Zebra','SN23456Z','PNTC72026',1,'2024-09-27 11:49:09',NULL,1,4),(27,'COL027','Honeywell Dolphin CT40','Coletor de dados leve e compacto','Honeywell','SN34567AA','PNCT40027',0,'2024-09-27 11:49:09',NULL,1,4),(28,'COL028','Datalogic Memor 1','Coletor de dados Android compacto','Datalogic','SN45678BB','PNMEMOR10028',1,'2024-09-27 11:49:09',NULL,2,2),(29,'COL029','Cognex MX-1000','Coletor de dados para ambientes industriais','Cognex','SN56789CC','PNMX1000029',0,'2024-09-27 11:49:09',NULL,1,2),(30,'COL030','Intermec CK3R','Coletor de dados leve com teclado f√≠sico','Intermec','SN67890DD','PNCK3R030',1,'2024-09-27 11:49:09',NULL,2,2);
/*!40000 ALTER TABLE `equipamentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estoques`
--

DROP TABLE IF EXISTS `estoques`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estoques` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `EnderecoId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Estoques_EnderecoId` (`EnderecoId`),
  CONSTRAINT `FK_Estoques_Enderecos_EnderecoId` FOREIGN KEY (`EnderecoId`) REFERENCES `enderecos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estoques`
--

LOCK TABLES `estoques` WRITE;
/*!40000 ALTER TABLE `estoques` DISABLE KEYS */;
INSERT INTO `estoques` VALUES (1,'CodeData Campinas',1),(2,'CodeData S√£o Paulo',2),(4,'CodeData Nordeste',4);
/*!40000 ALTER TABLE `estoques` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movimentacoesequipamentos`
--

DROP TABLE IF EXISTS `movimentacoesequipamentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `movimentacoesequipamentos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EquipamentoId` int NOT NULL,
  `EnderecoId` int NOT NULL,
  `SolicitacaoId` int NOT NULL,
  `DocumentoId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_MovimentacoesEquipamentos_DocumentoId` (`DocumentoId`),
  KEY `IX_MovimentacoesEquipamentos_EnderecoId` (`EnderecoId`),
  KEY `IX_MovimentacoesEquipamentos_EquipamentoId` (`EquipamentoId`),
  KEY `IX_MovimentacoesEquipamentos_SolicitacaoId` (`SolicitacaoId`),
  CONSTRAINT `FK_MovimentacoesEquipamentos_Documentos_DocumentoId` FOREIGN KEY (`DocumentoId`) REFERENCES `documentos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_MovimentacoesEquipamentos_Enderecos_EnderecoId` FOREIGN KEY (`EnderecoId`) REFERENCES `enderecos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_MovimentacoesEquipamentos_Equipamentos_EquipamentoId` FOREIGN KEY (`EquipamentoId`) REFERENCES `equipamentos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_MovimentacoesEquipamentos_Solicitacoes_SolicitacaoId` FOREIGN KEY (`SolicitacaoId`) REFERENCES `solicitacoes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movimentacoesequipamentos`
--

LOCK TABLES `movimentacoesequipamentos` WRITE;
/*!40000 ALTER TABLE `movimentacoesequipamentos` DISABLE KEYS */;
INSERT INTO `movimentacoesequipamentos` VALUES (1,1,5,2,7),(2,2,5,2,7),(3,3,5,2,7),(4,4,5,2,7),(5,5,5,2,7),(6,6,7,4,9),(7,7,7,4,9),(8,8,7,4,9),(9,9,7,4,9),(10,10,9,6,11),(11,11,9,6,11),(12,12,9,6,11),(13,13,9,6,11),(14,14,3,1,6),(15,15,3,1,6),(16,16,6,3,8),(17,17,8,5,10),(18,18,8,5,10),(19,19,10,7,12),(20,20,10,7,12);
/*!40000 ALTER TABLE `movimentacoesequipamentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificacoes`
--

DROP TABLE IF EXISTS `notificacoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificacoes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Titulo` varchar(100) NOT NULL,
  `Mensagem` text NOT NULL,
  `Visualizado` tinyint(1) NOT NULL DEFAULT '0',
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Notificacoes_UserId` (`UserId`),
  CONSTRAINT `FK_Notificacoes_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacoes`
--

LOCK TABLES `notificacoes` WRITE;
/*!40000 ALTER TABLE `notificacoes` DISABLE KEYS */;
INSERT INTO `notificacoes` VALUES (1,'TESTE 1','Mensagem de teste 1',0,'2024-10-02 13:24:46','e355beef-b062-4bf8-8337-96982b9df3f7'),(2,'TESTE 1','Mensagem de teste 1',0,'2024-10-02 13:26:46','45cc817c-b1f5-4f59-ae2b-b9874338f4ca');
/*!40000 ALTER TABLE `notificacoes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `solicitacoes`
--

DROP TABLE IF EXISTS `solicitacoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `solicitacoes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Tipo` tinyint(1) NOT NULL DEFAULT '0',
  `Numero` varchar(50) NOT NULL,
  `DataInicio` datetime NOT NULL,
  `DataFinal` datetime NOT NULL,
  `Descricao` text NOT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataAtualizado` datetime DEFAULT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClienteId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Solicitacoes_ClienteId` (`ClienteId`),
  KEY `IX_Solicitacoes_UserId` (`UserId`),
  CONSTRAINT `FK_Solicitacoes_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Solicitacoes_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `clientes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `solicitacoes`
--

LOCK TABLES `solicitacoes` WRITE;
/*!40000 ALTER TABLE `solicitacoes` DISABLE KEYS */;
INSERT INTO `solicitacoes` VALUES (1,1,'001','2024-01-01 00:00:00','2024-01-31 00:00:00','Descri√ß√£o da solicita√ß√£o 1','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',1),(2,1,'002','2024-02-01 00:00:00','2024-02-28 00:00:00','Descri√ß√£o da solicita√ß√£o 2','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',2),(3,1,'003','2024-03-01 00:00:00','2024-03-31 00:00:00','Descri√ß√£o da solicita√ß√£o 3','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',3),(4,1,'004','2024-04-01 00:00:00','2024-04-30 00:00:00','Descri√ß√£o da solicita√ß√£o 4','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',4),(5,1,'005','2024-05-01 00:00:00','2024-05-31 00:00:00','Descri√ß√£o da solicita√ß√£o 5','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',5),(6,0,'006','2024-06-01 00:00:00','2024-06-30 00:00:00','Descri√ß√£o da solicita√ß√£o 6','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',6),(7,0,'007','2024-07-01 00:00:00','2024-07-31 00:00:00','Descri√ß√£o da solicita√ß√£o 7','2024-09-27 12:00:10',NULL,'e355beef-b062-4bf8-8337-96982b9df3f7',7);
/*!40000 ALTER TABLE `solicitacoes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-10-17 17:06:34
