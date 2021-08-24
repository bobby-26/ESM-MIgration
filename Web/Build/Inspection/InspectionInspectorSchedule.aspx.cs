using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionInspectorSchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucConfirmComplete.Visible = false;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionInspectorSchedule.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionSchedule')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuInspectionScheduleSearch.AccessRights = this.ViewState;
            MenuInspectionScheduleSearch.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "SEARCH", ToolBarDirection.Right);
            toolbarmain.AddButton("Inspection List", "LIST", ToolBarDirection.Right);
            MenuGeneral.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                if (Request.QueryString["INSPECTOR"] != null && Request.QueryString["INSPECTOR"].ToString() != "")
                {
                    ViewState["INSPECTOR"] = Request.QueryString["INSPECTOR"].ToString();
                    txtInspector.Text = ViewState["INSPECTOR"].ToString();

                }
                else
                    ViewState["INSPECTOR"] = "";

                ucVessel.bind();
                if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                {
                    ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration.ToString();
                    ucVessel.Enabled = false;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                ViewState["PAGEURL"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
            }

            MenuGeneral.SelectedMenuIndex = 1;
          //  BindData();
            //  SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionInspectorList.aspx");
        }

    }

    private void ShowNavigationError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please Select a Inspection and Navigate to other Tabs";
        ucError.Visible = true;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //int vesselid = -1;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINSPECTIONCOMPANYNAME", "FLDINSPECTIONNAME",
                                 "FLDCOMPLETIONDATE", "FLDNAMEOFINSPECTOR"};
        string[] alCaptions = { "Vessel", "Reference Number", "Company", "Vetting",
                                  "Last Done Date", "Inspector"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



        string status = "";
        status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CMP");


        DataSet ds = PhoenixInspectionSchedule.InspectionInspectorScheduleSearch(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , null
            , null
            , General.GetNullableInteger(ucVessel.SelectedVessel)
            , null
            , General.GetNullableString(status)
            , null
            , null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , null
            , null
            , null
            , null
            , null
            , General.GetNullableString(ViewState["INSPECTOR"].ToString())
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspection List</h3></td>");
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

    protected void InspectionScheduleSearch_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvInspectionSchedule.Rebind();
            //  SetPageNavigator();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentInspectionScheduleFilterCriteria = null;
            // BindData();
            gvInspectionSchedule.Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //int vesselid = -1;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINSPECTIONCOMPANYNAME", "FLDINSPECTIONNAME",
                                 "FLDCOMPLETIONDATE", "FLDNAMEOFINSPECTOR"};
        string[] alCaptions = { "Vessel", "Reference Number", "Company", "Vetting",
                                  "Last Done Date", "Inspector"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        string status = "";
        status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CMP");


        DataSet ds = PhoenixInspectionSchedule.InspectionInspectorScheduleSearch(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , null
            , null
            , General.GetNullableInteger(ucVessel.SelectedVessel)
            , null
            , General.GetNullableString(status)
            , null
            , null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount
            , null
            , null
            , null
            , null
            , null
            , General.GetNullableString(ViewState["INSPECTOR"].ToString())
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvInspectionSchedule", "Inspection List", alCaptions, alColumns, ds);

        gvInspectionSchedule.DataSource = ds;
        gvInspectionSchedule.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        //string a = ViewState["INSPECTIONSCHEDULEID"].ToString();
        //gvInspectionSchedule.SelectedIndex = -1;
        //for (int i = 0; i < gvInspectionSchedule.Rows.Count; i++)
        //{
        //    if (gvInspectionSchedule.DataKeys[i].Value.ToString().Equals(ViewState["INSPECTIONSCHEDULEID"].ToString()))
        //    {
        //        gvInspectionSchedule.SelectedIndex = i;

        //    }
        //}
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblInspectionScheduleId = (RadLabel)gvInspectionSchedule.Items[rowindex].FindControl("lblInspectionScheduleId");
            if (lblInspectionScheduleId != null)
            {
                ViewState["INSPECTIONSCHEDULEID"] = lblInspectionScheduleId.Text;
                //ifMoreInfo.Attributes["src"] = "../Inspection/InspectionScheduleGeneral.aspx?INSPECTIONSCHEDULEID=" + Filter.CurrentSelectedInspectionSchedule;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvInspectionSchedule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
              //  BindPageURL(nCurrentRow);
                SetRowSelection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteInspectionSchedule(Guid inspectionscheduleid)
    {
        // PhoenixInspectionSchedule.DeleteInspectionSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionscheduleid);
    }

    protected void gvInspectionSchedule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string scheduleid = ((RadLabel)e.Item.FindControl("lblInspectionScheduleId")).Text;
            DataRowView dr = (DataRowView)e.Item.DataItem;

            RadLabel lblInspectionScheduleId = (RadLabel)e.Item.FindControl("lblInspectionScheduleId");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");

            LinkButton lnkInspection = (LinkButton)e.Item.FindControl("lnkInspection");
            if (lnkInspection != null)
            {
                lnkInspection.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionInspectorDeficiencySummary.aspx?SOURCEID=" + lblInspectionScheduleId.Text + "&VESSELID=" + lblVesselId.Text + "'); return true;");
            }

            LinkButton lnkMinorNc = (LinkButton)e.Item.FindControl("lnkMinorNc");
            if (lnkMinorNc != null)
                lnkMinorNc.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionInspectorDeficiencySummary.aspx?SOURCEID=" + lblInspectionScheduleId.Text + "&VESSELID=" + lblVesselId.Text + "'); return true;");

            //LinkButton lnkMajorNc = (LinkButton)e.Row.FindControl("lnkMajorNc");
            //if (lnkMajorNc != null)
            //    lnkMajorNc.Attributes.Add("onclick", "javascript:Openpopup('Report','','../Inspection/InspectionInspectorDeficiencySummary.aspx?SOURCEID=" + lblInspectionScheduleId.Text + "&VESSELID=" + lblVesselId.Text + "'); return true;");

            //LinkButton lnkObservation = (LinkButton)e.Row.FindControl("lnkObservation");
            //if (lnkObservation != null)
            //    lnkObservation.Attributes.Add("onclick", "javascript:Openpopup('Report','','../Inspection/InspectionInspectorDeficiencySummary.aspx?SOURCEID=" + lblInspectionScheduleId.Text + "&VESSELID=" + lblVesselId.Text + "'); return true;");

        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvInspectionSchedule.SelectedIndex = -1;
    //    gvInspectionSchedule.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvInspectionSchedule.SelectedIndex = -1;
    //    gvInspectionSchedule.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvInspectionSchedule.Rebind();
        // SetPageNavigator();
        if (Session["NewSchedule"] != null && Session["NewSchedule"].ToString() == "Y")
        {
            //gvInspectionSchedule.SelectedIndex = 0;
            //Session["NewSchedule"] = "N";
            //BindPageURL(gvInspectionSchedule.SelectedIndex);
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SCHEDULEID"] != null)
            {
                PhoenixInspectionSchedule.UpdateInspectionScheduleStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["SCHEDULEID"].ToString()));
                gvInspectionSchedule.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        gvInspectionSchedule.Rebind();
        // SetPageNavigator();
    }

    protected void gvInspectionSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionSchedule.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspectionSchedule_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
