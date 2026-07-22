-- MySQL dump 10.13  Distrib 8.0.12, for Win64 (x86_64)
--
-- Host: localhost    Database: aycnucleo
-- ------------------------------------------------------
-- Server version	8.0.12
use aycnucleo;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
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
 SET character_set_client = utf8mb4 ;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `acta`
--

DROP TABLE IF EXISTS `acta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `acta` (
  `Id_Acta` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha_Celebrado` datetime(6) NOT NULL,
  `NoActa` int(11) NOT NULL,
  `Completado` bit(1) NOT NULL DEFAULT b'0',
  `Fecha_Completado` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id_Acta`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `activo_negocio`
--

DROP TABLE IF EXISTS `activo_negocio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `activo_negocio` (
  `Id_Activo_Negocio` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Estado` longtext,
  `Id_Evaluacion` int(11) NOT NULL,
  `Nombre` longtext,
  `Precio_Compra` decimal(65,30) NOT NULL,
  `Precio_Peritado` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Activo_Negocio`),
  KEY `IX_Activo_Negocio_Id_Evaluacion` (`Id_Evaluacion`),
  CONSTRAINT `FK_Activo_Negocio_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `agencia`
--

DROP TABLE IF EXISTS `agencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `agencia` (
  `Id_Agencia` int(11) NOT NULL AUTO_INCREMENT,
  `Codigo_Agencia` longtext,
  `Nombre` longtext,
  PRIMARY KEY (`Id_Agencia`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ahorro`
--

DROP TABLE IF EXISTS `ahorro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ahorro` (
  `Id_Ahorro` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha_Retiro` datetime(6) NOT NULL,
  `Id_Linea_Ahorro` int(11) NOT NULL,
  `Id_Persona` int(11) NOT NULL,
  `No_Ahorro` longtext,
  `Saldo_Actual` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Ultima_Fecha_Traslado` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Fecha_Apertura` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id_Ahorro`),
  KEY `IX_Ahorro_Id_Linea_Ahorro` (`Id_Linea_Ahorro`),
  KEY `IX_Ahorro_Id_Persona` (`Id_Persona`),
  CONSTRAINT `FK_Ahorro_Linea_Ahorro_Id_Linea_Ahorro` FOREIGN KEY (`Id_Linea_Ahorro`) REFERENCES `linea_ahorro` (`id_linea_ahorro`) ON DELETE CASCADE,
  CONSTRAINT `FK_Ahorro_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `banco`
--

DROP TABLE IF EXISTS `banco`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `banco` (
  `Id_Banco` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `No_Cuenta` longtext,
  `Nombre` longtext,
  PRIMARY KEY (`Id_Banco`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `caja`
--

DROP TABLE IF EXISTS `caja`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `caja` (
  `Id_Caja` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Agencia` int(11) NOT NULL,
  `Id_Usuario` longtext,
  `Nombre_PC` longtext,
  `Activa` bit(1) NOT NULL DEFAULT b'0',
  `Boveda` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id_Caja`),
  KEY `IX_Caja_Id_Agencia` (`Id_Agencia`),
  CONSTRAINT `FK_Caja_Agencia_Id_Agencia` FOREIGN KEY (`Id_Agencia`) REFERENCES `agencia` (`id_agencia`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cartera`
--

DROP TABLE IF EXISTS `cartera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cartera` (
  `Id_Cartera` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Usuario` longtext,
  `No` varchar(2) DEFAULT NULL,
  `Nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id_Cartera`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `categoria_gasto`
--

DROP TABLE IF EXISTS `categoria_gasto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `categoria_gasto` (
  `Id_Categoria_Gasto` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Categoria_Gasto`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cobro_periodico`
--

DROP TABLE IF EXISTS `cobro_periodico`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cobro_periodico` (
  `Id_Cobro_Periodico` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Linea_Credito` int(11) NOT NULL,
  `Nombre` longtext,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Cobro_Periodico`),
  KEY `IX_Cobro_Periodico_Id_Linea_Credito` (`Id_Linea_Credito`),
  CONSTRAINT `FK_Cobro_Periodico_Linea_Credito_Id_Linea_Credito` FOREIGN KEY (`Id_Linea_Credito`) REFERENCES `linea_credito` (`id_linea`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cobro_periodico_historial`
--

DROP TABLE IF EXISTS `cobro_periodico_historial`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cobro_periodico_historial` (
  `Id_Cobro_Periodico_Historial` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Linea_Credito_Historial` int(11) NOT NULL,
  `Nombre` longtext,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Cobro_Periodico_Historial`),
  KEY `IX_Cobro_Periodico_Historial_Id_Linea_Credito_Historial` (`Id_Linea_Credito_Historial`),
  CONSTRAINT `FK_Cobro_Periodico_Historial_Linea_Credito_Historial_Id_Linea_Cr` FOREIGN KEY (`Id_Linea_Credito_Historial`) REFERENCES `linea_credito_historial` (`id_linea_credito_historial`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `colecta`
--

DROP TABLE IF EXISTS `colecta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `colecta` (
  `Id_Colecta` int(11) NOT NULL AUTO_INCREMENT,
  `Anulado` bit(1) NOT NULL,
  `Borrado_Logico` bit(1) NOT NULL,
  `Codigo_AHorro` longtext,
  `Codigo_Credito` longtext,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `DUI` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Id_Ahorro` int(11) NOT NULL,
  `Id_Cliente` int(11) NOT NULL,
  `Id_Credito` int(11) NOT NULL,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `Monto` decimal(65,30) NOT NULL,
  `No_Comprobante` longtext,
  `Nombre_Completo` longtext,
  `Procesado` bit(1) NOT NULL,
  `Id_Colecta_Diaria` int(11) NOT NULL DEFAULT '0',
  `Id_Transaccion` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id_Colecta`),
  KEY `IX_Colecta_Id_Colecta_Diaria` (`Id_Colecta_Diaria`),
  CONSTRAINT `FK_Colecta_Colecta_Diaria_Id_Colecta_Diaria` FOREIGN KEY (`Id_Colecta_Diaria`) REFERENCES `colecta_diaria` (`id_colecta_diaria`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `colecta_diaria`
--

DROP TABLE IF EXISTS `colecta_diaria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `colecta_diaria` (
  `Id_Colecta_Diaria` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Fecha` datetime(6) NOT NULL,
  `Id_Usuario` longtext,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `Procesado_Total` bit(1) NOT NULL DEFAULT b'0',
  `No_Comprobante_Final` longtext,
  `No_Comprobante_Inicio` longtext,
  PRIMARY KEY (`Id_Colecta_Diaria`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `comite`
--

DROP TABLE IF EXISTS `comite`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `comite` (
  `Id_Comite` int(11) NOT NULL AUTO_INCREMENT,
  `Ampliado` bit(1) NOT NULL,
  `Aprobado` bit(1) NOT NULL,
  `Comentario` longtext,
  `Fecha_Verificado` datetime(6) NOT NULL,
  `Id_Credito` int(11) NOT NULL,
  `Id_Usuario` longtext,
  `Verificado` bit(1) NOT NULL,
  PRIMARY KEY (`Id_Comite`),
  KEY `IX_Comite_Id_Credito` (`Id_Credito`),
  CONSTRAINT `FK_Comite_Credito_Id_Credito` FOREIGN KEY (`Id_Credito`) REFERENCES `credito` (`id_credito`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `configuracion_comite_rol`
--

DROP TABLE IF EXISTS `configuracion_comite_rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `configuracion_comite_rol` (
  `Id_Configuracion_Comite` int(11) NOT NULL AUTO_INCREMENT,
  `Ampliado` bit(1) NOT NULL,
  `Rol_Nombre` longtext,
  PRIMARY KEY (`Id_Configuracion_Comite`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `configuracion_contabilidad`
--

DROP TABLE IF EXISTS `configuracion_contabilidad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `configuracion_contabilidad` (
  `Id_Configuracion_Contabilidad` int(11) NOT NULL AUTO_INCREMENT,
  `Concepto` longtext,
  `Id_Cuenta` int(11) NOT NULL,
  `Id_Operacion` int(11) NOT NULL,
  `Identificador` longtext,
  `Descripcion` longtext,
  `Modo_Almacen_Valor` longtext,
  `Id_Definicion_Transaccion` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id_Configuracion_Contabilidad`),
  KEY `IX_Configuracion_Contabilidad_Id_Definicion_Transaccion` (`Id_Definicion_Transaccion`),
  KEY `IX_Configuracion_Contabilidad_Id_Operacion` (`Id_Operacion`),
  CONSTRAINT `FK_Configuracion_Contabilidad_Definicion_Transaccion_Id_Definici` FOREIGN KEY (`Id_Definicion_Transaccion`) REFERENCES `definicion_transaccion` (`id_definicion_transaccion`) ON DELETE CASCADE,
  CONSTRAINT `FK_Configuracion_Contabilidad_Operacion_Id_Operacion` FOREIGN KEY (`Id_Operacion`) REFERENCES `operacion` (`id_operacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `configuracion_general`
--

DROP TABLE IF EXISTS `configuracion_general`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `configuracion_general` (
  `Id_Configuracion` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` longtext,
  `Tipo` longtext,
  `Valor` longtext,
  PRIMARY KEY (`Id_Configuracion`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `costo_venta`
--

DROP TABLE IF EXISTS `costo_venta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `costo_venta` (
  `Id_Costo_Venta` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Evaluacion` int(11) NOT NULL,
  `Margen_Bruto` decimal(65,30) NOT NULL,
  `Margen_Bruto_Porcentaje` decimal(65,30) NOT NULL,
  `Precio_Compra` decimal(65,30) NOT NULL,
  `Precio_Venta` decimal(65,30) NOT NULL,
  `Unidad_Medida` longtext,
  PRIMARY KEY (`Id_Costo_Venta`),
  KEY `IX_Costo_Venta_Id_Evaluacion` (`Id_Evaluacion`),
  CONSTRAINT `FK_Costo_Venta_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `credito`
--

DROP TABLE IF EXISTS `credito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `credito` (
  `Id_Credito` int(11) NOT NULL AUTO_INCREMENT,
  `Aprobado` bit(1) NOT NULL,
  `Aprobado_En` datetime(6) NOT NULL,
  `Codigo_Credito` longtext,
  `Cuota_Total` decimal(65,30) NOT NULL,
  `Id_Linea` int(11) NOT NULL,
  `Id_Persona` int(11) NOT NULL,
  `Liquidado_En` datetime(6) NOT NULL,
  `Monto_pago` decimal(65,30) NOT NULL,
  `Numero_cuotas` int(11) NOT NULL,
  `PlanAyC` decimal(65,30) NOT NULL,
  `Plazo` int(11) NOT NULL,
  `Tasa_Interes` decimal(65,30) NOT NULL,
  `Valor_Cuota` decimal(65,30) NOT NULL,
  `Valor_IVA` decimal(65,30) NOT NULL,
  `Liquidado` bit(1) NOT NULL DEFAULT b'0',
  `Comentario` longtext,
  `Id_Acta` int(11) DEFAULT NULL,
  `Cancelado` bit(1) NOT NULL DEFAULT b'0',
  `Cancelado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Vencido` bit(1) NOT NULL DEFAULT b'0',
  `Vencido_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Activar_Deducciones` bit(1) NOT NULL DEFAULT b'0',
  `Codigo` longtext,
  `Descripcion_linea` longtext,
  `Destino_Credito` int(11) NOT NULL DEFAULT '0',
  `Dia_Gracia` int(11) NOT NULL DEFAULT '0',
  `Estructuracion` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Monto_Rango_Final` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Monto_Rango_Inicial` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `PlanAyC_Cobros_Periodico_Dinero` bit(1) NOT NULL DEFAULT b'0',
  `Plazo_Rango_Final` int(11) NOT NULL DEFAULT '0',
  `Plazo_Rango_Inicial` int(11) NOT NULL DEFAULT '0',
  `Recargo` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Tasa` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Tasa_Mora` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Tipo_Garantia` int(11) NOT NULL DEFAULT '0',
  `Id_Linea_Credito_Historial` int(11) NOT NULL DEFAULT '0',
  `Id_Periodo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id_Credito`),
  KEY `IX_Credito_Id_Linea` (`Id_Linea`),
  KEY `IX_Credito_Id_Persona` (`Id_Persona`),
  KEY `IX_Credito_Id_Acta` (`Id_Acta`),
  KEY `IX_Credito_Id_Periodo` (`Id_Periodo`),
  CONSTRAINT `FK_Credito_Acta_Id_Acta` FOREIGN KEY (`Id_Acta`) REFERENCES `acta` (`id_acta`),
  CONSTRAINT `FK_Credito_Linea_Credito_Id_Linea` FOREIGN KEY (`Id_Linea`) REFERENCES `linea_credito` (`id_linea`) ON DELETE CASCADE,
  CONSTRAINT `FK_Credito_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cuenta_cobrar`
--

DROP TABLE IF EXISTS `cuenta_cobrar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cuenta_cobrar` (
  `Id_Cuenta_Cobrar` int(11) NOT NULL AUTO_INCREMENT,
  `Completo` decimal(65,30) NOT NULL,
  `Id_Evaluacion` int(11) NOT NULL,
  `Recuperado` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Cuenta_Cobrar`),
  KEY `IX_Cuenta_Cobrar_Id_Evaluacion` (`Id_Evaluacion`),
  CONSTRAINT `FK_Cuenta_Cobrar_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cuenta_contable`
--

DROP TABLE IF EXISTS `cuenta_contable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cuenta_contable` (
  `Id_Cuenta_Contable` int(11) NOT NULL AUTO_INCREMENT,
  `Codigo` longtext,
  `Descripcion` longtext,
  `Id_Tipo` int(11) NOT NULL,
  PRIMARY KEY (`Id_Cuenta_Contable`),
  KEY `IX_Cuenta_Contable_Id_Tipo` (`Id_Tipo`),
  CONSTRAINT `FK_Cuenta_Contable_Tipo_Id_Tipo` FOREIGN KEY (`Id_Tipo`) REFERENCES `tipo` (`id_tipo`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5206 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `deduccion_desembolso`
--

DROP TABLE IF EXISTS `deduccion_desembolso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `deduccion_desembolso` (
  `Id_Deduccion_Desembolso` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Linea_Credito` int(11) NOT NULL,
  `Nombre` longtext,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Deduccion_Desembolso`),
  KEY `IX_Deduccion_Desembolso_Id_Linea_Credito` (`Id_Linea_Credito`),
  CONSTRAINT `FK_Deduccion_Desembolso_Linea_Credito_Id_Linea_Credito` FOREIGN KEY (`Id_Linea_Credito`) REFERENCES `linea_credito` (`id_linea`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `deduccion_historial`
--

DROP TABLE IF EXISTS `deduccion_historial`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `deduccion_historial` (
  `Id_Deduccion_Historial` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Linea_Credito_Historial` int(11) NOT NULL,
  `Nombre` longtext,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Deduccion_Historial`),
  KEY `IX_Deduccion_Historial_Id_Linea_Credito_Historial` (`Id_Linea_Credito_Historial`),
  CONSTRAINT `FK_Deduccion_Historial_Linea_Credito_Historial_Id_Linea_Credito_` FOREIGN KEY (`Id_Linea_Credito_Historial`) REFERENCES `linea_credito_historial` (`id_linea_credito_historial`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `definicion_transaccion`
--

DROP TABLE IF EXISTS `definicion_transaccion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `definicion_transaccion` (
  `Id_Definicion_Transaccion` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Identificador` longtext,
  PRIMARY KEY (`Id_Definicion_Transaccion`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `detalle_partida`
--

DROP TABLE IF EXISTS `detalle_partida`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `detalle_partida` (
  `Id_Detalle_Partida` int(11) NOT NULL AUTO_INCREMENT,
  `Abono` decimal(65,30) NOT NULL,
  `Borrado_Logico` bit(1) NOT NULL,
  `Cargo` decimal(65,30) NOT NULL,
  `Concepto` longtext,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Fecha` datetime(6) NOT NULL,
  `Id_Cuenta_Contable` int(11) NOT NULL,
  `Id_Partida_Contable` int(11) NOT NULL,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `Saldo` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Detalle_Partida`),
  KEY `IX_Detalle_Partida_Id_Cuenta_Contable` (`Id_Cuenta_Contable`),
  KEY `IX_Detalle_Partida_Id_Partida_Contable` (`Id_Partida_Contable`),
  CONSTRAINT `FK_Detalle_Partida_Cuenta_Contable_Id_Cuenta_Contable` FOREIGN KEY (`Id_Cuenta_Contable`) REFERENCES `cuenta_contable` (`id_cuenta_contable`) ON DELETE CASCADE,
  CONSTRAINT `FK_Detalle_Partida_Partida_Contable_Id_Partida_Contable` FOREIGN KEY (`Id_Partida_Contable`) REFERENCES `partida_contable` (`id_partida_contable`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `documento`
--

DROP TABLE IF EXISTS `documento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `documento` (
  `Id_Documento` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha` datetime(6) NOT NULL,
  `Id_Persona` int(11) NOT NULL,
  `NombreArchivo` longtext,
  `Archivo_Base64` longtext,
  PRIMARY KEY (`Id_Documento`),
  KEY `IX_Documento_Id_Persona` (`Id_Persona`),
  CONSTRAINT `FK_Documento_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `evaluacion`
--

DROP TABLE IF EXISTS `evaluacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `evaluacion` (
  `Id_Evaluacion` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Id_Asesor` longtext,
  `Id_Persona` int(11) NOT NULL,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `No_Dias_Buenos` int(11) NOT NULL,
  `No_Dias_Descanso` int(11) NOT NULL,
  `No_Dias_Malos` int(11) NOT NULL,
  `Venta_Mensual_Bueno` decimal(65,30) NOT NULL,
  `Venta_Semanal_Bueno` decimal(65,30) NOT NULL,
  `Venta_Semanal_Malo` decimal(65,30) NOT NULL,
  `Venta_Mensual_Malo` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Antiguedad_Negocio` int(11) NOT NULL DEFAULT '0',
  `Cuota` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Monto` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Monto_Credito_Mayor_Historial` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Mora` int(11) NOT NULL DEFAULT '0',
  `Plazo` int(11) NOT NULL DEFAULT '0',
  `Referencia_Comprobada` bit(1) NOT NULL DEFAULT b'0',
  `Reside_Zona_Peligrosa` bit(1) NOT NULL DEFAULT b'0',
  `Riesgo_Info_Red` int(11) NOT NULL DEFAULT '0',
  `Sector_Economico` int(11) NOT NULL DEFAULT '0',
  `Venta_Diaria_Bueno` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Venta_Diaria_Mala` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id_Evaluacion`),
  KEY `IX_Evaluacion_Id_Persona` (`Id_Persona`),
  CONSTRAINT `FK_Evaluacion_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `gastos`
--

DROP TABLE IF EXISTS `gastos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `gastos` (
  `Id_Gasto` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Evaluacion` int(11) NOT NULL,
  `Id_Tipo_Gasto` int(11) NOT NULL,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Gasto`),
  KEY `IX_Gastos_Id_Evaluacion` (`Id_Evaluacion`),
  KEY `IX_Gastos_Id_Tipo_Gasto` (`Id_Tipo_Gasto`),
  CONSTRAINT `FK_Gastos_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE,
  CONSTRAINT `FK_Gastos_Tipo_Gasto_Id_Tipo_Gasto` FOREIGN KEY (`Id_Tipo_Gasto`) REFERENCES `tipo_gasto` (`id_tipo_gasto`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `grupo`
--

DROP TABLE IF EXISTS `grupo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `grupo` (
  `Id_Grupo` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Grupo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inventario_negocio`
--

DROP TABLE IF EXISTS `inventario_negocio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `inventario_negocio` (
  `Id_Inventario_Negocio` int(11) NOT NULL AUTO_INCREMENT,
  `Cantidad` decimal(65,30) NOT NULL,
  `Detalle` longtext,
  `Id_Evaluacion` int(11) NOT NULL,
  `Precio_Compra` decimal(65,30) NOT NULL,
  `Total` decimal(65,30) NOT NULL,
  `Unidad_Compra` longtext,
  PRIMARY KEY (`Id_Inventario_Negocio`),
  KEY `IX_Inventario_Negocio_Id_Evaluacion` (`Id_Evaluacion`),
  CONSTRAINT `FK_Inventario_Negocio_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `linea_ahorro`
--

DROP TABLE IF EXISTS `linea_ahorro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `linea_ahorro` (
  `Id_Linea_Ahorro` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Tipo_Linea_Ahorro` int(11) NOT NULL,
  `Tasa_Interes` decimal(65,30) NOT NULL,
  `Frecuencia_Deposito` int(11) NOT NULL DEFAULT '0',
  `Mes_Capitalizable` int(11) NOT NULL DEFAULT '0',
  `Plazo` int(11) NOT NULL DEFAULT '0',
  `Monto_Minimo_Ahorro` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id_Linea_Ahorro`),
  CONSTRAINT `FK_Linea_Ahorro_Tipo_Linea_Ahorro_Id_Linea_Ahorro` FOREIGN KEY (`Id_Linea_Ahorro`) REFERENCES `tipo_linea_ahorro` (`id_tipo_linea_ahorro`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `linea_credito`
--

DROP TABLE IF EXISTS `linea_credito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `linea_credito` (
  `Id_Linea` int(11) NOT NULL AUTO_INCREMENT,
  `Codigo` longtext,
  `Descripcion_linea` longtext,
  `Dia_Gracia` int(11) NOT NULL,
  `PlanAyC` decimal(65,30) NOT NULL,
  `Recargo` decimal(65,30) NOT NULL,
  `Tasa` decimal(65,30) NOT NULL,
  `Tasa_Mora` decimal(65,30) NOT NULL,
  `Tipo_Garantia` int(11) NOT NULL,
  `Plazo_Rango_Inicial` int(11) DEFAULT '0',
  `Plazo_Rango_Final` int(11) DEFAULT '0',
  `Estructuracion` decimal(65,30) DEFAULT '0.000000000000000000000000000000',
  `Monto_Rango_Inicial` decimal(65,30) DEFAULT '0.000000000000000000000000000000',
  `Monto_Rango_Final` decimal(65,30) DEFAULT '0.000000000000000000000000000000',
  `Destino_Credito` int(11) DEFAULT '0',
  `PlanAyC_Cobros_Periodico_Dinero` bit(1) NOT NULL DEFAULT b'0',
  `Activar_Deducciones` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id_Linea`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `linea_credito_historial`
--

DROP TABLE IF EXISTS `linea_credito_historial`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `linea_credito_historial` (
  `Id_Linea_Credito_Historial` int(11) NOT NULL AUTO_INCREMENT,
  `Activar_Deducciones` bit(1) NOT NULL,
  `Codigo` longtext,
  `Descripcion_linea` longtext,
  `Destino_Credito` int(11) NOT NULL,
  `Dia_Gracia` int(11) NOT NULL,
  `Estructuracion` decimal(65,30) NOT NULL,
  `Id_Credito` int(11) DEFAULT NULL,
  `Monto_Rango_Final` decimal(65,30) NOT NULL,
  `Monto_Rango_Inicial` decimal(65,30) NOT NULL,
  `PlanAyC` decimal(65,30) NOT NULL,
  `PlanAyC_Cobros_Periodico_Dinero` bit(1) NOT NULL,
  `Plazo_Rango_Final` int(11) NOT NULL,
  `Plazo_Rango_Inicial` int(11) NOT NULL,
  `Recargo` decimal(65,30) NOT NULL,
  `Tasa` decimal(65,30) NOT NULL,
  `Tasa_Mora` decimal(65,30) NOT NULL,
  `Tipo_Garantia` int(11) NOT NULL,
  PRIMARY KEY (`Id_Linea_Credito_Historial`),
  UNIQUE KEY `IX_Linea_Credito_Historial_Id_Credito` (`Id_Credito`),
  CONSTRAINT `FK_Linea_Credito_Historial_Credito_Id_Credito` FOREIGN KEY (`Id_Credito`) REFERENCES `credito` (`id_credito`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `naturaleza`
--

DROP TABLE IF EXISTS `naturaleza`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `naturaleza` (
  `Id_Naturaleza` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Naturaleza`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `operacion`
--

DROP TABLE IF EXISTS `operacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `operacion` (
  `Id_Operacion` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Operacion`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `otro_ingreso`
--

DROP TABLE IF EXISTS `otro_ingreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `otro_ingreso` (
  `Id_Otro_Ingreso` int(11) NOT NULL AUTO_INCREMENT,
  `Frecuencia` longtext,
  `Id_Evaluacion` int(11) NOT NULL,
  `Id_Tipo_Ingreso` int(11) NOT NULL,
  `Valor` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Otro_Ingreso`),
  KEY `IX_Otro_Ingreso_Id_Evaluacion` (`Id_Evaluacion`),
  KEY `IX_Otro_Ingreso_Id_Tipo_Ingreso` (`Id_Tipo_Ingreso`),
  CONSTRAINT `FK_Otro_Ingreso_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE,
  CONSTRAINT `FK_Otro_Ingreso_Tipo_Ingreso_Id_Tipo_Ingreso` FOREIGN KEY (`Id_Tipo_Ingreso`) REFERENCES `tipo_ingreso` (`id_tipo_ingreso`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `partida_contable`
--

DROP TABLE IF EXISTS `partida_contable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `partida_contable` (
  `Id_Partida_Contable` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Comprobante` longtext,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Fecha` datetime(6) NOT NULL,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `Saldo` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Partida_Contable`)
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `periodo`
--

DROP TABLE IF EXISTS `periodo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `periodo` (
  `Id_Periodo` int(11) NOT NULL AUTO_INCREMENT,
  `Valor` int(11) NOT NULL,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Periodo`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persona`
--

DROP TABLE IF EXISTS `persona`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `persona` (
  `Id_Persona` int(11) NOT NULL AUTO_INCREMENT,
  `Apellidos` longtext,
  `ConocidoPor` longtext,
  `Contrasena` longtext,
  `Correo` longtext,
  `Dui` longtext,
  `ExpedicionDocumentoIdentifcacion` longtext,
  `ExpiracionDocumentoIdentificacion` longtext,
  `FechaNacimiento` datetime(6) NOT NULL,
  `ISSS` longtext,
  `Id_Cartera` int(11) NOT NULL,
  `LugarNacimiento` longtext,
  `NUP` longtext,
  `Nacionalidad` longtext,
  `Nit` longtext,
  `Nombres` longtext,
  `Ocupacion` longtext,
  `Pasaporte` longtext,
  `Profesion` longtext,
  `Telefono` longtext,
  `Usuario` longtext,
  `Personas_Cargo` int(11) NOT NULL DEFAULT '0',
  `Referencia_Crediticia_Comprobable` bit(1) NOT NULL DEFAULT b'0',
  `Residencia` int(11) NOT NULL DEFAULT '0',
  `EstadoFamiliar` int(11) NOT NULL DEFAULT '0',
  `Sexo` int(11) NOT NULL DEFAULT '0',
  `Activo` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id_Persona`),
  KEY `IX_Persona_Id_Cartera` (`Id_Cartera`),
  CONSTRAINT `FK_Persona_Cartera_Id_Cartera` FOREIGN KEY (`Id_Cartera`) REFERENCES `cartera` (`id_cartera`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31925 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persona_referencia`
--

DROP TABLE IF EXISTS `persona_referencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `persona_referencia` (
  `Id_Persona` int(11) NOT NULL,
  `Id_Referencia` int(11) NOT NULL,
  `Id_Persona_Referencia` int(11) NOT NULL AUTO_INCREMENT,
  `Parentesco` longtext,
  `Tipo_Referencia` longtext,
  PRIMARY KEY (`Id_Persona`,`Id_Referencia`),
  UNIQUE KEY `AK_Persona_Referencia_Id_Persona_Referencia` (`Id_Persona_Referencia`),
  KEY `IX_Persona_Referencia_Id_Referencia` (`Id_Referencia`),
  CONSTRAINT `FK_Persona_Referencia_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE,
  CONSTRAINT `FK_Persona_Referencia_Referencia_Id_Referencia` FOREIGN KEY (`Id_Referencia`) REFERENCES `referencia` (`id_referencia`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persona_relaciones`
--

DROP TABLE IF EXISTS `persona_relaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `persona_relaciones` (
  `Id_Sujeto` int(11) NOT NULL,
  `Id_Predicado` int(11) NOT NULL,
  `Id_Persona_Relaciones` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Tipo_Relacion` int(11) NOT NULL,
  `Activo` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id_Sujeto`,`Id_Predicado`),
  UNIQUE KEY `AK_Persona_Relaciones_Id_Persona_Relaciones` (`Id_Persona_Relaciones`),
  KEY `IX_Persona_Relaciones_Id_Predicado` (`Id_Predicado`),
  KEY `IX_Persona_Relaciones_Id_Tipo_Relacion` (`Id_Tipo_Relacion`),
  CONSTRAINT `FK_Persona_Relaciones_Persona_Id_Predicado` FOREIGN KEY (`Id_Predicado`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE,
  CONSTRAINT `FK_Persona_Relaciones_Persona_Id_Sujeto` FOREIGN KEY (`Id_Sujeto`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE,
  CONSTRAINT `FK_Persona_Relaciones_Tipo_Relacion_Id_Tipo_Relacion` FOREIGN KEY (`Id_Tipo_Relacion`) REFERENCES `tipo_relacion` (`id_tipo_relacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persona_tipo_persona`
--

DROP TABLE IF EXISTS `persona_tipo_persona`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `persona_tipo_persona` (
  `Id_Persona` int(11) NOT NULL,
  `Id_Tipo_Persona` int(11) NOT NULL,
  `Id_Persona_Tipo_Persona` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id_Persona`,`Id_Tipo_Persona`),
  UNIQUE KEY `AK_Persona_Tipo_Persona_Id_Persona_Tipo_Persona` (`Id_Persona_Tipo_Persona`),
  KEY `IX_Persona_Tipo_Persona_Id_Tipo_Persona` (`Id_Tipo_Persona`),
  CONSTRAINT `FK_Persona_Tipo_Persona_Persona_Id_Persona` FOREIGN KEY (`Id_Persona`) REFERENCES `persona` (`id_persona`) ON DELETE CASCADE,
  CONSTRAINT `FK_Persona_Tipo_Persona_Tipo_Persona_Id_Tipo_Persona` FOREIGN KEY (`Id_Tipo_Persona`) REFERENCES `tipo_persona` (`id_tipo_persona`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `prestamo`
--

DROP TABLE IF EXISTS `prestamo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `prestamo` (
  `Id_Prestamo` int(11) NOT NULL AUTO_INCREMENT,
  `Cuota` decimal(65,30) NOT NULL,
  `Frecuencia` longtext,
  `Id_Evaluacion` int(11) NOT NULL,
  `Monto_Otorgado` decimal(65,30) NOT NULL,
  `Saldo` decimal(65,30) NOT NULL,
  `Total` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id_Prestamo`),
  KEY `IX_Prestamo_Id_Evaluacion` (`Id_Evaluacion`),
  CONSTRAINT `FK_Prestamo_Evaluacion_Id_Evaluacion` FOREIGN KEY (`Id_Evaluacion`) REFERENCES `evaluacion` (`id_evaluacion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `referencia`
--

DROP TABLE IF EXISTS `referencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `referencia` (
  `Id_Referencia` int(11) NOT NULL AUTO_INCREMENT,
  `FechaNacimiento` datetime(6) NOT NULL,
  `Nombre` longtext,
  `Residencia` longtext,
  `Telefono` longtext,
  PRIMARY KEY (`Id_Referencia`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo`
--

DROP TABLE IF EXISTS `tipo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo` (
  `Id_Tipo` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_gasto`
--

DROP TABLE IF EXISTS `tipo_gasto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_gasto` (
  `Id_Tipo_Gasto` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  `Id_Categoria_Gasto` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id_Tipo_Gasto`),
  KEY `IX_Tipo_Gasto_Id_Categoria_Gasto` (`Id_Categoria_Gasto`),
  CONSTRAINT `FK_Tipo_Gasto_Categoria_Gasto_Id_Categoria_Gasto` FOREIGN KEY (`Id_Categoria_Gasto`) REFERENCES `categoria_gasto` (`id_categoria_gasto`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_ingreso`
--

DROP TABLE IF EXISTS `tipo_ingreso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_ingreso` (
  `Id_Tipo_Ingreso` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Ingreso`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_linea_ahorro`
--

DROP TABLE IF EXISTS `tipo_linea_ahorro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_linea_ahorro` (
  `Id_Tipo_Linea_Ahorro` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Linea_Ahorro`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_operacion_ahorro`
--

DROP TABLE IF EXISTS `tipo_operacion_ahorro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_operacion_ahorro` (
  `Id_Tipo_Operacion_Ahorro` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Operacion_Ahorro`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_operacion_credito`
--

DROP TABLE IF EXISTS `tipo_operacion_credito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_operacion_credito` (
  `Id_Tipo_Operacion_Credito` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Operacion_Credito`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_operacion_simple`
--

DROP TABLE IF EXISTS `tipo_operacion_simple`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_operacion_simple` (
  `Id_Tipo_Operacion_Simple` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Operacion_Simple`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_persona`
--

DROP TABLE IF EXISTS `tipo_persona`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_persona` (
  `Id_Tipo_Persona` int(11) NOT NULL AUTO_INCREMENT,
  `Tipo_Persona_definicion` longtext,
  PRIMARY KEY (`Id_Tipo_Persona`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_relacion`
--

DROP TABLE IF EXISTS `tipo_relacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_relacion` (
  `Id_Tipo_Relacion` int(11) NOT NULL AUTO_INCREMENT,
  `Tipo_Relacion_Definicion` longtext,
  PRIMARY KEY (`Id_Tipo_Relacion`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipo_transaccion`
--

DROP TABLE IF EXISTS `tipo_transaccion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipo_transaccion` (
  `Id_Tipo_Transaccion` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` longtext,
  PRIMARY KEY (`Id_Tipo_Transaccion`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaccion_ahorro`
--

DROP TABLE IF EXISTS `transaccion_ahorro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `transaccion_ahorro` (
  `Id_Transaccion_Ahorro` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Dias` int(11) NOT NULL,
  `Fecha` datetime(6) NOT NULL,
  `Id_Ahorro` int(11) NOT NULL,
  `Id_Tipo_Operacion_Ahorro` int(11) NOT NULL,
  `Interes_Acumulado` decimal(65,30) NOT NULL,
  `Interes_Aplicado` decimal(65,30) NOT NULL,
  `Saldo` decimal(65,30) NOT NULL,
  `Valor` decimal(65,30) NOT NULL,
  `Creado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Eliminado_Por` longtext,
  `Modificado` bit(1) NOT NULL DEFAULT b'0',
  `Modificado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Modificado_Por` longtext,
  PRIMARY KEY (`Id_Transaccion_Ahorro`),
  KEY `IX_Transaccion_Ahorro_Id_Ahorro` (`Id_Ahorro`),
  KEY `IX_Transaccion_Ahorro_Id_Tipo_Operacion_Ahorro` (`Id_Tipo_Operacion_Ahorro`),
  CONSTRAINT `FK_Transaccion_Ahorro_Ahorro_Id_Ahorro` FOREIGN KEY (`Id_Ahorro`) REFERENCES `ahorro` (`id_ahorro`) ON DELETE CASCADE,
  CONSTRAINT `FK_Transaccion_Ahorro_Tipo_Operacion_Ahorro_Id_Tipo_Operacion_Ah` FOREIGN KEY (`Id_Tipo_Operacion_Ahorro`) REFERENCES `tipo_operacion_ahorro` (`id_tipo_operacion_ahorro`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaccion_caja`
--

DROP TABLE IF EXISTS `transaccion_caja`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `transaccion_caja` (
  `Id_Transaccion_Caja` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Fecha` datetime(6) NOT NULL,
  `Id_Caja` int(11) NOT NULL,
  `Id_Tipo_Transaccion` int(11) NOT NULL,
  `Id_Transaccion_Ahorro` int(11) NOT NULL,
  `Id_Transaccion_Credito` int(11) NOT NULL,
  `Monto` decimal(65,30) NOT NULL,
  `No_Comprobante` longtext,
  `Creado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Eliminado_Por` longtext,
  `Modificado` bit(1) NOT NULL DEFAULT b'0',
  `Modificado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Modificado_Por` longtext,
  `Id_Transaccion_Simple` int(11) NOT NULL DEFAULT '0',
  `Saldo` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id_Transaccion_Caja`),
  KEY `IX_Transaccion_Caja_Id_Caja` (`Id_Caja`),
  KEY `IX_Transaccion_Caja_Id_Tipo_Transaccion` (`Id_Tipo_Transaccion`),
  CONSTRAINT `FK_Transaccion_Caja_Caja_Id_Caja` FOREIGN KEY (`Id_Caja`) REFERENCES `caja` (`id_caja`) ON DELETE CASCADE,
  CONSTRAINT `FK_Transaccion_Caja_Tipo_Transaccion_Id_Tipo_Transaccion` FOREIGN KEY (`Id_Tipo_Transaccion`) REFERENCES `tipo_transaccion` (`id_tipo_transaccion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaccion_credito`
--

DROP TABLE IF EXISTS `transaccion_credito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `transaccion_credito` (
  `Id_Transaccion_Credito` int(11) NOT NULL AUTO_INCREMENT,
  `Abono_capital` decimal(65,30) NOT NULL,
  `Dias` int(11) NOT NULL,
  `FechaPago` datetime(6) NOT NULL,
  `Id_Credito` int(11) NOT NULL,
  `Id_Tipo_Operecion_Credito` int(11) NOT NULL,
  `Interes` decimal(65,30) NOT NULL,
  `IvaSobrePlanAyC` decimal(65,30) NOT NULL,
  `No` int(11) NOT NULL,
  `PlanAyC` decimal(65,30) NOT NULL,
  `Saldo_Capital` decimal(65,30) NOT NULL,
  `Valor_Cuota` decimal(65,30) NOT NULL,
  `Anticipado` bit(1) NOT NULL DEFAULT b'0',
  `Puntual` bit(1) NOT NULL DEFAULT b'0',
  `Vencido` bit(1) NOT NULL DEFAULT b'0',
  `Pagos_Acumulado` int(11) NOT NULL DEFAULT '0',
  `Recargo` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Borrado_Logico` bit(1) NOT NULL DEFAULT b'0',
  `Creado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Eliminado_Por` longtext,
  `Modificado` bit(1) NOT NULL DEFAULT b'0',
  `Modificado_En` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Modificado_Por` longtext,
  `Otros_Cobros` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id_Transaccion_Credito`),
  KEY `IX_Transaccion_Credito_Id_Credito` (`Id_Credito`),
  KEY `IX_Transaccion_Credito_Id_Tipo_Operecion_Credito` (`Id_Tipo_Operecion_Credito`),
  CONSTRAINT `FK_Transaccion_Credito_Credito_Id_Credito` FOREIGN KEY (`Id_Credito`) REFERENCES `credito` (`id_credito`) ON DELETE CASCADE,
  CONSTRAINT `FK_Transaccion_Credito_Tipo_Operacion_Credito_Id_Tipo_Operecion_` FOREIGN KEY (`Id_Tipo_Operecion_Credito`) REFERENCES `tipo_operacion_credito` (`id_tipo_operacion_credito`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaccion_simple`
--

DROP TABLE IF EXISTS `transaccion_simple`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `transaccion_simple` (
  `Id_Transaccion_Simple` int(11) NOT NULL AUTO_INCREMENT,
  `Borrado_Logico` bit(1) NOT NULL,
  `Creado_En` datetime(6) NOT NULL,
  `Creado_Por` longtext,
  `Eliminado_En` datetime(6) NOT NULL,
  `Eliminado_Por` longtext,
  `Id_Tipo_Operacion_Simple` int(11) NOT NULL,
  `Modificado` bit(1) NOT NULL,
  `Modificado_En` datetime(6) NOT NULL,
  `Modificado_Por` longtext,
  `Monto` decimal(65,30) NOT NULL,
  `Id_Banco` int(11) NOT NULL DEFAULT '0',
  `Id_Caja` int(11) NOT NULL DEFAULT '0',
  `No_Cuenta` longtext,
  `Nombre_Banco` longtext,
  `Saldo` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `Id_Caja_Boveda` int(11) NOT NULL DEFAULT '0',
  `Comprobante_Banco` longtext,
  `Comprobante_Gasto` longtext,
  PRIMARY KEY (`Id_Transaccion_Simple`),
  KEY `IX_Transaccion_Simple_Id_Tipo_Operacion_Simple` (`Id_Tipo_Operacion_Simple`),
  CONSTRAINT `FK_Transaccion_Simple_Tipo_Operacion_Simple_Id_Tipo_Operacion_Si` FOREIGN KEY (`Id_Tipo_Operacion_Simple`) REFERENCES `tipo_operacion_simple` (`id_tipo_operacion_simple`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario_agencia`
--

DROP TABLE IF EXISTS `usuario_agencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `usuario_agencia` (
  `Id_Usuario_Agencia` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Agencia` int(11) NOT NULL,
  `Id_Usuario` longtext,
  PRIMARY KEY (`Id_Usuario_Agencia`),
  KEY `IX_Usuario_Agencia_Id_Agencia` (`Id_Agencia`),
  CONSTRAINT `FK_Usuario_Agencia_Agencia_Id_Agencia` FOREIGN KEY (`Id_Agencia`) REFERENCES `agencia` (`id_agencia`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-10-05 12:34:54
