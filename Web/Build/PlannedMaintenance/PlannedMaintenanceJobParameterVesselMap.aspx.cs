using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;

public partial class PlannedMaintenanceJobParameterVesselMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Revise", "REVISE", ToolBarDirection.Right);
            toolbar.AddButton("Copy", "COPY", ToolBarDirection.Right);
            menuRevise.AccessRights = this.ViewState;
            menuRevise.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameterVesselMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameterVesselMap.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameterVesselMap.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuParameter.AccessRights = this.ViewState;
            MenuParameter.MenuList = toolbar1.Show();
            
            if (!IsPostBack)
            {
                ViewState["VesselId"] = string.Empty;
                gvJobParameter.PageSize = General.ShowRecords(gvJobParameter.PageSize);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ucVessel.Visible = false;
                    lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                    lblVesselName.Visible = true;
                    ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                }
                else
                {
                    ucVessel.Visible = true;
                    if(PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        ucVessel.Enabled = false;
                        ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    }
                    lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                    lblVesselName.Visible = false;
                }
                if(!string.IsNullOrEmpty(ViewState["VesselId"].ToString()))
                    BindRevisionDetail();
    
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPARAMETERNAME", "FLDMINVALUE", "FLDMAXVALUE" };
            string[] alCaptions = { "Parameter", "Minimum", "Maximum" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            else
                ViewState["VesselId"] = ucVessel.SelectedVessel;

            DataTable dt = PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselSearch(General.GetNullableInteger(ViewState["VesselId"].ToString()),
                                                                                            General.GetNullableGuid(ucComponent.SelectedValue),
                                                                                            General.GetNullableGuid(ucJob.SelectedValue),
                                                                                            sortexpression, sortdirection,
                                                                                            gvJobParameter.CurrentPageIndex + 1,
                                                                                            gvJobParameter.PageSize,
                                                                                            ref iRowCount, ref iTotalPageCount);

            gvJobParameter.Columns.FindByUniqueName("Action").Visible = SessionUtil.CanAccess(this.ViewState, "EDIT");
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvJobParameter", "Job Parameter", alCaptions, alColumns, ds);

            gvJobParameter.DataSource = dt;
            gvJobParameter.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"]  = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvJobParameter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string vslId = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string compId = ((RadLabel)e.Item.FindControl("lblCompId")).Text;
                string jobId = ((RadLabel)e.Item.FindControl("lblJobId")).Text;
                string parameterId = ((RadLabel)e.Item.FindControl("lblParameterId")).Text;
                string minValue = ((UserControlMaskNumber)e.Item.FindControl("txtMinValueEdit")).Text;
                string maxValue = ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueEdit")).Text;

                //if (!isValidParameter(minValue, maxValue))
                //{
                //    e.Canceled = true;
                //    ucError.Visible = true;
                //    return;
                //}

                PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselMap(int.Parse(vslId), new Guid(compId),new Guid(jobId), new Guid(parameterId)
                                                                                , General.GetNullableDecimal(minValue), General.GetNullableDecimal(maxValue));
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }
        }
    }
    private bool isValidParameter(string min,string max)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(min))
            ucError.ErrorMessage = "Minimum value is required.";

        if (string.IsNullOrEmpty(max))
            ucError.ErrorMessage = "Maximum value is required.";

        return (!ucError.IsError);
    }
    
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            ViewState["VesselId"] = General.GetNullableInteger(ucVessel.SelectedVessel);

        ucComponent.selectedvesselid = int.Parse(ViewState["VesselId"].ToString());

        gvJobParameter.Rebind();

        BindRevisionDetail();
    }

    protected void MenuParameter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "FIND")
            {
                gvJobParameter.CurrentPageIndex = 0;
                gvJobParameter.Rebind();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ucJob.SelectedValue = "";
                ucJob.Text = "";
                ucComponent.SelectedValue = "";
                ucComponent.Text = "";
                gvJobParameter.CurrentPageIndex = 0;
                gvJobParameter.Rebind();
            }
            else if (CommandName.ToUpper() == "EXCEL")
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

    protected void ucJob_TextChangedEvent(object sender, EventArgs e)
    {
        gvJobParameter.CurrentPageIndex = 0;
        gvJobParameter.Rebind();
    }

    protected void menuRevise_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string commandName = ((RadToolBarButton)dce.Item).CommandName;
            if (commandName.ToUpper() == "REVISE")
            {
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceJobParameterVesselRevision.aspx?VESSELID=" + ViewState["VesselId"] + "';showDialog('Parameter Revision" + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if(commandName.ToUpper() == "COPY")
            {
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceJobParameterVesselCopy.aspx?VESSELID=" + ViewState["VesselId"] + "';showDialog('Parameter Copy" + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindRevisionDetail()
    {
        txtRevisionNo.Text = "";
        dtRevisedDate.SelectedDate = General.GetNullableDateTime("");
        txtReason.Text = "";

        DataTable dt;
        dt = PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselRevisionDetail(int.Parse(ViewState["VesselId"].ToString()));
        if(dt.Rows.Count > 0)
        {
            txtRevisionNo.Text = dt.Rows[0]["FLDREVISIONNO"].ToString();
            dtRevisedDate.SelectedDate = General.GetNullableDateTime(dt.Rows[0]["FLDREVISEDDATE"].ToString());
            txtReason.Text = dt.Rows[0]["FLDREASON"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindRevisionDetail();
    }

    protected void cmdRevision_Click(object sender, EventArgs e)
    {
        string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceJobParameterVesselRevisionHistory.aspx?VESSELID=" + ViewState["VesselId"] + "';showDialog('Revision History" + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDJOB", "FLDCOMPONENT","FLDPARAMETERNAME", "FLDMINVALUE", "FLDMAXVALUE" };
            string[] alCaptions = { "Job", "Component","Parameter", "Minimum", "Maximum" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvJobParameter.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            else
                ViewState["VesselId"] = ucVessel.SelectedVessel;

            DataTable dt = PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselSearch(General.GetNullableInteger(ViewState["VesselId"].ToString()),
                                                                                            General.GetNullableGuid(ucComponent.SelectedValue),
                                                                                            General.GetNullableGuid(ucJob.SelectedValue),
                                                                                            sortexpression, sortdirection,
                                                                                            1,
                                                                                            iRowCount,
                                                                                            ref iRowCount, ref iTotalPageCount);


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.ShowExcel("Parameter", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
