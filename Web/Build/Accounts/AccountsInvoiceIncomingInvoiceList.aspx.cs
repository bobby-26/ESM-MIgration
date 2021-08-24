using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsInvoiceIncomingInvoiceList : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsInvoiceIncomingInvoiceList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCompany')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsInvoiceIncomingInvoiceList.aspx", "Find", "search.png", "FIND");
            // toolbar.AddImageLink("javascript:Openpopup('codehelp1','','../Accounts/AccountsInvoiceOutgoingInvoice.aspx')", "Add", "add.png", "ADDDISPATCH");

            MenuRegistersCompany.AccessRights = this.ViewState;
            MenuRegistersCompany.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Incoming Invoices", "INCOMMING", ToolBarDirection.Right);
            toolbarmain.AddButton("OutGoing Invoices", "OUTGOINGINVOICE", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;


            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                dgDispatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("OUTGOINGINVOICE"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceOutGoingInvoiceList.aspx");
            }
            if (CommandName.ToUpper().Equals("INCOMMING"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceIncomingInvoiceList.aspx");
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDAIRWAYBILLNUMBER", "FLDDATEOFDISPATCH", "FLDINVOICECODELIST", "FLDORIGINATIONGCOMPANYCODE" };
        string[] alCaptions = { "Airway Bill number", "Dispatch date", "Invoice List", "Originating Company code" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsInvoice.InvoiceEarmarkDespatchSearch(
            General.GetNullableString(txtAirwaybillnumber.Text)
          , General.GetNullableDateTime(txtDespatchfromdate.Text)
          , General.GetNullableDateTime(txtDespatchtodate.Text)
          , chkShowall.Checked ? 1 : 0
          , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
          , 2
          , sortexpression
          , sortdirection
          , int.Parse(ViewState["PAGENUMBER"].ToString())
          , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
          , ref iRowCount
          , ref iTotalPageCount
          );

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceDispatch.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Invoice Dispatch</h3></td>");
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

    protected void RegistersCompany_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsInvoice.InvoiceEarmarkDespatchSearch(
              General.GetNullableString(txtAirwaybillnumber.Text)
            , General.GetNullableDateTime(txtDespatchfromdate.Text)
            , General.GetNullableDateTime(txtDespatchtodate.Text)
            , chkShowall.Checked ? 1 : 0
            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
            , 2
            , sortexpression
            , sortdirection
            , int.Parse(ViewState["PAGENUMBER"].ToString())
            , dgDispatch.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            );

        string[] alColumns = { "FLDAIRWAYBILLNUMBER", "FLDDATEOFDISPATCH", "FLDINVOICECODELIST", "FLDORIGINATIONGCOMPANYCODE" };
        string[] alCaptions = { "Airway Bill number", "Dispatch date", "Invoice List", "Originating Company code" };
        General.SetPrintOptions("gvCompany", "Invoice Dispatch", alCaptions, alColumns, ds);


        dgDispatch.DataSource = ds;
        dgDispatch.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void dgDispatch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgDispatch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void dgDispatch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDespatch(((RadLabel)e.Item.FindControl("lblInvoiceEarmarkId")).Text);
            }
            else
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void dgDispatch_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

                Label l = (Label)e.Item.FindControl("lblInvoiceEarmarkId");

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkAirwaybillnumber");
                lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/AccountsInvoiceIncomingInvoice.aspx?qEarmarkId=" + l.Text + "');return false;");

                ImageButton db1 = (ImageButton)e.Item.FindControl("cmdEdit");
                db1.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/AccountsInvoiceIncomingInvoice.aspx?qEarmarkId=" + l.Text + "');return false;");

            }
        }

        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
        }
    }


    private void DeleteDespatch(string strDespatchId)
    {
        PhoenixAccountsInvoice.InvoiceEarmarkDelete(new Guid(strDespatchId), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
