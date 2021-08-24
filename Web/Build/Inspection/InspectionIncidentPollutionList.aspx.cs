using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionIncidentPollutionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                SessionUtil.PageAccessRights(this.ViewState);
                VesselConfiguration();
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Inspection/InspectionIncidentPollutionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvIncidentPollution')", "Print Grid", "icon_print.png", "PRINT");
                //toolbar.AddImageLink("../Inspection/InspectionIncidentGeneral.aspx?NewMode=true", "Add", "add.png", "INCIDENTADD");
                //toolbar.AddImageLink("javascript:Openpopup('Filter','','InspectionScheduleFilter.aspx'); return false;", "Filter", "search.png", "FIND");
                //toolbar.AddImageButton("../Inspection/InspectionScheduleSearch.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                MenuIncidentPollutionSearch.AccessRights = this.ViewState;
                MenuIncidentPollutionSearch.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("List", "LIST");
                toolbar.AddButton("Incident Details", "INCIDENTDETAILS");   
                toolbar.AddButton("Consequence", "CONSEQUENCE");
                toolbar.AddButton("Investigation", "INVESTIGATION");
                toolbar.AddButton("RCA", "MSCAT");
                toolbar.AddButton("CAR", "CAR");
                //toolbar.AddButton("Action", "ACTION");
                toolbar.AddButton("Defect Work Order", "WORKREQUEST");
                toolbar.AddButton("Requisition", "REQUISITION");
                toolbar.AddButton("Company Response", "COMPANYRESPONSE");
                toolbar.AddButton("Attachments", "ATTACHMENTS");

                MenuIncidentGeneral.AccessRights = this.ViewState;
                MenuIncidentGeneral.MenuList = toolbar.Show();
                MenuIncidentGeneral.SelectedMenuIndex = 2;
                Filter.CurrentIncidentTab = "CONSEQUENCE";
                toolbar = new PhoenixToolbar();                
                //toolbar.AddButton("Component Damage", "DAMAGE");
                toolbar.AddButton("Property Damage", "PROPERTYDAMAGE");
                toolbar.AddButton("Health and Safety", "INJURY");
                toolbar.AddButton("Process Loss", "PROCESSLOSS");
                toolbar.AddButton("Security", "SECURITY");
                toolbar.AddButton("Environmental", "POLLUTION");
                //toolbar.AddButton("Drug and Alcohol Test", "DRUGALCOHOLTEST");
                MenuIncidentReportGeneral.AccessRights = this.ViewState;
                MenuIncidentReportGeneral.MenuList = toolbar.Show();
                MenuIncidentReportGeneral.SelectedMenuIndex = 4;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("INCIDENTDETAILS"))
        {
            Filter.CurrentIncidentTab = "INCIDENTDETAILS";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("INVESTIGATION"))
        {
            Filter.CurrentIncidentTab = "INVESTIGATION";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=INVESTIGATION", true);
        }
        else if (dce.CommandName.ToUpper().Equals("MSCAT"))
        {
            Filter.CurrentIncidentTab = "MSCAT";
            Response.Redirect("../Inspection/InspectionIncidentMSCAT.aspx?selectTab=MSCAT", true);
        }
        else if (dce.CommandName.ToUpper().Equals("CAR"))
        {
            Filter.CurrentIncidentTab = "CAR";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=CAR", true);
        }
        else if (dce.CommandName.ToUpper().Equals("WORKREQUEST"))
        {
            Filter.CurrentIncidentTab = "WORKREQUEST";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=WORKREQUEST", true);
        }
        else if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Filter.CurrentIncidentTab = "ATTACHMENTS";
            Response.Redirect("../Inspection/InspectionIncidentAttachment.aspx?selectTab=ATTACHMENTS", true);
        }
        else if (dce.CommandName.ToUpper().Equals("REQUISITION"))
        {
            Filter.CurrentIncidentTab = "REQUISITION";
            Response.Redirect("../Inspection/InspectionIncidentPurchaseForm.aspx?", true);
        }
        else if (dce.CommandName.ToUpper().Equals("COMPANYRESPONSE"))
        {
            Filter.CurrentIncidentTab = "COMPANYRESPONSE";
            Response.Redirect("../Inspection/InspectionIncidentCompanyResponse.aspx?", true);
        }
        else if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx", true);
        }
    }

    protected void IncidentReportGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("INCIDENTDETAILS"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDetails.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("PROPERTYDAMAGE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentPropertyDamageList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("INJURY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentInjuryList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("PROCESSLOSS"))
        {
            Response.Redirect("../Inspection/InspectionIncidentProcessLossList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("SECURITY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentSecurityList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("POLLUTION"))
        {
            Response.Redirect("../Inspection/InspectionIncidentPollutionList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("DRUGALCOHOLTEST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDrugAndAlcoholTest.aspx", true);
        }   
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFSUBSTANCE", "FLDRELEASETYPENAME", "FLDRELEASECATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDESTIMATEDCOST", "FLDCATEGORY" };
        string[] alCaptions = { "Name of Substance", "Release Type", "Release Category", "Release Subcategory", "Estimated Cost", "Consequence Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionIncidentPollution.IncidentPollutionSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentIncidentID)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentPollutionList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Incident Environment Release List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void IncidentPollutionSearch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentIncidentFilterCriteria = null;
            BindData();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFSUBSTANCE", "FLDRELEASETYPENAME", "FLDRELEASECATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDESTIMATEDCOST", "FLDCATEGORY" };
        string[] alCaptions = { "Name of Substance", "Release Type", "Release Category", "Release Subcategory", "Estimated Cost", "Consequence Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncidentPollution.IncidentPollutionSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentIncidentID)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvIncidentPollution", "Incident Environment Release List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvIncidentPollution.DataSource = ds;
            gvIncidentPollution.DataBind();

            if (ViewState["POLLUTIONID"] == null)
            {
                gvIncidentPollution.SelectedIndex = 0;
                ViewState["POLLUTIONID"] = ((Label)gvIncidentPollution.Rows[0].FindControl("lblIncidentPollutionId")).Text;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentPollutionGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?pollutionid=" + ViewState["POLLUTIONID"].ToString();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?pollutionid=" + ViewState["POLLUTIONID"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            ds.Tables[0].Rows.Clear();
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvIncidentPollution);

            ViewState["POLLUTIONID"] = null;
            ViewState["PAGEURL"] = "../Inspection/InspectionIncidentPollutionGeneral.aspx";
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionIncidentPollutionGeneral.aspx";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvIncidentDamage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["POLLUTIONID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIncidentPollutionId")).Text;

                BindPageURL(nCurrentRow);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string inspectionincidentpollutionid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIncidentPollutionId")).Text;
                ViewState["POLLUTIONID"] = null;

                PhoenixInspectionIncidentPollution.InspectionIncidentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(inspectionincidentpollutionid));
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvIncidentPollution.SelectedIndex = -1;
        for (int i = 0; i < gvIncidentPollution.Rows.Count; i++)
        {
            if (gvIncidentPollution.DataKeys[i].Value.ToString().Equals(ViewState["POLLUTIONID"].ToString()))
            {
                gvIncidentPollution.SelectedIndex = i;
                ViewState["POLLUTIONID"] = ((Label)gvIncidentPollution.Rows[i].FindControl("lblIncidentPollutionId")).Text;
                break;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["POLLUTIONID"] = ((Label)gvIncidentPollution.Rows[rowindex].FindControl("lblIncidentPollutionId")).Text;

            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?pollutionid=" + ViewState["POLLUTIONID"].ToString();

            SetRowSelection();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIncidentDamage_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvIncidentDamage_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvIncidentDamage_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvIncidentPollution.SelectedIndex = -1;
        gvIncidentPollution.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvIncidentDamage_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            
            if (db != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //ImageButton db = (ImageButton)e.Row.FindControl("cmdEdit");
            //string incidentid = ((Label)e.Row.FindControl("lblInspectionIncidentId")).Text;
            //if (db != null) db.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','InspectionSchedule.aspx?incidentid=" + incidentid + "')");
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvIncidentPollution.SelectedIndex = -1;
        gvIncidentPollution.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvIncidentPollution.SelectedIndex = -1;
        gvIncidentPollution.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["POLLUTIONID"] = null;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
