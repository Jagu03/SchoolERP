/*=========================================================================================================
                                       MergeClassMaster
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeClassMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeClassMaster AS SELECT 1'
END
GO

ALTER PROCEDURE [SchoolAcad].[MergeClassMaster]
(
	  @EditId		    INT=0
	, @ClassName		NVARCHAR(350)
	, @DisplayOrder		INT
	, @IsActive         TINYINT
	, @Remarks			NVARCHAR(1000)
	, @CreatedUserId	SMALLINT	
	, @LoginId			BIGINT = 0
	, @result           NVARCHAR(350) = '' OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
SET NOCOUNT ON

	    IF @EditId = 0
	    BEGIN
		IF EXISTS (SELECT 1 FROM [SchoolAcad].[ClassMaster] WHERE ClassName = @ClassName)
		    BEGIN
		        SET @result = 'The Specified ClassName already exists.'
			END
			ELSE
			BEGIN
				INSERT INTO [SchoolAcad].[ClassMaster](ClassName,DisplayOrder,IsActive,Remarks,CreatedUserId,LoginId)
				VALUES (@ClassName,@DisplayOrder,@IsActive,@Remarks,@CreatedUserId,@LoginId)

			    SET @result = 'The ClassName Saved Successfully'
            END
           END
	     ELSE
	    BEGIN
		   UPDATE [SchoolAcad].[ClassMaster] SET
			   ClassName =@ClassName,
			   DisplayOrder = @DisplayOrder,
			   IsActive = @IsActive,
			   Remarks = @Remarks,
			   CreatedUserId = @CreatedUserId,
			   LoginId = @LoginId
		  WHERE ClassId = @EditId

		SET @result = 'The ClassName Updated Successfully.'
		END

SET NOCOUNT OFF
END
GO

/*=========================================================================================================
                                       MergeSectionMaster
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.MergeSectionMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeSectionMaster AS SELECT 1'
END
GO

ALTER PROCEDURE [SchoolAcad].[MergeSectionMaster]
(
	  @EditId		    INT=0
	, @SectionName		NVARCHAR(350)
	, @IsActive         TINYINT	
	, @CreatedUserId	SMALLINT	
	, @LoginId			BIGINT = 0
	, @result           NVARCHAR(350) = '' OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
SET NOCOUNT ON

 IF @EditId = 0
	    BEGIN
		IF EXISTS (SELECT 1 FROM [SchoolAcad].[SectionMaster] WHERE SectionName = @SectionName)
		    BEGIN
		        SET @result = 'The Specified SectionName already exists.'
			END
			ELSE
			BEGIN
				INSERT INTO [SchoolAcad].[SectionMaster](SectionName,IsActive,CreatedUserId,LoginId)
				VALUES (@SectionName,@IsActive,@CreatedUserId,@LoginId)

				SET @result = 'The SectionName Saved Successfully'
			END
           END
	     ELSE
	    BEGIN
		     UPDATE [SchoolAcad].[SectionMaster] SET
			   SectionName =@SectionName,
			   IsActive = @IsActive,
			   CreatedUserId = @CreatedUserId,
			   LoginId = @LoginId
		  WHERE SectionId = @EditId

		SET @result = 'The SectionName Updated Successfully.'
		END

SET NOCOUNT OFF
END
GO

/*=========================================================================================================
                                       MergeClassSectionAllocation
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeClassSectionAllocation',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeClassSectionAllocation AS SELECT 1'
END
GO

ALTER PROCEDURE [SchoolAcad].[MergeClassSectionAllocation]
(
	  @EditId		    INT = 0
	, @ClassId		    INT
	, @SectionId	    INT
	, @AcadYearId	    SMALLINT
	, @IsActive         TINYINT
	, @Remarks		    NVARCHAR(1000)
	, @CreatedUserId	SMALLINT
	, @LoginId		    BIGINT = 0
	, @result           NVARCHAR(350) = '' OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS(SELECT 1 FROM SchoolAcad.ClassSectionAllocation
			WHERE ClassId = @ClassId
			  AND SectionId = @SectionId
			  AND AcadYearId = @AcadYearId
		)
		BEGIN
			SET @result = 'Class and Section already allocated for this Academic Year.'
		END
		ELSE
		BEGIN
			INSERT INTO SchoolAcad.ClassSectionAllocation(ClassId, SectionId, AcadYearId, IsActive, Remarks, CreatedUserId, LoginId)
			VALUES(@ClassId, @SectionId, @AcadYearId, @IsActive, @Remarks, @CreatedUserId, @LoginId)

			SET @result = 'Class Section Allocation Saved Successfully.'
		END
	END
	ELSE
	BEGIN
		UPDATE SchoolAcad.ClassSectionAllocation
		SET
			ClassId = @ClassId,
			SectionId = @SectionId,
			AcadYearId = @AcadYearId,
			IsActive = @IsActive,
			Remarks = @Remarks,
			CreatedUserId = @CreatedUserId,
			LoginId = @LoginId
		WHERE AllocationId = @EditId

		SET @result = 'Class Section Allocation Updated Successfully.'
	END

	SET NOCOUNT OFF;
END
GO
/*=========================================================================================================
                                       MergeSubjectMaster
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.MergeSubjectMaster',N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeSubjectMaster AS SELECT 1'
END
GO

ALTER PROCEDURE [SchoolAcad].[MergeSubjectMaster]
(
	  @EditId		    INT = 0
	, @SubjectName		NVARCHAR(50)
	, @SubjectCode		NVARCHAR(50)
	, @ShortName		NVARCHAR(50)
	, @IsChoice         TINYINT
	, @SubjTypeId       SMALLINT
	, @Remarks		    NVARCHAR(100)
	, @CreatedUserId	SMALLINT
	, @LoginId		    BIGINT
	, @result           NVARCHAR(350) = '' OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS(SELECT 1	FROM SchoolAcad.SubjectMaster WHERE SubjectName = @SubjectName
			  AND ISNULL(SubjectCode,'') = ISNULL(@SubjectCode,''))
		BEGIN
			SET @result = 'Subject already exists.'
		END
		ELSE
		BEGIN
			INSERT INTO SchoolAcad.SubjectMaster(SubjectName, SubjectCode, ShortName, IsChoice, SubjTypeId, Remarks,CreatedUserId, LoginId)
			VALUES(@SubjectName, @SubjectCode, @ShortName,@IsChoice, @SubjTypeId, @Remarks,	@CreatedUserId, @LoginId)

			SET @result = 'Subject saved successfully.'
		END
	END
	ELSE
	BEGIN
			UPDATE SchoolAcad.SubjectMaster
			SET
				SubjectName = @SubjectName,
				SubjectCode = @SubjectCode,
				ShortName = @ShortName,
				IsChoice = @IsChoice,
				SubjTypeId = @SubjTypeId,
				Remarks = @Remarks,
				CreatedUserId = @CreatedUserId,
				LoginId = @LoginId
			WHERE SubjectId = @EditId

			SET @result = 'Subject updated successfully.'
		END

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeClassSectionSubjectMap
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.MergeClassSectionSubjectMap', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeClassSectionSubjectMap AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeClassSectionSubjectMap
(
	  @EditId          INT = 0
	, @ClassId         INT
	, @SectionId       INT
	, @SubjectId       INT
	, @AcadYearId      SMALLINT
	, @IsActive        TINYINT
	, @Remarks         NVARCHAR(200)
	, @CreatedUserId   SMALLINT
	, @LoginId         BIGINT
	, @result          NVARCHAR(300) OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS (	SELECT 1 FROM SchoolAcad.ClassSectionSubjectMap	WHERE ClassId = @ClassId AND SectionId = @SectionId
			  AND SubjectId = @SubjectId  AND AcadYearId = @AcadYearId)
		BEGIN
			SET @result = 'Subject already mapped.'
		END
		ELSE
		BEGIN
			INSERT INTO SchoolAcad.ClassSectionSubjectMap(ClassId, SectionId, SubjectId, AcadYearId,
				IsActive, Remarks, CreatedUserId, LoginId)
			VALUES(@ClassId, @SectionId, @SubjectId, @AcadYearId,
				@IsActive, @Remarks, @CreatedUserId, @LoginId)

			SET @result = 'Subject mapped successfully.'
		END
	END
	ELSE
	BEGIN
		UPDATE SchoolAcad.ClassSectionSubjectMap
		SET
		    ClassId  = @ClassId,
			SectionId = @SectionId,
			SubjectId = @SubjectId,
			AcadYearId = @AcadYearId,
			IsActive = @IsActive,
			Remarks = @Remarks,
			CreatedUserId = @CreatedUserId,
			LoginId = @LoginId
		WHERE MapId = @EditId

		SET @result = 'Subject mapping updated.'
	END

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeSyllabusFinalization
============================================================================================================*/

IF OBJECT_ID(N'SchoolAcad.MergeSyllabusFinalization', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeSyllabusFinalization AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeSyllabusFinalization
(
	  @EditId          INT = 0
	, @ClassId         INT
	, @SectionId       INT
	, @SubjectId       INT
	, @AcadYearId      SMALLINT
	, @IsFinalized     TINYINT
	, @Remarks         NVARCHAR(1000)
	, @CreatedUserId   SMALLINT
	, @LoginId         BIGINT
	, @Result          NVARCHAR(300) OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS( SELECT 1 FROM SchoolAcad.SyllabusFinalization WHERE ClassId=@ClassId
			  AND SectionId=@SectionId  AND SubjectId=@SubjectId AND AcadYearId=@AcadYearId	)
		BEGIN
			SET @Result = 'Syllabus already exists.'
		END
		ELSE
		BEGIN
			INSERT INTO SchoolAcad.SyllabusFinalization( ClassId, SectionId, SubjectId, AcadYearId,
				IsFinalized, FinalizedDate,	Remarks, CreatedUserId, LoginId	)
			VALUES ( @ClassId, @SectionId, @SubjectId, @AcadYearId,
				@IsFinalized,CASE WHEN @IsFinalized = 1 THEN GETDATE() END,
				@Remarks, @CreatedUserId, @LoginId )

			SET @Result = 'Syllabus saved successfully.'
		END
	END
	ELSE
	BEGIN
		UPDATE SchoolAcad.SyllabusFinalization
		SET
		    ClassId = @ClassId,
			SectionId = @SectionId,
			SubjectId = @SubjectId,
			AcadYearId = @AcadYearId,
			IsFinalized = @IsFinalized,
			FinalizedDate = CASE WHEN @IsFinalized = 1 THEN GETDATE() END,
			Remarks = @Remarks,
			LoginId = @LoginId
		WHERE SyllabusFinalId = @EditId

		SET @Result = 'Syllabus updated successfully.'
	END

	SET NOCOUNT OFF;
END
GO