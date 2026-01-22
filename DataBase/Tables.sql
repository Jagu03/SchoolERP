IF OBJECT_ID(N'[SchoolAcad].[ClassMaster]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.ClassMaster
	CREATE TABLE SchoolAcad.ClassMaster
	(
	 ClassId            INT		        NOT NULL	IDENTITY(1,1)
	  ,ClassName        NVARCHAR(50)    NOT NULL
	  ,DisplayOrder     INT             NOT NULL
      ,IsActive         TINYINT         NOT NULL    DEFAULT 1   
	  ,Remarks			NVARCHAR(1000)	NULL
	  ,CreatedUserId	SMALLINT		NOT NULL
	  ,CreatedDateTime	DATETIME		NOT NULL	DEFAULT GETDATE()
	  ,LoginId			BIGINT			NOT NULL
	  ,CONSTRAINT  PK__ClassMaster__ClassId       PRIMARY KEY (ClassId)
	  ,CONSTRAINT  UK__ClassMaster__ClassName     UNIQUE NONCLUSTERED (ClassName)
	  ,CONSTRAINT  FK__ClassMaster__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[SectionMaster]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.SectionMaster
	-- DROP TABLE SchoolAcad.SectionMaster
	CREATE TABLE SchoolAcad.SectionMaster
	(
	 SectionId          INT		        NOT NULL	IDENTITY(1,1)
	  ,SectionName      NVARCHAR(50)    NOT NULL
      ,IsActive         TINYINT         NOT NULL    DEFAULT 1   
	  ,CreatedUserId	SMALLINT		NOT NULL
	  ,CreatedDateTime	DATETIME		NOT NULL	DEFAULT GETDATE()
	  ,LoginId			BIGINT			NOT NULL
	  ,CONSTRAINT  PK__SectionMaster__SectionId       PRIMARY KEY (SectionId)
	  ,CONSTRAINT  UK__SectionMaster__SectionName     UNIQUE NONCLUSTERED (SectionName)
	  ,CONSTRAINT  FK__SectionMaster__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[ClassSectionAllocation]',N'U') IS NULL 
BEGIN
    -- SELECT * FROM SchoolAcad.ClassSectionAllocation
	-- DROP TABLE SchoolAcad.ClassSectionAllocation
	CREATE TABLE SchoolAcad.ClassSectionAllocation
	(
		 AllocationId      INT            NOT NULL        IDENTITY(1,1)
		,ClassId           INT            NOT NULL
		,SectionId         INT            NOT NULL
		,AcadYearId        TINYINT        NOT NULL
		,IsActive          TINYINT        NOT NULL DEFAULT 1
		,Remarks           NVARCHAR(1000) NULL
		,CreatedUserId     SMALLINT       NOT NULL
		,CreatedDateTime   DATETIME       NOT NULL DEFAULT GETDATE()
		,LoginId           BIGINT         NOT NULL
	    ,CONSTRAINT  PK__ClassSectionAllocation__AllocationId       PRIMARY KEY (AllocationId)
	    ,CONSTRAINT  FK__ClassMaster__ClassId  FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId)
		,CONSTRAINT  FK__ClassSectionAllocation__SectionId FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId)
		,CONSTRAINT  UK__ClassSectionAllocation__ClassId_SectionId_AcadYearId UNIQUE NONCLUSTERED (ClassId,SectionId,AcadYearId)
		,CONSTRAINT  FK__ClassSectionAllocation__AcadYearId  FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId)
	    ,CONSTRAINT  FK__ClassSectionAllocation__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[SubjectMaster]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.SubjectMaster
	-- DROP TABLE SchoolAcad.SubjectMaster
	CREATE TABLE SchoolAcad.SubjectMaster
	(
	 SubjectId          INT		        NOT NULL	IDENTITY(1,1)
	  ,SubjectName      NVARCHAR(50)    NOT NULL
	  ,SubjectCode      NVARCHAR(50)    NULL
	  ,ShortName        NVARCHAR(50)    NULL
	  ,IsChoice         TINYINT         NULL        DEFAULT 0
      ,SubjTypeId       SMALLINT        NOT NULL   
	  ,Remarks          NVARCHAR(100)   NULL
	  ,CreatedUserId	SMALLINT		NOT NULL
	  ,CreatedDateTime	DATETIME		NOT NULL	DEFAULT GETDATE()
	  ,LoginId			BIGINT			NOT NULL
	  ,CONSTRAINT  PK__SubjectMaster__SubjectId       PRIMARY KEY (SubjectId)
	  ,CONSTRAINT  UK__SubjectMaster__SubjectName_SubjectCode     UNIQUE NONCLUSTERED (SubjectName,SubjectCode)
	  ,CONSTRAINT  FK__SubjectMaster__SubjTypeId  FOREIGN KEY (SubjTypeId) REFERENCES Data.Lookups(LookupId)
	  ,CONSTRAINT  FK__SubjectMaster__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[ClassSectionSubjectMap]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.ClassSectionSubjectMap
	-- DROP TABLE SchoolAcad.ClassSectionSubjectMap
	CREATE TABLE SchoolAcad.ClassSectionSubjectMap
	(
	  MapId             INT		        NOT NULL	IDENTITY(1,1)
	  ,ClassId          INT             NOT NULL
	  ,SectionId        INT             NOT NULL
	  ,SubjectId        INT             NOT NULL
	  ,AcadYearId       TINYINT         NOT NULL
	  ,IsActive         TINYINT         NOT NULL    DEFAULT 1
   	  ,Remarks          NVARCHAR(1000)  NULL
	  ,CreatedUserId	SMALLINT		NOT NULL
	  ,CreatedDateTime	DATETIME		NOT NULL	DEFAULT GETDATE()
	  ,LoginId			BIGINT			NOT NULL
	  ,CONSTRAINT  PK__ClassSectionSubjectMap__MapId    PRIMARY KEY (MapId)
	  ,CONSTRAINT  UK__ClassSectionSubjectMap__ClassId_SectionId_SubjectId_AcadYearId    UNIQUE NONCLUSTERED (ClassId,SectionId,SubjectId,AcadYearId)
	  ,CONSTRAINT  FK__ClassSectionSubjectMap__ClassId  FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId)
	  ,CONSTRAINT  FK__ClassSectionSubjectMap__SectionId  FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId)
	  ,CONSTRAINT  FK__ClassSectionSubjectMap__SubjectId  FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId)
	  ,CONSTRAINT  FK__ClassSectionSubjectMap__AcadYearId  FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId)
	  ,CONSTRAINT  FK__ClassSectionSubjectMap__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[SyllabusFinalization]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.SyllabusFinalization
	-- DROP TABLE SchoolAcad.SyllabusFinalization
	CREATE TABLE SchoolAcad.SyllabusFinalization
	(
	  SyllabusFinalId   INT		        NOT NULL	IDENTITY(1,1)
	  ,ClassId          INT             NOT NULL
	  ,SectionId        INT             NOT NULL
	  ,SubjectId        INT             NOT NULL
	  ,AcadYearId       TINYINT         NOT NULL
	  ,IsFinalized      TINYINT         NOT NULL DEFAULT 0
	  ,FinalizedDate    DATETIME        NULL
   	  ,Remarks          NVARCHAR(1000)  NULL
	  ,CreatedUserId	SMALLINT		NOT NULL
	  ,CreatedDateTime	DATETIME		NOT NULL	DEFAULT GETDATE()
	  ,LoginId			BIGINT			NOT NULL
	  ,CONSTRAINT  PK__SyllabusFinalization__SyllabusFinalId    PRIMARY KEY (SyllabusFinalId)
	  ,CONSTRAINT  UK__SyllabusFinalization__ClassId_SectionId_SubjectId_AcadYearId    UNIQUE NONCLUSTERED (ClassId,SectionId,SubjectId,AcadYearId)
	  ,CONSTRAINT  FK__SyllabusFinalization__ClassId  FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId)
	  ,CONSTRAINT  FK__SyllabusFinalization__SectionId  FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId)
	  ,CONSTRAINT  FK__SyllabusFinalization__SubjectId  FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId)
	  ,CONSTRAINT  FK__SyllabusFinalization__AcadYearId  FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId)
	  ,CONSTRAINT  FK__SyllabusFinalization__CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO