using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsAirfareInvoiceAdminHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareInvoiceAdminHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "icon_print.png", "PRINT");
            MenuHistory.AccessRights = this.ViewState;
            MenuHistory.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);
            MenuHistoryMain.AccessRights = this.ViewState;
            MenuHistoryMain.MenuList = toolbarmain.Show();

            MenuHistoryMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["ID"] = "";
                ViewState["AGENTINVOICEID"] = "";
                if (Request.QueryString["ID"] != null)
                    ViewState["ID"] = Request.QueryString["ID"].ToString();


                rblHistoryType.SelectedIndex = 0;
                InvoiceEdit();
            }
            // BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuHistoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                if (Request.QueryString["Source"] != null && Request.QueryString["Source"].ToString() == "Register")
                    Response.Redirect("../Accounts/AccountsAirfareInvoiceRegisterMaster.aspx?ID=" + ViewState["ID"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminMaster.aspx?ID=" + ViewState["ID"].ToString());
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminAttachments.aspx?ID=" + ViewState["ID"].ToString());
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
        DataSet ds = new DataSet();

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        string type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVHistoryList(General.GetNullableGuid(ViewState["AGENTINVOICEID"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelInvoiceHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Invoice History" + "-" + ViewState["Invoicenumber"] + "</h3></td>");
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

    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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

    private void BindData()
    {
        string type = "";
        DataSet ds = new DataSet();

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVHistoryList(General.GetNullableGuid(ViewState["AGENTINVOICEID"].ToString())
                                                     , type);
        General.SetPrintOptions("gvInvoiceHistory", "Travel Invoice History", alCaptions, alColumns, ds);

        gvInvoiceHistory.DataSource = ds;

    }

    protected void ReBindData(object sender, EventArgs e)
    {
        //BindData();
    }
    protected void InvoiceEdit()
    {
        if (ViewState["ID"] != null)
        {
            DataSet ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareInvoiceAdminEdit(General.GetNullableInteger((ViewState["ID"].ToString())));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["AGENTINVOICEID"] = dr["FLDAGENTINVOICEID"].ToString();
            }
        }
    }

    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
