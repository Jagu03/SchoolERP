IF OBJECT_ID(N'[SchoolAcad].[ClassMaster]',N'U') IS NULL 
BEGIN 
    -- SELECT * FROM SchoolAcad.ClassMaster
	-- DROP TABLE SchoolAcad.ClassMaster
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

IF OBJECT_ID(N'[SchoolAcad].[AssessmentPolicyMaster]', N'U') IS NULL
BEGIN
    -- SELECT * FROM SchoolAcad.AssessmentPolicyMaster
	CREATE TABLE SchoolAcad.AssessmentPolicyMaster
	(
		PolicyId        INT            NOT NULL      IDENTITY(1,1),
		ClassId         INT            NOT NULL,
		SubjectId       INT            NOT NULL,
		AcadYearId      TINYINT        NOT NULL,
		IsActive        TINYINT        NOT NULL DEFAULT 1,
		Remarks         NVARCHAR(1000) NULL,
		CreatedUserId   SMALLINT       NOT NULL,
		CreatedDateTime DATETIME       NOT NULL DEFAULT GETDATE(),
		LoginId         BIGINT         NOT NULL,
		CONSTRAINT PK_AssessmentPolicyMaster PRIMARY KEY (PolicyId),
		CONSTRAINT UK_AssessmentPolicyMaster UNIQUE (ClassId, SubjectId, AcadYearId),
		CONSTRAINT FK_AssessmentPolicyMaster_Class FOREIGN KEY (ClassId)		REFERENCES SchoolAcad.ClassMaster(ClassId),
		CONSTRAINT FK_AssessmentPolicyMaster_Subject FOREIGN KEY (SubjectId)	REFERENCES SchoolAcad.SubjectMaster(SubjectId),
		CONSTRAINT FK_AssessmentPolicyMaster_AcadYearId FOREIGN KEY (AcadYearId)REFERENCES IBase.AcadYear(AcadYearId),
		CONSTRAINT FK_AssessmentPolicyMaster_User FOREIGN KEY (CreatedUserId)	REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[AssessmentPolicyComponent]', N'U') IS NULL
BEGIN
     -- SELECT * FROM SchoolAcad.AssessmentPolicyComponent
	 -- DROP TABLE SchoolAcad.AssessmentPolicyComponent
	CREATE TABLE SchoolAcad.AssessmentPolicyComponent
	(
		ComponentId     INT          NOT NULL    IDENTITY(1,1),
		PolicyId        INT          NOT NULL,
		ComponentName   NVARCHAR(50) NOT NULL, -- Internal / External / Project
		MaxMarks        INT          NOT NULL,
		Weightage       DECIMAL(5,2) NOT NULL,
		IsMandatory     TINYINT      NOT NULL DEFAULT 1,
		DisplayOrder    INT          NOT NULL,
		Remarks         NVARCHAR(1000) NULL,
		CreatedUserId   SMALLINT       NOT NULL,
		CreatedDateTime DATETIME       NOT NULL DEFAULT GETDATE(),
		LoginId         BIGINT         NOT NULL,
		CONSTRAINT PK_AssessmentPolicyComponent PRIMARY KEY (ComponentId),
		CONSTRAINT UK_AssessmentPolicyComponent__ComponentName__PolicyId UNIQUE NONCLUSTERED (ComponentName,PolicyId),
		CONSTRAINT FK_AssessmentPolicyComponent__PolicyId FOREIGN KEY (PolicyId)	REFERENCES SchoolAcad.AssessmentPolicyMaster(PolicyId),
		CONSTRAINT FK_AssessmentPolicyComponent__CreatedUserId FOREIGN KEY (CreatedUserId)	REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[StaffSubjectAllocation]', N'U') IS NULL
BEGIN
    -- SELECT * FROM SchoolAcad.StaffSubjectAllocation
	CREATE TABLE SchoolAcad.StaffSubjectAllocation
	(
		AllocationId     INT			NOT NULL    IDENTITY(1,1),
		StaffId          INT			NOT NULL,
		ClassId          INT			NOT NULL,
		SectionId        INT			NOT NULL,
		SubjectId        INT            NOT NULL,
		AcadYearId       TINYINT        NOT NULL,
		IsActive         TINYINT        NOT NULL DEFAULT 1,
		Remarks          NVARCHAR(1000) NULL,
		CreatedUserId    SMALLINT       NOT NULL,
		CreatedDateTime  DATETIME       NOT NULL DEFAULT GETDATE(),
		LoginId          BIGINT         NOT NULL,
		CONSTRAINT PK_StaffSubjectAllocation PRIMARY KEY (AllocationId),
		CONSTRAINT UK_StaffSubjectAllocation__StaffId_ClassId_SectionId_SubjectId_AcadYearId
		                UNIQUE NONCLUSTERED(StaffId, ClassId, SectionId, SubjectId, AcadYearId),
		CONSTRAINT FK_StaffSubjectAllocation_StaffId FOREIGN KEY (StaffId)  REFERENCES Staff.BasicInfo(StaffId),
		CONSTRAINT FK_StaffSubjectAllocation_Class FOREIGN KEY (ClassId)	  REFERENCES SchoolAcad.ClassMaster(ClassId),
		CONSTRAINT FK_StaffSubjectAllocation_Section FOREIGN KEY (SectionId)REFERENCES SchoolAcad.SectionMaster(SectionId),
		CONSTRAINT FK_StaffSubjectAllocation_Subject FOREIGN KEY (SubjectId)REFERENCES SchoolAcad.SubjectMaster(SubjectId),
		CONSTRAINT FK_StaffSubjectAllocation_AcadYear FOREIGN KEY (AcadYearId)	REFERENCES IBase.AcadYear(AcadYearId),
		CONSTRAINT FK_StaffSubjectAllocation_CreatedUserId FOREIGN KEY (CreatedUserId)REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'[SchoolAcad].[LessonPlan]', N'U') IS NULL
BEGIN
    -- SELECT * FROM SchoolAcad.LessonPlan
	-- DROP TABLE SchoolAcad.LessonPlan
	CREATE TABLE SchoolAcad.LessonPlan
	(
		LessonPlanId     INT			NOT NULL   IDENTITY(1,1),
		ClassId          INT			NOT NULL,
		SectionId        INT			NOT NULL,
		SubjectId        INT			NOT NULL,
		StaffId          INT			NOT NULL,
		AcadYearId       TINYINT		NOT NULL,
		TopicTitle       NVARCHAR(200)  NOT NULL,
		TopicDescription NVARCHAR(2000) NULL,
		PlannedFromDate  DATE			NOT NULL,
		PlannedToDate    DATE			NOT NULL,
		Unit             TINYINT        NOT NULL,
		TeachingMethod   NVARCHAR(100)  NOT NULL,
		StudentLearningMethod NVARCHAR(100)   NOT NULL,
		StatusText       NVARCHAR(20)   NOT NULL DEFAULT 'Planned', 	-- Planned / InProgress / Completed
		IsActive         TINYINT        NOT NULL DEFAULT 1,
		Remarks          NVARCHAR(1000) NULL,
		CreatedUserId    SMALLINT       NOT NULL,
		CreatedDateTime  DATETIME		NOT NULL DEFAULT GETDATE(),
		LoginId          BIGINT			NOT NULL,
		CONSTRAINT PK_LessonPlan PRIMARY KEY (LessonPlanId),
		CONSTRAINT FK_LessonPlan_Class FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId),
		CONSTRAINT UK_LessonPlan__AcadYearId_ClassId_SubjectId_StaffId_Unit_TopicTitle UNIQUE NONCLUSTERED (AcadYearId,ClassId,SubjectId,StaffId,Unit,TopicTitle),
		CONSTRAINT FK_LessonPlan_Section FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId),
		CONSTRAINT FK_LessonPlan_Subject FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId),
		CONSTRAINT FK_LessonPlan_StaffId FOREIGN KEY (StaffId) REFERENCES Staff.Basicinfo(StaffId),
		CONSTRAINT FK_LessonPlan_AcadYear FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId),
		CONSTRAINT FK_LessonPlan_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
	)
END
GO

IF OBJECT_ID(N'SchoolAcad.ClassroomTeaching', N'U') IS NULL
BEGIN
    --  SELECT * FROM  SchoolAcad.ClassroomTeaching
	--  DROP TABLE SchoolAcad.ClassroomTeaching
    CREATE TABLE SchoolAcad.ClassroomTeaching
    (
        ClassroomTeachingId   INT			NOT NULL    IDENTITY(1,1),
        AcadYearId			  TINYINT		NOT NULL,
        ClassId				  INT			NOT NULL,
        SectionId			  INT			NOT NULL,
        SubjectId			  INT			NOT NULL,
        StaffId				  INT			NOT NULL,
        TeachingDate		  DATE			NOT NULL,
        PeriodNo              TINYINT		NOT NULL,      -- 1 to 8
        UnitNo				  TINYINT		NOT NULL,
        TopicTitle            NVARCHAR(200)	NOT NULL,
        TopicDescription      NVARCHAR(200)	NULL,
        TeachingMethod        NVARCHAR(200) NOT NULL,
        StudentLearningMethod NVARCHAR(200) NOT NULL,
        IsCompleted			  TINYINT		NOT NULL DEFAULT 1,
        Remarks				  NVARCHAR(1000)NULL,
        IsActive			  TINYINT		NOT NULL DEFAULT 1,
        CreatedUserId		  SMALLINT		NOT NULL,
        CreatedDateTime		  DATETIME		NOT NULL DEFAULT GETDATE(),
        ModifiedUserId		  SMALLINT		NULL,
        ModifiedDateTime	  DATETIME		NULL,
        LoginId				  BIGINT		NOT NULL,

        CONSTRAINT PK_ClassroomTeaching PRIMARY KEY (ClassroomTeachingId),
		CONSTRAINT UK_ClassroomTeaching__AcadYearId_ClassId_SubjectId_StaffId_UnitNo_PeriodNo_TopicTitle UNIQUE NONCLUSTERED (AcadYearId,ClassId,SubjectId,StaffId,UnitNo,PeriodNo,TopicTitle),
        CONSTRAINT FK_ClassroomTeaching_AcadYear FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId),
        CONSTRAINT FK_ClassroomTeaching_Class FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId),
        CONSTRAINT FK_ClassroomTeaching_Section FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId),
        CONSTRAINT FK_ClassroomTeaching_Subject FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId),
        CONSTRAINT FK_ClassroomTeaching_Staff FOREIGN KEY (StaffId) REFERENCES Staff.BasicInfo(StaffId),
		CONSTRAINT FK_ClassroomTeaching_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
    )
END
GO

IF OBJECT_ID(N'SchoolAcad.AssignmentMaster', N'U') IS NULL
BEGIN
    --  SELECT * FROM  SchoolAcad.AssignmentMaster
	--  DROP TABLE SchoolAcad.AssignmentMaster
    CREATE TABLE SchoolAcad.AssignmentMaster
    (
        AssignmentId	INT  NOT NULL IDENTITY(1,1),
        AcadYearId		TINYINT NOT NULL,
        ClassId			INT NOT NULL,
        SectionId		INT NOT NULL,
        SubjectId		INT NOT NULL,
        StaffId			INT NOT NULL,
        AssignmentType  NVARCHAR(20) NOT NULL,       -- Assignment / Homework
        Title			NVARCHAR(200) NOT NULL,
        TitleDescription NVARCHAR(2000) NULL,
        GivenDate		DATE NOT NULL,
        DueDate			DATE NOT NULL,
        MaxMarks		INT NOT NULL,
        IsActive		TINYINT NOT NULL DEFAULT 1,
        Remarks			NVARCHAR(1000) NULL,
        CreatedUserId	SMALLINT NOT NULL,
        CreatedDateTime DATETIME NOT NULL DEFAULT GETDATE(),
        LoginId			BIGINT NOT NULL,
        CONSTRAINT PK_AssignmentMaster PRIMARY KEY (AssignmentId),
		CONSTRAINT UK_AssignmentMaster__AcadYearId_ClassId_SectionId_SubjectId_StaffId_Title UNIQUE NONCLUSTERED (AcadYearId,ClassId,SectionId,SubjectId,StaffId,Title),
        CONSTRAINT FK_AssignmentMaster_AcadYearId FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId),
        CONSTRAINT FK_AssignmentMaster_ClassId FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId),
        CONSTRAINT FK_AssignmentMaster_SectionId FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId),
        CONSTRAINT FK_AssignmentMaster_SubjectId FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId),
        CONSTRAINT FK_AssignmentMaster_StaffId FOREIGN KEY (StaffId) REFERENCES Staff.BasicInfo(StaffId),
		CONSTRAINT FK_AssignmentMaster_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
    )
END
GO

IF OBJECT_ID(N'SchoolAcad.AssignmentStudentMark', N'U') IS NULL
BEGIN
    -- SELECT * FROM SchoolAcad.AssignmentStudentMark
	-- DROP TABLE SchoolAcad.AssignmentStudentMark
    CREATE TABLE SchoolAcad.AssignmentStudentMark
    (
        MarkId          INT NOT NULL IDENTITY(1,1),
        AssignmentId    INT NOT NULL,
        StudentId       INT NOT NULL,
        SubmissionStatus NVARCHAR(20) NOT NULL,       -- Submitted / Late / NotSubmitted / Exempt
        ObtainedMarks   INT NULL,
        SubmissionDate  DATE NULL,
        Remarks			NVARCHAR(500) NULL,
        CreatedUserId   SMALLINT NOT NULL,
        CreatedDateTime DATETIME NOT NULL DEFAULT GETDATE(),
		LoginId         BIGINT			NOT NULL,
        CONSTRAINT PK_AssignmentStudentMark PRIMARY KEY (MarkId),
		CONSTRAINT UK_AssignmentStudentMark_AssignmentId_StudentId UNIQUE NONCLUSTERED (AssignmentId,StudentId),
        CONSTRAINT FK_AssignmentStudentMark_Assignment  FOREIGN KEY (AssignmentId)  REFERENCES SchoolAcad.AssignmentMaster(AssignmentId),
		CONSTRAINT FK__AssignmentStudentMark__StudentId	FOREIGN KEY (StudentId)			REFERENCES Academic.StudAdmnInfo(StudentId),		
		CONSTRAINT FK_AssignmentStudentMark_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
    )
END
GO

IF OBJECT_ID(N'SchoolAcad.StudentElectiveSubject', N'U') IS NULL
BEGIN
    -- SELECT * FROM SchoolAcad.StudentElectiveSubject
	-- DROP TABLE SchoolAcad.StudentElectiveSubject
    CREATE TABLE SchoolAcad.StudentElectiveSubject
    (
        StudentElectiveId INT IDENTITY(1,1) PRIMARY KEY,
        StudentId         INT NOT NULL,
        ClassId           INT NOT NULL,
        AcadYearId        TINYINT NOT NULL,
        SubjectId         INT NOT NULL,   -- Hindi / French
        IsActive          TINYINT NOT NULL DEFAULT 1,
        CreatedUserId     SMALLINT NOT NULL,
        CreatedDateTime   DATETIME NOT NULL DEFAULT GETDATE(),
        LoginId           BIGINT NOT NULL,
        CONSTRAINT UK_StudentElectiveSubject_StudentId_AcadYearId UNIQUE NONCLUSTERED (StudentId, AcadYearId),
        CONSTRAINT FK_StudentElectiveSubject_Student FOREIGN KEY (StudentId) REFERENCES Academic.StudAdmnInfo(StudentId),
        CONSTRAINT FK_StudentElectiveSubject_Subject FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId),
        CONSTRAINT FK_StudentElectiveSubject_Class FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId),
		CONSTRAINT FK_StudentElectiveSubject_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
    )
END
GO

IF OBJECT_ID(N'SchoolAcad.TimeTable', N'U') IS NULL
BEGIN
    -- SELECT * FROM  SchoolAcad.TimeTable
	-- DROP TABLE SchoolAcad.TimeTable
    CREATE TABLE SchoolAcad.TimeTable
    (
        TimeTableId      INT          NOT NULL    IDENTITY(1,1),
        AcadYearId       TINYINT      NOT NULL,
        ClassId          INT		  NOT NULL,
        SectionId        INT		  NOT NULL,
        DayOfWeekId      TINYINT	  NOT NULL, -- 1=Mon,2=Tue...
        PeriodNoId       TINYINT      NOT NULL, -- 1 to 8
        SubjectId        INT	      NOT NULL,
        StaffId          INT		  NOT NULL,
		StaffSubjectId   INT		  NOT NULL,
        IsActive         TINYINT      NOT NULL DEFAULT 1,
        Remarks          NVARCHAR(500) NULL,
        CreatedUserId    SMALLINT     NOT NULL,
        CreatedDateTime  DATETIME     NOT NULL DEFAULT GETDATE(),
        ModifiedUserId   SMALLINT     NULL,
        ModifiedDateTime DATETIME     NULL,
        LoginId          BIGINT       NOT NULL,       
		CONSTRAINT PK_TimeTable_TimeTableId PRIMARY KEY (TimeTableId),
		CONSTRAINT FK_TimeTable_ClassId FOREIGN KEY (ClassId) REFERENCES SchoolAcad.ClassMaster(ClassId),
		CONSTRAINT UK_TimeTable__AcadYearId_ClassId_SubjectId_StaffId_DayOfWeekId_PeriodNoId_StaffSubjectId UNIQUE NONCLUSTERED (AcadYearId,ClassId,SubjectId,StaffId,DayOfWeekId,PeriodNoId,StaffSubjectId),
		CONSTRAINT FK_TimeTable_SectionId FOREIGN KEY (SectionId) REFERENCES SchoolAcad.SectionMaster(SectionId),
		CONSTRAINT FK_TimeTable_SubjectId FOREIGN KEY (SubjectId) REFERENCES SchoolAcad.SubjectMaster(SubjectId),
		CONSTRAINT FK_TimeTable_StaffId FOREIGN KEY (StaffId) REFERENCES Staff.Basicinfo(StaffId),
		CONSTRAINT FK_TimeTable_AcadYearId FOREIGN KEY (AcadYearId) REFERENCES IBase.AcadYear(AcadYearId),
		CONSTRAINT StaffSubjectId  FOREIGN KEY (StaffSubjectId) REFERENCES SchoolAcad.StaffSubjectAllocation (AllocationId),
		CONSTRAINT FK_TimeTable_CreatedUserId FOREIGN KEY (CreatedUserId) REFERENCES HR.Users(UserId)
    )
END
GO