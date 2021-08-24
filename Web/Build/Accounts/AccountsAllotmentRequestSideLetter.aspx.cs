using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;

public partial class AccountsAllotmentRequestSideLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                ViewState["ALLOTMENTID"] = "";
                ViewState["VESSELID"] = "";
                ViewState["EMPLOYEEID"] = "";
                ViewState["SIDELETTERAMOUNT"] = "0";  

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();               
            }

            BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = PhoenixAccountsAllotmentRequestSystemChecking.CrewSideLetterSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
        
            
        if (ds.Tables[0].Rows.Count > 0)
        {

            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["EMPLOYEEID"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
            txtWageCalculationDate.Text = ds.Tables[0].Rows[0]["FLDWAGECALCULATIONDATE"].ToString();

            gvSideLetter.DataSource = ds.Tables[0];
            gvSideLetter.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSideLetter);
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            gvSLRequest.DataSource = ds.Tables[1];
            gvSLRequest.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[1];
            ShowNoRecordsFound(dt, gvSLRequest);
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            GridView _gv = gvSLRequest;
            DropDownList ddlBankAccount = (DropDownList)_gv.FooterRow.FindControl("ddlBankId");

            if (ddlBankAccount != null)
            {
                ddlBankAccount.Items.Clear();

                ListItem liBank = new ListItem("--Select--", "DUMMY");
                ddlBankAccount.Items.Add(liBank);

                ddlBankAccount.DataSource = ds.Tables[2];
                ddlBankAccount.DataBind();
            }
        }
    }
    protected void gvSideLetter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void gvSLRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            DataSet ds = PhoenixAccountsAllotmentRequestSystemChecking.CrewSideLetterSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList ddlComponent = (DropDownList)e.Row.FindControl("ddlComponent");                
                ddlComponent.DataSource = ds.Tables[0];
                ddlComponent.DataBind();
            }
        }
    }
    protected void gvSLRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gvSLRequest = (GridView)sender;
        if (e.CommandName.ToUpper() == "ADD")
        {
            string componenetid = ((DropDownList)_gvSLRequest.FooterRow.FindControl("ddlComponent")).SelectedValue;
            string bankid    = ((DropDownList)_gvSLRequest.FooterRow.FindControl("ddlBankId")).SelectedValue;
            string amount = ((UserControlMaskNumber)_gvSLRequest.FooterRow.FindControl("txtActualAmountAdd")).Text;

            if (!IsValidSideLetter(componenetid,bankid,amount))
            {
                ucError.Visible = true;
                return;
            }
            else
            {

                PhoenixAccountsAllotmentRequestSystemChecking.AllotmentSideLetterRequestInsert(General.GetNullableGuid(componenetid)
                    , General.GetNullableDateTime(txtWageCalculationDate.Text)
                    , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                    , General.GetNullableDecimal(amount)
                    , General.GetNullableGuid(bankid)
                    , General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
                ucStatus.Visible = true;
                ucStatus.Text = "Allotment Request Added";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                BindData();
            }
        }
    }
    private bool IsValidSideLetter(string componentid,string bankid,string amount)
    {
        ucError.HeaderMessage="Please provide the following required information";
        if (General.GetNullableGuid(componentid) == null)
             ucError.ErrorMessage = "Component Required";
         if(General.GetNullableGuid(bankid)==null)
             ucError.ErrorMessage = "Bank Required";
         if(General.GetNullableDecimal(amount)==null)
             ucError.ErrorMessage = "Amount Required";
         else if(General.GetNullableDecimal(amount) < General.GetNullableDecimal(ViewState["SIDELETTERAMOUNT"].ToString()))
             ucError.ErrorMessage = "Amount Should be less than or equal to " + ViewState["SIDELETTERAMOUNT"].ToString();

        return (!ucError.IsError);
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
}
