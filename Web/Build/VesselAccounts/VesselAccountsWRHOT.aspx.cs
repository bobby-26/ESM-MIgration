using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI;

public partial class VesselAccountsWRHOT :  PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvOPA.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvOPA.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
         
            toolbarmain.AddButton("Finalize", "FINALIZE");
            toolbarmain.AddButton("Show report", "REPORT");
            toolbarmain.AddButton("Back", "BACK");
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();
            

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Enabled = false;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsWRHOT.aspx", "Find", "search.png", "FIND");            
            MenuOPAList.AccessRights = this.ViewState;
            MenuOPAList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindYear();
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                    ddlVessel.SelectedVessel = Request.QueryString["vesselid"];
                }
                if (Request.QueryString["month"] != null && Request.QueryString["month"].ToString() != "")
                {
                    ViewState["MONTH"] = Request.QueryString["month"].ToString();
                    ddlMonth.SelectedValue = Request.QueryString["month"];
                }

                if (Request.QueryString["year"] != null && Request.QueryString["year"].ToString() != "")
                {
                    ViewState["YEAR"] = Request.QueryString["year"].ToString();
                    ddlYear.SelectedValue = Request.QueryString["year"];
                }
                BindEmployee();
                if (Request.QueryString["signonoffid"] != null && Request.QueryString["signonoffid"].ToString() != "")
                {
                    ViewState["SIGNONOFFID"] = Request.QueryString["signonoffid"].ToString();
                    ddlEmployee.SelectedValue = Request.QueryString["signonoffid"];
                }
                BindData();lastmonthBindData();
                
            }
            BindData();lastmonthBindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindEmployee()
    {
        if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        {
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = PhoenixVesselAccountsRHMonthlyOT.ListOfOnBoardEmployee(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableInteger(ddlMonth.SelectedValue)
                , General.GetNullableInteger(ddlYear.SelectedValue));

            ddlEmployee.DataTextField = "FLDNAME";
            ddlEmployee.DataValueField = "FLDSIGNONOFFID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 5); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["VESSELID"] != null && ViewState["MONTH"] != null && ViewState["YEAR"] != null && ViewState["SIGNONOFFID"] != null)
                {
                    Response.Redirect("../VesselAccounts/VesselAccountsWRHOTDetail.aspx?&vesselid=" + ViewState["VESSELID"].ToString() + "&month=" + ViewState["MONTH"].ToString() + "&year=" + ViewState["YEAR"].ToString() + "&signonoffid=" + ViewState["SIGNONOFFID"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../VesselAccounts/VesselAccountsWRHOTDetail.aspx");
                }
                
            }
            if (dce.CommandName.ToUpper().Equals("FINALIZE"))
            {
                

                if (!IsValidEarningDeduction(ViewState["VESSELID"].ToString(), General.GetNullableString(ddlMonth.SelectedValue), General.GetNullableString(ddlYear.SelectedValue)))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsRHMonthlyOT.Updatelock(General.GetNullableInteger(ddlMonth.SelectedValue)
                                                            , General.GetNullableInteger(ddlYear.SelectedValue)
                                                            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlEmployee.SelectedValue));
                PhoenixVesselAccountsRHMonthlyOT.Updatesummary(General.GetNullableInteger(ddlMonth.SelectedValue)
                                                            , General.GetNullableInteger(ddlYear.SelectedValue)
                                                            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                            );
                PhoenixVesselAccountsRHMonthlyOT.insertearning(General.GetNullableInteger(ddlMonth.SelectedValue)
                                                            , General.GetNullableInteger(ddlYear.SelectedValue)
                                                            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                            , General.GetNullableDecimal(lblAmount.Text));
                
                
                ucStatus.Text = "Monthly overtime report finalized";

            }
            if (dce.CommandName.ToUpper().Equals("REPORT"))
            {
                //Response.Redirect("../VesselAccounts/VesselAccountsMonthlyOvertimeSummary.aspx?&vesselid=" + ViewState["VESSELID"].ToString(), false);
                if (ViewState["VESSELID"] != null && ViewState["MONTH"] != null && ViewState["YEAR"] != null)
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=7&reportcode=MONTHLYOVERTIMESUMMARY&vesselid=" + ViewState["VESSELID"].ToString() + "&month=" + ViewState["MONTH"].ToString() + "&year=" + ViewState["YEAR"].ToString() + " & showmenu=0&showword=no", false);
                }
                  
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {

        ViewState["SIGNONOFFID"] = ddlEmployee.SelectedValue;
        BindData();lastmonthBindData();
    }
    protected void ddlyear_selectedindexchange(object sender, EventArgs e)
    {
        ViewState["YEAR"] = ddlYear.SelectedValue;
        BindData(); lastmonthBindData();
    }
    protected void ddlmonth_selectedindexchange(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        BindData(); lastmonthBindData();
    }
    protected void ddlvessel_selectedindexchange(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ddlVessel.SelectedVessel;
        BindData(); lastmonthBindData();
    }
    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Seafarer and then Navigate to other Tabs";
        ucError.Visible = true;

    }

    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["VESSELID"] = ddlVessel.SelectedVessel;
                ViewState["MONTH"] = ddlMonth.SelectedValue;
                ViewState["YEAR"] = ddlYear.SelectedValue;

                BindData();lastmonthBindData();
                //SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = "Dummy";
                ddlMonth.SelectedValue = "";
                ddlYear.SelectedValue = "2018";
                ViewState["VESSELID"] = "Dummy";
                ViewState["MONTH"] = "";
                ViewState["YEAR"] = "2018";
                BindData();
                lastmonthBindData();
                //SetPageNavigator();
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
        
    }

    private void BindData()
    {
        DataSet ds = PhoenixVesselAccountsRHMonthlyOT.RestHourOTSearch(General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                                            ,General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                                            , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : ddlMonth.SelectedValue)
                                                                            , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : ddlYear.SelectedValue)
                                                                            );
        if (ds.Tables[0].Rows.Count > 0)
        {
            if(ds.Tables[0].Rows.Count>0)
            {
                lblAmount.Text = ds.Tables[0].Rows[0]["FLDOTWAGE"].ToString();
            }
            gvOPA.DataSource = ds;
            gvOPA.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOPA);
        }
    }
    private void lastmonthBindData()
    {
        DataSet ds = PhoenixVesselAccountsRHMonthlyOT.RestHourPreviousOTSearch(General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                                            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                                            , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : ddlMonth.SelectedValue)
                                                                            , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : ddlYear.SelectedValue)
                                                                            );
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPrevious.DataSource = ds;
            gvPrevious.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPrevious);
        }
    }

    protected void gvOPA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

               

                //TextBox startdate = (TextBox)e.Row.FindControl("ucStartDate");
                //TextBox enddate = (TextBox)e.Row.FindControl("ucendDate");
                //if (startdate != null) startdate.Enabled = false;
                //if (enddate != null) enddate.Enabled = false;
                Label OTtotalwage = (Label)e.Row.FindControl("lblOTtotalwageItem");

              
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

           
        }
    }
    protected void gvPrevious_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }
    protected void gvOPA_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();lastmonthBindData();
        //SetPageNavigator();
    }
    protected void gvOPA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidList(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShipCalendarId")).Text))
            {
                ucError.Visible = true;
                return;
            }


            PhoenixVesselAccountsRH.RestHourOPAUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblopaeditid")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShipCalendarId")).Text),
                    General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShipCalendarendId")).Text),
                    int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReportingHourEdit")).Text),
                    int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtendReportingHourEdit")).Text)
                    );

            _gridView.EditIndex = -1;
            BindData();lastmonthBindData();
            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOPA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());



            //if (e.CommandName.ToUpper().Equals("DETAIL"))
            //{
            //    Response.Redirect("VesselAccountsWRHOTDetail.aspx?RestHourStartID=" + ((Label)_gridView.Rows[nCurrentRow].FindControl("lblresthourstartid")).Text
            //                        + "&vesselid=" + ddlVessel.SelectedVessel + "&month=" + ddlMonth.SelectedValue + "&year=" + ddlYear.SelectedValue, true);
            //}
            if (e.CommandName.ToUpper().Equals("SEND"))
            {

                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPrevious_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SEND"))
            {
               PhoenixVesselAccountsRHMonthlyOT.insertearningordection(General.GetNullableInteger(ddlMonth.SelectedValue)
                                                   , General.GetNullableInteger(ddlYear.SelectedValue)
                                                   , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                   , General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                   , General.GetNullableDecimal(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEarningDeductionitem")).Text));

                ucStatus.Text = "Updated in portage bill";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidEarningDeduction(string Vesselid, string month, string year)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(Vesselid).HasValue)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }

        if (!General.GetNullableInteger(month).HasValue)
        {
            ucError.ErrorMessage = "Month is required.";
        }

        if (!General.GetNullableInteger(year).HasValue)
        {
            ucError.ErrorMessage = "Year is required.";
        }

        return (!ucError.IsError);
    }
    private bool IsValidList(string Startid)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (string.IsNullOrEmpty(Startid))
            ucError.ErrorMessage = "Date of Entry is required.";

        return (!ucError.IsError);
    }

    protected void gvOPA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();lastmonthBindData();

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
        BindData();lastmonthBindData();
        //SetPageNavigator();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOPA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            BindData();lastmonthBindData();
            //SetPageNavigator();

        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void gvOPA_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();

            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Overtime";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

           
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();

            HeaderCell.HorizontalAlign = HorizontalAlign.Center;

            HeaderGridRow.Cells.Add(HeaderCell);
            gvOPA.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }
}
