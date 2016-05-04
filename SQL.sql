/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50628
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MYSQL
Target Server Version : 50628
File Encoding         : 65001

Date: 2016-04-27 17:55:36
*/

CREATE SCHEMA `crawler` ;

use crawler ;

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for hotel
-- ----------------------------
-- DROP TABLE IF EXISTS `hotel`;
CREATE TABLE `hotels` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HotelCode` varchar(50) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Level` varchar(255) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `Contact` varchar(255) DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `Park` varchar(255) DEFAULT NULL,
  `OpeningDate` varchar(10) DEFAULT NULL,
  `LastDecorationDate` varchar(10) DEFAULT NULL,
  `RoomCount` varchar(10) DEFAULT NULL,
  `HighestFloor` varchar(10) DEFAULT NULL,
  `Fax` varchar(255) DEFAULT NULL,
  `NameEn` varchar(255) DEFAULT NULL,
  `Network` varchar(255) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotel
-- ----------------------------

-- ----------------------------
-- Table structure for hotelnetworkfacilities
-- ----------------------------

CREATE TABLE `hotelBasefacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HotelId` int(11) DEFAULT NULL,
  `BaseFacilityId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `BaseFacilityName` varchar(255) default null,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  `Remark` varchar(255) default null,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelnetworkfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for hotelroomfacilities
-- ----------------------------

CREATE TABLE `hotelroomfacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HotelId` int(11) DEFAULT NULL,
  `RoomFacilityId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `RoomFacilityName` varchar(255) default null,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  `Remark` varchar(255) default null,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelroomfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for hotelservices
-- ----------------------------

CREATE TABLE `hotelservicefacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HotelId` int(11) DEFAULT NULL,
  `ServiceFacilityId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `ServiceFacilityName` varchar(255) default null,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  `Remark` varchar(255) default null,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelservices
-- ----------------------------

-- ----------------------------
-- Table structure for networkfacilities
-- ----------------------------

CREATE TABLE `servicefacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of networkfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for roomfacilities
-- ----------------------------

CREATE TABLE `roomfacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of roomfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for services
-- ----------------------------DROP TABLE IF EXISTS `baseservices`;
CREATE TABLE `basefacilities` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of services
-- ----------------------------


create table `traffics`(
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `GPoint` varchar(20) default null,
  `BPoint` varchar(20) default null,
  `Distance` varchar(50) default null,
  `Category` varchar(10) default null,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  `HotelId` int default null,
  PRIMARY KEY (`Id`)
)

