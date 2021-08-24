using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.IO;
using Telerik.Web.UI;

public partial class AccountsInvoiceBySupplier : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (Request.QueryString["issupplierlogin"] != null)
        {
            ViewState["ISSUPPLIERLOGIN"] = Request.QueryString["issupplierlogin"].ToString();

            if (Request.QueryString["issupplierlogin"].ToString().ToUpper().Equals("1"))
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                //toolbar.AddButton("Confirm", "CONFIRM");
                // MenuInvoice1.Title = "Invoice";

                if (Request.QueryString["supid"] != null && Request.QueryString["supid"] != string.Empty)
                {
                    ViewState["supid"] = Request.QueryString["supid"].ToString();
                    txtVendorId.Text = ViewState["supid"].ToString();
                }
                if (Request.QueryString["supcode"] != null && Request.QueryString["supcode"] != string.Empty)
                {
                    ViewState["supcode"] = Request.QueryString["supcode"].ToString();
                    txtVendorCode.Text = ViewState["supcode"].ToString();
                }
                if (Request.QueryString["supname"] != null && Request.QueryString["supname"] != string.Empty)
                {
                    ViewState["supname"] = Request.QueryString["supname"].ToString();
                    txtVenderName.Text = ViewState["supname"].ToString();
                }

                txtInvoiceReceivedDateEdit.CssClass = "readonlytextbox";
                txtInvoiceReceivedDateEdit.Enabled = false;
                ddlInvoiceType.Enabled = false;
                ddlInvoiceType.CssClass = "readonlytextbox";
            }
            else
            {
                toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
        }

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.Title = "Invoice";
        MenuInvoice1.MenuList = toolbar.Show();


        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            ImgSupplierPickList.Attributes["onclick"] = null;
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            //txtInvoiceReceivedDateEdit.Text = DateTime.Now.ToString();

            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"].ToString();
                InvoiceEdit();
            }
        }
    }

    private void Reset()
    {
        ViewState["INVOICECODE"] = null;
        txtInvoiceNumber.Text = "";
        txtInvoiceDateEdit.Text = "";
        txtSupplierRefEdit.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        //txtVendorId.Text = "";
        //txtVendorCode.Text = "";
        //txtVenderName.Text = "";
        txtExchangeRateEdit.Text = "";
        ucInvoiceAmoutEdit.Text = "";
        txtStatus.Text = "";
        txtRemarks.Text = "";
        imgAttachment.Visible = false;
        txtInvoiceReceivedDateEdit.Text = "";
        ddlInvoiceType.SelectedHard = "";
        //ttlInvoice.Text = "Invoice      ()";
    }

    protected void Invoice_SetExchangeRate(object sender, EventArgs e)
    {
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ddlCurrencyCode.SelectedCurrency));
            if (dsInvoice.Tables.Count > 0 && dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtExchangeRateEdit.Text = string.Format(String.Format("{0:#####.000000}", drInvoice["FLDEXCHANGERATE"]));
            }
        }
        else
        {
            txtExchangeRateEdit.Text = "";
        }
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        string strInvoiceCode = string.Empty;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidInvoice())
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["INVOICECODE"] == null)
            {
                try
                {
                    if (ViewState["ISSUPPLIERLOGIN"] != null && ViewState["ISSUPPLIERLOGIN"].ToString().Equals("1"))
                    {
                        PhoenixAccountsInvoice.InvoiceBySupplierInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            txtSupplierRefEdit.Text,
                            int.Parse(ddlCurrencyCode.SelectedCurrency),
                            decimal.Parse(ucInvoiceAmoutEdit.Text),
                            decimal.Parse(txtExchangeRateEdit.Text),
                            DateTime.Parse(txtInvoiceDateEdit.Text),
                            General.GetNullableInteger(txtVendorId.Text),
                            "Draft",
                            txtRemarks.Text,
                            ref strInvoiceCode);

                        ucStatus.Text = "Invoice information is added";
                    }
                    else
                    {
                        ucError.ErrorMessage = "Only supplier can add invoice entry here.";
                        ucError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                Reset();
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                Session["New"] = "Y";
            }
            else
            {
                try
                {
                    PhoenixAccountsInvoice.InvoiceBySupplierUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        txtSupplierRefEdit.Text,
                        int.Parse(ddlCurrencyCode.SelectedCurrency),
                        decimal.Parse(ucInvoiceAmoutEdit.Text),
                        decimal.Parse(txtExchangeRateEdit.Text),
                        DateTime.Parse(txtInvoiceDateEdit.Text),
                        General.GetNullableDateTime(txtInvoiceReceivedDateEdit.Text),
                        General.GetNullableInteger(txtVendorId.Text),
                        txtStatus.Text,
                        General.GetNullableInteger(ddlInvoiceType.SelectedHard),
                        txtRemarks.Text,
                        new Guid(ViewState["INVOICECODE"].ToString()));

                    ucStatus.Text = "Invoice information is updated";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                strInvoiceCode = ViewState["INVOICECODE"].ToString();
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
        if (CommandName.ToUpper().Equals("CONFIRM"))
        {
            try
            {
                if (!IsValidInvoice())
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? copyinvoicecode = null;

                PhoenixAccountsInvoice.InvoiceBySupplierConfirm(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    txtSupplierRefEdit.Text,
                    int.Parse(ddlCurrencyCode.SelectedCurrency),
                    decimal.Parse(ucInvoiceAmoutEdit.Text),
                    decimal.Parse(txtExchangeRateEdit.Text),
                    DateTime.Parse(txtInvoiceDateEdit.Text),
                    General.GetNullableDateTime(txtInvoiceReceivedDateEdit.Text),
                    General.GetNullableInteger(txtVendorId.Text),
                    General.GetNullableInteger(ddlInvoiceType.SelectedHard),
                    txtRemarks.Text,
                    new Guid(ViewState["INVOICECODE"].ToString()),
                    ref copyinvoicecode);

                // Copy the attachments if any..

                DataSet dsEdit = PhoenixAccountsInvoice.InvoiceBySupplierEdit(
                    new Guid(ViewState["INVOICECODE"].ToString()));

                DataSet dsCopyEdit = PhoenixAccountsInvoice.InvoiceEdit(
                    new Guid(copyinvoicecode.ToString()));

                if (dsEdit.Tables.Count > 0 && dsEdit.Tables[0].Rows.Count > 0)
                {
                    ViewState["DTKey"] = dsEdit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }

                if (dsCopyEdit.Tables.Count > 0 && dsCopyEdit.Tables[0].Rows.Count > 0)
                {
                    ViewState["CopyDTKey"] = dsCopyEdit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }

                int iRowCount = 0;
                int iTotalPageCount = 0;

                DataSet ds = PhoenixCommonFileAttachment.AttachmentSearch(
                    new Guid(ViewState["DTKey"].ToString()),
                    null, null,
                    null, null,
                    1, 100, ref iRowCount, ref iTotalPageCount);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //string sourceDir = Session["sitepath"] + "/attachments/" + dr["FLDFILEPATH"].ToString();
                        //string backupDir = Session["sitepath"] + "Attachments/ACCOUNTS";
                        //string sourceDir = Server.MapPath("~") + "\\Attachments\\" + dr["FLDFILEPATH"].ToString().Replace("/", "\\");
                        //string backupDir = Server.MapPath("~") + "\\Attachments\\ACCOUNTS";

                        string sourceDir = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + dr["FLDFILEPATH"].ToString().Replace("/", "\\");
                        string backupDir = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/";

                        string filename = dr["FLDFILENAME"].ToString();
                        string olddtkey = dr["FLDDTKEY"].ToString();
                        string filepath = "ACCOUNTS/";
                        string filesize = dr["FLDFILESIZE"].ToString();
                        string attachmenttype = dr["FLDATTACHMENTTYPE"].ToString();

                        try
                        {
                            // Remove path from the file name. 
                            string fName = dr["FLDFILEPATH"].ToString().Replace("ACCOUNTS/", "");
                            string dtkey = string.Empty;

                            dtkey = PhoenixCommonFileAttachment.GenerateDTKey();
                            fName = fName.Replace(olddtkey, dtkey);
                            filepath = filepath + fName;

                            if (File.Exists(sourceDir))
                            {
                                try
                                {
                                    // Use the Path.Combine //Path.Combine(sourceDir, fName)// method to safely append the file name to the path.
                                    // Will overwrite if the destination file already exists.
                                    File.Copy(sourceDir, backupDir + "\\" + fName, true);

                                    PhoenixCommonFileAttachment.InsertAttachment(
                                        new Guid(ViewState["CopyDTKey"].ToString()),
                                        filename,
                                        filepath,
                                        long.Parse(filesize),
                                        null,
                                        null, //sync yes no
                                        attachmenttype,
                                        new Guid(dtkey));

                                }
                                // Catch exception if the file was already copied. 
                                catch (IOException copyError)
                                {
                                    ucError.ErrorMessage = copyError.Message;
                                    ucError.Visible = true;
                                }
                            }
                        }
                        catch (DirectoryNotFoundException dirNotFound)
                        {
                            ucError.ErrorMessage = dirNotFound.Message;
                            ucError.Visible = true;
                        }
                    }
                }

                ucStatus.Text = "Invoice is confirmed.";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
    }

    public bool IsValidInvoice()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime? dtinvoicedate = null, dtreceiveddate = null;

        if (txtSupplierRefEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice reference is required";

        if (ddlCurrencyCode.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";

        if (ucInvoiceAmoutEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Invoice amount is required.";

        if (txtExchangeRateEdit.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange rate is required.";

        if (txtInvoiceDateEdit.Text == null)
            ucError.ErrorMessage = "Invoice date is required.";
        else
            dtinvoicedate = DateTime.Parse(txtInvoiceDateEdit.Text);

        if (ViewState["ISSUPPLIERLOGIN"] != null && ViewState["ISSUPPLIERLOGIN"].ToString().Equals("0"))
        {
            if (txtInvoiceReceivedDateEdit.Text == null)
                ucError.ErrorMessage = "Invoice received date is required.";
            else
                dtreceiveddate = DateTime.Parse(txtInvoiceReceivedDateEdit.Text);

            if (dtreceiveddate < dtinvoicedate)
                ucError.ErrorMessage = "Invoice received date should be later than invoice date.";

            if (General.GetNullableInteger(ddlInvoiceType.SelectedHard) == null)
                ucError.ErrorMessage = "Invoice type is required.";
        }

        if (dtinvoicedate > DateTime.Today)
            ucError.ErrorMessage = "Invoice date should not be the future date.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void InvoiceEdit()
    {
        if (ViewState["INVOICECODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceBySupplierEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString());
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();

                if (ViewState["ISSUPPLIERLOGIN"] != null && ViewState["ISSUPPLIERLOGIN"].ToString().Equals("0"))
                {
                    txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
                    txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
                    txtVenderName.Text = drInvoice["FLDNAME"].ToString();
                }

                txtInvoiceReceivedDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICERECEIVEDDATE"].ToString());
                ddlInvoiceType.SelectedHard = drInvoice["FLDINVOICETYPE"].ToString();

                ddlCurrencyCode.SelectedCurrency = drInvoice["FLDCURRENCY"].ToString();
                txtExchangeRateEdit.Text = drInvoice["FLDEXCHANGERATE"].ToString();
                ucInvoiceAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEAMOUNT"]));
                txtRemarks.Text = drInvoice["FLDREMARKS"].ToString();
                txtStatus.Text = drInvoice["FLDINVOICESTATUS"].ToString();
                txtDTKey.Text = drInvoice["FLDDTKEY"].ToString();
            }
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
