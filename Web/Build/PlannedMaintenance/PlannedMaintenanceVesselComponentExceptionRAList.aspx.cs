using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Web;

public partial class PlannedMaintenanceVesselComponentExceptionRAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarnoonreportlist = new PhoenixToolbar();
            toolbarnoonreportlist.AddFontAwesomeButton("../plannedmaintenance/plannedmaintenancevesselcomponentexceptionralist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            MenuParameterList.AccessRights = this.ViewState;
            MenuParameterList.MenuList = toolbarnoonreportlist.Show();

            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvParameterList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        
        DataTable dt = PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselComponentsExceptionRAList(General.GetNullableInteger(ucVessel.SelectedVessel));
        gvParameterList.DataSource = dt;      
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
    private void Rebind()
    {
        gvParameterList.SelectedIndexes.Clear();
        gvParameterList.EditIndexes.Clear();
        gvParameterList.DataSource = null;
        gvParameterList.Rebind();
    }
    protected void gvParameterList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("MAP"))
            {
                PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselComponentsExceptionRAMAP(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblcomponentid")).Text)
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblglobalRAid")).Text), General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblvesselid")).Text));

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuParameterList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns;
        string[] alCaptions;

            alColumns = new string[2] { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME"};
            alCaptions = new string[2] { "Component Number", "Component Name" };

        DataTable dt = PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselComponentsExceptionRAList(General.GetNullableInteger(ucVessel.SelectedVessel));
        gvParameterList.DataSource = dt;

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ExceptionList.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Exception List</h3></td>");
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
}
