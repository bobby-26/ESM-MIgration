using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsAllotmentRejectionBankAccountChange : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)

            {
                ViewState["ALLOTMENTID"] = "";

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                EditEmployeedetails(ViewState["ALLOTMENTID"].ToString());
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
        DataSet ds = new DataSet();
        ds = PhoenixAccountsAllotmentRemittanceRejection.AllotmentBankChangeList(ViewState["ALLOTMENTID"].ToString());
       
            gvAllotmentBank.DataSource = ds.Tables[0];
           // gvAllotmentBank.DataBind();
       
    }
    private void EditEmployeedetails(string allotmentid)
    {
        try
        {
            DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentRequestEdit(new Guid(ViewState["ALLOTMENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtMonthAndYear.Text = dr["FLDMONTHNAME"].ToString() + "-" + dr["FLDYEAR"].ToString();
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKNAME"].ToString();
                txtallotmentType.Text = dr["FLDALLOTMENTTYPENAME"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                Title1.Text = dr["FLDREQUESTNUMBER"].ToString();
                //txtAllotmentAmount.Text = dr["FLDAMOUNT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentBank_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        UserControlEmployeeBankAccount ddlBankAccount = (UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccountEdit");


        if (e.Item is GridDataItem)
        {
            if (ddlBankAccount != null) ddlBankAccount.SelectedBankAccount = drv["FLDBANKACCOUNTID"].ToString();
        }
        //        if (ddlBankAccount != null) ddlBankAccount.SelectedBankAccount = drv["FLDBANKACCOUNTID"].ToString();
        //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        //        {
        //            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        //            //if (SessionUtil.CanAccess(this.ViewState, "cmdEdit"))
        //            //{
        //            //    if (edit != null) edit.Attributes.Add("style", "visibility:hidden");
        //            //}
        //        }
        //    }
    }
    protected void Rebind()
    {
        gvAllotmentBank.SelectedIndexes.Clear();
        gvAllotmentBank.EditIndexes.Clear();
        gvAllotmentBank.DataSource = null;
        gvAllotmentBank.Rebind();
    }

    protected void gvAllotmentBank_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
      try
        {
            if (e.CommandName.ToString().ToUpper() == "EDIT")
                return;
           else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
               string bank = ((UserControlEmployeeBankAccount)e.Item.FindControl("ddlBankAccountEdit")).SelectedBankAccount;

                PhoenixAccountsAllotmentRemittanceRejection.AllotmentRejectionBankupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["ALLOTMENTID"].ToString())
                    , new Guid(bank));

                ucStatus.Text = "Bank Details Updated Successfully.";

                Rebind();
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                //Script += "</script>" + "\n";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentBank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotmentBank.CurrentPageIndex + 1;
        BindData();
    }
}