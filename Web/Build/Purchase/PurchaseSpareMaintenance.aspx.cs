using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseSpareMaintenance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareMaintenance.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSpareMaintance')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareMaintenance.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseSpareMaintenance.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuSpareMain.AccessRights = this.ViewState;
            MenuSpareMain.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSpareMaintance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSpareMain_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareMaintance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSpareMaintance.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPurchaseSpareMaintenance.SpareRequiredMaintancesearch(
                General.GetNullableInteger(UcVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(UcVessel.SelectedVessel)
                , General.GetNullableInteger(txtDueDays.Text) == null ? 0 : General.GetNullableInteger(txtDueDays.Text)
                , General.GetNullableString(txtitemnumber.Text)
                , General.GetNullableString(txtname.Text)
                , sortexpression, sortdirection, gvSpareMaintance.CurrentPageIndex + 1,
                 gvSpareMaintance.PageSize, ref iRowCount, ref iTotalPageCount);

            string[] alColumns = { "FLDJOBNO","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDQUANTITY", "FLDROBQUANTITY", "FLDLEADTIME" };
            string[] alCaptions = { "Job Number", "Part Number", "Name", "Maker Reference", "Required Quantity", "ROB", "Lead Time" };

             General.SetPrintOptions("gvSpareMaintance", "Spare Maintenance", alCaptions, alColumns, ds);

            gvSpareMaintance.DataSource = ds;
            gvSpareMaintance.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void ClearFilter()
    {
        UcVessel.SelectedVessel = "";
        txtDueDays.Text = "";
        txtitemnumber.Text = "";
        txtname.Text = "";
        Rebind();
    }

    protected void Rebind()
    {
        gvSpareMaintance.SelectedIndexes.Clear();
        gvSpareMaintance.EditIndexes.Clear();
        gvSpareMaintance.DataSource = null;
        gvSpareMaintance.Rebind();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvSpareMaintance.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDJOBNO", "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDQUANTITY", "FLDROBQUANTITY", "FLDLEADTIME" };
        string[] alCaptions = { "Job Number", "Part Number", "Name", "Maker Reference", "Required Quantity", "ROB", "Lead Time" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixPurchaseSpareMaintenance.SpareRequiredMaintancesearch(
                General.GetNullableInteger(UcVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(UcVessel.SelectedVessel)
                , General.GetNullableInteger(txtDueDays.Text) == null ? 0 : General.GetNullableInteger(txtDueDays.Text)
                , General.GetNullableString(txtitemnumber.Text)
                , General.GetNullableString(txtname.Text)
                , sortexpression, sortdirection, gvSpareMaintance.CurrentPageIndex + 1,
                 gvSpareMaintance.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SpareMaintenance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Spare Maintenance</h3></td>");
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
