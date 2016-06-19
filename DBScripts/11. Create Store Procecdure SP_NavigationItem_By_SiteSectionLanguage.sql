
Create PROC SP_NavigationItem_By_SiteSectionLanguage

	@micrositeid NVARCHAR(50),
	@section NVARCHAR(50),
	@languageid NVARCHAR(10)
AS

BEGIN

	/*** Implement fallback languages */

	SELECT nil.[Text], ni.URL
		FROM dbo.tb_NavigationItem_Language nil
	INNER JOIN dbo.tb_Language l
		ON l.Id = nil.Language_Id
	INNER JOIN dbo.tb_NavigationItem ni
		ON ni.Id = nil.NavigationItem_Id
	INNER JOIN dbo.tb_Navigation n
		ON n.Id = ni.Navigation_Id	 
	WHERE nil.Language_Id = @languageid 
		AND n.MicroSite_Id = @micrositeid
		AND n.Section = @section

END