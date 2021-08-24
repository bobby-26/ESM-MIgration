using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRiskAssessmentPendingApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceRiskAssessmentPendingApproval.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPostponedApproval')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceRiskAssessmentPendingApproval.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceRiskAssessmentPendingApproval.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuPostponedApproval.AccessRights = this.ViewState;
            MenuPostponedApproval.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvPostponedApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucVessel.bind();
                ucVessel.DataBind();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() != "0")
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                    ucVessel.Visible = false;
                    lblvesselname.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                    lblvesselname.Visible = true;
                }

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPostponedApproval_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvPostponedApproval.CurrentPageIndex = 0;
                gvPostponedApproval.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = "";
                }
                txtnumber.Text = "";
                gvPostponedApproval.CurrentPageIndex = 0;
                gvPostponedApproval.Rebind();
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDDATE", "FLDPURPOSE", "FLDWORKORDERNUMBER", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDINTENDEDWORKDATE" };
            string[] alCaptions = { "Vessel", "WO Number", "Prepared", "Purpose", "WO Number", "Component Number", "Component Name", "Title", "Priority", "Due Date", "Intended Work" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceRiskAssessmentApproval.PMSRAPendingApprovalSearch(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableString(txtnumber.Text),
                    null, sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("RA Pending Approval", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDDATE", "FLDPURPOSE", "FLDWORKORDERNUMBER", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDINTENDEDWORKDATE" };
            string[] alCaptions = { "Vessel", "WO Number", "Prepared", "Purpose", "WO Number", "Component Number", "Component Name", "Title", "Priority", "Due Date", "Intended Work" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixPlannedMaintenanceRiskAssessmentApproval.PMSRAPendingApprovalSearch(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableString(txtnumber.Text), null,
                        sortexpression, sortdirection,
                        gvPostponedApproval.CurrentPageIndex + 1,
                        gvPostponedApproval.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

            General.SetPrintOptions("gvPostponedApproval", "RA Pending Approval", alCaptions, alColumns, ds);

            gvPostponedApproval.DataSource = ds;
            gvPostponedApproval.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPostponedApproval_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            ImageButton imgApprove = (ImageButton)e.Item.FindControl("imgApprove");
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAid");

            if (lblInstallcode != null)
            {
                if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                {
                    if (imgApprove != null)
                    {
                        imgApprove.ToolTip = "Emergency Override";
                        imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblRAid.Text + "&TYPE=3','medium'); return true;");
                    }
                }
                else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                {
                    if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblRAid.Text + "&TYPE=3','large'); return true;");
                        }
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

    protected void gvPostponedApproval_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                ImageButton imgApprove = (ImageButton)e.Item.FindControl("imgApprove");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAid");

                if (lblInstallcode != null)
                {
                    if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.ToolTip = "Emergency Override";
                            imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblRAid.Text + "&TYPE=3','medium'); return true;");
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblRAid.Text + "&TYPE=3','large'); return true;");
                            }
                        }
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

    protected void gvPostponedApproval_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click1(object sender, EventArgs e)
    {
        gvPostponedApproval.Rebind();
    }
}
