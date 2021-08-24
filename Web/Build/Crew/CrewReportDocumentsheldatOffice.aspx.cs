using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportDocumentsheldatOffice : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDocumentsheldatOffice.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar1.AddImageButton("../Crew/CrewReportDocumentsheldatOffice.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar1.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewDocumentsheldatOffice.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWDOCUMENT");
            MenuShowExcel.AccessRights = this.ViewState;
             MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ucZone.ZoneList = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
                ucRank.RankList = PhoenixRegistersRank.ListRank();
            }
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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
                Response.Redirect("../Crew/CrewReportDocumentsheldatOffice.aspx");
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
                Rebind();
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDDOXINHANDYN", "FLDPDSTATUS", "FLDZONE", "FLDMODIFIEDBY", "FLDMODIFIEDDATE", "FLDREMARKS" };
        string[] alCaptions = { "Sl.no", "File no", "Name", "Rank", "Dox In Hand(Y/N)", "PD Status", "Field Office Dox Hand At", "User", "Date", "Remarks" };
        string[] filtercolumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDDOXINHANDYN", "FLDPDSTATUS", "FLDZONE", "FLDMODIFIEDBY", "FLDMODIFIEDDATE", "FLDREMARKS" };
        string[] filtercaptions = { "Sl.no", "File no", "Name", "Rank", "Dox In Hand(Y/N)", "PD Status", "Field Office Dox Hand At", "User", "Date", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewDocumentsheldatOffice.CrewDocumentsheldatOfficeReport((txtFileNo.Text) == "" ? null : txtFileNo.Text
                                                                                 , (ucZone.selectedlist) == "Dummy" ? null : ucZone.selectedlist
                                                                                 , (ucRank.selectedlist) == "," ? null : ucRank.selectedlist
                                                                                 , null
                                                                                 , sortexpression, sortdirection
                                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                 , General.ShowRecords(null)
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 );

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentsheldatOffice.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
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
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDDOXINHANDYN", "FLDPDSTATUS", "FLDZONE", "FLDMODIFIEDBY", "FLDMODIFIEDDATE", "FLDREMARKS" };
        string[] alCaptions = { "Sl.no", "File no", "Name", "Rank", "Dox In Hand(Y/N)", "PD Status", "Field Office Dox Hand At", "User", "Date", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewDocumentsheldatOffice.CrewDocumentsheldatOfficeReport((txtFileNo.Text) == "" ? null : txtFileNo.Text
                                                                                 , ((ucZone.selectedlist) == "Dummy" || (ucZone.selectedlist) == "") ? null : ucZone.selectedlist
                                                                                 , ((ucRank.selectedlist) == "Dummy" || (ucRank.selectedlist) == "") ? null : ucRank.selectedlist
                                                                                 , null
                                                                                 , sortexpression, sortdirection
                                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                 , gvCrew.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 );

        General.SetPrintOptions("gvCrew", "Documents Held At Office", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
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

    protected void gvCrew_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            int Empid = Int32.Parse(((RadLabel)e.Item.FindControl("lblEmployeeid")).Text);
            string Zone = ((UserControlZone)e.Item.FindControl("ddlZone")).SelectedZone;
            int DoxYn = int.Parse(((RadLabel)e.Item.FindControl("lblDIHyn")).Text);
            int ProposedYn = int.Parse(((RadLabel)e.Item.FindControl("lblProposedyn")).Text);
            string Remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
            if (DoxYn == 1)
            {
                PhoenixCrewDocumentsheldatOffice.DocumentsheldatOfficeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Empid, DoxYn, ProposedYn, General.GetNullableInteger(Zone), Remarks);
            }
            else
            {
                ucError.Text = "Cannot Update the Entry.Since Document in hand is no";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private bool IsValidFilter(string remarks, string zone)
    {
        ucError.HeaderMessage = "Please provide the following required information";

            if (string.IsNullOrEmpty(remarks))
            {
                ucError.ErrorMessage = "Remarks is required.";
            }

            if (General.GetNullableInteger(zone) == null)
            {
                ucError.ErrorMessage = "Zone is Required";
            }
           
        
     
        return (!ucError.IsError);
    }


    
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
               
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                    if (ed != null)
                    {
                        ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                    }
                    UserControlZone ddlZone = (UserControlZone)e.Item.FindControl("ddlZone");
                    if (ddlZone != null) ddlZone.SelectedZone = drv["FLDZONEID"].ToString();
               
                    RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");

                    if (lbtn != null)
                    {
                        lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                        lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                    }
                    if ((drv["FLDDOXINHANDYN"].ToString()) != "Yes")
                        edit.Visible = false;
                

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}
