using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class AccountsEmployeeAllotmentRequestSideLetter : PhoenixBasePage
{
    DataTable dtbank = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                SessionUtil.PageAccessRights(this.ViewState);
                ViewState["COMPONENTID"] = null; ViewState["COMPONENTNAME"] = null; ViewState["BALANCE"] = null;
                ViewState["VESSELID"] = "";
                ViewState["EMPLOYEEID"] = "";
                ViewState["MONTH"] = "";
                ViewState["YEAR"] = "";
                ViewState["SIDELETTERAMOUNT"] = "0";
                ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
                ViewState["VESSELNAME"] = Request.QueryString["VESSELNAME"].ToString();
                ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
                txtVesselName.Text = ViewState["VESSELNAME"].ToString();
                txtMonthAndYear.Text = DateTime.Parse("01/" + ViewState["MONTH"].ToString() + "/" + ViewState["YEAR"].ToString()).ToString("MMM") + "-" + ViewState["YEAR"].ToString();
                EditEmployeedetails(int.Parse(ViewState["EMPLOYEEID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditEmployeedetails(int EmployeeId, int VesselId)
    {
        try
        {
            DataTable dt = PhoenixAccountsEmployeeAllotmentRequest.EditAllotmentEmployeeDetails(EmployeeId, VesselId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData(int i)
    {
        try
        {
            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.CrewSideLetterSearch(int.Parse(ViewState["EMPLOYEEID"].ToString())
                                                                                        , int.Parse(ViewState["VESSELID"].ToString())
                                                                                        , int.Parse(ViewState["MONTH"].ToString())
                                                                                        , int.Parse(ViewState["YEAR"].ToString())
                                                                                     , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()));


            if (i == 1)
            {
                gvSideLetter.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                    ViewState["EMPLOYEEID"] = dr["FLDEMPLOYEEID"].ToString();
                    txtWageCalculationDate.Text = dr["FLDWAGECALCULATIONDATE"].ToString();

                    ViewState["COMPONENTID"] = null; ViewState["COMPONENTNAME"] = null; ViewState["BALANCE"] = null;
                    string componenetid = dr["FLDCOMPONENTID"].ToString(); //((RadLabel)gvSideLetter.FindControl("lblComponentId")).Text;
                    string lnkComponenetName = dr["FLDCOMPONENTNAME"].ToString(); //((LinkButton)gvSideLetter.FindControl("lnkComponenetName")).Text;
                    string lblBalance = dr["FLDBALANCEAMOUNT"].ToString(); // ((RadLabel)gvSideLetter.FindControl("lblBalance")).Text;
                    ViewState["COMPONENTID"] = componenetid; ViewState["COMPONENTNAME"] = lnkComponenetName; ViewState["BALANCE"] = lblBalance;

                }
            }

            if (i == 2)
            {
                gvSLRequest.DataSource = ds.Tables[1];
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                dtbank = ds.Tables[2];

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    if (dr["FLDISDEFAULT"].ToString() == "1")
                        ViewState["DEFAULTBANK"] = dr["FLDBANKACCOUNTID"].ToString();
                }
            }
            else
            {
                dtbank = null;
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
        gvSLRequest.SelectedIndexes.Clear();
        gvSLRequest.EditIndexes.Clear();
        gvSLRequest.DataSource = null;
        gvSLRequest.Rebind();
    }

    protected void Lock_Confirm(object sender, EventArgs e)
    {
        try
        {
            GridFooterItem footeritem = (GridFooterItem)gvSLRequest.MasterTableView.GetItems(GridItemType.Footer)[0];
            string componenetid = ((RadLabel)footeritem.FindControl("lblcomponentid")).Text;
            string bankid = ((RadDropDownList)footeritem.FindControl("ddlBankId")).SelectedValue;
            string amount = ((UserControlMaskNumber)footeritem.FindControl("txtActualAmountAdd")).Text;
            PhoenixAccountsAllotmentRequestSystemChecking.AllotmentSideLetterRequestInsert(General.GetNullableGuid(componenetid)
                                                                                             , General.GetNullableDateTime(txtWageCalculationDate.Text)
                                                                                             , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                                             , General.GetNullableDecimal(amount)
                                                                                             , General.GetNullableGuid(bankid)
                                                                                             , null
                                                                                             , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                                                                                             , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()));
            ucStatus.Visible = true;
            ucStatus.Text = "Allotment Request Added";
            Rebind();
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('filter','ifMoreInfo','keepopen');";
            Script += "</script>" + "\n";

            DataSet ds = new DataSet();
            ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                 General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                                 General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                                 null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows) // Loop over the rows.
                {
                    PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(row["FLDALLOTMENTID"].ToString()));
                }
            }
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSideLetter(string componentid, string bankid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableGuid(componentid) == null)
            ucError.ErrorMessage = "Component Required";
        if (General.GetNullableGuid(bankid) == null)
            ucError.ErrorMessage = "Bank Required";
        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount Required";
        else if (General.GetNullableDecimal(amount) > General.GetNullableDecimal(ViewState["BALANCE"].ToString()))
            ucError.ErrorMessage = "Amount Should be less than or equal to " + ViewState["BALANCE"].ToString();
        if (General.GetNullableDecimal(amount) <= 0)
            ucError.ErrorMessage = "Amount Should be greater than Zero";

        return (!ucError.IsError);
    }

    protected void gvSideLetter_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper() == "SELECT")
        {
            //  int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            ViewState["COMPONENTID"] = null; ViewState["COMPONENTNAME"] = null; ViewState["COMPONENTNAME"] = null;
            string componenetid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            string lnkComponenetName = ((LinkButton)e.Item.FindControl("lnkComponenetName")).Text;
            string lblBalance = ((RadLabel)e.Item.FindControl("lblBalance")).Text;
            ViewState["COMPONENTID"] = componenetid; ViewState["COMPONENTNAME"] = lnkComponenetName; ViewState["BALANCE"] = lblBalance;
            //  _gridView.SelectedIndex = nCurrentRow; BindData();
            UserControlMaskNumber amount = ((UserControlMaskNumber)e.Item.FindControl("txtActualAmountAdd"));
            RadLabel Componentname = ((RadLabel)e.Item.FindControl("lblcomponentname"));
            Componentname.Text = ViewState["COMPONENTNAME"] != null ? ViewState["COMPONENTNAME"].ToString() : "";
            amount.Text = ViewState["BALANCE"] != null ? ViewState["BALANCE"].ToString() : "";

        }
    }

    protected void gvSideLetter_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvSideLetter_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSideLetter.CurrentPageIndex + 1;
        BindData(1);
    }

    protected void gvSLRequest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "ADD")
        {
            string componenetid = ((RadLabel)e.Item.FindControl("lblcomponentid")).Text;
            string bankid = ((RadDropDownList)e.Item.FindControl("ddlBankId")).SelectedValue;
            string amount = ((UserControlMaskNumber)e.Item.FindControl("txtActualAmountAdd")).Text;
            if (!IsValidSideLetter(componenetid, bankid, amount))
            {
                ucError.Visible = true;
                return;
            }
            string confirmtext;

            confirmtext = "Are you sure that the seafarer is onboard ?";
            RadWindowManager1.RadConfirm(confirmtext, "confirm", 320, 150, null, "Confirm");

        }
    }

    protected void gvSLRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            RadDropDownList ddlBankAccount = (RadDropDownList)e.Item.FindControl("ddlBankId");

            if (ddlBankAccount != null)
            {
                ddlBankAccount.Items.Clear();
                ddlBankAccount.DataSource = dtbank;
                ddlBankAccount.DataBind();
                ddlBankAccount.Items.Insert(0, new DropDownListItem("--Select--", ""));
                if (ViewState["DEFAULTBANK"] != null && ViewState["DEFAULTBANK"].ToString() != "")
                    ddlBankAccount.SelectedValue = ViewState["DEFAULTBANK"].ToString();
            }
            UserControlMaskNumber amount = ((UserControlMaskNumber)e.Item.FindControl("txtActualAmountAdd"));
            RadLabel Componentname = ((RadLabel)e.Item.FindControl("lblcomponentname"));
            RadLabel lblcomponentid = ((RadLabel)e.Item.FindControl("lblcomponentid"));
            amount.Text = ViewState["BALANCE"] != null ? ViewState["BALANCE"].ToString() : string.Empty;
            Componentname.Text = ViewState["COMPONENTNAME"] != null ? ViewState["COMPONENTNAME"].ToString() : "";
            lblcomponentid.Text = ViewState["COMPONENTID"] != null ? ViewState["COMPONENTID"].ToString() : "";
        }
    }
    protected void gvSLRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        BindData(2);
    }
}
