using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsAirfareInvoiceRegisterMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);

            MenuInvoiceMain.AccessRights = this.ViewState;
            MenuInvoiceMain.MenuList = toolbarmain.Show();

            MenuInvoiceMain.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CREDITNOTEID"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["ID"] = null;

                if (Request.QueryString["ID"] != null)
                {
                    ViewState["ID"] = Request.QueryString["ID"].ToString();
                    // hiddenkey.Value = Request.QueryString["ID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareInvoiceRegister.aspx?ID=" + ViewState["ID"];
                }
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareInvoiceRegisterMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareInvoiceAdminFilter.aspx?Source=Register", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareInvoiceRegisterMaster.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuInvoice.AccessRights = this.ViewState;
            MenuInvoice.MenuList = toolbargrid.Show();
            gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        DataSet ds = new DataSet();

        int ioutputPageNumber = 0;
        NameValueCollection nvc = Filter.CurrentAirfareInvoiceAdminFilter;

        ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareRegisterSearch(nvc != null ? General.GetNullableString(nvc.Get("txtrequisitionno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtpassengername").ToString()) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtticketno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtdeparturefrom")) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtdepartureto")) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtarrivalfrom")) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtarrivalto")) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtinvoiceno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierId").ToString()) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtAccountId").ToString()) : null
                                                                    , gvInvoice.CurrentPageIndex + 1
                                                                    , gvInvoice.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , hiddenkey != null ? General.GetNullableInteger(hiddenkey.Value) : null
                                                                    , ref ioutputPageNumber
                                                                        );

        string[] alCaptions = { "Employee ID", "Passenger Name", "Departure Date", "Origin", "Destination", "Vessel", "Invoice Cancelled", "Invoice Status", "Charging Status" };
        string[] alColumns = { "FLDFILENO", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDORIGIN", "FLDDESTINATION", "FLDVESSELACCOUNTCODE", "FLDINVOICECANCELLED", "FLDINVOICESTATUS", "FLDCHARGINGSTATUS" };

        General.SetPrintOptions("gvInvoice", "Airfare Invoice Register", alCaptions, alColumns, ds);

        gvInvoice.DataSource = ds;
        gvInvoice.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["ID"] == null)
            {
                ViewState["ID"] = ds.Tables[0].Rows[0]["FLDID"].ToString();
                ///  gvInvoice.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareInvoiceRegister.aspx?ID=" + ViewState["ID"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareInvoiceRegister.aspx?ID=";
            }
        }
        //  ViewState["PAGENUMBER"] = ioutputPageNumber;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        // hiddenkey.Value = null;
    }
    private void SetRowSelection()
    {
        gvInvoice.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvInvoice.Items)
        {
            if (item.GetDataKeyValue("FLDID").ToString().Equals(ViewState["ID"].ToString()))
            {
                gvInvoice.SelectedIndexes.Add(item.ItemIndex);

            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvInvoice.Rebind();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbId = ((RadLabel)gvInvoice.Items[rowindex].FindControl("lblID"));
            if (lbId != null)
                ViewState["ID"] = lbId.Text;

            if (ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareInvoiceRegister.aspx?ID=" + ViewState["ID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Employee ID", "Passenger Name", "Departure Date", "Origin", "Destination", "Vessel", "Invoice Cancelled", "Invoice Status", "Charging Status" };
        string[] alColumns = { "FLDFILENO", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDORIGIN", "FLDDESTINATION", "FLDVESSELACCOUNTCODE", "FLDINVOICECANCELLED", "FLDINVOICESTATUS", "FLDCHARGINGSTATUS" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int ioutputPageNumber = 0;
        NameValueCollection nvc = Filter.CurrentAirfareInvoiceAdminFilter;

        ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareRegisterSearch(nvc != null ? General.GetNullableString(nvc.Get("txtrequisitionno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtpassengername").ToString()) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtticketno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtdeparturefrom").ToString()) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtdepartureto").ToString()) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtarrivalfrom").ToString()) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtarrivalto").ToString()) : null
                                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtinvoiceno").ToString()) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierId").ToString()) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtAccountId").ToString()) : null
                                                                    , 1
                                                                    , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , null
                                                                    , ref ioutputPageNumber
                                                                        );


        Response.AddHeader("Content-Disposition", "attachment; filename=AirfareInvoiceRegister.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Airfare Invoice Register</h3></td>");
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
    protected void MenuInvoiceMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareInvoiceRegister.aspx?CREDITNOTEID=" + ViewState["CREDITNOTEID"].ToString() ;
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminAttachments.aspx?ID=" + ViewState["ID"].ToString());
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminHistory.aspx?Source=Register&ID=" + ViewState["ID"].ToString());
            }
            //else
            //    MenuInvoiceMain.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuInvoice_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAirfareInvoiceAdminFilter = null;
                gvInvoice.CurrentPageIndex = 0;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "ChangePageSize")
            {
                return;
            }

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }

            if (e.CommandName == "Page")
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
    protected void Rebind()
    {
        gvInvoice.SelectedIndexes.Clear();
        gvInvoice.EditIndexes.Clear();
        gvInvoice.DataSource = null;
        gvInvoice.Rebind();
    }

    protected void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
