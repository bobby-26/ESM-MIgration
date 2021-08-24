using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaBankInformationList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvBankInformation.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
        toolbar.AddImageButton("../PreSea/PreSeaBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"],
                                                        "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBankInformation')", "Print Grid", "icon_print.png", "PRINT");
        MenuRegistersBankInformation.AccessRights = this.ViewState;    
        MenuRegistersBankInformation.MenuList = toolbar.Show();
        PhoenixToolbar toolbarAddress = new PhoenixToolbar();
        toolbarAddress.AddButton("Bank", "BANK");
        toolbarAddress.AddButton("Address", "ADDRESS");
        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        MenuBankInformation.AccessRights = this.ViewState;
        MenuBankInformation.MenuList = toolbarAddress.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
           
        }
        BindData();
        SetPageNavigator();      
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBANKNAME", "FLDBANKCODE", "FLDBRANCHCODE", "FLDSWIFTCODE", "FLDIBANNUMBER", "FLDBENEFICIARYNAME"};
        string[] alCaptions = { "Bank Name", "Bank Code", "Branch Code", "Swift Code", "IBAN Number", "Beneficiary Name" };
        string[] alColumnsIntermediateBank = { "FLDIBANKNAME", "FLDIBANKCODE", "FLDIBRANCHCODE",
                                               "FLDISWIFTCODE", "FLDIIBANNUMBER" };
        string[] alCaptionsIntermediateBank = { "Bank Name", "Bank Code", "Branch Code", "Swift Code", "IBAN Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixPreSeaBankInformationAddress.BankInformationAddressSearch(0,
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../Registers/RegistersOffice.aspx?addresscode" + ViewState["ADDRESSCODE"]);
        }
 
    }

    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../PreSea/PreSeaOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
        }
    }


    protected void RegistersBankInformation_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
 
            ds = PhoenixPreSeaBankInformationAddress.BankInformationAddressSearch(1,
                null,
                Int32.Parse(ViewState["ADDRESSCODE"].ToString()),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount
            );
            General.SetPrintOptions("gvBankInformation", "Bank Details", alCaptions, alColumns, ds);
           
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBankInformation.DataSource = ds;
            gvBankInformation.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBankInformation);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvBankInformation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidBank(((TextBox)_gridView.FooterRow.FindControl("txtBankNameAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtAccountNumberAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtBranchCodeAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtSwiftCodeAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtIBANNumberAdd")).Text,
                ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyBankAdd")).SelectedCurrency.ToString(),
                ViewState["ADDRESSCODE"].ToString(), ((TextBox)_gridView.FooterRow.FindControl("txtBankCodeAdd")).Text, 
                ((TextBox)_gridView.FooterRow.FindControl("txtBeneficiaryName")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtEmailidAdd")).Text)
                )
            {
                ucError.Visible = true;
                return;
            }

            InsertBankInformationAddress(
                ((TextBox)_gridView.FooterRow.FindControl("txtBankNameAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtBankCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtBranchCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtSwiftCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtIBANNumberAdd")).Text
                ,ViewState["ADDRESSCODE"].ToString()
                ,null
                ,((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyBankAdd")).SelectedCurrency.ToString()
                ,((TextBox)_gridView.FooterRow.FindControl("txtAccountNumberAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtIBankNameAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtIBankCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtIBranchCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtISwiftCodeAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtIIBANNumberAdd")).Text
                ,ViewState["ADDRESSCODE"].ToString()
                ,((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyIBankAdd")).SelectedCurrency.ToString()
                ,((TextBox)_gridView.FooterRow.FindControl("txtIAccountNumberAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtBeneficiaryName")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtContactNameAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtEmailidAdd")).Text
                ,((TextBox)_gridView.FooterRow.FindControl("txtPhoneNumberAdd")).Text
                ,General.GetNullableInteger(((UserControlHard)_gridView.FooterRow.FindControl("ddlSwiftcodetype")).SelectedHard.ToString())
                ,((TextBox)_gridView.FooterRow.FindControl("txtAdditionalbankinfo")).Text
                ,General.GetNullableInteger(((UserControlCountry)_gridView.FooterRow.FindControl("ucBankCountry")).SelectedCountry.ToString())
                ,General.GetNullableInteger(((UserControlHard)_gridView.FooterRow.FindControl("ddlSwiftcodetypeinter")).SelectedHard.ToString())
                ,General.GetNullableInteger(((UserControlCountry)_gridView.FooterRow.FindControl("ucIntermediateBankCountry")).SelectedCountry.ToString())
            );
            BindData();
            ((TextBox)_gridView.FooterRow.FindControl("txtBankNameAdd")).Focus();
          
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {           
            if (!IsValidBank(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankNameEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAccountNumberEdit")).Text,
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBranchCodeEdit")).Text,
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSwiftCodeEdit")).Text,
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIBANNumberEdit")).Text,
                  ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyBankEdit")).SelectedCurrency.ToString(),
                  ViewState["ADDRESSCODE"].ToString(), ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankCodeEdit")).Text,
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBeneficiaryEdit")).Text,
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmailidEdit")).Text
                  ))
            {
                ucError.Visible = true;
                return;
            }
      
            UpdateBankInformationAddress(
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBankidEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankNameEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBranchCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSwiftCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIBANNumberEdit")).Text
                    ,ViewState["ADDRESSCODE"].ToString()
                    ,null
                    ,((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyBankEdit")).SelectedCurrency.ToString()
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAccountNumberEdit")).Text
                    ,((Label)_gridView.Rows[nCurrentRow].FindControl("lbliBankidEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtiBankNameEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtiBankCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtiBranchCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtiSwiftCodeEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIIBANNumberEdit")).Text
                    ,ViewState["ADDRESSCODE"].ToString()
                    ,((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyIBankEdit")).SelectedCurrency.ToString()
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIAccountNumberEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBeneficiaryEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContactNameEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmailidEdit")).Text
                    ,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPhoneNumberEdit")).Text
                    , General.GetNullableInteger(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlSwiftcodetypeedit")).SelectedHard.ToString())
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAdditionalbankinfoEdit")).Text
                    , General.GetNullableInteger(((UserControlCountry)_gridView.Rows[nCurrentRow].FindControl("ucBankCountryedit")).SelectedCountry.ToString())
                    , General.GetNullableInteger(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlSwiftcodetypeinteredit")).SelectedHard.ToString())
                    , General.GetNullableInteger(((UserControlCountry)_gridView.Rows[nCurrentRow].FindControl("ucIntermediateBankCountryedit")).SelectedCountry.ToString())
             );
            _gridView.EditIndex = -1;
            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {

            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
          
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteBankInformationAddress(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblbankid")).Text)
                                        , Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lbliBankid")).Text));
        }
        else
        {
            _gridView.EditIndex = -1;
            BindData();
        }
        SetPageNavigator();
    }

    protected void gvBankInformation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvBankInformation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    private void DeleteBankInformationAddress(int bankid,int ibankid)
    {
        
        PhoenixPreSeaBankInformationAddress.DeleteBankInformationAddress(1, bankid,ibankid);
        
    }
   

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvBankInformation.SelectedIndex = -1;
        gvBankInformation.EditIndex = -1;
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


    private void InsertBankInformationAddress(
         
        string bankname
        ,string bankcode
        ,string branchcode
        ,string swiftcode
        ,string ibannumber
        ,string addresscode
        ,string interbank
        ,string currencyid
        ,string accountno
        ,string ibankname
        ,string ibankcode
        ,string ibranchcode
        ,string iswiftcode
        ,string iibannumber
        ,string iaddresscode
        ,string icurrencyid
        ,string iaccountno
        ,string beneficiaryname
        ,string contactname
        ,string emailid
        ,string phonenumber
        ,int? swiftcodecaption
        ,string additionalbankinfo
        ,int? bankcountrycode
        ,int? iswiftcodecaption
        ,int? ibankcountrycode
        )

        {
       
        PhoenixPreSeaBankInformationAddress.InsertBankInformationAddress(
            1
            , bankname
            , bankcode
            , branchcode
            , swiftcode
            , ibannumber,Convert.ToInt32(addresscode)
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
            );
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
        )
    {
       
        
        PhoenixPreSeaBankInformationAddress.UpdateBankInformationAddress(
            1
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
            );
    }


    private bool IsValidBank(string bankname, string accountno, string branchcode, string swiftcode, string ibannumber, string currencyid, string addresscode, string bankcode, string beneficiaryname, string email)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvBankInformation;
        Int16 result;

        if (beneficiaryname.Trim().Equals(""))
            ucError.ErrorMessage = "Beneficiary Name is required.";
 
        if (bankname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (bankcode.Trim().Length >0)
        {
            if (branchcode.Trim().Equals(""))
                ucError.ErrorMessage = "Branch Code is required.";
        }

        if (swiftcode.Trim().Equals("") && bankcode.Trim().Equals("") )
            ucError.ErrorMessage = "Bank Code Or Swift Code is required.";

        if (ibannumber.Trim().Equals(""))
            ucError.ErrorMessage = "IBAN Number is required.";

        if (accountno.Trim().Equals(""))
            ucError.ErrorMessage = "Account Number is required.";        

        if (currencyid.Trim().Equals("") || !Int16.TryParse(currencyid, out result))
            ucError.ErrorMessage = "Currency is required.";

        if (!email.Trim().Equals(""))
        {
            if (!General.IsvalidEmail(email))
                ucError.ErrorMessage = "In Valid E-Mail.";
        }

        return (!ucError.IsError);
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

    protected void Registersbankinfo_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
      
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
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
        BindData();
        SetPageNavigator();
    }

    protected void gvBankInformation_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete()");
                Label lbl = (Label)e.Row.FindControl("lblbankid");

               
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCurrencyId = (Label)e.Row.FindControl("lblCurrencyId");
            if (lblCurrencyId != null && lblCurrencyId.Text != "")
           {
               UserControlCurrency uc = ((UserControlCurrency)e.Row.FindControl("ucCurrencyBankEdit"));
               uc.SelectedCurrency = lblCurrencyId.Text.ToString();
           }
            Label lblICurrencyId = (Label)e.Row.FindControl("lblICurrencyId");
            if (lblICurrencyId != null && lblICurrencyId.Text != "")
           {
               UserControlCurrency uic = ((UserControlCurrency)e.Row.FindControl("ucCurrencyIBankEdit"));
               uic.SelectedCurrency = lblICurrencyId.Text.ToString();
           }

            Label lblIBankCountryid = (Label)e.Row.FindControl("lblIBankCountryid");
            if (lblIBankCountryid != null && lblIBankCountryid.Text != "")
            {
                UserControlCountry uc = ((UserControlCountry)e.Row.FindControl("ucIntermediateBankCountryEdit"));
                uc.SelectedCountry = lblIBankCountryid.Text.ToString();
            }

            Label lbliswiftcaptioncode = (Label)e.Row.FindControl("lbliswiftcaptioncode");
            if (lbliswiftcaptioncode != null && lbliswiftcaptioncode.Text != "")
            {
                UserControlHard uic = ((UserControlHard)e.Row.FindControl("ddlSwiftcodetypeinteredit"));
                uic.SelectedHard = lbliswiftcaptioncode.Text.ToString();
            }

            Label lblcountryid = (Label)e.Row.FindControl("lblcountryid");
            if (lblcountryid != null && lblcountryid.Text != "")
            {
                UserControlCountry uc = ((UserControlCountry)e.Row.FindControl("ucBankCountryEdit"));
                uc.SelectedCountry = lblcountryid.Text.ToString();
            }

            Label lblswiftcaptioncode = (Label)e.Row.FindControl("lblswiftcaptioncode");
            if (lblswiftcaptioncode != null && lblswiftcaptioncode.Text != "")
            {
                UserControlHard uic = ((UserControlHard)e.Row.FindControl("ddlSwiftcodetypeedit"));
                uic.SelectedHard = lblswiftcaptioncode.Text.ToString();
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

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
    protected void gvbankinformation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument.ToString() != "")
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaBankInformationAddress.DeleteBankInformationAddress(1, 
                     Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblbankid")).Text)
                    ,Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lbliBankid")).Text));
            }
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


    protected void gvBankInformation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SetPageNavigator();
        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        SetPageNavigator();
    }
   
}
