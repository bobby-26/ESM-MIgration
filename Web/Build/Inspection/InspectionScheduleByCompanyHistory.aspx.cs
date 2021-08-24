using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;

public partial class InspectionScheduleByCompanyHistory : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvScheduleForCompany.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvScheduleForCompany.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);                   
            if (!IsPostBack)
            {                
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Inspection/InspectionScheduleByCompany.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvScheduleForCompany')", "Print Grid", "icon_print.png", "PRINT");
                MenuScheduleGroup.AccessRights = this.ViewState;
                MenuScheduleGroup.MenuList = toolbar.Show();
                MenuScheduleGroup.SetTrigger(pnlBudgetGroup);

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Schedule List", "SCHEDULE");
                toolbarmain.AddButton("History", "HISTORY");
                MenuSchedule.AccessRights = this.ViewState;
                MenuSchedule.MenuList = toolbarmain.Show();
                

                VesselConfiguration();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.bind();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }                                
                GetInspectionCompanyList();
            }
            MenuSchedule.SelectedMenuIndex = 1;
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSchedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SCHEDULE"))
            {
                Response.Redirect("../Inspection/InspectionScheduleByCompany.aspx");
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDVESSELTYPE", "FLDLASTDONEDATE", "FLDDUEDATE", "FLDBASISDETAILS", "FLDSCHEDULENUMBER", "FLDSCHEDULESTATUS" };
        string[] alCaptions = { "Company", "Vessel", "Type", "Last Done Date", "Due Date", "Basis", "Schedule Number", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.Equals("0") ? null : General.GetNullableInteger(ucVessel.SelectedVessel);

        ds = PhoenixInspectionSchedule.InspectionScheduleByCompanyHistorySearch(
                                                    vesselid,
                                                    General.GetNullableGuid(ddlCompany.SelectedValue),
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    (int)ViewState["PAGENUMBER"],
                                                    iRowCount,
                                                    ref iRowCount,
                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>CDI / SIRE Schedule</h3></td>");
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

    protected void BudgetGroup_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvScheduleForCompany.SelectedIndex = -1;
                gvScheduleForCompany.EditIndex = -1;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDVESSELTYPE", "FLDLASTDONEDATE", "FLDDUEDATE", "FLDBASISDETAILS", "FLDSCHEDULENUMBER", "FLDSCHEDULESTATUS" };
        string[] alCaptions = { "Company", "Vessel", "Type", "Last Done Date", "Due Date", "Basis", "Schedule Number", "Status" };

        int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.Equals("0") ? null : General.GetNullableInteger(ucVessel.SelectedVessel);

        DataSet ds = PhoenixInspectionSchedule.InspectionScheduleByCompanyHistorySearch(
                                                    vesselid,
                                                    General.GetNullableGuid(ddlCompany.SelectedValue),
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    (int)ViewState["PAGENUMBER"],
                                                    General.ShowRecords(null),
                                                    ref iRowCount,
                                                    ref iTotalPageCount);

        General.SetPrintOptions("gvScheduleForCompany", "CDI / SIRE Schedule", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvScheduleForCompany.DataSource = ds;
            gvScheduleForCompany.DataBind();            
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvScheduleForCompany);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvScheduleForCompany_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvScheduleForCompany_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvScheduleForCompany.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvScheduleForCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvScheduleForCompany_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sb.CommandName))
                    sb.Visible = false;
            }

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName))
                    cb.Visible = false;
            }            

            ImageButton imgNewSchedule = (ImageButton)e.Row.FindControl("imgNewSchedule");
            DataRowView dv = (DataRowView)e.Row.DataItem;
            if (imgNewSchedule != null)
            {
                if (General.GetNullableGuid(dv["FLDSHEDULEID"].ToString()) != null && General.GetNullableInteger(dv["FLDLATESTYN"].ToString()) == 1)
                    imgNewSchedule.Visible = true;
                if (!SessionUtil.CanAccess(this.ViewState, imgNewSchedule.CommandName))
                    imgNewSchedule.Visible = false;
            }

            DropDownList ddlBasis = (DropDownList)e.Row.FindControl("ddlBasis");
            Label lblCompanyId = (Label)e.Row.FindControl("lblCompanyId");
            if (ddlBasis != null)
            {
                Guid? companyid = lblCompanyId != null ? General.GetNullableGuid(lblCompanyId.Text) : null;
                DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(companyid);
                ddlBasis.DataSource = ds;
                ddlBasis.DataTextField = "FLDCOMPANYNAME";
                ddlBasis.DataValueField = "FLDCOMPANYID";
                ddlBasis.DataBind();
                ddlBasis.Items.Insert(0, new ListItem("--select--","Dummy"));               
            }
            
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

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
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;
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
        {
            return true;
        }

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

    protected void chkShowAll_Changed(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    private bool IsValidDetails(string vesselid )
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required";

        return (!ucError.IsError);
    } 

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }    
    
    protected void Text_Changed(object sender, EventArgs e)
    {
        BindData();
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ucLastDoneDateEdit_TextChanged(object sender, EventArgs e)
    {
        UserControlDate ucLastDoneDate = (UserControlDate)sender;
        GridViewRow row = (GridViewRow)ucLastDoneDate.Parent.Parent;
        ucLastDoneDate = (UserControlDate)row.Cells[1].FindControl("ucLastDoneDateEdit");
        UserControlDate ucDueDate = (UserControlDate)row.Cells[2].FindControl("ucDueDateEdit");

        if (ucLastDoneDate != null)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(ucLastDoneDate.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(6);
            ucDueDate.Text = dtDueDate.ToString();
        }
    }

    protected void GetInspectionCompanyList()
    {
        DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
}
