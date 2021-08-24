using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReportPassengerList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            //cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportPassengerList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPassengerList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "MIDNIGHTREPORTLIST");
            toolbarvoyagetap.AddButton("MidNight Report", "MIDNIGHTREPORT");
            toolbarvoyagetap.AddButton("Tank Plan", "TANKPLAN");
            toolbarvoyagetap.AddButton("HSE", "HSE");
            toolbarvoyagetap.AddButton("Passenger List", "PASSENGERLIST");
            toolbarvoyagetap.AddButton("Crew", "CREW");
            toolbarvoyagetap.AddButton("Technical", "TECHNICAL");

            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();
            MenuReportTap.SelectedMenuIndex = 4;

            if (!IsPostBack)
            {
                gvPassengerList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;

                }
                else
                {
                    ViewState["VESSELID"] = "";
                    ucVessel.Enabled = false;
                }

                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //BindData();
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MIDNIGHTREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("MIDNIGHTREPORT"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("HSE"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportHSE.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("CREW"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportCrew.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportTechnical.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("TANKPLAN"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportTankPlan.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("PASSENGERLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportPassengerList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }

    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvPassengerList.EditIndex = -1;
                //gvPassengerList.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                //SetPageNavigator();
            }
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
    private void EditMidNightReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(new Guid(Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["REPORTDATE"] = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            txtDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            txtCrew.Text = ds.Tables[0].Rows[0]["FLDCREW"].ToString();
            ucLifeBoatCapacity.Text = ds.Tables[0].Rows[0]["FLDLIFEBOATCAPACITY"].ToString();
            ucPassenger.Text = ds.Tables[0].Rows[0]["FLDPASSENGERCOUNT"].ToString();

            ucBreakfast.Text = ds.Tables[0].Rows[0]["FLDBREAKFAST"].ToString();
            ucLunch.Text = ds.Tables[0].Rows[0]["FLDLUNCH"].ToString();
            ucDinner.Text = ds.Tables[0].Rows[0]["FLDDINNER"].ToString();
            ucSupper.Text = ds.Tables[0].Rows[0]["FLDSUPPER"].ToString();
            ucTea1.Text = ds.Tables[0].Rows[0]["FLDCLIENTTEA1"].ToString();
            ucTea2.Text = ds.Tables[0].Rows[0]["FLDCLIENTTEA2"].ToString();

            ucSupBreakFast.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYBREAKFAST"].ToString();
            ucSupLunch.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYLUNCH"].ToString();
            ucSupDinner.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYDINNER"].ToString();
            ucSupSupper.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYSUPPER"].ToString();
            ucSupTea1.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYTEA1"].ToString();
            ucSupTea2.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYTEA2"].ToString();

            if (General.GetNullableInteger(ucPassenger.Text) > General.GetNullableInteger(ucLifeBoatCapacity.Text))
                ucLifeBoatCapacity.CssClass = "maxhighlight";

            
        }
    }

    //protected void gvPassengerList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!checkvalue(
    //                (((TextBox)_gridView.FooterRow.FindControl("txtPassengerAdd")).Text),
    //                 (((DropDownList)_gridView.FooterRow.FindControl("ddlTypeAdd")).SelectedValue),
    //                (((UserControlDate)_gridView.FooterRow.FindControl("txtEmbarkedDateAdd")).Text),
    //                (((UserControlDate)_gridView.FooterRow.FindControl("txtDisembarkedDateAdd")).Text)))
                   
    //    return;

    //            PhoenixCrewOffshoreDMRPassengerList.DMRPassengerInsert(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                General.GetNullableGuid(Session["MIDNIGHTREPORTID"].ToString()),
    //                ((TextBox)_gridView.FooterRow.FindControl("txtPassengerAdd")).Text,
    //                 General.GetNullableString(((DropDownList)_gridView.FooterRow.FindControl("ddlTypeAdd")).SelectedValue),
    //                General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtEmbarkedDateAdd")).Text),
    //                General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtDisembarkedDateAdd")).Text),
    //                General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text));

    //            BindData();
    //            EditMidNightReport();

    //        }
    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixCrewOffshoreDMRPassengerList.DMRPassengerDelete(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblPassengerId")).Text));
    //        }

    //        SetPageNavigator();
    //        EditMidNightReport();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool checkvalue(string PassengerName,string type,string embarkeddate,string disembarkeddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((PassengerName == null) || (PassengerName.Trim() == ""))
            ucError.ErrorMessage = "Passenger Name is required.";

        if (General.GetNullableString(type)==null)
            ucError.ErrorMessage = "Type is required.";

        if (General.GetNullableDateTime(embarkeddate) == null)
            ucError.ErrorMessage = "Embarked date is required.";

        if (General.GetNullableDateTime(embarkeddate) != null && (General.GetNullableDateTime(embarkeddate) > General.GetNullableDateTime(txtDate.Text)))
            ucError.ErrorMessage = "Embarked date should be before report date.";

        if (General.GetNullableDateTime(embarkeddate) != null && General.GetNullableDateTime(disembarkeddate) != null)
        {
            if ((General.GetNullableDateTime(embarkeddate) > General.GetNullableDateTime(disembarkeddate)))
                ucError.ErrorMessage = "Disembarked date should be furture date of Embarked date.";
        }

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDPASSENGERNAME", "FLDTYPE", "FLDEMBARKEDDATE", "FLDDISEMBARKEDDATE", "FLDREMARKS" };
        string[] alCaptions = { "Sl.No", "Passenger Name","Type","Embarked Date","Disembarked Date","Remark"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreDMRPassengerList.DMRPassengerSearch("", new Guid(Session["MIDNIGHTREPORTID"].ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
              gvPassengerList.PageSize, ref iRowCount, ref iTotalPageCount);

       General.SetPrintOptions("gvPassengerList", "DMR Passenger", alCaptions, alColumns, ds);

        gvPassengerList.DataSource = ds;
        gvPassengerList.VirtualItemCount = iRowCount;
       // gvPassengerList.DataBind();
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       // SetPageNavigator();
    }

    //protected void gvPassengerList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            DataRowView drv = (DataRowView)e.Row.DataItem;

    //            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
    //            DropDownList ddltype = (DropDownList)e.Row.FindControl("ddlTypeEdit");
    //            if (ddltype != null)
    //            {
    //                ddltype.SelectedValue = drv["FLDTYPE"].ToString();
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //            if (db != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                    db.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvPassengerList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvPassengerList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPassengerList, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassengerList_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvPassengerList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (!checkvalue((((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtPassengerEdit")).Text),
    //                            (((RadDropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit")).SelectedValue),
    //                            (((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEmbarkedDateEdit")).Text),
    //                            (((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDisembarkedDateEdit")).Text)))
    //            return;

    //        PhoenixCrewOffshoreDMRPassengerList.DMRPassengerUpdate(
    //            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //             General.GetNullableGuid(Session["MIDNIGHTREPORTID"].ToString()),
    //            new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblPassengerIdEdit")).Text),
    //            ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtPassengerEdit")).Text,
    //            General.GetNullableString(((RadDropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit")).SelectedValue),
    //            General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtEmbarkedDateEdit")).Text),
    //            General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDisembarkedDateEdit")).Text),
    //            General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text));

    //        _gridView.EditIndex = -1;
    //        BindData();
    //        //SetPageNavigator();
    //        EditMidNightReport();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvPassengerList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        //SetPageNavigator();
    }

    protected void gvPassengerList_Sorting(object sender, GridViewSortEventArgs se)
    {
        //gvPassengerList.EditIndex = -1;
        //gvPassengerList.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    //gvPassengerList.SelectedIndex = -1;
    //    //gvPassengerList.EditIndex = -1;
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
    //    //gvPassengerList.SelectedIndex = -1;
    //    //gvPassengerList.EditIndex = -1;
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
    //    {
    //        return true;
    //    }

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

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    //gvPassengerList.SelectedIndex = -1;
    //    //gvPassengerList.EditIndex = -1;
    //    ViewState["PAGENUMBER"] = 1;
    //    BindData();
    //    SetPageNavigator();
    //}

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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();


        string[] alColumns = { "FLDROWNUMBER", "FLDPASSENGERNAME", "FLDTYPE", "FLDEMBARKEDDATE", "FLDDISEMBARKEDDATE", "FLDREMARKS" };
        string[] alCaptions = { "Sl.No", "Passenger Name", "Type", "Embarked Date", "Disembarked Date", "Remark" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreDMRPassengerList.DMRPassengerSearch("", new Guid(Session["MIDNIGHTREPORTID"].ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"PassengerList.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Passenger List</h3></td>");
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


    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPassengerUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                        , General.GetNullableInteger(ucBreakfast.Text)
                        , General.GetNullableInteger(ucLunch.Text)
                        , General.GetNullableInteger(ucDinner.Text)
                        , General.GetNullableInteger(ucTea1.Text), General.GetNullableInteger(ucTea2.Text), General.GetNullableInteger(ucSupper.Text)
                        , General.GetNullableInteger(ucSupBreakFast.Text), General.GetNullableInteger(ucSupLunch.Text), General.GetNullableInteger(ucSupDinner.Text), General.GetNullableInteger(ucSupSupper.Text)
                        , General.GetNullableInteger(ucSupTea1.Text), General.GetNullableInteger(ucSupTea2.Text)
                     );

                    ucStatus.Text = "MidNight Report Updated";
                
                EditMidNightReport();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassengerList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPassengerList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassengerList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            //if (e.Item is  GridItemType.Footer)
            //{
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                //}
            }

            if (e.Item is GridDataItem)
            {

                GridDataItem item = (GridDataItem)e.Item;

              
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
              
                RadDropDownList ddltype = (RadDropDownList)e.Item.FindControl("ddlTypeEdit");
                if (ddltype != null)
                {
                    ddltype.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDTYPE").ToString(); //  drv["FLDTYPE"].ToString();
                }
            }


            //GridFooterItem footerItem = (GridFooterItem)gvPassengerList.MasterTableView.GetItems(GridItemType.Footer)[0];
            //// Button btn = (Button)footerItem.FindControl("Button1");
            //ImageButton db1 = (ImageButton)footerItem.FindControl("cmdAdd");
           
            //    if (db1 != null)
            //    {
            //        if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName))
            //        db1.Visible = false;
            //    }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassengerList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            RadGrid _gridView = (RadGrid)sender;
           


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)_gridView.MasterTableView.GetItems(GridItemType.Footer)[0];
                // Button btn = (Button)footerItem.FindControl("Button1");
               
                if (!checkvalue(
                    (((RadTextBox)footerItem.FindControl("txtPassengerAdd")).Text),
                     (((RadDropDownList)footerItem.FindControl("ddlTypeAdd")).SelectedValue),
                    (((UserControlDate)footerItem.FindControl("txtEmbarkedDateAdd")).Text),
                    (((UserControlDate)footerItem.FindControl("txtDisembarkedDateAdd")).Text)))

                    return;
               

                PhoenixCrewOffshoreDMRPassengerList.DMRPassengerInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(Session["MIDNIGHTREPORTID"].ToString()),
                    ((RadTextBox)footerItem.FindControl("txtPassengerAdd")).Text,
                     General.GetNullableString(((RadDropDownList)footerItem.FindControl("ddlTypeAdd")).SelectedValue),
                    General.GetNullableDateTime(((UserControlDate)footerItem.FindControl("txtEmbarkedDateAdd")).Text),
                    General.GetNullableDateTime(((UserControlDate)footerItem.FindControl("txtDisembarkedDateAdd")).Text),
                    General.GetNullableString(((RadTextBox)footerItem.FindControl("txtRemarksAdd")).Text));

                BindData();
                gvPassengerList.Rebind();
                EditMidNightReport();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewOffshoreDMRPassengerList.DMRPassengerDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblPassengerId")).Text));
            }

           // SetPageNavigator();
          //  EditMidNightReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassengerList_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            //RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (!checkvalue((((RadTextBox)eeditedItem.FindControl("txtPassengerEdit")).Text),
                                (((RadDropDownList)eeditedItem.FindControl("ddlTypeEdit")).SelectedValue),
                                (((UserControlDate)eeditedItem.FindControl("txtEmbarkedDateEdit")).Text),
                                (((UserControlDate)eeditedItem.FindControl("txtDisembarkedDateEdit")).Text)))
                return;
            string lblpassengerid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDPASSENGERID"].ToString();
            PhoenixCrewOffshoreDMRPassengerList.DMRPassengerUpdate(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(Session["MIDNIGHTREPORTID"].ToString()),
                new Guid(lblpassengerid),
                ((RadTextBox)eeditedItem.FindControl("txtPassengerEdit")).Text,
                General.GetNullableString(((RadDropDownList)eeditedItem.FindControl("ddlTypeEdit")).SelectedValue),
                General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("txtEmbarkedDateEdit")).Text),
                General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("txtDisembarkedDateEdit")).Text),
                General.GetNullableString(((RadTextBox)eeditedItem.FindControl("txtRemarksEdit")).Text));

           // _gridView.EditIndex = -1;
            BindData();
            //SetPageNavigator();
            EditMidNightReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
