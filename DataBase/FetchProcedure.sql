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
   
   SELECT CM.ClassId AS ClsId
   ,CM.ClassName AS txt
   ,CM.DisplayOrder AS dpyor
   ,CM.IsActive  AS isc
   ,CM.Remarks   AS rmk
   ,CM.CreatedUserId AS cuid
   ,CM.LoginId   AS Logid
   FROM SchoolAcad.ClassMaster CM 
   --WHERE IsActive = 1

       SELECT fsp.id as spid, fsp.semPeriodName as [spn], ay.AcadYearName as [ayn], ay.insFullname as [ifn]
	    , ay.insShortName as [isn], ay.location as [lc]
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

    SELECT SM.SectionId AS SecId
   ,SM.SectionName AS txt
   ,SM.IsActive  AS isc
   ,SM.CreatedUserId AS cuid
   ,SM.LoginId   AS Logid
   FROM SchoolAcad.SectionMaster SM 
   --WHERE IsActive = 1

       SELECT fsp.id as spid, fsp.semPeriodName as [spn], ay.AcadYearName as [ayn], ay.insFullname as [ifn]
	    , ay.insShortName as [isn], ay.location as [lc]
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
	@AcadYearId SMALLINT
)
AS
BEGIN
	SELECT
		 CSA.AllocationId AS alid
		,CM.ClassId AS clsid
		,CM.ClassName AS clstxt
		,REPLACE(CM.ClassName, ' ', '') + '-' + SM.SectionName AS txt
		,SM.SectionId AS secid
		,SM.SectionName AS sectxt
		,CSA.AcadYearId AS ayid
		,CSA.IsActive AS isc
		,CSA.Remarks AS rmk
	FROM SchoolAcad.ClassSectionAllocation CSA
	INNER JOIN SchoolAcad.ClassMaster CM ON CSA.ClassId = CM.ClassId
	INNER JOIN SchoolAcad.SectionMaster SM ON CSA.SectionId = SM.SectionId
	WHERE CSA.AcadYearId = @AcadYearId

	SELECT fsp.id as spid, fsp.semPeriodName as [spn], ay.AcadYearName as [ayn], ay.insFullname as [ifn]
	    , ay.insShortName as [isn], ay.location as [lc]
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
		 SM.SubjectId     AS subid
		,SM.SubjectName   AS txt
		,SM.SubjectCode   AS subcode
		,SM.ShortName     AS shorttxt
		,SM.IsChoice      AS ischoice
		,SM.SubjTypeId    AS stypid
		,L.FullName       AS ftxt
		,SM.Remarks       AS rmk
	FROM SchoolAcad.SubjectMaster SM
	LEFT JOIN Data.Lookups L ON SM.SubjTypeId = L.LookupId

	SELECT fsp.id as spid, fsp.semPeriodName as [spn], ay.AcadYearName as [ayn], ay.insFullname as [ifn]
	    , ay.insShortName as [isn], ay.location as [lc]
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
  -- EXEC SchoolAcad.FetchClassSectionSubjectMap @ClassId = 2 ,@AcadYearId = 6 , @SectionId = 3

ALTER PROCEDURE SchoolAcad.FetchClassSectionSubjectMap
(
	@ClassId INT,
	@SectionId INT = NULL, 
	@AcadYearId SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		m.MapId AS Mapid,
		c.ClassName AS txt,
		c.ClassId AS clsid,
		s.SectionName AS secn,
		s.SectionId AS secid,
		sub.SubjectName AS ftxt,
		sub.SubjectId AS subid,
		m.AcadYearId AS ayid,
		m.IsActive AS isn,
		m.Remarks AS rmk,
		m.CreatedDateTime AS crdt,
		s.SectionId AS sessid
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

	SELECT fsp.id as spid, fsp.semPeriodName as [spn], ay.AcadYearName as [ayn], ay.insFullname as [ifn]
	    , ay.insShortName as [isn], ay.location as [lc]
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
    -- EXEC SchoolAcad.FetchSyllabusFinalization @ClassId = 1  ,@AcadYearId = 6
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
