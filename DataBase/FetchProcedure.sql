/*=========================================================================================================
                                       FetchClassMaster
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchClassMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchClassMaster AS SELECT 1'
END
GO
   -- EXEC SchoolAcad.FetchClassMaster
ALTER PROCEDURE [SchoolAcad].[FetchClassMaster]
AS
BEGIN
   SET NOCOUNT ON;

	   SELECT CM.ClassId AS ClassId
	   ,CM.ClassName AS ClassName
	   ,CM.DisplayOrder AS DisplayOrder
	   ,CM.IsActive  AS IsActive
	   ,CM.Remarks   AS Remarks
	   ,CM.CreatedUserId AS CreatedUserId
	   ,CM.LoginId   AS LoginId
	   ,CM.ModifiedUserId AS ModifiedUserId
	   ,CM.ModifiedDateTime AS ModifiedDateTime
	   FROM SchoolAcad.ClassMaster CM 
	   --WHERE IsActive = 1

		   SELECT fsp.id as semPeriodid, fsp.semPeriodName as [semPeriodName], ay.AcadYearName as [AcadYearName], ay.insFullname as [insFullname]
			, ay.insShortName as [insShortName], ay.location as [location]
			, Logo1Image AS [logo1]
			, Logo2Image AS [logo2]
			, CASE WHEN Logo1Image IS NULL THEN 1 ELSE 0 END AS [logost1]
			, CASE WHEN Logo2Image IS NULL THEN 1 ELSE 0 END AS [logost2]
		FROM Academic.FetchSemPeriodInfo() fsp
		INNER JOIN IBase.AllAcadYearInfo() ay ON fsp.AcadYearId = ay.AcadYearId
		INNER JOIN Data.Institutes i	      ON i.InstId = ay.instId
		WHERE ay.isActive = 1

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       FetchSectionMaster
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.FetchSectionMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchSectionMaster AS SELECT 1'
END
GO
 -- EXEC SchoolAcad.FetchSectionMaster
ALTER PROCEDURE [SchoolAcad].[FetchSectionMaster]
AS
BEGIN

    SELECT SM.SectionId AS SectionId
   ,SM.SectionName AS SectionName
   ,SM.IsActive  AS IsActive
   ,SM.CreatedUserId AS CreatedUserId
   ,SM.LoginId   AS LoginId
   FROM SchoolAcad.SectionMaster SM 
   --WHERE IsActive = 1

       SELECT fsp.id as semPeriodid, fsp.semPeriodName as [semPeriodName], ay.AcadYearName as [AcadYearName], ay.insFullname as [insFullname]
	    , ay.insShortName as [insShortName], ay.location as [location]
	    , Logo1Image AS [logo1]
	    , Logo2Image AS [logo2]
	    , CASE WHEN Logo1Image IS NULL THEN 1 ELSE 0 END AS [logost1]
	    , CASE WHEN Logo2Image IS NULL THEN 1 ELSE 0 END AS [logost2]
    FROM Academic.FetchSemPeriodInfo() fsp
    INNER JOIN IBase.AllAcadYearInfo() ay ON fsp.AcadYearId = ay.AcadYearId
    INNER JOIN Data.Institutes i	      ON i.InstId = ay.instId
	WHERE ay.isActive = 1
END
GO

/*=========================================================================================================
                                       FetchClassSectionAllocation
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchClassSectionAllocation',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchClassSectionAllocation AS SELECT 1'
END
GO

-- EXEC SchoolAcad.FetchClassSectionAllocation @AcadYearId = 6

ALTER PROCEDURE [SchoolAcad].[FetchClassSectionAllocation]
(
	@AcadYearId TINYINT
)
AS
BEGIN
	SELECT
		 CSA.AllocationId AS AllocationId
		,CM.ClassId AS ClassId
		,CM.ClassName AS ClassName
		,REPLACE(CM.ClassName, ' ', '') + '-' + SM.SectionName AS txt
		,SM.SectionId AS SectionId
		,SM.SectionName AS SectionName
		,CSA.AcadYearId AS AcadYearId
		,CSA.IsActive AS IsActive
		,CSA.Remarks AS Remarks
	FROM SchoolAcad.ClassSectionAllocation CSA
	INNER JOIN SchoolAcad.ClassMaster CM ON CSA.ClassId = CM.ClassId
	INNER JOIN SchoolAcad.SectionMaster SM ON CSA.SectionId = SM.SectionId
	WHERE CSA.AcadYearId = @AcadYearId

	SELECT fsp.id as semPeriodid, fsp.semPeriodName as [semPeriodName], ay.AcadYearName as [AcadYearName], ay.insFullname as [insFullname]
	    , ay.insShortName as [insShortName], ay.location as [location]
	    , Logo1Image AS [logo1]
	    , Logo2Image AS [logo2]
	    , CASE WHEN Logo1Image IS NULL THEN 1 ELSE 0 END AS [logost1]
	    , CASE WHEN Logo2Image IS NULL THEN 1 ELSE 0 END AS [logost2]
    FROM Academic.FetchSemPeriodInfo() fsp
    INNER JOIN IBase.AllAcadYearInfo() ay ON fsp.AcadYearId = ay.AcadYearId
    INNER JOIN Data.Institutes i	      ON i.InstId = ay.instId
	WHERE ay.isActive = 1
END
GO

/*=========================================================================================================
                                       FetchSubjectMaster
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.FetchSubjectMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchSubjectMaster AS SELECT 1'
END
GO
     -- EXEC SchoolAcad.FetchSubjectMaster
ALTER PROCEDURE [SchoolAcad].[FetchSubjectMaster]
AS
BEGIN
	SELECT
		 SM.SubjectId     AS SubjectId
		,SM.SubjectName   AS SubjectName
		,SM.SubjectCode   AS SubjectCode
		,SM.ShortName     AS ShortName
		,SM.IsChoice      AS IsChoice
		,SM.SubjTypeId    AS SubjTypeId
		,L.FullName       AS FullName
		,SM.Remarks       AS Remarks
	FROM SchoolAcad.SubjectMaster SM
	LEFT JOIN Data.Lookups L ON SM.SubjTypeId = L.LookupId

	SELECT fsp.id as semPeriodid, fsp.semPeriodName as [semPeriodName], ay.AcadYearName as [AcadYearName], ay.insFullname as [insFullname]
	    , ay.insShortName as [insShortName], ay.location as [location]
	    , Logo1Image AS [logo1]
	    , Logo2Image AS [logo2]
	    , CASE WHEN Logo1Image IS NULL THEN 1 ELSE 0 END AS [logost1]
	    , CASE WHEN Logo2Image IS NULL THEN 1 ELSE 0 END AS [logost2]
    FROM Academic.FetchSemPeriodInfo() fsp
    INNER JOIN IBase.AllAcadYearInfo() ay ON fsp.AcadYearId = ay.AcadYearId
    INNER JOIN Data.Institutes i	      ON i.InstId = ay.instId
	WHERE ay.isActive = 1
END
GO

/*=========================================================================================================
                                       FetchClassSectionSubjectMap
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.FetchClassSectionSubjectMap',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchClassSectionSubjectMap AS SELECT 1'
END
GO
  -- EXEC SchoolAcad.FetchClassSectionSubjectMap @ClassId = 2 ,@AcadYearId = 16 , @SectionId = 3

ALTER PROCEDURE SchoolAcad.FetchClassSectionSubjectMap
(
	@ClassId INT,
	@SectionId INT = NULL, 
	@AcadYearId TINYINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		m.MapId AS Mapid,
		c.ClassName AS ClassName,
		c.ClassId AS ClassId,
		s.SectionName AS SectionName,
		s.SectionId AS SectionId,
		sub.SubjectName AS SubjectName,
		sub.SubjectId AS SubjectId,
		m.AcadYearId AS AcadYearId,
		m.IsActive AS IsActive,
		m.Remarks AS Remarks,
		m.CreatedDateTime AS CreatedDateTime
	FROM SchoolAcad.ClassSectionSubjectMap m
	INNER JOIN SchoolAcad.ClassMaster c   ON c.ClassId = m.ClassId
	INNER JOIN SchoolAcad.SectionMaster s ON s.SectionId = m.SectionId
	INNER JOIN SchoolAcad.SubjectMaster sub ON sub.SubjectId = m.SubjectId
	WHERE m.ClassId = @ClassId
	  --AND m.SectionId = @SectionId
	  AND m.AcadYearId = @AcadYearId
	  AND m.IsActive = 1
	  --AND m.SectionId = ISNULL(@SectionId, m.SectionId)
	  AND (@SectionId IS NULL OR m.SectionId = @SectionId)
	ORDER BY sub.SubjectName;

	SELECT fsp.id as semPeriodid, fsp.semPeriodName as [semPeriodName], ay.AcadYearName as [AcadYearName], ay.insFullname as [insFullname]
	    , ay.insShortName as [insShortName], ay.location as [location]
	    , Logo1Image AS [logo1]
	    , Logo2Image AS [logo2]
	    , CASE WHEN Logo1Image IS NULL THEN 1 ELSE 0 END AS [logost1]
	    , CASE WHEN Logo2Image IS NULL THEN 1 ELSE 0 END AS [logost2]
    FROM Academic.FetchSemPeriodInfo() fsp
    INNER JOIN IBase.AllAcadYearInfo() ay ON fsp.AcadYearId = ay.AcadYearId
    INNER JOIN Data.Institutes i	      ON i.InstId = ay.instId
	WHERE ay.isActive = 1
END
GO

/*=========================================================================================================
                                       FetchSyllabusFinalization
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchSyllabusFinalization', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchSyllabusFinalization AS SELECT 1'
END
GO
    -- EXEC SchoolAcad.FetchSyllabusFinalization @ClassId = 2  ,@AcadYearId = 6
ALTER PROCEDURE SchoolAcad.FetchSyllabusFinalization
(
	@ClassId INT,
	@SectionId INT = NULL,
	@AcadYearId SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		sf.SyllabusFinalId,
		c.ClassName,
		s.SectionName,
		sub.SubjectName,
		sf.IsFinalized,
		sf.FinalizedDate,
		sf.Remarks
	FROM SchoolAcad.SyllabusFinalization sf
	INNER JOIN SchoolAcad.ClassMaster c ON c.ClassId = sf.ClassId
	INNER JOIN SchoolAcad.SectionMaster s ON s.SectionId = sf.SectionId
	INNER JOIN SchoolAcad.SubjectMaster sub ON sub.SubjectId = sf.SubjectId
	WHERE sf.ClassId = @ClassId
	 -- AND sf.SectionId = @SectionId
	  AND sf.AcadYearId = @AcadYearId
	  AND (@SectionId IS NULL OR s.SectionId = @SectionId) 
	ORDER BY sub.SubjectName;
END
GO

/*=========================================================================================================
                                       FetchAssessmentPolicy
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchAssessmentPolicy', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchAssessmentPolicy AS SELECT 1'
END
GO
   -- exec  SchoolAcad.FetchAssessmentPolicy @ClassId =1,@SubjectId =2,@AcadYearId=6
ALTER PROCEDURE SchoolAcad.FetchAssessmentPolicy
(
	@ClassId INT,
	@SubjectId INT,
	@AcadYearId SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pm.PolicyId,
		c.ClassName,
		s.SubjectName,
		pc.ComponentName,
		pc.MaxMarks,
		pc.Weightage,
		pc.IsMandatory,
		pc.DisplayOrder
	FROM SchoolAcad.AssessmentPolicyMaster pm
	INNER JOIN SchoolAcad.AssessmentPolicyComponent pc	ON pc.PolicyId = pm.PolicyId
	INNER JOIN SchoolAcad.ClassMaster c	ON c.ClassId = pm.ClassId
	INNER JOIN SchoolAcad.SubjectMaster s ON s.SubjectId = pm.SubjectId
	WHERE pm.ClassId = @ClassId
	  AND pm.SubjectId = @SubjectId
	  AND pm.AcadYearId = @AcadYearId
	ORDER BY pc.DisplayOrder
END
GO

/*=========================================================================================================
                                       FetchStaffSubjectAllocation
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.FetchStaffSubjectAllocation', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchStaffSubjectAllocation AS SELECT 1'
END
GO

   -- EXEC  SchoolAcad.FetchStaffSubjectAllocation @AcadYearId = 6

ALTER PROCEDURE SchoolAcad.FetchStaffSubjectAllocation
(
	--@ClassId INT,
	--@SectionId INT,
	@AcadYearId SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ssa.AllocationId AS [AllocationId],
		sbi.FullName AS [txt],
		c.ClassName AS [clsfn],
		s.SectionName AS [secname],
		sub.SubjectName AS [subfn],
		ssa.IsActive AS [st],
		--CASE WHEN ssa.IsActive = 0 then 'In-Active' else 'Active' end As [st],
		ssa.Remarks AS [rmk],
		ssa.CreatedDateTime AS [cdt],
		ay.YearName AS [yn]
	FROM SchoolAcad.StaffSubjectAllocation ssa
	INNER JOIN IBase.AcadYear ay ON ay.AcadyearId = ssa.AcadYearId
	INNER JOIN Staff.BasicInfo sbi ON sbi.StaffId = ssa.StaffId
	INNER JOIN SchoolAcad.ClassMaster c ON c.ClassId = ssa.ClassId
	INNER JOIN SchoolAcad.SectionMaster s ON s.SectionId = ssa.SectionId
	INNER JOIN SchoolAcad.SubjectMaster sub ON sub.SubjectId = ssa.SubjectId
	--WHERE a.ClassId = @ClassId
	--  AND a.SectionId = @SectionId
	  WHERE ssa.AcadYearId = @AcadYearId
	  AND ssa.IsActive = 1
	ORDER BY sub.SubjectName
END
GO

/*=========================================================================================================
                                       FetchLessonPlan
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchLessonPlan', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchLessonPlan AS SELECT 1'
END
GO
  -- EXEC SchoolAcad.FetchLessonPlan @ClassId = 2,@SubjectId = 4 ,@AcadYearId = 6  
ALTER PROCEDURE SchoolAcad.FetchLessonPlan
(
	@ClassId INT,
	--@SectionId INT,
	@SubjectId INT,
	@AcadYearId SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		lp.LessonPlanId AS Lessonpld,
		c.ClassName AS Clsn,
		c.ClassId AS clsid,
		sec.SectionId AS secid,
		sec.SectionName AS secfn,
		sub.SubjectId AS subid,
		sub.SubjectName AS Subfn,
		sbi.FullName AS TeacherName,
		sbi.StaffId AS staffid,
		lp.TopicTitle AS Toptil,
		lp.PlannedFromDate AS PlanFd,
		lp.PlannedToDate AS PlanTd,
		lp.StatusText AS Stustxt,
		lp.Remarks AS rmk,
		ay.YearName AS [yn],
		lp.Unit AS [unit],
		lp.TeachingMethod AS [TechMtd],
		lp.StudentLearningMethod AS [StudLrnMtd],
		lp.TopicDescription AS [TopDesc]
	FROM SchoolAcad.LessonPlan lp
	INNER JOIN IBase.AcadYear ay ON ay.AcadyearId = lp.AcadYearId
	INNER JOIN SchoolAcad.ClassMaster c ON c.ClassId = lp.ClassId
	INNER JOIN SchoolAcad.SectionMaster sec ON sec.SectionId = lp.SectionId
	INNER JOIN SchoolAcad.SubjectMaster sub ON sub.SubjectId = lp.SubjectId
	INNER JOIN Staff.BasicInfo sbi ON sbi.StaffId = lp.StaffId
	WHERE lp.ClassId = @ClassId
	  --AND lp.SectionId = @SectionId
	  AND lp.SubjectId = @SubjectId
	  AND lp.AcadYearId = @AcadYearId
	  AND lp.IsActive = 1
	ORDER BY lp.PlannedFromDate
END
GO

/*=========================================================================================================
                                       FetchClassroomTeaching
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchClassroomTeaching', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchClassroomTeaching AS SELECT 1'
END
GO
   -- EXEC SchoolAcad.FetchClassroomTeaching @AcadYearId=6,@ClassId=1,@SubjectId=1,@FromDate='2025-06-10',@ToDate='2026-01-28'
ALTER PROCEDURE SchoolAcad.FetchClassroomTeaching
(
    @AcadYearId SMALLINT,
    @ClassId INT,
    @SubjectId INT,
    @FromDate DATE,
    @ToDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ct.ClassroomTeachingId,
        ct.TeachingDate,
        ct.PeriodNo,
        ct.UnitNo,
        ct.TopicTitle,
		ct.TopicDescription,
        ct.TeachingMethod,
        ct.StudentLearningMethod,
        ct.IsCompleted,
        sbi.FullName AS TeacherName,
        ct.Remarks
    FROM SchoolAcad.ClassroomTeaching ct
    INNER JOIN Staff.BasicInfo sbi ON sbi.StaffId = ct.StaffId
    WHERE ct.AcadYearId = @AcadYearId
      AND ct.ClassId = @ClassId
      --AND ct.SectionId = @SectionId
      AND ct.SubjectId = @SubjectId
      AND ct.TeachingDate BETWEEN @FromDate AND @ToDate
	  --AND ct.TeachingDate = @FromDate
      AND ct.IsActive = 1
    ORDER BY ct.TeachingDate, ct.PeriodNo
END
GO

/*=========================================================================================================
                                       FetchAssignments
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchAssignments', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchAssignments AS SELECT 1'
END
GO
    -- EXEC SchoolAcad.FetchAssignments @ClassId = 4 , @SubjectId = 4 , @AcadYearId = 6
ALTER PROCEDURE SchoolAcad.FetchAssignments
(
    @ClassId INT,
    --@SectionId INT,
    @SubjectId INT,
    @AcadYearId SMALLINT
)
AS
BEGIN
    SELECT AssignmentId ,
        AssignmentType,
        Title,
        GivenDate,
        DueDate,
        MaxMarks
    FROM SchoolAcad.AssignmentMaster
    WHERE ClassId = @ClassId
      --AND SectionId = @SectionId
      AND SubjectId = @SubjectId
      AND AcadYearId = @AcadYearId
      AND IsActive = 1
END
GO

/*=========================================================================================================
                                       FetchAssignmentStudentMarks
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchAssignmentStudentMarks', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchAssignmentStudentMarks AS SELECT 1'
END
GO
   -- EXEC SchoolAcad.FetchAssignmentStudentMarks @AssignmentId = 1
ALTER PROCEDURE SchoolAcad.FetchAssignmentStudentMarks
(
    @AssignmentId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ASM.MarkId AS [mrkid],
        ASM.AssignmentId AS [Assid],
        ASM.StudentId AS [studid],
        S.FirstName AS [StudFn],
        ASM.SubmissionStatus AS [substs],
        ASM.ObtainedMarks AS [obtmrk],
        ASM.SubmissionDate AS [SubDte],
        ASM.Remarks AS [rmk]
    FROM SchoolAcad.AssignmentStudentMark ASM
    INNER JOIN Academic.StudAdmnInfo S  ON ASM.StudentId = S.StudentId
    WHERE ASM.AssignmentId = @AssignmentId
    ORDER BY S.StudentName;

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       FetchElectiveStudents
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchElectiveStudents', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchElectiveStudents AS SELECT 1'
END
GO
  -- EXEC SchoolAcad.FetchElectiveStudents @ClassId = 1 , @AcadYearId = 6
ALTER PROCEDURE SchoolAcad.FetchElectiveStudents
(
    @ClassId INT,
    --@SubjectId INT,
    @AcadYearId SMALLINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StudentId,
        s.FullName,
        e.SubjectId,
        sub.SubjectName
    FROM SchoolAcad.StudentElectiveSubject e
    INNER JOIN Academic.StudAdmnInfo s ON s.StudentId = e.StudentId
    INNER JOIN SchoolAcad.SubjectMaster sub ON sub.SubjectId = e.SubjectId
    WHERE e.ClassId = @ClassId
     -- AND e.SubjectId = @SubjectId
      AND e.AcadYearId = @AcadYearId
      AND e.IsActive = 1
    ORDER BY s.StudentName;

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       FetchHourSetting
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchHourSetting', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchHourSetting AS SELECT 1'
END
GO
   --- EXEC SchoolAcad.FetchHourSetting @ClassId = 5

ALTER PROCEDURE SchoolAcad.FetchHourSetting
(
    @ClassId INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        hs.HourSettingId,
        hs.HourId,
        hs.FromTime,
        hs.ToTime,
        hs.IsCommon,
        hs.IsActive,
        hs.ClassId
    FROM SchoolAcad.HourSetting hs
    WHERE hs.IsActive = 1
      AND (hs.ClassId = @ClassId OR hs.IsCommon = 1)
    ORDER BY hs.HourId
END
GO

/*=========================================================================================================
                                       FetchTimeTable
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.FetchTimeTable', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchTimeTable AS SELECT 1'
END
GO
  -- exec SchoolAcad.FetchTimeTable @AcadYearId = 6,@ClassId=5,@SectionId=2
ALTER PROCEDURE SchoolAcad.FetchTimeTable
(
    @AcadYearId TINYINT,
    @ClassId    INT,
    @SectionId  INT
)
AS
BEGIN
    SELECT
        tt.TimeTableId,
        d.FullName,
        tt.PeriodNoId,
        hs.FromTime,
        hs.ToTime,
        s.SubjectName,
        st.FIrstName
    FROM SchoolAcad.TimeTable tt
    INNER JOIN Data.TimeTableDays d ON d.DayId = tt.DayId
    INNER JOIN SchoolAcad.HourSetting hs ON hs.HourSettingId = tt.HourId
    INNER JOIN SchoolAcad.SubjectMaster s ON s.SubjectId = tt.SubjectId
    INNER JOIN Staff.Basicinfo st ON st.StaffId = tt.StaffId
    WHERE tt.AcadYearId = @AcadYearId
      AND tt.ClassId    = @ClassId
      AND tt.SectionId  = @SectionId
      AND tt.IsActive   = 1
    ORDER BY d.DayId, tt.PeriodNoId
END
GO
/*=========================================================================================================
                                     
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.FetchStudentCurrentClass', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchStudentCurrentClass AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.FetchStudentCurrentClass
(
      @StudentId  INT
    , @AcadYearCr TINYINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sc.StudentId,
        s.FirstName,
        c.ClassName,
        ay.YearName
    FROM SchoolAcad.StudClasses sc
    INNER JOIN Academic.StudAdmnInfo s ON s.StudentId = sc.StudentId
    INNER JOIN SchoolAcad.ClassMaster c ON c.ClassId = sc.ClassId
    INNER JOIN IBase.AcadYear ay ON ay.AcadYearId = sc.AcadYearCr
    WHERE sc.StudentId = @StudentId
      AND sc.AcadYearCr = @AcadYearCr
END
GO



IF OBJECT_ID(N'SchoolAcad.FetchClassStudents', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.FetchClassStudents AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.FetchClassStudents
(
      @AcadYearCr TINYINT
    , @ClassId    INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sc.StudClassId,
        sc.StudentId,
        s.StudentName
    FROM SchoolAcad.StudClasses sc
    INNER JOIN Academic.StudAdmnInfo s ON s.StudentId = sc.StudentId
    WHERE sc.AcadYearCr = @AcadYearCr
      AND sc.ClassId = @ClassId
    ORDER BY s.StudentName
END
GO
