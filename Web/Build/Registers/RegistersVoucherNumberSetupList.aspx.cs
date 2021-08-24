using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class RegistersVoucherNumberSetupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersVoucherNumberSetupList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVoucherNumberSetup')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuVoucherNumberSetup.AccessRights = this.ViewState;
        MenuVoucherNumberSetup.MenuList = toolbar.Show();
        //MenuVoucherNumberSetup.SetTrigger(pnlVoucherNumberSetup);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvVoucherNumberSetup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvVoucherNumberSetup.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;

        string[] alColumns = { "FLDVOUCHERTYPE",  "FLDVOUCHERTYPEPREFIX" };
        string[] alCaptions = { "Voucher ", "Code" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsVoucherNumberSetup.VoucherNumberSetupSearch(null, null, null, null, sortexpression, sortdirection,
            gvVoucherNumberSetup.CurrentPageIndex + 1,
            gvVoucherNumberSetup.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVoucherNumberSetup.DataSource = ds;
            gvVoucherNumberSetup.VirtualItemCount = iRowCount;
        }
        
        General.SetPrintOptions("gvVoucherNumberSetup", "Voucher Short Code", alCaptions, alColumns, ds);
    }

    protected void gvVoucherNumberSetup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        RadGrid _gridView = (RadGrid)sender;
        //GridEditableItem eeditedItem = e.Item as GridEditableItem;

        //if (e.CommandName.ToUpper().Equals("SAVE"))
        //{
        //    InsertVoucherNumberSetup(
        //        (((RadTextBox)eeditedItem.FindControl("hidtxtVoucherNumberFormatCode")).Text.Trim()),
        //        int.Parse(((RadTextBox)eeditedItem.FindControl("hidtxtSubVoucherCode")).Text.Trim()),
        //        ((RadTextBox)eeditedItem.FindControl("txtVoucherTypeEdit")).Text.Trim(),
        //        string.Empty
        //    );
        //    gvVoucherNumberSetup.SelectedIndexes.Clear();
        //    gvVoucherNumberSetup.EditIndexes.Clear();
        //    gvVoucherNumberSetup.Rebind();

        //}

        //if (e.CommandName.ToUpper().Equals("EDIT"))
        //{
        //    gvVoucherNumberSetup.Rebind();
        //}

        //else
        //{
        //    gvVoucherNumberSetup.SelectedIndexes.Clear();
        //    gvVoucherNumberSetup.EditIndexes.Clear();
        //    gvVoucherNumberSetup.Rebind();
        //}
    }

    protected void gvVoucherNumberSetup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadImageButton edit = (RadImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            RadImageButton del = (RadImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            RadImageButton save = (RadImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            RadImageButton cancel = (RadImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
        }
    }

    public void InsertVoucherNumberSetup(string strVoucherNumberFormatCode, int iSubVoucherCode,  string strVoucherType, string strCurrencyCode)
    {
        int? iVoucherNumberFormatCode = null;
        if (strVoucherNumberFormatCode != string.Empty)
            iVoucherNumberFormatCode = int.Parse(strVoucherNumberFormatCode);
        if (!IsValidNumberFormat(strVoucherType))
        {
            ucError.Visible = true;
            return;
        }
        try
        {
            PhoenixAccountsVoucherNumberSetup.VoucherNumberSetupInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,iVoucherNumberFormatCode, iSubVoucherCode, strVoucherType, strCurrencyCode);
            ucStatus.Text = "Voucher Short Code Saved Successfully.";
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidNumberFormat(string strVoucherType)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strVoucherType.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";
        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVOUCHERTYPE",  "FLDVOUCHERTYPEPREFIX" };
        string[] alCaptions = { "Voucher ",  "Code" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsVoucherNumberSetup.VoucherNumberSetupSearch(null, null, null, null, null, null,
            gvVoucherNumberSetup.CurrentPageIndex + 1,
            gvVoucherNumberSetup.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VoucherShortCode.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voucher Short Code</h3></td>");
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
    protected void gvVoucherNumberSetup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void MenuVoucherNumberSetup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
        }
    }
}
