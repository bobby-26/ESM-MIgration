using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;


public partial class AccountsVesselVisitITSuperintendentVisitLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarMain = new PhoenixToolbar();

            toolbarMain.AddButton("Vessel Visit", "VISIT");
            toolbarMain.AddButton("Line Item", "LINEITEM");
            MenuLineItemMain.AccessRights = this.ViewState;
            MenuLineItemMain.MenuList = toolbarMain.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Report", "REPORT");
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                ViewState["VisitId"] = Request.QueryString["visitId"];
                ViewState["PAGENUMBER"] = 1;
            }
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItemMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("VISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=1");
            }
            else if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitLineItem.aspx?visitId=" + ViewState["VisitId"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVISITLINEITEM&visitId=" + ViewState["VisitId"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixAccountsVesselVisitITSuperintendentLineItem.VisitLineItemSearch(new Guid(ViewState["VisitId"].ToString()),
                                                                                (int)ViewState["PAGENUMBER"],
                                                                                General.ShowRecords(null),
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds;
                gvLineItem.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvLineItem);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                TextBox txtDescription = ((TextBox)_gridView.FooterRow.FindControl("txtDescription"));
                if (!IsValidData(txtDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVisitITSuperintendentLineItem.InsertVisitLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["VisitId"].ToString()),
                    txtDescription.Text);

                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblLineItemId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblLineItemId"));
                PhoenixAccountsVesselVisitITSuperintendentLineItem.DeleteVisitLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblLineItemId.Text));
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
            DataRowView drv = (DataRowView)e.Row.DataItem;
            RadioButtonList rblGrading = (RadioButtonList)e.Row.FindControl("rblGrading");
            RadioButtonList rblCompleted = (RadioButtonList)e.Row.FindControl("rblCompleted");

            if (General.GetNullableInteger(drv["FLDGRADING"].ToString()) != null)
                rblGrading.Items.FindByValue(drv["FLDGRADING"].ToString()).Selected = true;

            if (General.GetNullableInteger(drv["FLDCOMPLETED"].ToString()) != null)

                //rblCompleted.SelectedValue(drv["FLDCOMPLETED"].ToString()).sele
               rblCompleted.Items.FindByValue(drv["FLDCOMPLETED"].ToString()).Selected = true;

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

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
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
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvLineItem.SelectedIndex = -1;
        gvLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void rblCompleted_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rblCompleted = (RadioButtonList)sender;
        GridViewRow gvrow = (GridViewRow)rblCompleted.Parent.Parent;

        string lblLineItemId = ((Label)gvrow.FindControl("lblLineItemId")).Text;
        string rblGrading = ((RadioButtonList)gvrow.FindControl("rblGrading")).SelectedValue;
        string completed = rblCompleted.SelectedValue;

        if (rblGrading == "")
            rblGrading = null;

        PhoenixAccountsVesselVisitITSuperintendentLineItem.UpdateVisitLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(lblLineItemId), General.GetNullableInteger(completed), General.GetNullableInteger(rblGrading));
    }

    protected void rblGrading_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rblGrading = (RadioButtonList)sender;
        GridViewRow gvrow = (GridViewRow)rblGrading.Parent.Parent;

        string lblLineItemId = ((Label)gvrow.FindControl("lblLineItemId")).Text;
        string rblCompleted = ((RadioButtonList)gvrow.FindControl("rblCompleted")).SelectedValue;
        string grading = rblGrading.SelectedValue;

        if (rblCompleted == "")
            rblCompleted = null;

        PhoenixAccountsVesselVisitITSuperintendentLineItem.UpdateVisitLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(lblLineItemId),General.GetNullableInteger(rblCompleted), General.GetNullableInteger(grading) );

    }

    private bool IsValidData(string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }
}
