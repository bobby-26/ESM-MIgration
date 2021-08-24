using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportExperienceSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportExperienceSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportExperienceSummary.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                BindCourse();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCourse()
    {
        lstCourse.Items.Clear();
        lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
        lstCourse.DataTextField = "FLDCOURSE";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
        lstCourse.Items.Insert(0, new RadListBoxItem("--Select--", ""));

    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFileNo.Text = "";
                rblPrinciple.SelectedValue = "0";
                ddlPrinciple.SelectedAddress = "";
                lstRank.selectedlist = "";
                lstVessel.SelectedVessel = "";
                lstVesselType.SelectedVesseltype = "";
                lstPool.SelectedPool = "";
                lstCourse.SelectedIndex = -1;
                ShowReport();
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
                if (!IsValidFilter(ddlPrinciple.SelectedAddress))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
                }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDLICENCE", "FLDDCEYN", 
                                 "FLDRANKEXPERIENCE","FLDRANKEXPERIENCEINDAYS", "FLDOPERATOREXPERIENCE","FLDOPERATOREXPERIENCEINDAYS", 
                                 "FLDTANKEREXP","FLDTANKEREXPINDAYS", "FLDALLTANKEREXP","FLDALLTANKEREXPINDAYS", "FLDCOURSELIST" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Vessel", "Type of Licence", "If DEC for Oil In Hands ", 
                                  " Sea Time In Rank"," Sea Time In Rank in Days", "Calendar Time with Operator","Calendar Time with Operator in Days",
                                  "Sea Time on this Type of Tanker", "Sea Time on this Type of Tanker in Days",
                                  " Sea Time on all Types on Tankers"," Sea Time on all Types on Tankers in Days", "Courses" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strCourse = "";
        foreach (RadListBoxItem item in lstCourse.Items)
        {
            if (item.Selected)
            {
                strCourse = strCourse + item.Value + ",";
            }
        }
        strCourse = (strCourse.Length > 0 ? strCourse.TrimEnd(',') : strCourse);

        DataTable dt = PhoenixCrewExperienceSummary.CrewExperienceSummarySearch(txtFileNo.Text
                     , byte.Parse(rblPrinciple.SelectedValue)
                     , General.GetNullableInteger(ddlPrinciple.SelectedAddress)
                     , lstVesselType.SelectedVesseltype.Replace("Dummy", "").TrimStart(',')
                     , lstVessel.SelectedVessel.Replace("Dummy", "").TrimStart(',')
                     , lstPool.SelectedPool.Replace("Dummy", "").TrimStart(',')
                     , lstRank.selectedlist.Replace("Dummy", "").TrimStart(',')
                     , sortexpression, sortdirection
                     , 1
                     , iRowCount
                     , ref iRowCount
                     , ref iTotalPageCount
                     , byte.Parse(rblShowin.SelectedValue)
                     , strCourse
                     , General.GetNullableInteger(rblShowAllRecords.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=Experience_Summary.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Experience Summary</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDLICENCE", "FLDDCEYN", "FLDRANKEXPERIENCE", "FLDOPERATOREXPERIENCE", "FLDTANKEREXP", "FLDALLTANKEREXP", "FLDCOURSELIST" };
        string[] alCaptions = { "SI.No", "File No", "Name", "Rank", "Batch", "Vessel", "Type of Licence", "If DEC for Oil In Hands ", " Sea Time In Rank", "Calendar Time with Operator", "Sea Time on this Type of Tanker", " Sea Time on all Types on Tankers", "Courses" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string strCourse = "";
        foreach (RadListBoxItem item in lstCourse.Items)
        {
            if (item.Selected)
            {
                strCourse = strCourse + item.Value + ",";
            }
        }
        strCourse = (strCourse.Length > 0 ? strCourse.TrimEnd(',') : strCourse);
        DataTable dt = PhoenixCrewExperienceSummary.CrewExperienceSummarySearch(txtFileNo.Text
                     , byte.Parse(rblPrinciple.SelectedValue)
                     , General.GetNullableInteger(ddlPrinciple.SelectedAddress)
                     , lstVesselType.SelectedVesseltype.Replace("Dummy", "").TrimStart(',')
                     , lstVessel.SelectedVessel.Replace("Dummy", "").TrimStart(',')
                     , lstPool.SelectedPool.Replace("Dummy", "").TrimStart(',')
                     , lstRank.selectedlist.Replace("Dummy", "").TrimStart(',')
                     , sortexpression, sortdirection
                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                     , gvCrew.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount
                     , byte.Parse(rblShowin.SelectedValue)
                     , strCourse
                     , General.GetNullableInteger(rblShowAllRecords.SelectedValue));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Experience Summary", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewDetails','','../Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");


        }

    }

    public bool IsValidFilter(string Principle)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(Principle).HasValue)
        {
            ucError.ErrorMessage = "Principle is required.";
        }
        return (!ucError.IsError);

    }
    protected void rblPrinciple_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPrinciple.SelectedValue == "0")
        {
            ddlPrinciple.AddressList = PhoenixRegistersAddress.ListAddress("126");
        }
        else
        {
            ddlPrinciple.AddressList = PhoenixRegistersAddress.ListAddress("128");
        }
    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
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



    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}
