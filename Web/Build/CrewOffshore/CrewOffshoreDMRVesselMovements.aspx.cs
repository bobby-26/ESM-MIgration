using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRVesselMovements : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
             //   BindData();

            }
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                foreach (GridDataItem gvr in gvVesselMovements.MasterTableView.Items)
                {
                    if (gvVesselMovements.Items.Count > 0)
                    {
                        RadDropDownList ddlActivity = (RadDropDownList)gvr.FindControl("ddlActivityAdd");
                        RadLabel OperationalTaskId = (RadLabel)gvr.FindControl("lblOperationalTaskId");
                        RadTextBox txtDescription = (RadTextBox)gvr.FindControl("txtDescriptionAdd");

                        string FromTime = (((UserControlMaskedTextBox)gvr.FindControl("txtFromTimeAdd")).TextWithLiterals.Trim() == "__:__") ? string.Empty : ((UserControlMaskedTextBox)gvr.FindControl("txtFromTimeAdd")).TextWithLiterals;
                        string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;

                        
                        if (fromtime != "" && ddlActivity.SelectedValue!="")
                        {
                            if (!IsValidVesselMovements(fromtime))
                            {
                                ucError.Visible = true;
                                return;
                            }
                            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                                    , General.GetNullableGuid(ddlActivity.SelectedValue), General.GetNullableString(txtDescription.Text), General.GetNullableDateTime(Session["MIDNIGHTREPORTDATE"].ToString() + " " + fromtime));
                        }
                    }
                }
                String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);

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
        string[] alColumns = { "FLDTASKNAME" };
        string[] alCaptions = { "Operational Task Name" };

        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.DMRListOperationalTaskSearch();

        General.SetPrintOptions("gvVesselMovements", "DMR Vessel Movements", alCaptions, alColumns, ds);
        gvVesselMovements.DataSource = ds;
      

    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    //protected void gvVesselMovements_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DropDownList at = (DropDownList)e.Row.FindControl("ddlActivityAdd");
    //        if (at != null)
    //        {
    //            DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
    //            at.DataSource = ds;
    //            at.DataBind();
    //            at.Items.Insert(0, new ListItem("--Select--", ""));
    //        }


    //    }
    //}
    private bool IsValidVesselMovements(string fromtime)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (fromtime.Trim().Equals(""))
            ucError.ErrorMessage = "From Time is required.";
        if (General.GetNullableDateTime(Session["MIDNIGHTREPORTDATE"].ToString() + " " + fromtime) == null)
            ucError.ErrorMessage = "Invalid From Time.";
        return (!ucError.IsError);
    }


    protected void gvVesselMovements_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVesselMovements_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadDropDownList at = (RadDropDownList)e.Item.FindControl("ddlActivityAdd");
            if (at != null)
            {
                DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
                at.DataSource = ds;
                at.DataBind();
               // at.Items.Insert(0, new ListItem("--Select--", ""));
            }


        }
    }
}
