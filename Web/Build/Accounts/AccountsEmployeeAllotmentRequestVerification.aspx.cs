using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsEmployeeAllotmentRequestVerification : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Checked", "CHECKEDBYACCOUNTS",ToolBarDirection.Right);
            MenuChecking.AccessRights = this.ViewState;
            MenuChecking.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["ALLOTMENTID"] = "";
                ViewState["ALLOTMENTTYPE"] = "";
                ViewState["VESSELID"] = "";
                ViewState["CHECKTYPE"] = "1";
                ViewState["EMPLOYEEID"] = "";
                ViewState["MONTH"] = null;
                ViewState["YEAR"] = null;
                ViewState["VESSELNAME"] = null;
                ViewState["NEWPROCESS"] = "0";
                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                if (Request.QueryString["CHECKTYPE"] != null)
                    ViewState["CHECKTYPE"] = Request.QueryString["CHECKTYPE"].ToString();
                if (Request.QueryString["NEWPROCESS"] != null)
                {
                    ViewState["NEWPROCESS"] = Request.QueryString["NEWPROCESS"].ToString();
                    PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(ViewState["ALLOTMENTID"].ToString()));
                }
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
                ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
                ViewState["VESSELNAME"] = Request.QueryString["VESSELNAME"].ToString();
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
                txtRquestno.Text = dr["FLDREQUESTNUMBER"].ToString();
                txtAllotmentAmount.Text = dr["FLDAMOUNT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PBRebind()
    {
        gvPBBalance.SelectedIndexes.Clear();
        gvPBBalance.EditIndexes.Clear();
        gvPBBalance.DataSource = null;
        gvPBBalance.Rebind();
    }
    protected void Rebind()
    {
        gvCrewBankAccount.SelectedIndexes.Clear();
        gvCrewBankAccount.EditIndexes.Clear();
        gvCrewBankAccount.DataSource = null;
        gvCrewBankAccount.Rebind();
    }
    private void BindData()
    {
        DataTable dtPB = new DataTable();
        dtPB = PhoenixAccountsAllotmentRequestSystemChecking.CrewBPBalanceAndRemarksSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
        gvPBBalance.DataSource = dtPB;
        gvPBBalance.VirtualItemCount = dtPB.Rows.Count;
    }
    private void BindBankData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixAccountsAllotmentRequestSystemChecking.CrewBankDetailsAndRemarksSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
        gvCrewBankAccount.DataSource = dt;
        gvCrewBankAccount.VirtualItemCount = dt.Rows.Count;
    }
    protected void MenuChecking_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper() == "CHECKEDBYACCOUNTS")
            {
                PhoenixAccountsAllotmentRequestSystemChecking.AllotmentRequestCheckedStatusChangeWrap(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()),General.GetNullableInteger(ViewState["NEWPROCESS"].ToString()));
                ucStatus.Visible = true;
                ucStatus.Text = "Status Changed";
                MenuChecking.Visible = false;
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvPBBalance_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null)
            {
                cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);
                cmdSave.Attributes.Add("onclick", "javascript:this.onclick=function(){if (document.getElementById('lblProgress') != null) document.getElementById('lblProgress').value='Processing now. Please Wait!'; return false;};");// this will fix the multiple click problem
            }


        }


    }
    protected void gvPBBalance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = "";
                string remarksid = "";
                string remarks = "";
                string allotmenttype = "";
                string amount = "";
                string vslamount = "";

                dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                remarksid = ((RadLabel)e.Item.FindControl("lblRemarksId")).Text;
                allotmenttype = ((RadLabel)e.Item.FindControl("lblAllotmentType")).Text;
                remarks = ((RadTextBox)e.Item.FindControl("txtBankVerificationRemarks")).Text;
                amount = ((RadLabel)e.Item.FindControl("lblOfficeFinalBal")).Text;
                vslamount = ((RadLabel)e.Item.FindControl("lblVesselFinalBal")).Text;

                if (General.GetNullableString(remarks) == null)
                {
                    ucError.ErrorMessage = "Remarks Required";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(remarksid) == null)
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksInsert(
                           General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()), General.GetNullableGuid(dtkey)
                           , 1, General.GetNullableString(remarks),General.GetNullableInteger(ViewState["NEWPROCESS"].ToString()));
                }
                else
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksUpdate(
                             General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                             , General.GetNullableGuid(remarksid), 1
                             , General.GetNullableString(remarks)
                             , General.GetNullableInteger(ViewState["NEWPROCESS"].ToString()));
                }
                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(ViewState["ALLOTMENTID"].ToString()));
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

                PBRebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPBBalance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvCrewBankAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton Def = (ImageButton)e.Item.FindControl("CmdDefault");
            if (cmdSave != null)
                cmdSave.Attributes.Add("onclick", "javascript:this.onclick=function(){if (document.getElementById('lblProgress') != null) document.getElementById('lblProgress').value='Processing now. Please Wait!'; return false;};");// this will fix the multiple click problem
            if (drv["FLDISDEFAULT"].ToString() == "1")
                Def.Visible = true;
            else
                Def.Visible = false;
            RadLabel lblActiveYN = (RadLabel)e.Item.FindControl("lblActiveYN");

            if (lblActiveYN != null && ed != null)
            {
                if (lblActiveYN.Text == "0")
                    ed.Visible = false;
                else
                    ed.Visible = true;
            }
            CheckBox chkBankId = (CheckBox)e.Item.FindControl("chkBankId");
            if (chkBankId != null)
            {
                if (drv["FLDCHKBANK"].ToString() == "1")
                    chkBankId.Checked = true;
                else
                    chkBankId.Checked = false;
            }
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            img.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            img.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

        }


    }
    protected void gvCrewBankAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = "";
                string remarksid = "";
                string remarks = "";
                dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                remarksid = ((RadLabel)e.Item.FindControl("lblRemarksId")).Text;
                remarks = ((RadTextBox)e.Item.FindControl("txtBankVerificationRemarks")).Text;
                if (General.GetNullableString(remarks) == null)
                {
                    ucError.ErrorMessage = "Remarks Required";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(remarksid) == null)
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksInsert(
                        General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()), new Guid(dtkey), 2, General.GetNullableString(remarks)
                        , General.GetNullableInteger(ViewState["NEWPROCESS"].ToString()));
                }
                else
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksUpdate(
                         General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()), General.GetNullableGuid(remarksid)
                         , 2, General.GetNullableString(remarks)
                         , General.GetNullableInteger(ViewState["NEWPROCESS"].ToString()));
                }

                PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestValidation(new Guid(ViewState["ALLOTMENTID"].ToString()));
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewBankAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindBankData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
