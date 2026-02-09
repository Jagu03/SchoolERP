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
	, @ClassName		NVARCHAR(100)
	, @DisplayOrder		INT
	, @IsActive         BIT
	, @Remarks			NVARCHAR(1000)
	, @CreatedUserId	SMALLINT	
	, @LoginId			BIGINT = 0
	, @result           NVARCHAR(350) = '' OUTPUT
)
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
		       IF EXISTS (SELECT 1 FROM [SchoolAcad].[ClassMaster] WHERE ClassName = @ClassName AND ClassId <> @EditId)
			   BEGIN
			       SET @result = 'The specified ClassName already exists.';
				END
		   UPDATE [SchoolAcad].[ClassMaster] SET
			   ClassName =@ClassName,
			   DisplayOrder = @DisplayOrder,
			   IsActive = @IsActive,
			   Remarks = @Remarks,
			   ModifiedUserId  = @CreatedUserId,
			   ModifiedDateTime = GETDATE(),
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
	, @IsActive         BIT	
	, @CreatedUserId	SMALLINT	
	, @LoginId			BIGINT = 0
	, @result           NVARCHAR(350) = '' OUTPUT
)
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
	, @AcadYearId	    TINYINT
	, @IsActive         BIT
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
	, @IsChoice         BIT
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
	, @AcadYearId      TINYINT
	, @IsActive        BIT
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

/*=========================================================================================================
                                       MergeAssessmentPolicyMaster
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeAssessmentPolicyMaster', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeAssessmentPolicyMaster AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeAssessmentPolicyMaster
(
	@EditId        INT = 0
	, @ClassId       INT
	, @SubjectId     INT
	, @AcadYearId    SMALLINT
	, @IsActive      TINYINT
	, @Remarks       NVARCHAR(1000)
	, @CreatedUserId SMALLINT
	, @LoginId       BIGINT
	, @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS (	SELECT 1 FROM SchoolAcad.AssessmentPolicyMaster
			WHERE ClassId=@ClassId AND SubjectId=@SubjectId AND AcadYearId=@AcadYearId )
		BEGIN
			SET @Result = 'Assessment policy already exists.'
		END
		ELSE
		BEGIN
			INSERT INTO SchoolAcad.AssessmentPolicyMaster (ClassId, SubjectId, AcadYearId, IsActive, Remarks, CreatedUserId, LoginId)
			VALUES(@ClassId, @SubjectId, @AcadYearId, @IsActive, @Remarks, @CreatedUserId, @LoginId)

			SET @Result = 'Assessment policy saved successfully.'
		END
	END
	ELSE
	BEGIN
		UPDATE SchoolAcad.AssessmentPolicyMaster
		SET
		    ClassId = @ClassId,
			SubjectId = @SubjectId,
			AcadYearId = @AcadYearId,
		    IsActive = @IsActive,
			Remarks = @Remarks,
			LoginId = @LoginId
		WHERE PolicyId = @EditId

		SET @Result = 'Assessment policy updated successfully.'
	END
END
GO

/*=========================================================================================================
                                       SaveAssessmentPolicyComponent
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.SaveAssessmentPolicyComponent', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.SaveAssessmentPolicyComponent AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.SaveAssessmentPolicyComponent
(
	  @EditId        INT = 0          -- ComponentId
	, @PolicyId      INT
	, @ComponentName NVARCHAR(50)
	, @MaxMarks      INT
	, @Weightage     DECIMAL(5,2)
	, @IsMandatory   TINYINT
	, @DisplayOrder  INT
	, @Remarks       NVARCHAR(1000)
	, @CreatedUserId SMALLINT
	, @LoginId       BIGINT
	, @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS(SELECT 1 FROM SchoolAcad.AssessmentPolicyComponent
			WHERE PolicyId = @PolicyId  AND ComponentName = @ComponentName)
		BEGIN
			SET @Result = 'Component already exists for this policy.'
			RETURN
		END

		INSERT INTO SchoolAcad.AssessmentPolicyComponent
		(PolicyId, ComponentName, MaxMarks, Weightage,IsMandatory, DisplayOrder, Remarks,CreatedUserId, LoginId)
		VALUES(@PolicyId, @ComponentName, @MaxMarks, @Weightage,@IsMandatory, @DisplayOrder, @Remarks,@CreatedUserId, @LoginId)

		SET @Result = 'Component added successfully.'
	END
	ELSE
	BEGIN
		-- UPDATE
		UPDATE SchoolAcad.AssessmentPolicyComponent
		SET
			ComponentName = @ComponentName,
			MaxMarks      = @MaxMarks,
			Weightage     = @Weightage,
			IsMandatory   = @IsMandatory,
			DisplayOrder  = @DisplayOrder,
			Remarks       = @Remarks,
			LoginId       = @LoginId
		WHERE ComponentId = @EditId

		SET @Result = 'Component updated successfully.'
	END

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeTeacherSubjectAllocation
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeTeacherSubjectAllocation', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeTeacherSubjectAllocation AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeTeacherSubjectAllocation
(
	  @EditId        INT = 0
	, @StaffId       INT
	, @ClassId       INT
	, @SectionId     INT
	, @SubjectId     INT
	, @AcadYearId    TINYINT
	, @IsActive      TINYINT
	, @Remarks       NVARCHAR(1000)
	, @CreatedUserId SMALLINT
	, @LoginId       BIGINT
	, @Result        NVARCHAR(300) OUTPUT

)
AS
BEGIN
	SET NOCOUNT ON;

	IF @EditId = 0
	BEGIN
		IF EXISTS (	SELECT 1 FROM SchoolAcad.StaffSubjectAllocation	WHERE StaffId  = @StaffId
			  AND ClassId  = @ClassId  AND SectionId  = @SectionId
			  AND SubjectId = @SubjectId  AND AcadYearId = @AcadYearId	)
		BEGIN
			SET @Result = 'Staff already allocated for this subject.'
			RETURN
		END

		INSERT INTO SchoolAcad.StaffSubjectAllocation	(StaffId, ClassId, SectionId, SubjectId,
			AcadYearId, IsActive, Remarks,CreatedUserId, LoginId)
		VALUES(	@StaffId, @ClassId, @SectionId, @SubjectId,@AcadYearId, @IsActive, @Remarks,
			@CreatedUserId, @LoginId)

		SET @Result = 'Staff allocated successfully.'
	END
	ELSE
	BEGIN

		UPDATE SchoolAcad.StaffSubjectAllocation
		SET
			StaffId    = @StaffId,
			ClassId    = @ClassId,
			SectionId  = @SectionId,
			SubjectId  = @SubjectId,
			IsActive   = @IsActive,
			Remarks    = @Remarks,
			LoginId    = @LoginId
		WHERE AllocationId = @EditId

		SET @Result = 'Staff allocation updated successfully.'
	END

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeLessonPlan
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeLessonPlan', N'P') IS NULL
BEGIN
	EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeLessonPlan AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeLessonPlan
(
	  @EditId           INT = 0
	, @ClassId          INT
	, @SectionId        INT
	, @SubjectId        INT
	, @StaffId          INT
	, @AcadYearId       TINYINT
	, @TopicTitle       NVARCHAR(200)
	, @TopicDescription NVARCHAR(2000)
	, @PlannedFromDate  DATE
	, @PlannedToDate    DATE
	, @Unit             TINYINT
	, @TeachingMethod   NVARCHAR(100)
	, @StudentLearningMethod NVARCHAR(100)
	, @Status           NVARCHAR(20)
	, @IsActive         TINYINT
	, @Remarks          NVARCHAR(1000)
	, @CreatedUserId    SMALLINT
	, @LoginId          BIGINT
	, @Result           NVARCHAR(300) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	-- INSERT
	IF @EditId = 0
	BEGIN
	 IF EXISTS (SELECT 1 FROM SchoolAcad.LessonPlan  WHERE AcadYearId = @AcadYearId
                  AND ClassId = @ClassId AND SubjectId = @SubjectId AND StaffId = @StaffId
                  AND Unit = @Unit AND TopicTitle = @TopicTitle AND IsActive = 1)
            BEGIN
                SET @Result = 'Lesson plan already exists.';
                RETURN;
            END

		INSERT INTO SchoolAcad.LessonPlan(ClassId, SectionId, SubjectId, StaffId, AcadYearId,
			TopicTitle, TopicDescription,PlannedFromDate, PlannedToDate, Unit , TeachingMethod,
			StudentLearningMethod, StatusText, IsActive, Remarks,CreatedUserId, LoginId)
		VALUES(	@ClassId, @SectionId, @SubjectId, @StaffId, @AcadYearId,
			@TopicTitle, @TopicDescription,@PlannedFromDate, @PlannedToDate,@Unit , @TeachingMethod,
			@StudentLearningMethod,@Status, @IsActive, @Remarks,@CreatedUserId, @LoginId)

		SET @Result = 'Lesson plan saved successfully.'
	END
	ELSE
	BEGIN
		-- UPDATE
		UPDATE SchoolAcad.LessonPlan
		SET
			ClassId          = @ClassId,
			SectionId        = @SectionId,
			SubjectId        = @SubjectId,
			StaffId          =  @StaffId,
			TopicTitle       = @TopicTitle,
			TopicDescription = @TopicDescription,
			PlannedFromDate  = @PlannedFromDate,
			PlannedToDate    = @PlannedToDate,
			Unit             = @Unit,
			TeachingMethod   = @TeachingMethod,
			StudentLearningMethod = @StudentLearningMethod,
			StatusText       = @Status,
			IsActive         = @IsActive,
			Remarks          = @Remarks,
			LoginId          = @LoginId
		WHERE LessonPlanId = @EditId

		SET @Result = 'Lesson plan updated successfully.'
	END

	SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeClassroomTeaching
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeClassroomTeaching', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeClassroomTeaching AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeClassroomTeaching
(
      @EditId        INT = 0
    , @AcadYearId    SMALLINT
    , @ClassId       INT
    , @SectionId     INT
    , @SubjectId     INT
    , @StaffId       INT
    , @TeachingDate DATE
    , @PeriodNo      TINYINT
    , @UnitNo        TINYINT
    , @TopicTitle    NVARCHAR(200)
    , @TopicDescription NVARCHAR(2000)
    , @TeachingMethod NVARCHAR(200)
    , @StudentLearningMethod NVARCHAR(200)
    , @IsCompleted   TINYINT
    , @Remarks       NVARCHAR(1000)
    , @UserId        SMALLINT
    , @LoginId       BIGINT
    , @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @EditId = 0
    BEGIN
        INSERT INTO SchoolAcad.ClassroomTeaching (AcadYearId, ClassId, SectionId, SubjectId, StaffId,
            TeachingDate, PeriodNo, UnitNo, TopicTitle, TopicDescription,
            TeachingMethod, StudentLearningMethod, IsCompleted, Remarks, CreatedUserId, LoginId )
        VALUES ( @AcadYearId, @ClassId, @SectionId, @SubjectId, @StaffId,
            @TeachingDate, @PeriodNo, @UnitNo, @TopicTitle, @TopicDescription,
            @TeachingMethod, @StudentLearningMethod, @IsCompleted, @Remarks, @UserId, @LoginId )

        SET @Result = 'Classroom teaching saved successfully.'
    END
    ELSE
    BEGIN
        UPDATE SchoolAcad.ClassroomTeaching
        SET
            TeachingDate = @TeachingDate,
            PeriodNo = @PeriodNo,
            UnitNo = @UnitNo,
            TopicTitle = @TopicTitle,
            TopicDescription = @TopicDescription,
            TeachingMethod = @TeachingMethod,
            StudentLearningMethod = @StudentLearningMethod,
            IsCompleted = @IsCompleted,
            Remarks = @Remarks,
            ModifiedUserId = @UserId,
            ModifiedDateTime = GETDATE(),
            LoginId = @LoginId
        WHERE ClassroomTeachingId = @EditId

        SET @Result = 'Classroom teaching updated successfully.'
    END

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeAssignment
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeAssignment', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeAssignment AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeAssignment
(
   @EditId			  INT = 0,
    @AcadYearId		  SMALLINT,
    @ClassId		  INT,
    @SectionId		  INT,
    @SubjectId		  INT,
    @StaffId		  INT,
    @AssignmentType   NVARCHAR(20),
    @Title			  NVARCHAR(200),
    @TitleDescription NVARCHAR(500),
    @GivenDate		  DATE,
    @DueDate		  DATE,
    @MaxMarks		  INT,
	@Remarks          NVARCHAR(20),
    @CreatedUserId	  SMALLINT,
    @LoginId		  BIGINT,
	@Result			  NVARCHAR(350) = '' OUTPUT
)
WITH ENCRYPTION
AS
BEGIN
SET NOCOUNT ON
      IF @EditId = 0
	BEGIN
		INSERT INTO SchoolAcad.AssignmentMaster (AcadYearId, ClassId, SectionId, SubjectId, StaffId,
			AssignmentType, Title, TitleDescription,GivenDate, DueDate, MaxMarks,Remarks,CreatedUserId, LoginId)
		VALUES(@AcadYearId, @ClassId, @SectionId, @SubjectId, @StaffId,@AssignmentType, @Title, @TitleDescription,
			@GivenDate, @DueDate, @MaxMarks,@Remarks,@CreatedUserId, @LoginId)
		SET @Result = 'Assignment Save Successfully.'
	END
	ELSE
	BEGIN
	    UPDATE SchoolAcad.AssignmentMaster SET 
				AcadYearId = @AcadYearId,
				ClassId = @ClassId,
				SectionId = @SectionId,
				SubjectId = @SubjectId,
				StaffId = @StaffId,
				AssignmentType = @AssignmentType,
				Title = @Title,
				TitleDescription = @TitleDescription,
				GivenDate = @GivenDate,
				DueDate = @DueDate,
				MaxMarks = @MaxMarks,
				Remarks = @Remarks,
				CreatedUserId = @CreatedUserId,
				LoginId = @LoginId 
				WHERE AssignmentId = @EditId
	SET @Result = 'Assignment Updated Successfully.'
	END
END
GO

/*=========================================================================================================
                                       MergeAssignmentStudentMark
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeAssignmentStudentMark', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeAssignmentStudentMark AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeAssignmentStudentMark
(
    @EditId           INT = 0,
    @AssignmentId     INT,
    @StudentId        INT,
    @SubmissionStatus NVARCHAR(20),
    @ObtainedMarks    INT,
    @SubmissionDate   DATE,
    @Remarks          NVARCHAR(500),
    @CreatedUserId    SMALLINT,
	@LoginId		  BIGINT,
	@Result			  NVARCHAR(350) = '' OUTPUT
)
AS
BEGIN
SET NOCOUNT ON 
   IF @EditId = 0
   BEGIN
		INSERT INTO SchoolAcad.AssignmentStudentMark (AssignmentId, StudentId,
			SubmissionStatus, ObtainedMarks,SubmissionDate, Remarks, CreatedUserId,LoginId)
		VALUES(@AssignmentId, @StudentId,@SubmissionStatus, @ObtainedMarks,
			@SubmissionDate, @Remarks,@CreatedUserId,@LoginId)
	SET @Result = 'StudentMark Save Successfully.'
  END
  ELSE
  BEGIN
      UPDATE SchoolAcad.AssignmentStudentMark SET
			  AssignmentId = @AssignmentId,
			  StudentId = @StudentId,
			  SubmissionStatus = @SubmissionStatus,
			  ObtainedMarks = @ObtainedMarks,
              SubmissionDate = @SubmissionDate,
			  Remarks = @Remarks,
			  LoginId = @LoginId
			  WHERE MarkId = @EditId
  SET @Result = 'StudentMark Update Successfully.'
  END
SET NOCOUNT OFF
END
GO

/*=========================================================================================================
                                       MergeStudentElectiveSubject
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeStudentElectiveSubject', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeStudentElectiveSubject AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeStudentElectiveSubject
(
    @EditId INT = 0,
    @StudentId INT,
    @ClassId INT,
    @AcadYearId SMALLINT,
    @SubjectId INT,
    @CreatedUserId SMALLINT,
    @LoginId BIGINT,
    @Result NVARCHAR(200) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @EditId = 0
    BEGIN
        INSERT INTO SchoolAcad.StudentElectiveSubject (StudentId, ClassId, AcadYearId, SubjectId,
            CreatedUserId, LoginId)
        VALUES (@StudentId, @ClassId, @AcadYearId, @SubjectId,
            @CreatedUserId, @LoginId);

        SET @Result = 'Elective subject assigned successfully.';
    END
    ELSE
    BEGIN
        UPDATE SchoolAcad.StudentElectiveSubject
        SET
		    StudentId = @StudentId,
			ClassId = @ClassId,
			AcadYearId = @AcadYearId,
            SubjectId = @SubjectId,
            LoginId = @LoginId
        WHERE StudentElectiveId = @EditId;

        SET @Result = 'Elective subject updated successfully.';
    END

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       MergeHourSetting
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.MergeHourSetting', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.MergeHourSetting AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.MergeHourSetting
(
      @EditId        INT = 0
    , @FromDt        DATE
    , @ClassId       INT NOT NULL
    , @HourId        TINYINT
    , @FromTime      NVARCHAR(10)
    , @ToTime        NVARCHAR(10)
    , @SessNameId    TINYINT = NULL
    , @IsActive      BIT
    , @IsCommon      BIT
    , @CourseId      SMALLINT
    , @CreatedUserId SMALLINT
    , @LoginId       BIGINT
    , @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @EditId = 0
    BEGIN
        IF EXISTS (SELECT 1 FROM SchoolAcad.HourSetting
            WHERE HourId = @HourId AND ISNULL(ClassId,0) = ISNULL(@ClassId,0))
        BEGIN
            SET @Result = 'Hour already exists for this class.'
            RETURN
        END

        INSERT INTO SchoolAcad.HourSetting(FromDt, ClassId, HourId, FromTime, ToTime,
            SessNameId, IsActive, IsCommon,CreatedUserId, CourseId, LoginId)
        VALUES(@FromDt, @ClassId, @HourId, @FromTime, @ToTime,@SessNameId, @IsActive, @IsCommon,
            @CreatedUserId, @CourseId, @LoginId)

        SET @Result = 'Hour setting saved successfully.'
    END
    ELSE
    BEGIN
        UPDATE SchoolAcad.HourSetting SET
            FromDt = @FromDt,
            ClassId = @ClassId,
            HourId = @HourId,
            FromTime = @FromTime,
            ToTime = @ToTime,
            SessNameId = @SessNameId,
            IsActive = @IsActive,
            IsCommon = @IsCommon,
            CourseId = @CourseId,
            LoginId = @LoginId
        WHERE HourSettingId = @EditId

        SET @Result = 'Hour setting updated successfully.'
    END

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                       SaveTimeTable
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.SaveTimeTable', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.SaveTimeTable AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.SaveTimeTable
(    
    @AcadYearId     TINYINT,
    @ClassId        INT,
    @SectionId      INT,
    @DayId          SMALLINT,
    @HourId         INT,
	@hourList		NVARCHAR(500) = '',
    @PeriodNoId     TINYINT,
    @SubjectId      INT,
    @StaffId        INT,
    @Remarks        NVARCHAR(500),
    @CreatedUserId  SMALLINT,
    @LoginId        BIGINT,
    @Result         NVARCHAR(300) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    

    SET NOCOUNT OFF;
END
GO

/*=========================================================================================================
                                      
============================================================================================================*/
IF OBJECT_ID(N'SchoolAcad.PromoteStudentClass', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.PromoteStudentClass AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.PromoteStudentClass
(
      @StudentId     INT
    , @FromAcadYear  TINYINT
    , @ToAcadYear    TINYINT
    , @NewClassId    INT
    , @CreatedUserId SMALLINT
    , @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate student exists in previous year
    IF NOT EXISTS (
        SELECT 1 FROM SchoolAcad.StudClasses
        WHERE StudentId = @StudentId
          AND AcadYearCr = @FromAcadYear
    )
    BEGIN
        SET @Result = 'Student not found in previous academic year.'
        RETURN
    END

    -- Prevent duplicate promotion
    IF EXISTS (
        SELECT 1 FROM SchoolAcad.StudClasses
        WHERE StudentId = @StudentId
          AND AcadYearCr = @ToAcadYear
    )
    BEGIN
        SET @Result = 'Student already promoted for this academic year.'
        RETURN
    END

    -- INSERT promotion record
    INSERT INTO SchoolAcad.StudClasses
    (
        StudentId,
        AcadYearCr,
        ClassId,
        CreatedUserId
    )
    VALUES
    (
        @StudentId,
        @ToAcadYear,
        @NewClassId,
        @CreatedUserId
    )

    SET @Result = 'Student promoted successfully.'
    SET NOCOUNT OFF;
END
GO


IF OBJECT_ID(N'SchoolAcad.AddNewStudentClass', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.AddNewStudentClass AS SELECT 1'
END
GO

ALTER PROCEDURE SchoolAcad.AddNewStudentClass
(
      @StudentId     INT
    , @AcadYearCr    TINYINT
    , @ClassId       INT
    , @CreatedUserId SMALLINT
    , @Result        NVARCHAR(300) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM SchoolAcad.StudClasses
        WHERE StudentId = @StudentId
          AND AcadYearCr = @AcadYearCr
    )
    BEGIN
        SET @Result = 'Student already exists for this academic year.'
        RETURN
    END

    INSERT INTO SchoolAcad.StudClasses
    (
        StudentId,
        AcadYearCr,
        ClassId,
        CreatedUserId
    )
    VALUES
    (
        @StudentId,
        @AcadYearCr,
        @ClassId,
        @CreatedUserId
    )

    SET @Result = 'New student admitted successfully.'
    SET NOCOUNT OFF;
END
GO



IF OBJECT_ID(N'SchoolAcad.SaveAttendance_ForClass_Ondate', N'P') IS NULL
BEGIN
    EXEC sp_executesql N'CREATE PROCEDURE SchoolAcad.SaveAttendance_ForClass_Ondate AS SELECT 1'
END
GO

CREATE PROCEDURE [SchoolAcad].[SaveAttendance_ForClass_Ondate]
(
  @ClassId         INT
  ,@StudList       NVARCHAR(MAX)
  ,@attendanceStatusId SMALLINT
  ,@dateStr        NVARCHAR(15)
  ,@sessionId      TINYINT    -- 1.Fullday 2.FN 3.AN
  ,@remarks        NVARCHAR(500) = NULL
  ,@studStat       TINYINT = 1
  ,@CreatedUserId  SMALLINT
  ,@LoginId        BIGINT
  ,@Result         NVARCHAR(300) OUTPUT
)

AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @dateId int

    SET @dateId = CONVERT(int, CONVERT(datetime , @dateStr , 103))

    ;WITH StudList AS(
        SELECT sai.StudentId 
        FROM Academic.StudAdmnInfo sai
        INNER JOIN SchoolAcad.StudClasses SC ON SC.StudentId  = sai.StudentId
        WHERE SC.ClassId = @ClassId
              AND sai.StudStat = CASE @studStat WHEN 2 THEN sai.StudStat ELSE @studStat END
	    EXCEPT
	    SELECT studentId FROM [SchoolAcad].[SchoolAttendance]
	    WHERE classId= @classId AND DateId = @DateId
     )

    SET NOCOUNT OFF;
END
GO