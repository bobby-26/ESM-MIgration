﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportMissingTravelDocuments : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMissingTravelDocuments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMissingTravelDocuments.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                ucVessel.SelectedVessel = "";
                ucNationality.SelectedNationalityValue = "";
                ucBatch.SelectedBatch = "";
                ddlSelectFrom.SelectedHard = "";
                ucDocNationality.SelectedList = "";
                ucRank.selectedlist = "";
                Filter.CurrentMissingDocumentFilter = null;

                Rebind();

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
                if (!IsValidFilter(ucVessel.SelectedVessel, ddlSelectFrom.SelectedHard, ucRank.selectedlist))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucVessel", ucVessel.SelectedVessel.ToString());
                    criteria.Add("ucNationality", ucNationality.SelectedNationalityValue.ToString());
                    criteria.Add("ucBatch", ucBatch.SelectedBatch.ToString());
                    criteria.Add("rblSelectFrom", ddlSelectFrom.SelectedHard);
                    criteria.Add("ucRank", ucRank.selectedlist);
                    criteria.Add("ucDocNationality", ucDocNationality.SelectedList);
                    Filter.CurrentMissingDocumentFilter = criteria;
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string DocumentsSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstDocuments.Items)
        {
            if (item.Selected == true)
            {
                if (item.Value != "--Select--")
                {
                    strlist.Append(item.Value.ToString());
                }
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDDOCUMENTNAME", "FLDDOCNATIONALITY" };
        string[] alCaptions = { "File No", "Name", "Rank", "Batch", "Vessel", "Sign On Date", "Relief Due", "Document", "Nationality" };
        string[] FilteralColumns = { "FLDSELECTEDNATIONALITY", "FLDSELECTEDSTATUS", "FLDSELECTEDVESSEL", "FLDSELECTEDTRAININGBATCH" };
        string[] FilteralCaptions = { "Nationality", "Status", "Vessel", "Training batch" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewMissingSearch.CrewReportMissingTravelDoc(
                           General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           iRowCount,
                           ref iRowCount,
                           ref iTotalPageCount, General.GetNullableString(DocumentsSelectedList())
                           , (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist)
                           , (ucDocNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucDocNationality.SelectedList));

        Response.AddHeader("Content-Disposition", "attachment; filename=Missing_Travel_Documents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Missing Travel Documents</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='LEFT'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilteralCaptions, FilteralColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
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
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDDOCUMENTNAME", "FLDDOCNATIONALITY" };
        string[] alCaptions = { "File No", "Emp Name", "Rank", "Batch", "Vessel", "Sign On Date", "Relief Due", "Document", "Nationality" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewMissingSearch.CrewReportMissingTravelDoc(
                           General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount, General.GetNullableString(DocumentsSelectedList())
                           , (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist)
                           , (ucDocNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucDocNationality.SelectedList));

        General.SetPrintOptions("gvCrew", "Missing Travel Documents", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblempcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
            }
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
    public bool IsValidFilter(string vessellist, string rblselectfrom, string RankList)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (rblselectfrom.Equals("") || rblselectfrom.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Status is required";
        }
        if (RankList.Equals("") || RankList.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Rank is required";
        }
        return (!ucError.IsError);
    }
}