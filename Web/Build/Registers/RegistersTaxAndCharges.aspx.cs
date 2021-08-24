using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Registers_RegistersTaxAndCharges : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarTitle = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersTaxAndCharges.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvTaxAndCharges')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersTaxAndCharges.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersTaxAndCharges.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Registers/RegistersTaxAndCharges.aspx", "Add New", "add.png", "NEW");
            MenuRegistersTaxAndCharges.AccessRights = this.ViewState;
            MenuRegistersTaxAndCharges.MenuList = toolbar.Show();

            MenuregistersTaxAndChargesMain.AccessRights = this.ViewState;
            MenuregistersTaxAndChargesMain.Title = "Tax and Charges";
            MenuregistersTaxAndChargesMain.MenuList = toolbarTitle.Show();

            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvTaxAndCharges.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRegistersTaxAndCharges_TabStripCommand(object sender, EventArgs e)
    {
        try
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
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../Registers/RegistersTaxAndChargesAdd.aspx?id=", true);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlactiveyn.SelectedValue = "";
                ddlPaymentmode.SelectedHard = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTaxAndCharges_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTaxAndCharges.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvTaxAndCharges.SelectedIndexes.Clear();
        gvTaxAndCharges.EditIndexes.Clear();
        gvTaxAndCharges.DataSource = null;
        gvTaxAndCharges.Rebind();
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPAYMENTMODEDESC", "FLDBANKACCOUNT", "FLDRANGEFROM", "FLDRANGETO", "FLDTAXPERCENTAGE", "FLDMAXGROSSAMOUNT", "FLDACTIVEYNDESC" };
        string[] alCaptions = { "Payment Mode", "Bank Account", "Range From", "Range To", "Percentage", "Max Gross Amount", "Active YN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersTaxAndCharges.TaxAndChargesSearch(General.GetNullableInteger(ddlPaymentmode.SelectedHard)
                                                    , null, General.GetNullableInteger(ddlactiveyn.SelectedValue)
                                                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                                    , gvTaxAndCharges.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvTaxAndCharges", "Tax and Charges", alCaptions, alColumns, ds);


        gvTaxAndCharges.DataSource = ds;
        gvTaxAndCharges.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvTaxAndCharges_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string id = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;

                Response.Redirect("../Registers/RegistersTaxAndChargesAdd.aspx?id=" + id, true);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            Rebind();
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
        string[] alColumns = { "FLDPAYMENTMODEDESC", "FLDBANKACCOUNT", "FLDRANGEFROM", "FLDRANGETO", "FLDTAXPERCENTAGE", "FLDMAXGROSSAMOUNT", "FLDACTIVEYNDESC" };
        string[] alCaptions = { "Payment Mode", "Bank Account", "Range From", "Range To", "Percentage", "Max Gross Amount", "Active YN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = PhoenixRegistersTaxAndCharges.TaxAndChargesSearch(General.GetNullableInteger(ddlPaymentmode.SelectedHard)
                                            , null, General.GetNullableInteger(ddlactiveyn.SelectedValue)
                                            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TaxAndCharges.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>TaxAndCharges </h3></td>");
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
}
