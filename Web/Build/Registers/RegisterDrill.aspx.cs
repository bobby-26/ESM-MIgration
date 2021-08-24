using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;


public partial class Drill_DrillRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegisterDrill.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrilllist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add Drill','','Registers/RegisterDrillAdd.aspx','false','850px','420px')", "Add Drill", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        Tabstripdrillregistermenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            ViewState["DRILL"] = string.Empty;
            gvDrilllist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
       
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDAPPLIESTO", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDAFFECTEDBYCREWCHANGEYN", "FLDCREWCHANGEPERCENTAGE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
        string[] alCaptions = {  "Name", "Interval","Interval Type", "Applies To", "Fixed/Variable","Type","Affected by Crew Change?","Crew Change Percentage","Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };





        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegisterDrill.drilllist( General.GetNullableString(ViewState["DRILL"].ToString()),
                                                gvDrilllist.CurrentPageIndex + 1,
                                                gvDrilllist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Drills.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drills</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void drillregistermenu_TabStripCommand(object sender, EventArgs e)
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

   

 

    protected void gvDrilllist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDAPPLIESTO", "FLDFIXEDORVARIABLE", "FLDTYPE",  "FLDAFFECTEDBYCREWCHANGEYN", "FLDCREWCHANGEPERCENTAGE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Applies To", "Fixed/Variable", "Type", "Affected by Crew Change?", "Crew Change Percentage", "Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };




        DataTable dt = PhoenixRegisterDrill.drilllist(General.GetNullableString(ViewState["DRILL"].ToString()),
                                                    gvDrilllist.CurrentPageIndex + 1,
                                                gvDrilllist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);


       

        gvDrilllist.DataSource = dt;
        gvDrilllist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvDrilllist", "Drills ", alCaptions, alColumns, ds);
    }


    public void gvDrilllist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? drillid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLID").ToString());
                LinkButton appliesto = ((LinkButton)item.FindControl("btnappliesto"));
                if (appliesto != null)
                {
                    appliesto.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','List of Vessel Types','Inspection/InspectionDrillvesseltypelist.aspx?drillid=" + drillid + "','false','400px','320px');return false");
                }

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Drill Edit','Registers/RegisterDrillEdit.aspx?drillid=" + drillid + "','false','850px','420px');return false");

                }
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDDRILLNAME"].Controls[0];
                textBox.MaxLength = 198;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    public void gvDrilllist_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["DRILL"] = gvDrilllist.MasterTableView.GetColumn("FLDDRILLNAME").CurrentFilterValue;

            gvDrilllist.Rebind();

        }
    }

  

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvDrilllist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}