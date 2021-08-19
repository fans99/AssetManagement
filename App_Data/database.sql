create database AssetDB
go

use AssetDB

create table AreaTypeInfo
(
	AreaTypeID	Int primary key identity(1,1),
	AreaTypeName	nvarchar(50)		Not null,
	AreaTypeMark	nvarchar(200)		null
)

create table AreaInfo
(
	AreaID	Int primary key identity(1,1),
	AreaTypeID	Int references AreaTypeInfo(AreaTypeID) Not null,
	AreaName	Varchar(50)		Not null,
	AreaReMark	Varchar(200)
)

create table AssetTypeInfo
(
	AssetTypeID	int	primary key identity(1,1),
	AssetTypeName	nvarchar(50)		Not null,
	AssetTypeReMark	nvarchar(200)		null,
)

insert into AssetTypeInfo(AssetTypeName, AssetTypeReMark)
values('固定资产', '')
insert into AssetTypeInfo(AssetTypeName, AssetTypeReMark)
values('办公用品', '')

create table AssetInfo
(
	AssetID	Int	primary key identity(1,1),
	AssetTypeID	int	references AssetTypeInfo(AssetTypeID) Not NULL,
	AssetName	nvarchar(50)		Not null,
	AssetModel	nvarchar(20)		null,
	AssetCompany	nvarchar(20)		Not null,
	AssetReMark	nvarchar(200)		null,
)

create table AssetDetailInfo
(
	AssetDetailID	Int primary key identity(1,1),
	AssetID	int references AssetInfo(AssetID) Not null,
	AssetDetailNum	char(10)		Not null,
	AssetDetailUseState	int			Not null,
	AssetDetailUseDate	datetime			null,
	AssetDetailReturnDate	datetime			null,
	AssetDetailServiceState	int			Not null,
	AssetDetailDumState	int			Not null,
	EmpolyID	int			Not null,
	AreaID	int			Not null,
	AssetAreaReMark	nvarchar(200),
)

create table AssetDetailRecordInfo
(
	AssetDetailRecordID	Int primary key identity(1,1),
	AssetDetailID	Int			Not null,
	EmpolyID	int			Not null,
	AreaID	Int			Not null,
	AssetNum	Int			Not null,
	AssetDetailRecordUseDate	datetime	Not null,
	AssetDetailRecordReturnDate	datetime,
	AssetDetailRecordReMark	nvarchar(200),
)

select AssetDetailInfo.AssetID, AssetDetailID, AssetName from AssetDetailInfo 
left join AssetInfo on AssetDetailInfo.AssetID = AssetInfo.AssetID
where AssetDetailID in (select AssetDetailID from AssetDetailRecordInfo where EmpolyID = 2 and AssetDetailRecordReturnDate is NULL)

create table CampusInfo
(
	CampusID  Int	primary key identity(1,1),
	CampusName	nvarchar(50)		Not null,
	CampusReMark	nvarchar(200)		null,
)

insert into CampusInfo(CampusName, CampusReMark)
values('华为', '')
insert into CampusInfo(CampusName, CampusReMark)
values('小米', '')
insert into CampusInfo(CampusName, CampusReMark)
values('苹果', '')
insert into CampusInfo(CampusName, CampusReMark)
values('微软', '')
insert into CampusInfo(CampusName, CampusReMark)
values('网易', '')
insert into CampusInfo(CampusName, CampusReMark)
values('腾讯', '')
insert into CampusInfo(CampusName, CampusReMark)
values('阿里', '')
insert into CampusInfo(CampusName, CampusReMark)
values('亚马逊', '')

create table DeptInfo
(
	DeptID	Int primary key identity(1,1),
	CampusID   int	references CampusInfo(CampusID)	 NULL,
	DeptName 	nvarchar(50)		NOT NULL,
	DeptReMark 	nvarchar(200)		NULL,
)

insert into DeptInfo(CampusID, DeptName, DeptReMark)
values(1, '开发部', '')
insert into DeptInfo(CampusID, DeptName, DeptReMark)
values(2, '技术部', '')
insert into DeptInfo(CampusID, DeptName, DeptReMark)
values(3, '美工部', '')
insert into DeptInfo(CampusID, DeptName, DeptReMark)
values(4, '管理部', '')

create table RoleInfo
(
	RoleID	Int	primary key identity(1,1),
	RoleName 	nvarchar(50)		Not null,
	RoleReMark 	nvarchar(200)		null,
)

insert into RoleInfo(RoleName, RoleReMark)
values('管理员', '')
insert into RoleInfo(RoleName, RoleReMark)
values('普通员工', '')

create table EmpolyInfo
(
	EmpolyID	Int	primary key identity(1,1),
	EmpolyNum 	char(6)		Not null,
	EmpolyPwd 	nvarchar(50)		Not null,
	EmpolyName 	nvarchar(50)		Not null,
	EmpolySex 	char(2)		Not null,
	EmpolyldCard 	char(18)		Not null,
	Empolylmg 	nvarchar(100)		Not null,
	DeptID	int	references DeptInfo(DeptID)	null,
	EmpolyLevel 	int			Not null,
	RoleID	int references RoleInfo(RoleID)	Not null,
	InductionDate 	datetime			null,
	DepartureDate	datetime			null,
	EmpolyReMark 	nvarchar(200)		null,
)

insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100001', '123456', '张三', '男', '420984199901012611', '', 1, 1, 2, GETDATE(), '')
insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100002', '123456', '李四', '女', '420984199901022621', '', 2, 2, 1, GETDATE(), '')
insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100003', '123456', '王五', '男', '420984199903032513', '', 3, 3, 1, GETDATE(), '')
insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100004', '123456', '张三', '男', '420984199901012611', '', 1, 1, 2, GETDATE(), '')
insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100005', '123456', '李四', '女', '420984199901022621', '', 2, 2, 1, GETDATE(), '')
insert into EmpolyInfo(EmpolyNum, EmpolyPwd, EmpolyName, EmpolySex, EmpolyldCard, Empolylmg, DeptID, EmpolyLevel, RoleID, InductionDate, EmpolyReMark)
values('100006', '123456', '王五', '男', '420984199903032513', '', 3, 3, 1, GETDATE(), '')

create table AssetPutInfo
(
	AssetPutID	Int	primary key identity(1,1),
	AssetID	int references AssetInfo(AssetID),
	AssetPutCount	int		Not null,
	AssetPutDate	datetime	Not null,
	EmpolyID	int,
	AssetPutReMark	nvarchar(200),
)

create table DamageRecordInfo
(
	DamageRecordlID	Int	primary key identity(1,1),
	AssetDetailID	int	not null,
	EmpolyID	int null,
	DamageDate 		datetime 			not null,
	DamageCauses 	nvarchar(50)		null,
	ProblemDescription	nvarchar(50)		not null,
	Problemlmange 	nvarchar(200)		not null,
	RecordState 		int			not null,
	Repairman 	nvarchar(50)		null,
	RepairDates 	datetime 			null,
	DamageRecordReMark	nvarchar(200)		null
)

create table OfficeSuppliesRecordInfo
(
	OfficeID	Int	primary key identity(1,1),
	AssetID	int			Not null,
	OfficeApplylD 	int			Not null,
	OfficeApplyNum 	int			Not null,
	OfficeApplyState 	int			Not null,
	OfficeApplyDate	datetime			Not null,
	OfficeReceiveNum 	int			 null,
	OfficeReceiveDate	datetime			null,
	OfficeRemark		nvarchar(200)		null,
)