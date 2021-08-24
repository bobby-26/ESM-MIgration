using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class PlannedMaintenance_PlannedMaintenanceVesselWiseReportListFormat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselWiseReportListFormat.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTemplate')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselWiseReportListFormat.aspx", "Find", "search.png", "FIND");
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
         
                ddlTemplate.DataSource = PhoenixPlannedMaintenanceVesselCertificateTemplate.TemplateList();
                ddlTemplate.DataBind();
                ddlTemplate.Items.Insert(0, new ListItem("--Select--", "DUMMY"));
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    txtVessel.Visible = true;
                    ucVessel.Visible = false;
                    txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                }
                else
                {
                    txtVessel.Visible = false;
                    ucVessel.Visible = true;
                }
            }
            
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {
        int? vessel = null;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else
        {
            vessel = General.GetNullableInteger(ucVessel.SelectedVessel);
        }
        int? templateid = General.GetNullableInteger(ddlTemplate.SelectedValue);

        if (IsPostBack)
        {
            if (!IsValidInput(vessel, templateid))
            {
                ucError.Visible = true;
                return;
            }
       

            DataSet ds = PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselTemplateSearch(vessel,templateid);
            

            string[] alColumns = { "FLDCERTIFICATENAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDNEXTDUEDATE", "FLDWINDOWPERIODBEFORE", "FLDSURVEYTYPENAME", "FLDPLANDATE", "FLDSEAPORTNAME", "FLDANNIVERSARYDATE", "FLDLASTSURVEYDATE", "FLDLASTSURVEYTYPENAME", "FLDCERTIFICATEREMARKS" };
            string[] alCaptions = { "Certificate", "Issue Date", "Expiry Date", "Issued By", "Due Date for Survey/Follow up", "Window(Range)", "Type of Next Survey", "Planned Date of Survey", "Port", "Date of Initial Audit/Anniversary Date", "Date of last Survey", "Type of Last Audit/Survey", "Remarks" };

            General.SetPrintOptions("gvTemplate", "Certificates Multiple Formats", alCaptions, alColumns, ds);
       
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTemplate.DataSource = ds;
            gvTemplate.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt,gvTemplate);
        }
        }
    }

    private bool IsValidInput(int? vesselid, int? templateid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vesselid.Equals(null))
        {
            ucError.ErrorMessage = "Please Select the Vessel";
        }
        if (templateid.Equals(null))
        {
            ucError.ErrorMessage = "Please Select the Template";
        }
        return (!ucError.IsError);
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
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
        try
        {
            int? vessel = null;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
            else
            {
                vessel = General.GetNullableInteger(ucVessel.SelectedVessel);
            }
            int? templateid = General.GetNullableInteger(ddlTemplate.SelectedValue);

            if (IsPostBack)
            {
                if (!IsValidInput(vessel, templateid))
                {
                    ucError.Visible = true;
                    return;
                }
            }

            DataSet ds = PhoenixPlannedMaintenanceVesselCertificateTemplate.VesselTemplateSearch(vessel, templateid);
            DataTable dt = ds.Tables[0];

            string[] alColumns = { "FLDCERTIFICATENAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDNEXTDUEDATE", "FLDWINDOWPERIODBEFORE", "FLDSURVEYTYPENAME", "FLDPLANDATE", "FLDSEAPORTNAME", "FLDANNIVERSARYDATE", "FLDLASTSURVEYDATE", "FLDLASTSURVEYTYPENAME", "FLDCERTIFICATEREMARKS" };
            string[] alCaptions = { "Certificate Name", "Issue Date", "Expiry Date", "Issued By", "Due Date for Survey/Follow up", "Window(Range)", "Type of Next Survey", "Planned Date of Survey", "Port", "Date of Initial Audit/Anniversary Date", "Date of last Survey", "Type of Last Audit/Survey", "Remarks" };

            General.ShowExcel("Certificates Multiple Formats", dt, alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

}
