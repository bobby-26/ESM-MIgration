using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsProjectGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolmain = new PhoenixToolbar();
        PhoenixToolbar toolbar = new PhoenixToolbar();

        //toolmain.AddButton("List", "PROJECTLIST");

        // imgaccountcode.Attributes.Add("onclick", "return showPickList('spnPickListAccountCode', 'codehelp1', '', '../Common/CommonPickListAccount.aspx', true); ");

        //if (Request.QueryString["id"] == null)
        //{
        //    toolmain.AddButton("Project Code", "PROJECTCODE",ToolBarDirection.Right);

        //toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
        //}
        //else if (Request.QueryString["id"] != null)
        //{
        //    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();           

        //    toolmain.AddButton("Fund Request", "FUNDREQUEST",ToolBarDirection.Right);
        //    toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
        //    toolmain.AddButton("Project Code", "PROJECTCODE", ToolBarDirection.Right);

        //}

        //toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

        Menu.AccessRights = this.ViewState;
        Menu.MenuList = toolmain.Show();
        // Menu.SelectedMenuIndex = 0;

        MenuDirectPO.AccessRights = this.ViewState;
        MenuDirectPO.MenuList = toolbar.Show();
        MenuDirectPO.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "dispaly:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["PROJECTID"] = null;

            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
                EditOrder();
            }
            subaccount();
            PopulateParentProjectcode();
        }
    }

    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("PROJECTCODE"))
                {
                    Response.Redirect("../Accounts/AccountsProjectList.aspx?id=" + ViewState["PROJECTID"].ToString());
                }
                if (CommandName.ToUpper().Equals("LINEITEM"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString(), true);
                }
                if (CommandName.ToUpper().Equals("FUNDREQUEST"))
                {

                }
                //if (dce.CommandName.ToUpper().Equals("PROJECTLIST"))
                //{
                //    Response.Redirect("../Accounts/AccountsProjectList.aspx?id=" + ViewState["PROJECTID"].ToString());
                //}
            }
            else
            {
                if (CommandName.ToUpper().Equals("PROJECTCODE"))
                {
                    Response.Redirect("../Accounts/AccountsProjectList.aspx", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDirectPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string SubaccountList = string.Empty;
                if (!IsValidInvoice(txtTitle.Text.Trim(), ddltype.SelectedQuick, txtAccountId.Text.Trim(), txtBudgetAmount.Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["PROJECTID"] == null || ViewState["PROJECTID"].ToString() == "")
                {

                    PhoenixAccountProject.ProjectInsert(txtTitle.Text.Trim()
                                                        , General.GetNullableInteger(ddltype.SelectedQuick)
                                                        , ddltype.SelectedText
                                                        , General.GetNullableInteger(txtAccountId.Text.Trim())
                                                        , General.GetNullableDecimal(txtBudgetAmount.Text.Trim())
                                                        , int.Parse(chkAllowPOyn.Checked == true ? "1" : "0")
                                                        , int.Parse(chkAccountingVoucher.Checked == true ? "1" : "0")
                                                        , General.ReadCheckBoxList(cblsubAccount)
                                                        , null
                                                        , General.GetNullableInteger(ddlProjectCode.SelectedValue.ToString()));

                    ucStatus.Text = "Project Code Added";
                    Reset();
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    PhoenixAccountProject.ProjectUpdate(txtTitle.Text.Trim()
                                                        , General.GetNullableInteger(ddltype.SelectedQuick)
                                                        , ddltype.SelectedText
                                                        , General.GetNullableInteger(txtAccountId.Text.Trim())
                                                        , General.GetNullableDecimal(txtBudgetAmount.Text.Trim())
                                                        , int.Parse(chkAllowPOyn.Checked == true ? "1" : "0")
                                                        , int.Parse(chkAccountingVoucher.Checked == true ? "1" : "0")
                                                        , General.ReadCheckBoxList(cblsubAccount)
                                                        , int.Parse(ViewState["PROJECTID"].ToString())
                                                        , General.GetNullableInteger(ddlProjectCode.SelectedValue));

                    ucStatus.Text = "Project Code Updated";

                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                //string SubaccountList = string.Empty;
                //if (!IsValidInvoice(txtTitle.Text.Trim(), ddltype.SelectedQuick, txtAccountId.Text.Trim(), txtBudgetAmount.Text.Trim()))
                //{
                //    ucError.Visible = true;
                //    return;
                //}

                //PhoenixAccountProject.ProjectInsert(txtTitle.Text.Trim()
                //                                    , General.GetNullableInteger(ddltype.SelectedQuick)
                //                                    , ddltype.SelectedText
                //                                    , General.GetNullableInteger(txtAccountId.Text.Trim())
                //                                    , General.GetNullableDecimal(txtBudgetAmount.Text.Trim())
                //                                    , int.Parse(chkAllowPOyn.Checked == true ? "1" : "0")
                //                                    , int.Parse(chkAccountingVoucher.Checked == true ? "1" : "0")
                //                                    , General.ReadCheckBoxList(cblsubAccount)
                //                                    , null
                //                                    , General.GetNullableInteger(ddlProjectCode.SelectedValue.ToString()));

                //ucStatus.Text = "Project Code Added";

                //String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                //Session["New"] = "Y";
            }

            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Reset()
    {
        ViewState["PROJECTID"] = null;
        txtTitle.Text = string.Empty;
        txtAccountCode.Text = string.Empty;
        txtAccountDescription.Text = string.Empty;
        txtAccountId.Text = string.Empty;
        txtBudgetAmount.Text = string.Empty;
        txtCreatedBy.Text = string.Empty;
        txtprojectcode.Text = string.Empty;
        chkAccountingVoucher.Checked = false;
        chkAllowPOyn.Checked = false;
        ddltype.SelectedQuick = "";
        cblsubAccount.Items.Clear();
        imgShowAccount.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true);");
        imgShowAccount.Enabled = true;
        ddlProjectCode.SelectedValue = "";
    }
    private void EditOrder()
    {
        if (ViewState["PROJECTID"] != null)
        {
            DataTable dt = PhoenixAccountProject.ProjectEdit(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListAccountCode', 'codehelp1', '', '../Common/CommonPickListAccount.aspx', true); ");
                DataRow dr = dt.Rows[0];
                ViewState["PROJECTID"] = dr["FLDID"].ToString();
                txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                //  txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
                txtBudgetAmount.Text = dr["FLDBUDGETAMOUNT"].ToString();
                subaccount();
                txtCreatedBy.Text = dr["FLDCREATEDBY"].ToString();
                txtprojectcode.Text = dr["FLDPROJECTCODE"].ToString();
                MenuDirectPO.Title = "Project Code (" + dt.Rows[0]["FLDPROJECTCODE"].ToString() + ")";
                txtTitle.Text = dr["FLDTITLE"].ToString();
                ddltype.SelectedQuick = dr["FLDTYPE"].ToString();
                chkAccountingVoucher.Checked = dr["FLDALLOWNEWVOUCHER"].ToString() == "1" ? true : false;
                chkAllowPOyn.Checked = dr["FLDALLOWNEWPO"].ToString() == "1" ? true : false;
                General.BindCheckBoxList(cblsubAccount, dr["FLDSUBACCOUNTLIST"].ToString());
                imgShowAccount.Attributes.Add("onclick", "return false;");
                ddlProjectCode.SelectedValue = dr["FLDPARENTID"].ToString();
            }
        }
    }
    private bool IsValidInvoice(string title, string type, string accountid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (type.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Type is required.";

        if (title == "")
            ucError.ErrorMessage = "Title is required";
        //if (accountid == "")
        //    ucError.ErrorMessage = "Account Code is required";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        subaccount();
    }
    private void subaccount()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        if (txtAccountId.Text != string.Empty)
        {
            ds = PhoenixCommonRegisters.SubAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , General.GetNullableInteger(txtAccountId.Text)
                , General.GetNullableInteger(txtAccountUsage.Text)
                , null
                , null
                , null
                , null
                , 1
                , 1000
                , ref iRowCount
                , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCommonRegisters.SubAccountPOSHList();
        }

        ds.Tables[0].Columns.Add("Acoount", typeof(string), "FLDSUBACCOUNTCODE + ' - ' + FLDDESCRIPTION");

        cblsubAccount.DataTextField = "Acoount";
        cblsubAccount.DataValueField = "FLDBUDGETID";
        cblsubAccount.DataSource = ds;
        cblsubAccount.DataBind();
        // imgaccountcode.Attributes.Add("onclick", "return showPickList('spnPickListAccountCode', 'codehelp1', '', '../Common/CommonPickListAccount.aspx', true); ");
    }
    private void PopulateParentProjectcode()
    {
        ddlProjectCode.DataSource = PhoenixAccountProject.ListParentProjectCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlProjectCode.DataBind();
    }

}

