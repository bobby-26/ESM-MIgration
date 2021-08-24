using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBankInformationBankIdentifierMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            //toolbarMain.AddButton("Back", "BACK");
            toolbarMain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuBankMain.AccessRights = this.ViewState;
            MenuBankMain.MenuList = toolbarMain.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvQuick.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


                if (Request.QueryString["addresscode"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["addresscode"].ToString();
                }
                if (Request.QueryString["bankid"] != null)
                {
                    ViewState["BANKID"] = Request.QueryString["bankid"].ToString();
                    BankEdit();
                }
            }

            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BankEdit()
    {
        if (General.GetNullableInteger(ViewState["BANKID"].ToString()) != null)
        {
            DataSet ds = new DataSet();
            ds = PhoenixRegistersBankInformationAddress.BankIdentifierEdit(int.Parse(ViewState["BANKID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtBankName.Text = dr["FLDBANKNAME"].ToString();
                txtBeneficiary.Text = dr["FLDBENEFICIARYNAME"].ToString();
                txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
                txtAdditionalInformation1.Text = dr["FLDADDITIONALINFORMATION1"].ToString();
                txtAdditionalInformation2.Text = dr["FLDADDITIONALINFORMATION2"].ToString();
                txtAdditionalInformation3.Text = dr["FLDADDITIONALINFORMATION3"].ToString();
                txtAdditionalInformation4.Text = dr["FLDADDITIONALINFORMATION4"].ToString();
            }


        }
    }
    private void BindData()
    {
        if (General.GetNullableInteger(ViewState["BANKID"].ToString()) != null)
        {
            DataSet ds = new DataSet();
            ds = PhoenixRegistersBankInformationAddress.BankIdentifierList(int.Parse(ViewState["BANKID"].ToString()));

            gvQuick.DataSource = ds;
            gvQuick.VirtualItemCount = ds.Tables[0].Rows.Count;
        }
    }
    private bool IsValidQuick(string bankidentifier, string bankcode, string achbranchcode, string achaccountnumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvQuick;

        if (General.GetNullableInteger(bankidentifier) == null)
            ucError.ErrorMessage = "Bank Identifier is Required.";

        if (General.GetNullableString(bankcode) == null)
            ucError.ErrorMessage = "Bank Code is Required.";
        if (General.GetNullableInteger(bankidentifier) == 1360)
        {
            if (General.GetNullableString(achbranchcode) == null)
                ucError.ErrorMessage = "ACH Branch Code is Required.";

            if (General.GetNullableString(achaccountnumber) == null)
                ucError.ErrorMessage = "ACH Account Number is Required.";
        }

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void ddlSwiftcodetypeedit_TextChanged(object sender, EventArgs e)
    {
        UserControlHard cmboBx = sender as UserControlHard;
        GridFooterItem DataItem = (GridFooterItem)cmboBx.NamingContainer;
        RadTextBox txtACHBranchCodeAdd = (RadTextBox)DataItem.FindControl("txtACHBranchCodeAdd");
        RadTextBox txtACHAccountNumberAdd = (RadTextBox)DataItem.FindControl("txtACHAccountNumberAdd");
        UserControlHard ddlSwiftcodetypeedit = (UserControlHard)DataItem.FindControl("ddlSwiftcodetypeedit");

        if (ddlSwiftcodetypeedit != null && ddlSwiftcodetypeedit.SelectedHard == "1360")
        {
            if (txtACHAccountNumberAdd != null)
                txtACHAccountNumberAdd.CssClass = "input_mandatory";
            if (txtACHBranchCodeAdd != null)
                txtACHBranchCodeAdd.CssClass = "input_mandatory";
        }
        else
        {
            if (txtACHAccountNumberAdd != null)
                txtACHAccountNumberAdd.CssClass = "input";
            if (txtACHBranchCodeAdd != null)
                txtACHBranchCodeAdd.CssClass = "input";
        }
    }
    private bool IsValidDataEdit(int isdefault, int isactive)
    {
        ucError.HeaderMessage = "";

        if (isdefault == 1 && isactive == 0)
        {
            ucError.ErrorMessage = "Default Bank Identifier must be Active.";
        }

        return (!ucError.IsError);
    }
    protected void BankMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["BANKID"].ToString() != "" && ViewState["BANKID"] != null)
            {
                PhoenixRegistersBankInformationAddress.AdditionalBankInformationUpdate(int.Parse(ViewState["BANKID"].ToString())
                                                                                        , txtAdditionalInformation1.Text
                                                                                        , txtAdditionalInformation2.Text
                                                                                        , txtAdditionalInformation3.Text
                                                                                        , txtAdditionalInformation4.Text
                                                                                        );
                BankEdit();
            }

        }
        else if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
        }
    }

    protected void gvQuick_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvQuick_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null && del.Visible == true) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        if (e.Item is GridDataItem)
        {
            if (!e.Item.IsInEditMode)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            DataRowView dv = (DataRowView)e.Item.DataItem;
            CheckBox chkDefault = (CheckBox)e.Item.FindControl("chkDefault");
            CheckBox chkDefaultEdit = (CheckBox)e.Item.FindControl("chkDefaultEdit");
            if (dv["FLDISDEFAULT"].ToString() == "1")
            {
                if (chkDefault != null)
                    chkDefault.Checked = true;
                if (chkDefaultEdit != null)
                    chkDefaultEdit.Checked = true;
            }
            CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");
            CheckBox chkActiveEdit = (CheckBox)e.Item.FindControl("chkActiveEdit");
            if (dv["FLDISACTIVE"].ToString() == "1")
            {
                if (chkActive != null)
                    chkActive.Checked = true;
                if (chkActiveEdit != null)
                    chkActiveEdit.Checked = true;
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

        }

    }

    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                if (!IsValidQuick(((UserControlHard)footer.FindControl("ddlSwiftcodetypeedit")).SelectedHard
                                    , ((RadTextBox)footer.FindControl("txtBankCodeAdd")).Text
                                    , ((RadTextBox)footer.FindControl("txtACHBranchCodeAdd")).Text
                                    , ((RadTextBox)footer.FindControl("txtACHAccountNumberAdd")).Text
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersBankInformationAddress.BankIdentifierInsert(int.Parse(ViewState["BANKID"].ToString())
                                                                        , int.Parse(((UserControlHard)footer.FindControl("ddlSwiftcodetypeedit")).SelectedHard)
                                                                        , ((RadTextBox)footer.FindControl("txtBankCodeAdd")).Text
                                                                        , ((CheckBox)footer.FindControl("chkDefaultAdd")).Checked ? 1 : 0
                                                                        , ((CheckBox)footer.FindControl("chkActiveAdd")).Checked ? 1 : 0
                                                                        , ((RadTextBox)footer.FindControl("txtACHBranchCodeAdd")).Text
                                                                        , ((RadTextBox)footer.FindControl("txtACHAccountNumberAdd")).Text
                                                                        );
                BindData();
                gvQuick.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                gvQuick.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidDataEdit(((CheckBox)item.FindControl("chkDefaultEdit")).Checked ? 1 : 0
                              , ((CheckBox)item.FindControl("chkActiveEdit")).Checked ? 1 : 0
                              ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersBankInformationAddress.BankIdentifierUpdate(new Guid(((RadLabel)item.FindControl("lblBankCodeId")).Text)
                                                                            , int.Parse(ViewState["BANKID"].ToString())
                                                                            , ((CheckBox)item.FindControl("chkDefaultEdit")).Checked ? 1 : 0
                                                                            , ((CheckBox)item.FindControl("chkActiveEdit")).Checked ? 1 : 0
                                                                            );

                gvQuick.EditIndexes.Clear();
                BindData();
                gvQuick.Rebind();
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
