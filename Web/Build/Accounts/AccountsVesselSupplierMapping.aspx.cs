using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Purchase;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselSupplierMapping : PhoenixBasePage
{
    string strorderid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            txtVenderID.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
          
            toolbar.AddButton("Commitment", "COMMITMENT", ToolBarDirection.Right);
            toolbar.AddButton("Credit Purchase", "CREDITPURCHASE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            MenuGeneral.SelectedMenuIndex = 0;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselSupplierMapping.aspx?OrderId=" + Request.QueryString["OrderId"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderPartPaid')", "Print Grid", "icon_print.png", "PRINT");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
          
            toolbarsub.AddButton("Commit PO", "COMMIT", ToolBarDirection.Right);
            toolbarsub.AddButton("Supplier Mapping", "SUPPLIERMAPPING", ToolBarDirection.Right);
            MenuGeneralSub.AccessRights = this.ViewState;
            MenuGeneralSub.MenuList = toolbarsub.Show();

            MenuGeneralSub.SelectedMenuIndex = 1;

            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                strorderid = Request.QueryString["orderid"].ToString();
            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }
            else
                ViewState["vesselid"] = "0";

            if (Request.QueryString["vesselsupplierid"] != null && !string.IsNullOrEmpty(Request.QueryString["vesselsupplierid"].ToString()))
            {
                ViewState["VESSELSUPPLIERID"] = Request.QueryString["vesselsupplierid"];
            }
            else
                ViewState["VESSELSUPPLIERID"] = "";

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ADDRESSCODE"] = null;
                setSupplierDetails();
                gvOrderPartPaid.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvOrderPartPaid.SelectedIndexes.Clear();
        gvOrderPartPaid.EditIndexes.Clear();
        gvOrderPartPaid.DataSource = null;
        gvOrderPartPaid.Rebind();
    }

    protected void MenuGeneralSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUPPLIERMAPPING"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMIT"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseAdvancePayment.aspx?orderid=" + ViewState["orderid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&vesselsupplierid=" + ViewState["VESSELSUPPLIERID"].ToString());

                //String scriptpopup = String.Format(
                //    "javascript:parent.Openpopup('codehelp1', '', 'AccountsCreditPurchaseAdvancePayment.aspx?orderid=" + ViewState["orderid"].ToString() + "');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        DataTable dtorder = PhoenixAccountsVesselAccounting.EditOrderForm(new Guid(strorderid));

        if (dtorder.Rows.Count > 0)
        {
            txtOrderNumber.Text = dtorder.Rows[0]["FLDREFERENCENO"].ToString(); ;
        }

        Guid orderid = new Guid(strorderid);

        DataSet ds = PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentSearch(orderid, sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvOrderPartPaid.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

      
        General.SetPrintOptions("gvOrderPartPaid", "Part Paid", alCaptions, alColumns, ds);
        gvOrderPartPaid.DataSource = ds;
        gvOrderPartPaid.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT","FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Voucher Number", "Voucher Date" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        Guid orderid = new Guid(strorderid);

        DataSet ds = PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentSearch(orderid, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CreditPurchaseAdvancePayment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Credit Purchase Advance Payment</h3></td>");
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
        Response.End();
    }

    private bool IsValidAdvancePayment(string amount, string description)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    protected void gvOrderPartPaid_Sorting(object sender, GridViewSortEventArgs se)
    {
       
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private bool IsValidOrderPartPaid(string amount, string description,string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (currency.Trim().Equals("") || currency.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Currency is required";

        return (!ucError.IsError);
    }

    private void DeleteOrderPartPaid(Guid orderpartpaidid)
    {
        PhoenixPurchaseOrderPartPaid.DeleteOrderPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid);
    }

    private void ApprovedRequestAdvance(Guid orderpartpaidid,string vesselid,string currencyid)
    {
        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        DataSet ds = PhoenixAccountsVesselAccounting.CreditPurchaseApprove(new Guid(strorderid),orderpartpaidid,int.Parse(vesselid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(dr["FLDMAPPEDSUPPLIER"].ToString())
                , decimal.Parse(dr["FLDPARTPAIDAMOUNT"].ToString())
                , DateTime.Parse(DateTime.Now.ToString())
                , int.Parse(currencyid)
                , null
                , txtOrderNumber.Text
                , null
                , new Guid(strorderid)
                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 124, "VCP")),
                 PhoenixSecurityContext.CurrentSecurityContext.CompanyID, // companyid
                 General.GetNullableInteger(dr["FLDBUDGETCODEID"].ToString()), // budget code
                 General.GetNullableInteger(dr["FLDVESSELID"].ToString()), // vessel id
                 null,// bankid
                 General.GetNullableGuid(dr["FLDORDERPARTPAIDID"].ToString()) // part paid id
                 );
        }
    }

    private void setSupplierDetails()
    {
        DataTable dt = PhoenixAccountsVesselAccounting.VesselSupplierMapEdit(General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : string.Empty),
            General.GetNullableGuid(ViewState["VESSELSUPPLIERID"] != null ? ViewState["VESSELSUPPLIERID"].ToString() : string.Empty));
        string addresscode = "";
        if (dt.Rows.Count > 0)
        {
            txtSupplierCode.Text = dt.Rows[0]["FLDCODE"].ToString();
            txtSupplierName.Text = dt.Rows[0]["FLDNAME"].ToString();
            txtVenderCode.Text = dt.Rows[0]["FLDMAPPEDSUPPLIERCODE"].ToString();
            txtVenderName.Text = dt.Rows[0]["FLDMAPPEDSUPPLIER"].ToString();
            addresscode = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            txtVenderID.Text = dt.Rows[0]["FLDADDRESSCODE"].ToString();
        }
        cmdMoreInfoVesselSupplier.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Registers/RegistersVesselSupplierList.aspx?saveyn=n&SUPPLIERCODE=" + ViewState["VESSELSUPPLIERID"].ToString() + "');return true;");
        if (!string.IsNullOrEmpty(addresscode))
            //  cmdMoreInfoMappedSupplier.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Registers/RegistersOffice.aspx?VIEWONLY=Y&ADDRESSCODE=" + addresscode + "');return true;");
            cmdMoreInfoMappedSupplier.Attributes.Add("onclick", "openNewWindow('codehelp', '','" + Session["sitepath"] + "/Registers/RegistersOffice.aspx?VIEWONLY=Y&ADDRESSCODE=" + addresscode + "');return true;");
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CREDITPURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                try
                {
                    PhoenixAccountsVesselAccounting.VesselSupplierMapConfirm(int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["VESSELSUPPLIERID"].ToString()));
                    ucStatus.Text = "Mapped Supplier is confirmed.";
                }
                catch (Exception ee)
                {
                    ucError.ErrorMessage = ee.Message;
                    ucError.Visible = true;
                }                
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
    }

    protected void imgSupplier_Map(object sender, EventArgs e)
    {
        try
        {
            if (txtVenderID.Text=="")
            {
                ucError.ErrorMessage = "Mapped Supplier Code & Name is required";
                ucError.Visible = true;
                return;
            }

            PhoenixAccountsVesselAccounting.VesselSupplierMap(int.Parse(ViewState["vesselid"].ToString())
                        , new Guid(ViewState["VESSELSUPPLIERID"].ToString()), int.Parse(txtVenderID.Text));

            PhoenixAccountsVesselAccounting.VesselSupplierMapConfirm(int.Parse(ViewState["vesselid"].ToString()), new Guid(ViewState["VESSELSUPPLIERID"].ToString()));

            ucStatus.Text = "Supplier is mapped successfully.";
            setSupplierDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void imgUnMap_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixAccountsVesselAccounting.VesselSupplierUnmap(int.Parse(ViewState["vesselid"].ToString()), new Guid(ViewState["VESSELSUPPLIERID"].ToString()));
            setSupplierDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void gvOrderPartPaid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";



            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

            Guid orderid = new Guid(strorderid);

            DataTable dt = PhoenixAccountsVesselAccounting.EditOrderForm(new Guid(strorderid));
            string suppid = "";
            string currencyid = "";
            string refno = "";
            string vesselid = "";
            string billtocomp = "";
            string budgetcode = "";

            if (dt.Rows.Count > 0)
            {
                suppid = dt.Rows[0]["FLDMAPPEDSUPPLIER"].ToString();
                refno = dt.Rows[0]["FLDREFERENCENO"].ToString();
                vesselid = dt.Rows[0]["FLDVESSELID"].ToString();
                billtocomp = dt.Rows[0]["FLDBILLTOCOMPANY"].ToString();
                budgetcode = dt.Rows[0]["FLDBUDGETCODE"].ToString();
            }

            currencyid = ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOrderPartPaid(
                    ((RadMaskedTextBox)e.Item.FindControl("txtAmountAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text, currencyid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentInsert(General.GetNullableInteger(suppid)
                    , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                    , decimal.Parse(((RadMaskedTextBox)e.Item.FindControl("txtAmountAdd")).Text)
                    , DateTime.Parse(DateTime.Now.ToString())
                    , General.GetNullableInteger(currencyid)
                    , General.GetNullableString(refno)
                    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 127, "DRF")) //payment status
                    , General.GetNullableGuid(strorderid)
                    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 124, "VCP"))
                    , General.GetNullableInteger(vesselid)
                    , General.GetNullableInteger(billtocomp)
                    , General.GetNullableInteger(budgetcode));

                ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Focus();
                Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOrderPartPaid(
                    ((RadMaskedTextBox)e.Item.FindControl("txtAmountEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text, currencyid))
                {
                    ucError.Visible = true;
                    return;
                }
                string strorderpartpaidid = ((RadLabel)e.Item.FindControl("lblOrderPartPaidIdEdit")).Text;

                Guid orderpartpaidid = new Guid(strorderpartpaidid);

                currencyid = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency;

                PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentUpdate(orderpartpaidid
                     , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                     , decimal.Parse(((RadMaskedTextBox)e.Item.FindControl("txtAmountEdit")).Text)
                     , General.GetNullableInteger(currencyid));

                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentDelete(new Guid(((RadLabel)e.Item.FindControl("lblOrderPartPaidId")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                ApprovedRequestAdvance(new Guid(((RadLabel)e.Item.FindControl("lblOrderPartPaidId")).Text), vesselid, ((RadLabel)e.Item.FindControl("lblCurrencyID")).Text);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidOrderPartPaid(
                          ((RadMaskedTextBox)e.Item.FindControl("txtAmountEdit")).Text,
                          ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text, currencyid))
                {
                    ucError.Visible = true;
                    return;
                }

                string strorderpartpaidid = ((RadLabel)e.Item.FindControl("lblOrderPartPaidIdEdit")).Text;


                Guid orderpartpaidid = new Guid(strorderpartpaidid);

                PhoenixAccountsVesselAccounting.CreditPurchaseAdvancePaymentUpdate(orderpartpaidid
                          , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                          , decimal.Parse(((RadMaskedTextBox)e.Item.FindControl("txtAmountEdit")).Text)
                          , General.GetNullableInteger(currencyid));
                Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOrderPartPaid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
         
            DataRowView drv = (DataRowView)e.Item.DataItem;
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            RadLabel lb = (RadLabel)e.Item.FindControl("lblstatus");
            ImageButton imgb = (ImageButton)e.Item.FindControl("cmdApprove");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            LinkButton lnkDescription = (LinkButton)e.Item.FindControl("lnkDescription");

            if (imgb != null && lb != null && lb.Text == "APP")
            {
                imgb.Visible = false;
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
                if (cmdEdit != null)
                    cmdEdit.Visible = false;
                if (lnkDescription != null)
                    lnkDescription.CommandName = "";
                e.Item.Attributes["onclick"] = "";
            }

            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);

                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

                if (lblIsAtt.Text == string.Empty)
                    cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";

                cmdAtt.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.PURCHASE + "&type=ADVANCEPAYMENT" + "'); return false;");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            if (drv["FLDCURRENCYID"].ToString() != null)
            {
                UserControlCurrency cc = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");

                if (cc != null)
                {
                    cc.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                    cc.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlCurrency ccAdd = (UserControlCurrency)e.Item.FindControl("ucCurrencyAdd");

            if (ccAdd != null)
            {
                ccAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
            }
        }
    }

    protected void gvOrderPartPaid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrderPartPaid.CurrentPageIndex + 1;
        BindData();
    }
}
