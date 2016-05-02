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

CREATE SCHEMA `carwler` ;

use carwler ;

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for hotel
-- ----------------------------
-- DROP TABLE IF EXISTS `hotel`;
CREATE TABLE `hotel` (
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
INSERT INTO `hotel` VALUES ('2', '21056', '速8酒店北京紫竹院南路店', '经济型', '北京市海淀区紫竹院南路18号', '010-68785599', null, '速8酒店紫竹院南路店位于北京市海淀区西三环内紫竹院南路18号，周边景点有动物园，北京展览馆，中国世纪坛，中央电视塔等旅游景点。周边有首都师范大学，北京工商大学，中国劳动关系学院，民族大学等著名高校。酒店紧邻国家设计部等各大部委，酒店周边各大商场和网点一应俱全。', null, '2013', '2013', null, '4', '010-68700877', null);

-- ----------------------------
-- Table structure for hotelnetworkfacilities
-- ----------------------------
DROP TABLE IF EXISTS `hotelnetworkfacilities`;
CREATE TABLE `hotelnetworkfacilities` (
  `Id` int(11) NOT NULL,
  `HotelId` int(11) DEFAULT NULL,
  `NetworkFacilityId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelnetworkfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for hotelroomfacilities
-- ----------------------------
DROP TABLE IF EXISTS `hotelroomfacilities`;
CREATE TABLE `hotelroomfacilities` (
  `Id` int(11) NOT NULL,
  `HotelId` int(11) DEFAULT NULL,
  `RoomFacilityId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelroomfacilities
-- ----------------------------

-- ----------------------------
-- Table structure for hotelservices
-- ----------------------------
DROP TABLE IF EXISTS `hotelservices`;
CREATE TABLE `hotelservices` (
  `Id` int(11) NOT NULL,
  `HotelId` int(11) DEFAULT NULL,
  `ServiceId` int(11) DEFAULT NULL,
  `IsEnable` bit(1) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hotelservices
-- ----------------------------

-- ----------------------------
-- Table structure for networkfacilities
-- ----------------------------
DROP TABLE IF EXISTS `networkfacilities`;
CREATE TABLE `networkfacilities` (
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
DROP TABLE IF EXISTS `roomfacilities`;
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
-- ----------------------------
DROP TABLE IF EXISTS `services`;
CREATE TABLE `services` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `AddTime` timestamp Null DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of services
-- ----------------------------
