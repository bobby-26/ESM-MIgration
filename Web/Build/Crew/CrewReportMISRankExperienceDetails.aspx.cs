using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewReportMISRankExperienceDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRankExperienceDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
				
                ViewState["SHOWREPORT"] = null;

                if (Request.QueryString["nationality"].ToString() != null)
                    ViewState["nationality"] = Request.QueryString["nationality"].ToString();
                else
                    ViewState["nationality"] = "Dummy";

                if (Request.QueryString["status"].ToString() != null)
                    ViewState["status"] = Request.QueryString["status"].ToString();
                else
                    ViewState["status"] = "Dummy";

                if (Request.QueryString["condition"].ToString() != null)
                    ViewState["condition"] = Request.QueryString["condition"].ToString();
                else
                    ViewState["condition"] = "Dummy";

                if (Request.QueryString["exp"].ToString() != null)
                    ViewState["exp"] = Request.QueryString["exp"].ToString();
                else
                    ViewState["exp"] = "Dummy";

                if (Request.QueryString["principal"].ToString() != null)
                {
                    ViewState["principal"] = Request.QueryString["principal"].ToString();
                }
                else
                    ViewState["principal"] = "Dummy";

				if (Request.QueryString["pool"].ToString() != null)
					ViewState["pool"] = Request.QueryString["pool"].ToString();
				else
					ViewState["pool"] = "Dummy";
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //  BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewReportMIS.CrewReportMISRankAnalysisDetail(
                     General.GetNullableInteger(ucManager.SelectedAddress),
                     General.GetNullableInteger(ucRank.SelectedRank),
                     General.GetNullableString(ViewState["nationality"].ToString()),
                     General.GetNullableInteger(ViewState["status"].ToString()),
                     General.GetNullableInteger(ViewState["condition"].ToString()),
                     General.GetNullableInteger(ViewState["exp"].ToString()),
                     General.GetNullableInteger(ViewState["principal"].ToString()),
                     ViewState["principal"].ToString().Replace("Dummy", "").TrimStart(',')
                    , sortexpression, sortdirection
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

                StringBuilder sb = new StringBuilder();
                sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                sb.Append("<tr>");
                sb.Append("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                sb.Append("<td colspan='" + (ds.Tables[2].Rows.Count + 5).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp Rank Experience Statistics</h3></td>");
                sb.Append("</tr>");
                sb.Append("</TABLE>");
                sb.Append("<br />");
                sb.Append("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");                
                sb.Append("<tr>");
                sb.Append("<th colspan=\"5\"></th><th align=\"center\" colspan=\"" + (ds.Tables.Count > 0 ? ds.Tables[2].Rows.Count : 0) + "\">Experience(in Months)</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th>Sl No</th><th>File No</th><th>Batch</th><th>Employee Name</th><th>Rank</th>");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        sb.Append("<th>" + dr["FLDRANKCODE"] + "</th>");
                    }
                    sb.Append("</tr>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + dr["FLDROWNO"].ToString() + "</td>");
                        sb.Append("<td>" + dr["FLDFILENO"].ToString() + "</td>");
                        sb.Append("<td>" + dr["FLDEMPLOYEENAME"].ToString() + "</td>");
                        sb.Append("<td>" + dr["FLDPOSTEDRANK"].ToString() + "</td>");
                        sb.Append("<td>" + dr["FLDBATCH"].ToString() + "</td>");
                        foreach (DataRow dr2 in ds.Tables[2].Rows)
                        {
                            DataRow[] drs = ds.Tables[1].Select("FLDEMPLOYEEID=" + dr["FLDEMPLOYEEID"].ToString() + " AND FLDRANKID=" + dr2["FLDRANKID"].ToString());
                            if (drs.Length > 0)
                                sb.Append("<td>" + drs[0]["FLDEXPERIENCE"].ToString() + "</td>");
                            else
                                sb.Append("<td>&nbsp;</td>");
                        }
                        sb.Append("</tr>");
                    }
                }
                if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count == 0)
                    sb.Append("</tr>");
                sb.Append("</table>");

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=RankExperienceStatistics.xls");
                Response.Charset = "";
                Response.Write(sb.ToString());
                Response.End();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucManager.SelectedAddress, ucRank.SelectedRank))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                gvPB.CurrentPageIndex = 0;
                BindData();
                gvPB.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string manager,string rank)
    {  
            ucError.HeaderMessage = "Please provide the following required information";

            if (manager.Equals("") || manager.Equals("Dummy"))
            {
                ucError.ErrorMessage = "Select Manager";
            }
        if (rank.Equals("") || rank.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Rank";
        }
        return (!ucError.IsError);
    }
    private void BindData()
	{
        try
        { 
        ViewState["SHOWREPORT"] = 1;
		DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		ds = PhoenixCrewReportMIS.CrewReportMISRankAnalysisDetail(
					 General.GetNullableInteger(ucManager.SelectedAddress),
					 General.GetNullableInteger(ucRank.SelectedRank),
					 General.GetNullableString(ViewState["nationality"].ToString()),
					 General.GetNullableInteger(ViewState["status"].ToString()),
					 General.GetNullableInteger(ViewState["condition"].ToString()),
					 General.GetNullableInteger(ViewState["exp"].ToString()),
					 General.GetNullableInteger(ViewState["principal"].ToString()),
				   	 ViewState["principal"].ToString().Replace("Dummy", "").TrimStart(',')
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvPB.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

		if (ds.Tables[0].Rows.Count > 0)
		{
			if (ds.Tables.Count > 2 && ds.Tables[0].Rows.Count > 0)
			{                				              
				DataTable dt = ds.Tables[2];
                if (gvPB.Columns.Count < 6)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridBoundColumn field = new GridBoundColumn();
                        field.HeaderText = dt.Rows[i]["FLDRANKCODE"].ToString();
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ColumnGroupName = "group2";
                        gvPB.Columns.Add(field);
                    }
                }
				
				//gvPB.Controls[0].Controls.AddAt(0, row);
			}			
		}
		
        gvPB.DataSource = ds;
        gvPB.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        { 
        RadGrid gv = (RadGrid)sender;
        DataSet ds = null;
        if (typeof(DataSet) == gv.DataSource.GetType())
            ds = (DataSet)gv.DataSource;

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            if (ds != null)
            {
                string empid = drv["FLDEMPLOYEEID"].ToString();
                DataTable header = ds.Tables[2];
                DataTable data = ds.Tables[1];
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    string rankid = header.Rows[i]["FLDRANKID"].ToString();
                    DataRow[] dr = data.Select("FLDEMPLOYEEID = " + empid + " AND FLDRANKID = " + rankid);
                    e.Item.Cells[i + 7].Text = (dr.Length > 0 ? dr[0]["FLDEXPERIENCE"].ToString() : "");
                }
            }            
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //	return;
    //}
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
