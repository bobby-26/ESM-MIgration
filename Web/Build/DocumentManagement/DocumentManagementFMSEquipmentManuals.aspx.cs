using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using Telerik.Web.Spreadsheet;

public partial class DocumentManagementFMSEquipmentManuals : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            BindMenu();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    ddlvessel.Enabled = false;
                }
                else
                {
                    ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMenu()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("E Mails", "ESMA", ToolBarDirection.Left);
            toolbar.AddButton("ESM Filing", "ESMF", ToolBarDirection.Left);

            toolbar.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);

            toolbar.AddButton("Office Forms", "OFFF", ToolBarDirection.Left);

            toolbar.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbar.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbar.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbar.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);

            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbar.Show();
            MenuFMS.SelectedMenuIndex = 7;

        }
        else
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);

            toolbarmain.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbarmain.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbarmain.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbarmain.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);

            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbarmain.Show();
            MenuFMS.SelectedMenuIndex = 4;
        }
    }

    protected void MenuHistoryTemplate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "FIND")
            {
                if (General.GetNullableString(ddlvessel.SelectedVessel) == null)
                {
                    ucError.HeaderMessage = "Please Provide following information";
                    ucError.ErrorMessage = "Vessel.";
                    ucError.Visible = true;
                    return;
                }
                gvComponent.Rebind();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                gvComponent.CurrentPageIndex = 0;
                txtNumberSearch.Text = "";
                txtComponentNameSearch.Text = "";
                BindData();
                gvComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFMS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ESMA"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("ESMF"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SPFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2", false);
            }
            if (CommandName.ToUpper().Equals("OFFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1", false);
            }
            if (CommandName.ToUpper().Equals("MCFS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DRWS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSDrawingList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("MNSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSVesselSurveyScheduleList.aspx?", false);
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
            int? sortdirection = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixRegisterFMSMail.FMSComponentSearch(ddlvessel.SelectedValue
                                                                            , txtNumberSearch.Text
                                                                            , txtComponentNameSearch.Text
                                                                            , null
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , gvComponent.CurrentPageIndex + 1
                                                                            , gvComponent.PageSize
                                                                            , ref iRowCount, ref iTotalPageCount);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidNamee(string formname)
    {
        ucError.HeaderMessage = "Please enter a valid data.";

        if (string.IsNullOrEmpty(formname))
            ucError.ErrorMessage = "Enter name of the maintenance form name.";

        return (!ucError.IsError);
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper().Equals("REPORTS"))
            {
                //RadLabel formid = ((RadLabel)e.Item.FindControl("lblFormID"));
                //RadLabel lblFormName = ((RadLabel)e.Item.FindControl("lblFormName"));
                //Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplateReports.aspx?FORMID=" + formid.Text + "&FORMNAME=" + lblFormName.Text + "&VESSELID=" + ddlvessel.SelectedVessel, true);


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                LinkButton edit = ((LinkButton)item.FindControl("cmdReports"));
                if (edit != null)
                {
                    RadLabel compid = ((RadLabel)e.Item.FindControl("lblComponentId"));
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','','DocumentManagement/DocumentFMSEquipmentManualAttachmentList.aspx?JOBYN=0&COMPONENTJOBID=" + compid.Text + "&VESSELID=" + ddlvessel.SelectedValue + "','false','800px','420px');return false");

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
