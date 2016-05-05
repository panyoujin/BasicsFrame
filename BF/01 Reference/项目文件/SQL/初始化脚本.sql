DROP TABLE IF EXISTS `TB_CM_Menu`;
CREATE TABLE `TB_CM_Menu` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `MenuName` varchar(50) NOT NULL COMMENT '菜单名称',
  `IsRoot` tinyint(1) NOT NULL COMMENT '是否根节点',
  `IsFunction` tinyint(1) NOT NULL DEFAULT '0' COMMENT '用于权限控制, 指示这个是"菜单"还是"功能".',
  `ParentMenuID` int(11) DEFAULT NULL COMMENT '父节点',
  `Description` varchar(1000) DEFAULT NULL COMMENT '描述',
  `MenuUrl` varchar(300) DEFAULT NULL COMMENT '菜单地址',
  `IConUrl` varchar(50) DEFAULT NULL COMMENT '菜单图标地址',
  `Sort` int(11) DEFAULT NULL COMMENT '排序',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `CreationUser` varchar(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` datetime DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` varchar(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='[后台权限] 后台菜单';

DROP TABLE IF EXISTS `TB_CM_Role`;
CREATE TABLE `TB_CM_Role` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `RoleName` varchar(50) NOT NULL COMMENT '角色名称',
  `Description` varchar(1000) DEFAULT NULL COMMENT '描述',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `CreationUser` varchar(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` datetime DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` varchar(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='[后台权限] 管理员角色';

DROP TABLE IF EXISTS `TB_CM_Rolemenu`;
CREATE TABLE `TB_CM_Rolemenu` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `RoleID` int(11) NOT NULL COMMENT '角色ID',
  `MenuID` int(11) NOT NULL COMMENT '菜单ID',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `CreationUser` varchar(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` datetime DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` varchar(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='[后台权限] 管理员角色与菜单关系';

DROP TABLE IF EXISTS `TB_CM_User`;
CREATE TABLE `TB_CM_User` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `UserAccount` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL COMMENT '用户账号',
  `UserName` varchar(50) NOT NULL COMMENT '用户名称',
  `PhoneNum` varchar(50) DEFAULT NULL COMMENT '电话号码',
  `EMail` varchar(50) DEFAULT NULL COMMENT '邮箱',
  `UserPassword` varchar(50) DEFAULT NULL COMMENT '密码',
  `Sex` int(11) DEFAULT NULL COMMENT '性别',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `SessionID` varchar(50) DEFAULT NULL COMMENT '登录标识',
  `LoginTime` datetime DEFAULT NULL COMMENT '登录时间',
  `CreationUser` varchar(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` datetime DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` varchar(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='[后台权限] 管理员信息';

DROP TABLE IF EXISTS `TB_CM_Userrole`;
CREATE TABLE `TB_CM_Userrole` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `UserID` int(11) NOT NULL COMMENT '用户ID',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `RoleID` int(11) NOT NULL COMMENT '角色ID',
  `CreationUser` varchar(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` datetime DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` varchar(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='[后台权限] 管理员与角色关系';

DROP TABLE IF EXISTS `TB_MB_CommonModel`;
CREATE TABLE `TB_MB_CommonModel` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `User_ID` int(11) NOT NULL COMMENT '用户ID',
  `Model_ID` int(11) DEFAULT NULL COMMENT '模式ID',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效，默认有效',
  `CreationUser` varchar(50) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `ModificationUser` varchar(50) DEFAULT NULL,
  `ModificationDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='常用模式表';

DROP TABLE IF EXISTS `TB_MB_MemberInfo`;

CREATE TABLE `TB_MB_MemberInfo` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT,
  `Account` VARCHAR(50) NOT NULL COMMENT '账号',
  `Passwd` VARCHAR(50) NOT NULL COMMENT '密码',
  `Name` VARCHAR(50) DEFAULT NULL COMMENT '昵称',
  `Age` INT(4) DEFAULT NULL COMMENT '年龄',
  `Birthday` DATE DEFAULT NULL COMMENT '生日',
  `Sex` BIT(1) DEFAULT NULL COMMENT 'true:男；false:女',
  `Email` VARCHAR(80) DEFAULT NULL COMMENT '邮箱',
  `Phone` VARCHAR(20) DEFAULT NULL COMMENT '电话',
  `Remark` VARCHAR(200) DEFAULT NULL COMMENT '备注',
  `QQ` VARCHAR(20) DEFAULT NULL,
  `CreationDate` DATETIME DEFAULT NULL,
  `CreationUser` VARCHAR(50) DEFAULT NULL,
  `ModificationUser` VARCHAR(50) DEFAULT NULL,
  `ModificationDate` DATETIME DEFAULT NULL,
  `Status` BIT(1) DEFAULT b'1' COMMENT '是否删除',
  `IsAuto` BIT(1) DEFAULT NULL COMMENT '是否自动创建',
  `ImageUrl` VARCHAR(500) DEFAULT NULL COMMENT '头像',
  `IsAdmin` BIT(1) DEFAULT b'0' COMMENT '是否管理员',
  `SessionID` VARCHAR(50) DEFAULT NULL COMMENT '登录标识',
  PRIMARY KEY (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='用户表';

DROP TABLE IF EXISTS `TB_MB_Models`;
CREATE TABLE `TB_MB_Models` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `User_ID` int(11) NOT NULL DEFAULT '0' COMMENT '如果是后台管理添加，该字段为0',
  `Model_Name` varchar(50) NOT NULL COMMENT '模式名称，养生品名称',
  `IcoUrl` varchar(256) DEFAULT NULL COMMENT '图标地址',
  `ImageUrl` varchar(256) DEFAULT NULL COMMENT '图片地址',
  `Introduce` varchar(200) DEFAULT NULL COMMENT '简单介绍，用户自定义的需要动态生成',
  `Describe` varchar(1000) DEFAULT NULL COMMENT '材料描述',
  `Remarks` varchar(1000) DEFAULT NULL COMMENT '注意事项',
  `Sort` int(11) DEFAULT '0' COMMENT '排序，越大越前',
  `IsBubble` bit(1) DEFAULT b'0' COMMENT '是否泡料，默认不泡',
  `Bubble_Time` int(11) DEFAULT NULL COMMENT '泡料时长，单位分钟',
  `Bubble_Temperature` int(11) DEFAULT NULL COMMENT '泡料温度，单位摄氏度，最小单位1摄氏度',
  `Cook_Time` int(11) DEFAULT NULL COMMENT '煮料时长，单位分钟',
  `Cook_Temperature` int(11) DEFAULT NULL COMMENT '煮料温度，单位摄氏度，最小单位1摄氏度',
  `Is_Heat_Preservation` bit(1) DEFAULT b'0' COMMENT '是否保温，默认不保温',
  `Heat_Preservation_Time` int(11) DEFAULT NULL COMMENT '保温时长，单位分钟',
  `Heat_Preservation_Temperature` int(11) DEFAULT NULL COMMENT '保温温度，单位摄氏度，最小单位1摄氏度',
  `Removal_Chlorine_Time` int(11) DEFAULT NULL COMMENT '除氯时长，单位分钟',
  `Final_Temperature` int(11) DEFAULT NULL COMMENT '最终温度',
  `IsFerv` bit(1) DEFAULT b'0' COMMENT '是否煮沸,默认不煮沸',
  `Model_Type` int(2) DEFAULT '0' COMMENT '类型：0.系统提供;1.自定义,枚举：Model_Type',
  `Model_Status` int(2) DEFAULT '1' COMMENT '状态：0.公开;1.私密主要是给自定义的模式使用',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效，默认有效',
  `CreationUser` varchar(50) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `ModificationUser` varchar(50) DEFAULT NULL,
  `ModificationDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Index_UserAndName` (`User_ID`,`Model_Name`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='养生品,养生宝典,模式';

DROP TABLE IF EXISTS `TB_MB_Shopping`;
CREATE TABLE `TB_MB_Shopping` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Description` varchar(50) DEFAULT NULL,
  `Url` varchar(100) DEFAULT NULL,
  `Price` decimal(11,2) DEFAULT '0.00',
  `NowPrice` decimal(11,2) DEFAULT '0.00',
  `CreationDate` datetime DEFAULT NULL,
  `CreationUser` varchar(50) DEFAULT NULL,
  `ModificationUser` varchar(50) DEFAULT NULL,
  `ModificationDate` datetime DEFAULT NULL,
  `Status` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='商品表';

DROP TABLE IF EXISTS `TB_MC_Collection`;
CREATE TABLE `TB_MC_Collection` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `User_ID` int(11) NOT NULL,
  `Source_ID` int(11) NOT NULL COMMENT '收藏源ID',
  `Source_Type` int(2) NOT NULL DEFAULT '1' COMMENT '收藏源类型 1.模式;2.文章;',
  `CollectionTime` datetime DEFAULT NULL COMMENT '收藏时间',
  `Status` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `CreationUser` varchar(50) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `ModificationUser` varchar(50) DEFAULT NULL,
  `ModificationDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='收藏表';


DROP TABLE IF EXISTS `TB_MB_Kettle`;
CREATE TABLE `TB_MB_Kettle` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(50) DEFAULT NULL COMMENT '水壶名称',
  `Description` VARCHAR(100) DEFAULT NULL COMMENT '描述',
  `Version` VARCHAR(50) DEFAULT NULL COMMENT '水壶型号',
  `Status` BIT(1) NOT NULL DEFAULT b'1' COMMENT '是否有效',
  `CreationUser` VARCHAR(50) DEFAULT NULL,
  `CreationDate` DATETIME DEFAULT NULL,
  `ModificationUser` VARCHAR(50) DEFAULT NULL,
  `ModificationDate` DATETIME DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8 COMMENT='我的水壶';

DROP TABLE IF EXISTS `TB_MB_MyKettleRel`;
CREATE TABLE `TB_MB_MyKettleRel` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT,
  `MemberID` INT(11) NOT NULL COMMENT '用户ID',
  `KettleID` INT(11) NOT NULL COMMENT '水壶ID',
  `Default` BIT(1) DEFAULT b'0' COMMENT '是否默认显示 true，false',
  `CreationUser` VARCHAR(50) DEFAULT NULL,
  `CreationDate` DATETIME DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='水壶跟用户关系表';

DROP TABLE IF EXISTS `TB_MB_AttmntServer`;

CREATE TABLE `TB_MB_AttmntServer` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT COMMENT '唯一标识',
  `ServerName` VARCHAR(100) DEFAULT NULL COMMENT '服务器主机名',
  `ServerIP` VARCHAR(50) DEFAULT NULL COMMENT '服务器主机IP',
  `ServerDomain` VARCHAR(200) DEFAULT NULL COMMENT '服务器主机域名',
  `RelativePath` VARCHAR(100) DEFAULT NULL COMMENT '相对路径',
  `RemoteFolder` VARCHAR(100) DEFAULT NULL COMMENT '远程文件夹路径',
  `ServerAccount` VARCHAR(50) DEFAULT NULL COMMENT '远程文件夹访问账号',
  `ServerPassword` VARCHAR(50) DEFAULT NULL COMMENT '远程文件夹访问密码',
  `Status` BIT(1) DEFAULT b'1' COMMENT '是否可用',
  `CreationUser` VARCHAR(50) DEFAULT NULL COMMENT '创建人账号',
  `CreationDate` DATETIME DEFAULT NULL COMMENT '创建时间',
  `ModificationUser` VARCHAR(50) DEFAULT NULL COMMENT '修改人账号',
  `ModificationDate` DATETIME DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=MYISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='附件服务器配置表';