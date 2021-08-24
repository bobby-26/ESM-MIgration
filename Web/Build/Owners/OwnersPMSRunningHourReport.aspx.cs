using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Owners;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class OwnersPMSRunningHourReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPMSRunningHourReport.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dvStoreItemControl')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPMSRunningHourReport.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersPMSRunningHourReport.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuStockItemControl.AccessRights = this.ViewState;
            MenuStockItemControl.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                drpdwnCounterType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 111, 0, "HRS");
                drpdwnCounterType.DataBind();
                ViewState["ISTREENODECLICK"] = false;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
                dvStoreItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void drpdwnCounterType_DataBound(object sender, EventArgs e)
    {
        if (!IsPostBack)
            drpdwnCounterType.Items.Insert(0, new Telerik.Web.UI.DropDownListItem("--Select--", ""));
    }
    //protected void MenuReportRunningHour_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //        if (dce.CommandName.ToUpper().Equals("GO"))
    //        {
    //            string prams = "";

    //            prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
    //            prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
    //            prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
    //            prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
    //            prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
    //            prams += "&countertype=" + General.GetNullableString(drpdwnCounterType.SelectedValue);

    //            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=RUNNINGHOUR" + prams);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    protected void Rebind()
    {
        dvStoreItemControl.SelectedIndexes.Clear();
        dvStoreItemControl.EditIndexes.Clear();
        dvStoreItemControl.DataSource = null;
        dvStoreItemControl.Rebind();
    }
    protected void MenuStockItemControl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtComponentName.Text = "";
                txtComponentNumber.Text = "";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                drpdwnCounterType.SelectedValue = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCOUNTERTYPENAME", "FLDCURRENTVALUE", "FLDREADDATE" };
            string[] alCaptions = { "Component No.", "Name", "Counter Type", "Current Value", "Reading Date" };

            DataSet ds = PhoenixOwnersPlannedMaintenance.ReportRunningHour(General.GetNullableInteger(ucVessel.SelectedVessel),
                General.GetNullableString(txtComponentNumber.Text), General.GetNullableString(txtComponentName.Text),
                General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text),
                General.GetNullableInteger(drpdwnCounterType.SelectedValue), sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], dvStoreItemControl.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("dvStoreItemControl", "Running Hour", alCaptions, alColumns, ds);

            dvStoreItemControl.DataSource = ds;
            dvStoreItemControl.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCOUNTERTYPENAME", "FLDCURRENTVALUE", "FLDREADDATE" };
            string[] alCaptions = { "Component No.", "Name", "Counter Type", "Current Value", "Reading Date" };

            DataSet ds = PhoenixOwnersPlannedMaintenance.ReportRunningHour(General.GetNullableInteger(ucVessel.SelectedVessel),
                General.GetNullableString(txtComponentNumber.Text), General.GetNullableString(txtComponentName.Text),
                General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text),
                General.GetNullableInteger(drpdwnCounterType.SelectedValue), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], iRowCount
                , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=Running_Hour.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Running Hour</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    
    protected void dvStoreItemControl_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
        }
    }
    protected void dvStoreItemControl_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void dvStoreItemControl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dvStoreItemControl.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
