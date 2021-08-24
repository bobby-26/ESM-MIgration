using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewWorkingGearIssuePayByAccounts : PhoenixBasePage
{
    string username;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvWorkGearIssue.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvWorkGearIssue.UniqueID, "Edit$" + r.RowIndex.ToString());
            }

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        username = PhoenixSecurityContext.CurrentSecurityContext.FirstName + "" + PhoenixSecurityContext.CurrentSecurityContext.MiddleName
                             + "" + PhoenixSecurityContext.CurrentSecurityContext.LastName;

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddImageButton("../Crew/CrewWorkingGearIssuePayByAccounts.aspx", "<b>Find</b>", "search.png", "FIND");
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();
            MenuRegistersWorkingGearItem.SetTrigger(pnlWorkingGearItem);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
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

        string[] alColumns = { "FLDNAME", "FLDTRANSACTIONDATE", "FLDISSUEDVESSELNAME", "FLDJOINEDVESSELNAME", "FLDPAYABLEBYACCOUNT" };
        string[] alCaptions = { "SeaFarer Name", "Issue Date", "Vessel Issued for", "Joined Vessel", "On Account Of" };


        DataSet ds = new DataSet();

        ds = PhoenixCrewWorkingGearIssueSummary.WorkingGearIssuePendingBillList(General.GetNullableDateTime(txtFromDate.Text),
                                                                               General.GetNullableDateTime(txtToDate.Text));

        General.SetPrintOptions("gvWorkGearIssue", "Working Gear Item", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkGearIssue.DataSource = ds;
            gvWorkGearIssue.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvWorkGearIssue);
        }

    }

    protected void gvWorkGearIssue_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DropDownList dlpay = (DropDownList)e.Row.FindControl("ddlPayBy");
            if (dlpay != null)
            {
                dlpay.DataBind();
                dlpay.SelectedValue = drv["FLDPAYABLEBYACCOUNT"].ToString();
            }
            CheckBox chk = (CheckBox)e.Row.FindControl("chkVerified");
            if (chk != null)
                chk.Checked = drv["FLDVERIFIED"].ToString().Equals("Yes") ? true : false;

        }
    }

    protected void gvWorkGearIssue_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvWorkGearIssue_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            //((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantity")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkGearIssue_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string tranid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTransIdEdit")).Text;
            string empid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeIdEdit")).Text;
            string date = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTranDateEdit")).Text;
            string issuevessel = ((Label)_gridView.Rows[nCurrentRow].FindControl("LblIssueVesselIdEdit")).Text;
            string joinvessel = ((Label)_gridView.Rows[nCurrentRow].FindControl("LblJoinVesseIdEdit")).Text;
            string trantype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTranTypeEdit")).Text;
            string refno = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRefNoEdit")).Text;

            string payby = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlPayBy")).SelectedValue;
            string verified = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkVerified")).Checked ? "1" : "0";
            string verifiedby = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkVerified")).Checked ? username : "";

            if (General.GetNullableInteger(payby).HasValue)
            {
                PhoenixCrewWorkingGearIssueSummary.WorkingGearIssuePayByAccountUpdate(General.GetNullableGuid(tranid)
                                                                        , General.GetNullableByte(trantype)
                                                                        , General.GetNullableString(refno)
                                                                        , General.GetNullableInteger(empid)
                                                                        , General.GetNullableDateTime(date)
                                                                        , General.GetNullableInteger(issuevessel)
                                                                        , General.GetNullableInteger(joinvessel)
                                                                        , General.GetNullableByte(payby)
                                                                        , General.GetNullableByte(verified)
                                                                        , General.GetNullableString(username));
            }
            else
            {
                ucError.ErrorMessage = "Please fill the required fields";
                ucError.ErrorMessage = "Payable by Account is Required.";
                ucError.Visible = true;
                return;
            }
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    }
}
