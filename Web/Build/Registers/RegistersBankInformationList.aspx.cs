using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class RegistersBankInformationtionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["hidetoolbar"] = Request.QueryString["toolbar"];
        SessionUtil.PageAccessRights(this.ViewState);
        //   ucConfirm.Visible = false;
        //ucConfirm.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
        toolbar.AddFontAwesomeButton("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBankInformation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuRegistersBankInformation.AccessRights = this.ViewState;
        MenuRegistersBankInformation.MenuList = toolbar.Show();

        PhoenixToolbar toolbarAddress = new PhoenixToolbar();
        toolbarAddress.AddButton("Address", "ADDRESS", ToolBarDirection.Left);
        toolbarAddress.AddButton("Bank", "BANK", ToolBarDirection.Left);
        toolbarAddress.AddButton("Question", "QUESTION", ToolBarDirection.Left);
        toolbarAddress.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);
        toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT", ToolBarDirection.Left);
        toolbarAddress.AddButton("Address Correction", "CORRECTION", ToolBarDirection.Left);
        toolbarAddress.AddButton("Contacts", "CONTACTS", ToolBarDirection.Left);
        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        MenuBankMain.AccessRights = this.ViewState;
        MenuBankMain.MenuList = toolbarAddress.Show();
        MenuBankMain.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvBankInformation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBANKNAME", "FLDBANKCODE", "FLDBRANCHCODE", "FLDSWIFTCODE", "FLDBENEFICIARYNAME" };
        string[] alCaptions = { "Bank Name", "Bank Code", "Branch Code", "Swift Code", "Beneficiary Name" };
        string[] alColumnsIntermediateBank = { "FLDIBANKNAME", "FLDIBANKCODE", "FLDIBRANCHCODE",
                                               "FLDISWIFTCODE"};
        string[] alCaptionsIntermediateBank = { "Bank Name", "Bank Code", "Branch Code", "Swift Code" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersBankInformationAddress.BankInformationAddressSearch(0,
                null,
             Int32.Parse(ViewState["ADDRESSCODE"].ToString()),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BankInformation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank List</h3></td>");
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
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Intermediate Bank List</h3></td>");
        Response.Write("<td colspan='" + (alColumnsIntermediateBank.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptionsIntermediateBank.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptionsIntermediateBank[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumnsIntermediateBank.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumnsIntermediateBank[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void BankMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../Registers/RegistersOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=");
            }
            if (CommandName.ToUpper().Equals("QUESTION"))
            {
                Response.Redirect("../Registers/RegistersAddressQuestion.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Registers/RegistersAddressAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("AGREEMENTSATTACHMENT"))
            {
                Response.Redirect("../Registers/RegistersAgreementsAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CORRECTION"))
            {
                Response.Redirect("../Registers/RegistersAddressCorrection.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersSupplierInvoiceApprovalUserMap.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("CONTACTS"))
            {
                Response.Redirect("../Registers/RegistersAddressPurpose.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    //{

    //    //foreach (GridViewRow gvr in gvBankInformation.Rows)
    //    //{
    //    //    Label lbldtkey = ((Label)gvr.FindControl("lbldtkey") == null ? null : (Label)gvr.FindControl("lbldtkey"));
    //    //    ViewState["DTKey"] = lbldtkey.Text;
    //    //}


    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    if (dce.CommandName.ToUpper().Equals("ADDRESS"))
    //    {
    //        Response.Redirect("../Registers/RegistersOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
    //    }
    //}

    protected void RegistersBankInformation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBANKNAME", "FLDBANKCODE", "FLDBRANCHCODE", "FLDSWIFTCODE", "FLDIBANNUMBER", "FLDBENEFICIARYNAME" };
        string[] alCaptions = { "Bank Name", "Bank Code", "Branch Code", "Swift Code", "IBAN Number", "Beneficiary Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersBankInformationAddress.BankInformationAddressSearch(1,
            null,
            Int32.Parse(ViewState["ADDRESSCODE"].ToString()),
           (int)ViewState["PAGENUMBER"],
            gvBankInformation.PageSize,
            ref iRowCount,
            ref iTotalPageCount
        );
        General.SetPrintOptions("gvBankInformation", "Bank Details", alCaptions, alColumns, ds);
        gvBankInformation.DataSource = ds;
        gvBankInformation.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void DeleteBankInformationAddress(int bankid, int ibankid)
    {
        try
        {
            PhoenixRegistersBankInformationAddress.DeleteBankInformationAddress(1, bankid, ibankid);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void InsertBankInformationAddress(

        string bankname
        , string bankcode
        , string branchcode
        , string swiftcode
        , string ibannumber
        , string addresscode
        , string interbank
        , string currencyid
        , string accountno
        , string ibankname
        , string ibankcode
        , string ibranchcode
        , string iswiftcode
        , string iibannumber
        , string iaddresscode
        , string icurrencyid
        , string iaccountno
        , string beneficiaryname
        , string contactname
        , string emailid
        , string phonenumber
        , int? swiftcodecaption
        , string additionalbankinfo
        , int? bankcountrycode
        , int? iswiftcodecaption
        , int? ibankcountrycode
        , int? activeyn
        , string remarks
        , int? SupplierBankCharges
        )
    {
        PhoenixRegistersBankInformationAddress.InsertBankInformationAddress(
     PhoenixSecurityContext.CurrentSecurityContext.UserCode
     , bankname
     , bankcode
     , branchcode
     , swiftcode
     , ibannumber, Convert.ToInt32(addresscode)
     , null
     , Convert.ToInt32(currencyid)
     , accountno
     , ibankname
     , ibankcode
     , ibranchcode
     , iswiftcode
     , iibannumber
     , General.GetNullableInteger(iaddresscode)
     , General.GetNullableInteger(icurrencyid)
     , iaccountno
     , beneficiaryname
     , contactname
     , emailid
     , phonenumber
     , swiftcodecaption
     , additionalbankinfo
     , bankcountrycode
     , iswiftcodecaption
     , ibankcountrycode
     , activeyn
     , remarks
     , SupplierBankCharges
     );
    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {

            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;
            //if (ucCM.confirmboxvalue == 1)
            //{
            if (ViewState["NROW"] != null && ViewState["NROW"].ToString() != "")
            {
                int nRow = int.Parse(ViewState["NROW"].ToString());

                //Label lblLongTermActionId = ((Label)gvShipBoardTasks.Rows[nRow].FindControl("lblLongTermActionId"));
                if (int.Parse(ViewState["ACCOUNTNOFLAG"].ToString()) == 1)
                {
                    GridFooterItem footerItem = (GridFooterItem)gvBankInformation.MasterTableView.GetItems(GridItemType.Footer)[0];

                    InsertBankInformationAddress(
                    ((RadTextBox)footerItem.FindControl("txtBankNameAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtBankCodeAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtBranchCodeAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtSwiftCodeAdd")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , null
                    , ((RadComboBox)footerItem.FindControl("ucCurrencyBankAdd")).SelectedValue.ToString()
                    , ((RadTextBox)footerItem.FindControl("txtAccountNumberAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtIBankNameAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtIBankCodeAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtIBranchCodeAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtISwiftCodeAdd")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , ((UserControlCurrency)footerItem.FindControl("ucCurrencyIBankAdd")).SelectedCurrency.ToString()
                    , ((RadTextBox)footerItem.FindControl("txtIAccountNumberAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtBeneficiaryName")).Text
                    , ((RadTextBox)footerItem.FindControl("txtContactNameAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtEmailidAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txtPhoneNumberAdd")).Text
                    , General.GetNullableInteger(((UserControlHard)footerItem.FindControl("ddlSwiftcodetype")).SelectedHard.ToString())
                    , ((RadTextBox)footerItem.FindControl("txtAdditionalbankinfo")).Text
                    , General.GetNullableInteger(((UserControlCountry)footerItem.FindControl("ucBankCountry")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((UserControlHard)footerItem.FindControl("ddlSwiftcodetypeinter")).SelectedHard.ToString())
                    , General.GetNullableInteger(((UserControlCountry)footerItem.FindControl("ucIntermediateBankCountry")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((CheckBox)footerItem.FindControl("chkActiveYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(((RadTextBox)footerItem.FindControl("txtRemarksAdd")).Text)
                    , General.GetNullableInteger(((UserControlHard)footerItem.FindControl("ddlBankAdd")).SelectedHard.ToString())

                    );
                    Rebind();
                }
              else  if (int.Parse(ViewState["ACCOUNTNOFLAG"].ToString()) == 2)
                {
                    GridEditableItem Item = (GridEditableItem)gvBankInformation.MasterTableView.GetItems(GridItemType.EditItem)[0];

                    UpdateBankInformationAddress(
                    ((RadLabel)Item.FindControl("lblBankidEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtBankNameEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtBankCodeEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtBranchCodeEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtSwiftCodeEdit")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , null
                    , ((RadComboBox)Item.FindControl("ucCurrencyBankEdit")).SelectedValue.ToString()
                    , ((RadTextBox)Item.FindControl("txtAccountNumberEdit")).Text
                    , ((RadLabel)Item.FindControl("lbliBankidEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtiBankNameEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtiBankCodeEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtiBranchCodeEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtiSwiftCodeEdit")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , ((UserControlCurrency)Item.FindControl("ucCurrencyIBankEdit")).SelectedCurrency.ToString()
                    , ((RadTextBox)Item.FindControl("txtIAccountNumberEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtBeneficiaryEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtContactNameEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtEmailidEdit")).Text
                    , ((RadTextBox)Item.FindControl("txtPhoneNumberEdit")).Text
                    , General.GetNullableInteger(((UserControlHard)Item.FindControl("ddlSwiftcodetypeedit")).SelectedHard.ToString())
                    , null
                    , General.GetNullableInteger(((UserControlCountry)Item.FindControl("ucBankCountryedit")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((UserControlHard)Item.FindControl("ddlSwiftcodetypeinteredit")).SelectedHard.ToString())
                    , General.GetNullableInteger(((UserControlCountry)Item.FindControl("ucIntermediateBankCountryedit")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((CheckBox)Item.FindControl("chkActiveYN")).Checked ? "1" : "0")
                    , General.GetNullableString(((RadTextBox)Item.FindControl("txtRemarks")).Text)
                    , General.GetNullableInteger(((UserControlHard)Item.FindControl("ddlbankEdit")).SelectedHard.ToString())
               );
                    Rebind();
                }
                //else
                //{
                //    GridDataItem Item = (GridDataItem)gvBankInformation.MasterTableView.GetItems(GridItemType.EditItem)[0];
                //    Rebind();
                //}
            }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateBankInformationAddress(
        string bankid
        , string bankname
        , string bankcode
        , string branchcode
        , string swiftcode
        , string ibannumber
        , string addresscode
        , string interbank
        , string currencyid
        , string accountno
        , string ibankid
        , string ibankname
        , string ibankcode
        , string ibranchcode
        , string iswiftcode
        , string iibannumber
        , string iaddresscode
        , string icurrencyid
        , string iaccountno
        , string beneficiaryname
        , string contactname
        , string emailid
        , string phonenumber
        , int? swiftcodecaption
        , string additionalbankinfo
        , int? bankcountrycode
        , int? iswiftcodecaption
        , int? ibankcountrycode
        , int? activeyn
        , string remarks
        , int? SupplierBankCharges
        )
    {

        try
        {
            PhoenixRegistersBankInformationAddress.UpdateBankInformationAddress(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(bankid)
                , bankname, bankcode
                , branchcode
                , swiftcode
                , ibannumber
                , Convert.ToInt32(addresscode)
                , null
                , Convert.ToInt32(currencyid)
                , accountno
                , Convert.ToInt32(ibankid)
                , ibankname
                , ibankcode
                , ibranchcode
                , iswiftcode
                , iibannumber
                , General.GetNullableInteger(iaddresscode)
                , General.GetNullableInteger(icurrencyid)
                , iaccountno
                , beneficiaryname
                , contactname
                , emailid
                , phonenumber
                , swiftcodecaption
                , additionalbankinfo
                , bankcountrycode
                , iswiftcodecaption
                , ibankcountrycode
                , activeyn
                , remarks
                , SupplierBankCharges
                );
        }

        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }


    }


    private bool IsValidBank(string bankname, string accountno, string branchcode, string swiftcode, string ibannumber, string currencyid,
        string addresscode, string bankcode, string beneficiaryname, string email, int activeyn, string remarks, string countryid, string Swiftcodetype, string Iswiftcode, string IswiftcodeType, string ibankname, string iaccountno, string ibranchcode, string ibankcode, string bankadditionalinfo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvBankInformation;
        Int16 result;
        Regex r = new Regex(@"[~`!@#$%^&*()\-\+=|\{}':;,<>/?]");
        Regex regex = new Regex(@"\s");

        if (beneficiaryname.Trim().Equals(""))
            ucError.ErrorMessage = "Beneficiary Name is required.";

        if (bankname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (bankcode.Trim().Length > 0)
        {
            if (branchcode.Trim().Equals(""))
                ucError.ErrorMessage = "Branch Code is required.";
        }

        if (swiftcode.Trim().Equals("") && bankcode.Trim().Equals(""))
            ucError.ErrorMessage = "Bank Code Or Swift Code is required.";

        if (Swiftcodetype == "1154" && swiftcode.Trim().Equals(""))
            ucError.ErrorMessage = "Swift/BIC address Missing.";

        else if (Swiftcodetype == "1154" && swiftcode != "")
            if (!IsValidTextBox(swiftcode.Trim()))
                ucError.ErrorMessage = " Swift/BIC Address not completed.";

        if (IswiftcodeType == "1154" && Iswiftcode != "")
            if (!IsValidTextBox(Iswiftcode.Trim()))
                ucError.ErrorMessage = " Intermediate Swift/BIC Address not completed.";

        //if (ibannumber.Trim().Equals(""))
        //    ucError.ErrorMessage = "IBAN Number is required.";

        if (accountno.Trim().Equals(""))
            ucError.ErrorMessage = "Account Number is required.";

        if (currencyid.Trim().Equals("") || !Int16.TryParse(currencyid, out result))
            ucError.ErrorMessage = "Currency is required.";


        if (General.GetNullableString(email) == null)
            ucError.ErrorMessage = "Email is required.";


        if (!email.Trim().Equals(""))
        {
            if (!General.IsvalidEmail(email))
                ucError.ErrorMessage = "In Valid E-Mail.";
        }

        if (activeyn == 1)
        {
            if (General.GetNullableString(remarks) == null)
                ucError.ErrorMessage = "Remarks Required.";
        }

        if (countryid.Trim().Equals("") || countryid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Bank Country is required";

        if (r.IsMatch(beneficiaryname.Trim()) || r.IsMatch(bankname.Trim()) || r.IsMatch(branchcode.Trim()) || r.IsMatch(bankcode.Trim()) || r.IsMatch(branchcode.Trim()) || r.IsMatch(swiftcode.Trim())
            || r.IsMatch(Iswiftcode.Trim()) || r.IsMatch(accountno.Trim()) || r.IsMatch(ibankname.Trim()) || r.IsMatch(iaccountno.Trim()) || r.IsMatch(ibranchcode.Trim()) || r.IsMatch(ibankcode.Trim()) || r.IsMatch(bankadditionalinfo.Trim()))
        {
            ucError.ErrorMessage = "Contains Special characters.. Unable to add.";
        }

        if (accountno.Contains(" "))
            ucError.ErrorMessage = "Spacing in Account Number.. Unable to add.";

        return (!ucError.IsError);
    }
    protected void Registersbankinfo_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    public static bool IsValidTextBox(string text)
    {
        string regex = "^[0-9a-zA-Z ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);
        if (text.Length != 11)
            return (false);

        return true;
    }


    protected void gvBankInformation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBankInformation.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBankInformation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int? n = General.GetNullableInteger(drv["FLDACTIVEYN"].ToString());
            RadTextBox lblEmailid = (RadTextBox)e.Item.FindControl("lblEmailid");
            if (lblEmailid != null) lblEmailid.Text = @drv["FLDEMAILID"].ToString();
            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            if (!e.Item.IsInEditMode)
            {
                LinkButton dba = (LinkButton)e.Item.FindControl("cmdDelete");
                dba.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                RadLabel lbla = (RadLabel)e.Item.FindControl("lblbankid");

            }
            CheckBox chkitem = (CheckBox)e.Item.FindControl("chkActiveYNItem");
            if (chkitem != null)
            {
                chkitem.Enabled = false;
                if (n == 1)
                    chkitem.Checked = true;

            }
            CheckBox chk = (CheckBox)e.Item.FindControl("chkActiveYN");
            if (chk != null)
            {
                if (n == 1)
                    chk.Checked = true;
            }


            RadLabel lblCurrencyId = (RadLabel)e.Item.FindControl("lblCurrencyId");
            if (lblCurrencyId != null && lblCurrencyId.Text != "")
            {
                RadComboBox uc = ((RadComboBox)e.Item.FindControl("ucCurrencyBankEdit"));
                uc.SelectedValue = lblCurrencyId.Text.ToString();
            }
            RadLabel lblICurrencyId = (RadLabel)e.Item.FindControl("lblICurrencyId");
            if (lblICurrencyId != null && lblICurrencyId.Text != "")
            {
                UserControlCurrency uic = ((UserControlCurrency)e.Item.FindControl("ucCurrencyIBankEdit"));
                uic.SelectedCurrency = lblICurrencyId.Text.ToString();
            }

            RadLabel lblIBankCountryid = (RadLabel)e.Item.FindControl("lblIBankCountryid");
            if (lblIBankCountryid != null && lblIBankCountryid.Text != "")
            {
                UserControlCountry uc = ((UserControlCountry)e.Item.FindControl("ucIntermediateBankCountryEdit"));
                uc.SelectedCountry = lblIBankCountryid.Text.ToString();
            }
            //RadLabel lblbankinfoEdit = (RadLabel)e.Item.FindControl("lblbankinfoEdit");
            //if (lblbankinfoEdit != null && lblbankinfoEdit.Text != "")
            //{
            //    UserControlHard uc = ((UserControlHard)e.Item.FindControl("ddlinforEdit"));
            //    uc.SelectedHard = lblbankinfoEdit.Text.ToString();
            //}
            RadLabel lbliswiftcaptioncode = (RadLabel)e.Item.FindControl("lbliswiftcaptioncode");
            if (lbliswiftcaptioncode != null && lbliswiftcaptioncode.Text != "")
            {
                UserControlHard uic = ((UserControlHard)e.Item.FindControl("ddlSwiftcodetypeinteredit"));
                uic.SelectedHard = lbliswiftcaptioncode.Text.ToString();
            }

            RadLabel lblcountryid = (RadLabel)e.Item.FindControl("lblcountryid");
            if (lblcountryid != null && lblcountryid.Text != "")
            {
                UserControlCountry uc = ((UserControlCountry)e.Item.FindControl("ucBankCountryEdit"));
                uc.SelectedCountry = lblcountryid.Text.ToString();
            }
            RadLabel lbleditbankcharges = (RadLabel)e.Item.FindControl("lbleditbankcharges");
            if (lbleditbankcharges != null && lbleditbankcharges.Text != "")
            {
                UserControlHard us = ((UserControlHard)e.Item.FindControl("ddlbankEdit"));
                us.SelectedHard = lbleditbankcharges.Text.ToString();
            }
            LinkButton BankIdentifier = (LinkButton)e.Item.FindControl("cmdBankIdentifier");
            RadLabel lblbankid = (RadLabel)e.Item.FindControl("lblBankid");
            if (BankIdentifier != null)
            {
                BankIdentifier.Attributes.Add("onclick", "openNewWindow('BANKIDENTIFIER', '', '" + Session["sitepath"] + "/Registers/RegistersBankInformationBankIdentifierMapping.aspx?bankid=" + lblbankid.Text + "&addresscode=" + ViewState["ADDRESSCODE"] + "'); return false;");
            }
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAdditionalbankinformation");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblAdditionalbankinformation");
            if (lbl != null && uct != null)
            {
                lbl.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lbl.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
            if (lbl != null)
            {
                UserControlToolTip ucToolTipAdditionalbankinformation = (UserControlToolTip)e.Item.FindControl("ucToolTipAdditionalbankinformation");
                ucToolTipAdditionalbankinformation.Position = ToolTipPosition.TopCenter;
                ucToolTipAdditionalbankinformation.TargetControlId = lbl.ClientID;
            }
            if (lblEmailid != null)
            {
                UserControlToolTip ToolTip1 = (UserControlToolTip)e.Item.FindControl("ToolTip1");
                ToolTip1.Position = ToolTipPosition.TopCenter;
                ToolTip1.TargetControlId = lblEmailid.ClientID;
            }
            if (lblRemarks != null)
            {
                UserControlToolTip ToolTip2 = (UserControlToolTip)e.Item.FindControl("ToolTip2");
                ToolTip2.Position = ToolTipPosition.TopCenter;
                ToolTip2.TargetControlId = lblRemarks.ClientID;
            }
            RadLabel lblswiftcaptioncode = (RadLabel)e.Item.FindControl("lblswiftcaptioncode");
            if (lblswiftcaptioncode != null && lblswiftcaptioncode.Text != "")
            {
                UserControlHard uic = ((UserControlHard)e.Item.FindControl("ddlSwiftcodetypeedit"));
                uic.SelectedHard = lblswiftcaptioncode.Text.ToString();
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lbldtkey");
            ImageButton noAtt = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAttachment");

            if (drv["FLDATTACHMENTCOUNT"].ToString() != "0")
            {
                if (att != null)
                {
                    att.Visible = true;
                    noAtt.Visible = false;
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersBankInformationAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.REGISTERS + "&cmdname=UPLOAD');return true;");
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                }
            }
            else
            {

                if (noAtt != null)
                {
                    att.Visible = false;
                    noAtt.Visible = true;
                    noAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersBankInformationAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                             + PhoenixModule.REGISTERS + "&cmdname=UPLOAD');return true;");
                    noAtt.Visible = SessionUtil.CanAccess(this.ViewState, noAtt.CommandName);
                }
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
    protected void Rebind()
    {
        gvBankInformation.SelectedIndexes.Clear();
        gvBankInformation.EditIndexes.Clear();
        gvBankInformation.DataSource = null;
        gvBankInformation.Rebind();
    }

   
    protected void gvBankInformation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            ViewState["NROW"] = nCurrentRow.ToString();


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidBank(((RadTextBox)e.Item.FindControl("txtBankNameAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtAccountNumberAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtBranchCodeAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtSwiftCodeAdd")).Text,
                     "",
                     ((RadComboBox)e.Item.FindControl("ucCurrencyBankAdd")).SelectedValue.ToString(),
                     ViewState["ADDRESSCODE"].ToString(), ((RadTextBox)e.Item.FindControl("txtBankCodeAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtBeneficiaryName")).Text,
                     ((RadTextBox)e.Item.FindControl("txtEmailidAdd")).Text,
                     ((CheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked ? 1 : 0,
                     ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text,
                     ((UserControlCountry)e.Item.FindControl("ucBankCountry")).SelectedCountry,
                     ((UserControlHard)e.Item.FindControl("ddlSwiftcodetype")).SelectedHard,
                       ((RadTextBox)e.Item.FindControl("txtISwiftCodeAdd")).Text,
                       ((UserControlHard)e.Item.FindControl("ddlSwiftcodetypeinter")).SelectedHard,
                       ((RadTextBox)e.Item.FindControl("txtIBankNameAdd")).Text,
                       ((RadTextBox)e.Item.FindControl("txtIAccountNumberAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtIBranchCodeAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtIBankCodeAdd")).Text,
                      ((RadTextBox)e.Item.FindControl("txtAdditionalbankinfo")).Text
                     )
                     )
                {
                    ucError.Visible = true;
                    return;
                }

                try
                {
                    PhoenixRegistersBankInformationAddress.BankAccountValidate(((RadTextBox)e.Item.FindControl("txtAccountNumberAdd")).Text);
                }
                catch (Exception ex)
                {

                    ViewState["ACCOUNTNOFLAG"] = 1;
                    ucConfirm.Visible = true;
                    RadWindowManager1.RadConfirm(ex.Message, "ucConfirm", 320, 150, null, "Confirm");
                    return;

                }

                InsertBankInformationAddress(
                    ((RadTextBox)e.Item.FindControl("txtBankNameAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtBankCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtBranchCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSwiftCodeAdd")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , null
                    , ((RadComboBox)e.Item.FindControl("ucCurrencyBankAdd")).SelectedValue.ToString()
                    , ((RadTextBox)e.Item.FindControl("txtAccountNumberAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtIBankNameAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtIBankCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtIBranchCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtISwiftCodeAdd")).Text
                    , ""
                    , ViewState["ADDRESSCODE"].ToString()
                    , ((UserControlCurrency)e.Item.FindControl("ucCurrencyIBankAdd")).SelectedCurrency.ToString()
                    , ((RadTextBox)e.Item.FindControl("txtIAccountNumberAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtBeneficiaryName")).Text
                    , ((RadTextBox)e.Item.FindControl("txtContactNameAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtEmailidAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtPhoneNumberAdd")).Text
                    , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ddlSwiftcodetype")).SelectedHard.ToString())
                    , ((RadTextBox)e.Item.FindControl("txtAdditionalbankinfo")).Text
                    , General.GetNullableInteger(((UserControlCountry)e.Item.FindControl("ucBankCountry")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ddlSwiftcodetypeinter")).SelectedHard.ToString())
                    , General.GetNullableInteger(((UserControlCountry)e.Item.FindControl("ucIntermediateBankCountry")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text)
                    , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ddlBankAdd")).SelectedHard.ToString())
                    
                    );
                Rebind();

                ((RadTextBox)e.Item.FindControl("txtBankNameAdd")).Focus();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidBank(((RadTextBox)item.FindControl("txtBankNameEdit")).Text,
                    ((RadTextBox)item.FindControl("txtAccountNumberEdit")).Text,
                      ((RadTextBox)item.FindControl("txtBranchCodeEdit")).Text,
                      ((RadTextBox)item.FindControl("txtSwiftCodeEdit")).Text,
                      "",
                      ((RadComboBox)item.FindControl("ucCurrencyBankEdit")).SelectedValue.ToString(),
                      ViewState["ADDRESSCODE"].ToString(), ((RadTextBox)item.FindControl("txtBankCodeEdit")).Text,
                      ((RadTextBox)item.FindControl("txtBeneficiaryEdit")).Text,
                      ((RadTextBox)item.FindControl("txtEmailidEdit")).Text,
                      ((CheckBox)item.FindControl("chkActiveYN")).Checked ? 1 : 0,
                      ((RadTextBox)item.FindControl("txtRemarks")).Text,
                      ((UserControlCountry)item.FindControl("ucBankCountryEdit")).SelectedCountry,
                      ((UserControlHard)item.FindControl("ddlSwiftcodetypeedit")).SelectedHard,
                      ((RadTextBox)item.FindControl("txtISwiftCodeEdit")).Text,
                      ((UserControlHard)item.FindControl("ddlSwiftcodetypeinteredit")).SelectedHard,
                      ((RadTextBox)e.Item.FindControl("txtIBankNameEdit")).Text,
                       ((RadTextBox)e.Item.FindControl("txtIAccountNumberEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtIBranchCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtIBankCodeEdit")).Text,
                     ((UserControlHard)item.FindControl("ddlbankEdit")).SelectedHard

                      ))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }

                try
                {
                    PhoenixRegistersBankInformationAddress.BankAccountValidateupdate(int.Parse(((RadLabel)item.FindControl("lblBankidEdit")).Text), ((RadTextBox)item.FindControl("txtAccountNumberEdit")).Text);
                }
                catch (Exception ex)
                {
                    ViewState["ACCOUNTNOFLAG"] = 2;
                    e.Canceled = true;
                    ucConfirm.Visible = true;
                    RadWindowManager1.RadConfirm(ex.Message, "ucConfirm", 320, 150, null, "Confirm");
                    return;

                }

                UpdateBankInformationAddress(
                        ((RadLabel)item.FindControl("lblBankidEdit")).Text
                        , ((RadTextBox)item.FindControl("txtBankNameEdit")).Text
                        , ((RadTextBox)item.FindControl("txtBankCodeEdit")).Text
                        , ((RadTextBox)item.FindControl("txtBranchCodeEdit")).Text
                        , ((RadTextBox)item.FindControl("txtSwiftCodeEdit")).Text
                        , ""
                        , ViewState["ADDRESSCODE"].ToString()
                        , null
                        , ((RadComboBox)item.FindControl("ucCurrencyBankEdit")).SelectedValue.ToString()
                        , ((RadTextBox)item.FindControl("txtAccountNumberEdit")).Text
                        , ((RadLabel)item.FindControl("lbliBankidEdit")).Text
                        , ((RadTextBox)item.FindControl("txtiBankNameEdit")).Text
                        , ((RadTextBox)item.FindControl("txtiBankCodeEdit")).Text
                        , ((RadTextBox)item.FindControl("txtiBranchCodeEdit")).Text
                        , ((RadTextBox)item.FindControl("txtiSwiftCodeEdit")).Text
                        , ""
                        , ViewState["ADDRESSCODE"].ToString()
                        , ((UserControlCurrency)item.FindControl("ucCurrencyIBankEdit")).SelectedCurrency.ToString()
                        , ((RadTextBox)item.FindControl("txtIAccountNumberEdit")).Text
                        , ((RadTextBox)item.FindControl("txtBeneficiaryEdit")).Text
                        , ((RadTextBox)item.FindControl("txtContactNameEdit")).Text
                        , ((RadTextBox)item.FindControl("txtEmailidEdit")).Text
                        , ((RadTextBox)item.FindControl("txtPhoneNumberEdit")).Text
                        , General.GetNullableInteger(((UserControlHard)item.FindControl("ddlSwiftcodetypeedit")).SelectedHard.ToString())
                        , null
                        , General.GetNullableInteger(((UserControlCountry)item.FindControl("ucBankCountryedit")).SelectedCountry.ToString())
                        , General.GetNullableInteger(((UserControlHard)item.FindControl("ddlSwiftcodetypeinteredit")).SelectedHard.ToString())
                        , General.GetNullableInteger(((UserControlCountry)item.FindControl("ucIntermediateBankCountryedit")).SelectedCountry.ToString())
                        , General.GetNullableInteger(((CheckBox)item.FindControl("chkActiveYN")).Checked ? "1" : "0")
                        , General.GetNullableString(((RadTextBox)item.FindControl("txtRemarks")).Text)
                        , General.GetNullableInteger(((UserControlHard)item.FindControl("ddlbankEdit")).SelectedHard.ToString())
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteBankInformationAddress(Int32.Parse(((RadLabel)e.Item.FindControl("lblbankid")).Text)
                                            , Int32.Parse(((RadLabel)e.Item.FindControl("lbliBankid")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("BANKIDENTIFIER"))
            {
                RadLabel bankid = ((RadLabel)e.Item.FindControl("lblbankid"));

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
