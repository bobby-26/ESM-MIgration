using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
using System.Web;
public partial class OwnersPMSWorkorderList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Owners/OwnersPMSWorkorderList.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Owners/OwnersPMSWorkorderList.aspx", "Filter", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Owners/OwnersPMSWorkorderList.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                ddlComponentClass.Visible = true;
                lblClassName.Visible = true;
                gvComponenetJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            {
                //  divPage.Visible = false;
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
        gvComponenetJob.SelectedIndexes.Clear();
        gvComponenetJob.EditIndexes.Clear();
        gvComponenetJob.DataSource = null;
        gvComponenetJob.Rebind();
    }
    protected void ucVessel_OnTextChangedEvent(object sender, EventArgs e)
    {

        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            //  divPage.Visible = false;
            ddlComponentClass.DataSource = null;
            ddlComponentClass.DataBind();
            ddlComponentClass.Items.Clear();
            ddlComponentClass.DataSource = PhoenixPurchaseOrderLine.GetComponentHeads(int.Parse(ucVessel.SelectedVessel));
            ddlComponentClass.DataBind();
            ddlComponentClass.Visible = true;
            lblClassName.Visible = true;
            ddlComponentClass.SelectedIndex = 0;
        }
    }

    protected void MenuShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtComponentNumber.Text = "";
            txtWorkorderName.Text = "";
            ddlComponentClass.SelectedValue = "";
            ddlStockClassType.SelectedHard = "";
            ucVessel.SelectedVessel = "";
            Rebind();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void BindData()
    {
        try
        {
            //   divPage.Visible = true;
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = {  "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS"
                                 , "FLDRUNHOURSINCE", "FLDLASTCOUNTERUPDATE", "FLDCURRENTVALUE"};
            string[] alCaptions = { "Component Number", "Component Name", "Work Order Title", "Frequency", "Last Done Date", "Last Done Hours", "RunHour Since"
                                 , "Last Counter Update On", "Current Counter Value" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixOwnersPlannedMaintenance.ComponentJobList(General.GetNullableGuid(ddlComponentClass.SelectedValue.ToString()), General.GetNullableInteger(ucVessel.SelectedVessel), null,
                                     General.GetNullableString(txtWorkorderName.Text), General.GetNullableString(txtComponentNumber.Text),
                                     null, null, null, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvComponenetJob.PageSize,
                                    ref iRowCount, ref iTotalPageCount);
            gvComponenetJob.DataSource = ds;
            gvComponenetJob.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = {  "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS"
                                 , "FLDRUNHOURSINCE", "FLDLASTCOUNTERUPDATE", "FLDCURRENTVALUE"};
        string[] alCaptions = { "Component Number", "Component Name", "Work Order Title", "Frequency", "Last Done Date", "Last Done Hours", "RunHour Since"
                                 , "Last Counter Update On", "Current Counter Value" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixOwnersPlannedMaintenance.ComponentJobList(General.GetNullableGuid(ddlComponentClass.SelectedValue.ToString()), General.GetNullableInteger(ucVessel.SelectedVessel), null,
             General.GetNullableString(txtWorkorderName.Text), General.GetNullableString(txtComponentNumber.Text), null, null, null
                                            , sortexpression, sortdirection,
                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                            iRowCount,
                                            ref iRowCount,
                                            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Componenetjob.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"].ToString() +  "</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Component Job</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><left>Component Maintenance Record</left></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");


        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
    protected void gvComponenetJob_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }
    protected void gvComponenetJob_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvComponenetJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponenetJob.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
