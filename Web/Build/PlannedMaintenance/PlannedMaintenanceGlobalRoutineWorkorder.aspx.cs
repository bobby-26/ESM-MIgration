using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class PlannedMaintenanceGlobalRoutineWorkorder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvWorkorder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorder.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorder.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorder.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorderAdd.aspx','false','420px','300px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        MenuRoutineWorkorder.AccessRights = this.ViewState;
        MenuRoutineWorkorder.MenuList = toolbar.Show();


    }


    protected void MenuRoutineWorkorder_TabStripCommand(object sender, EventArgs e)
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
                UcDiscipline.SelectedDiscipline = "";
                txtTitle.Text = "";
                Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTITLE", "FLDDISCIPLINENAME" };
        string[] alCaptions = { "Title", "Responsibility" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkordersearch(General.GetNullableString(txtTitle.Text.Trim()),
                    General.GetNullableInteger(UcDiscipline.SelectedDiscipline),
                 sortexpression, sortdirection,
                 gvWorkorder.CurrentPageIndex + 1,
                 gvWorkorder.PageSize,
           ref iRowCount,
           ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RoutineWorkorder.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Routine Workorder</h3></td>");
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


    protected void Rebind()
    {
        gvWorkorder.SelectedIndexes.Clear();
        gvWorkorder.EditIndexes.Clear();
        gvWorkorder.DataSource = null;
        gvWorkorder.Rebind();
    }

    protected void gvWorkorder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkorder.CurrentPageIndex + 1;
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

            DataSet ds = PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkordersearch(General.GetNullableString(txtTitle.Text.Trim()),
                     General.GetNullableInteger(UcDiscipline.SelectedDiscipline),
                  sortexpression, sortdirection,
                  gvWorkorder.CurrentPageIndex + 1,
                  gvWorkorder.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            string[] alColumns = { "FLDTITLE", "FLDDISCIPLINENAME" };
            string[] alCaptions = { "Title", "Responsibility" };

            General.SetPrintOptions("gvWorkorder", "Routine Workorder", alCaptions, alColumns, ds);

            gvWorkorder.DataSource = ds;
            gvWorkorder.VirtualItemCount = iRowCount;

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
            gvWorkorder.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void gvWorkorder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? RoutineWO = General.GetNullableGuid(item.GetDataKeyValue("FLDROUTINEWOID").ToString());
                string Title = General.GetNullableString(((RadLabel)e.Item.FindControl("lblTitle")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Routine Workorder','PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorderEdit.aspx?ROUTINEWOID=" + RoutineWO + "','false','420px','300px');return false");
                }

                LinkButton job = ((LinkButton)item.FindControl("cmdComjob"));
                if(job != null)
                {
                    job.Attributes.Add("onclick", "javascript:openNewWindow('Filters', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceGlobalRoutineWorkorderDetails.aspx?ROUTINEWOID=" +RoutineWO + "&Title=" + Title + "');return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkorderDelete(General.GetNullableGuid(ViewState["RoutineWoId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkorder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["RoutineWoId"] = ((RadLabel)e.Item.FindControl("lblRoutineWoId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
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
}