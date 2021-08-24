using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class RegistersReadOnlyBankInformationtionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["hidetoolbar"] = Request.QueryString["toolbar"];
        SessionUtil.PageAccessRights(this.ViewState);
        //   ucConfirm.Visible = false;
        //ucConfirm.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
        toolbar.AddFontAwesomeButton("../Registers/RegistersReadOnlyBankInformationtionList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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

   
    protected void Rebind()
    {
        gvBankInformation.SelectedIndexes.Clear();
        gvBankInformation.EditIndexes.Clear();
        gvBankInformation.DataSource = null;
        gvBankInformation.Rebind();
    }   
}
