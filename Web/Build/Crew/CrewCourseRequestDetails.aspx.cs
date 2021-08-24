using System;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewCourseRequestDetails : PhoenixBasePage
{
	string strEmployeeId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);

			if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
				strEmployeeId = Request.QueryString["empid"];
			if (!IsPostBack)
			{
				txtVessel.Text = Request.QueryString["vessel"].ToString();
				txtRefNo.Text = Request.QueryString["refno"].ToString();			
				
				SetEmployeePrimaryDetails();			
			}
			BindCourseCompletionData();
			BindCoursePlannedData();
            BindCoursePendingData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CrewCourseCompletion_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void ShowExcel()
	{
		//string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONRANK", "FLDSIGNONREASON", "FLDSIGNONDATE" };
		//string[] alCaptions = { "Vessel", "Rank", "Reason", "Sign On Date" };

		//DataSet ds;

		//ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), null);

		//Response.AddHeader("Content-Disposition", "attachment; filename=Crew SignOn.xls");
		//Response.ContentType = "application/vnd.msexcel";
		//Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		//Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Sign On</center></h5></td></tr>");
		//Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
		//Response.Write("</tr>");
		//Response.Write("</TABLE>");
		//Response.Write("<br />");
		//Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		//Response.Write("<tr>");
		//for (int i = 0; i < alCaptions.Length; i++)
		//{
		//    Response.Write("<td>");
		//    Response.Write("<b>" + alCaptions[i] + "</b>");
		//    Response.Write("</td>");
		//}
		//Response.Write("</tr>");
		//foreach (DataRow dr in ds.Tables[0].Rows)
		//{
		//    Response.Write("<tr>");
		//    for (int i = 0; i < alColumns.Length; i++)
		//    {
		//        Response.Write("<td>");
		//        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
		//        Response.Write("</td>");
		//    }
		//    Response.Write("</tr>");
		//}
		//Response.Write("</TABLE>");
		//Response.End();
	}
	private void BindCourseCompletionData()
	{
	
		DataTable dt;
		dt = PhoenixCrewCourseCertificate.ListCrewCourseRequest(General.GetNullableInteger(strEmployeeId), Request.QueryString["refno"].ToString(),1);		
		if (dt.Rows.Count > 0)
		{
			gvCrewCourseCompletion.DataSource = dt;			
		}
		else
		{
            gvCrewCourseCompletion.DataSource = "";
        }
	}
	private void BindCoursePlannedData()
	{

		DataTable dt;
		dt = PhoenixCrewCourseCertificate.ListCrewCourseRequest(General.GetNullableInteger(strEmployeeId), Request.QueryString["refno"].ToString(), 2);

		if (dt.Rows.Count > 0)
		{
            gvCrewCoursePlanned.DataSource = dt;           
		}
		else
		{
            gvCrewCoursePlanned.DataSource = "";

        }
	}
    private void BindCoursePendingData()
    {

        DataTable dt;

        dt = PhoenixCrewCourseCertificate.ListCrewCourseRequest(General.GetNullableInteger(strEmployeeId), Request.QueryString["refno"].ToString(), 3);       

        if (dt.Rows.Count > 0)
        {
            gvCrewCoursePending.DataSource = dt;
            //gvCrewCoursePending.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCrewCoursePending.DataSource = "";
        }
    }
	
	private void SetEmployeePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

			if (dt.Rows.Count > 0)
			{
				txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
				txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
				txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
				
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
   
    protected void gvCrewCourseCompletion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindCourseCompletionData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCoursePlanned_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindCoursePlannedData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCoursePending_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindCoursePendingData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
