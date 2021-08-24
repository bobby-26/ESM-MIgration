using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Drawing;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderLogParameterList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["vesselid"] != null)
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?vesselid="+ Request.QueryString["vesselid"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        else
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvParameters')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenugvParameters.MenuList = toolbar.Show();
        //MenugvParameters.SetTrigger(pnlgvParameters);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            gvParameters.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        //BindData();
    }

    protected void gvParameters_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
        string[] alColumns = { "FLDPARAMETERNAME", "FLDVALUE" };
        string[] alCaptions = { "Parameter Name", "Parameter Value" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds;
        if (Request.QueryString["vesselid"] != null)
            ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.ParameterLogList(new Guid(ViewState["WORKORDERID"].ToString()), int.Parse(Request.QueryString["vesselid"]));
        else
            ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.ParameterLogList(new Guid(ViewState["WORKORDERID"].ToString()));

        General.SetPrintOptions("gvParameters", "Parameters", alCaptions, alColumns, ds);

        gvParameters.DataSource = ds;

    }

    protected void ShowExcel()
    {

        DataSet ds = new DataSet();
        string[] alColumns = {  "FLDPARAMETERNAME", "FLDVALUE"};
        string[] alCaptions = {  "Parameter Name", "Parameter Value" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Request.QueryString["vesselid"] != null)
            ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.ParameterLogList(new Guid(ViewState["WORKORDERID"].ToString()), int.Parse(Request.QueryString["vesselid"]));
        else
            ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.ParameterLogList(new Guid(ViewState["WORKORDERID"].ToString()));

        General.ShowExcel("Parameters", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void gvParameters_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvParameters_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvParameters_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {                
                RadLabel radLabel = (RadLabel)e.Item.FindControl("lblParameterValue");
                if (radLabel != null)
                {
                    if (drv["FLDISEXPECTEDANSWER"].ToString() == "-1")
                    {
                        string minvalue = drv["FLDMINVALUE"].ToString();
                        string maxvalue = drv["FLDMAXVALUE"].ToString();
                        decimal min;
                        decimal.TryParse(minvalue, out min);
                        decimal max;
                        decimal.TryParse(maxvalue, out max);
                        decimal val;
                        decimal.TryParse(drv["FLDVALUE"].ToString(), out val);
                        if (minvalue != string.Empty && maxvalue != string.Empty)
                        {
                            if (val >= min && val <= max)
                            {
                                radLabel.Attributes["style"] = "color: green !important";
                            }
                            else
                            {
                                radLabel.Attributes["style"] = "color: red !important";
                            }
                        }
                    }
                    else if(drv["FLDISEXPECTEDANSWER"].ToString() == "0")
                    {
                        radLabel.Attributes["style"] = "color: red !important";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}