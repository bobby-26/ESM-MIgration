using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;

public partial class PlannedMaintenanceWorkOrderFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuWorkOrderFilter.MenuList = toolbarmain.Show();

            txtTmpComponentName.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {

                if (Request.QueryString["Workorder"] != null && Request.QueryString["Workorder"].ToString() == "1"
                    && ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null)
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx");
                }
                ckPlaning.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.PLANNING));
                ckPlaning.DataBind();
                chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
                chkClasses.DataBind();
                chkStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERSTATUS), 0, "REQ,CAN,POP,ISS");
                chkStatus.DataBind();
                DataSet ds = PhoenixRegistersDiscipline.ListDiscipline();
                cblResponsibility.DataSource = ds;
                cblResponsibility.DataBind();
                ddlResponsibility.DataSource = ds;
                ddlResponsibility.DataBind();
                ddlResponsibility.Items.Insert(0, new RadComboBoxItem() { Text = "All", Value = "" });
                //ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STOCKCLASS).ToString();
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
                //WorkorderDataCorrection();
                if (Request.QueryString["frm"] != null)
                {
                    //wrinfo.Visible = false;
                    //compinfo.Visible = false;
                    //respinfo.Visible = false;
                    Div3.Attributes["style"] = "overflow: auto; width: 70%;";
                    ddlResponsibility.Visible = true;
                    cblResponsibility.Visible = false;
                    //statusinfo.Visible = false;
                    //woinfo.Visible = false;
                    rainfo.Visible = false;
                    //definfo.Visible = false;
                    lblPlanning.Visible = false;
                    dvPlaning.Visible = false;
                    lblUnplannedWork.Visible = false;
                    chkUnexpected.Visible = false;
                    chkDefect.Visible = false;
                    ddlDefect.Visible = true;
                    txtDateFrom.Visible = false;
                    txtDateTo.Visible = false;
                    ddlDueDays.Visible = true;
                }
                else
                {
                    cblResponsibility.Visible = true;
                    ddlResponsibility.Visible = false;
                    chkUnexpected.Visible = true;
                    chkDefect.Visible = true;
                    ddlDefect.Visible = false;
                    txtDateFrom.Visible = true;
                    txtDateTo.Visible = true;
                    ddlDueDays.Visible = false;
                }
            }
            img1.Attributes.Add("onclick", "javascript:return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx', true);");

            //onclick = "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true); "
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrderFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string ststus = "";
                string planning = "";
                string jobclass = "";
                string responsibility = "";
                foreach (ButtonListItem item in chkStatus.Items)
                {
                    if (item.Selected)
                        ststus = ststus + item.Value + ",";
                }
                foreach (ButtonListItem item in ckPlaning.Items)
                {
                    if (item.Selected)
                        planning = planning + item.Value + ",";
                }
                foreach (ButtonListItem item in chkClasses.Items)
                {
                    if (item.Selected)
                        jobclass = jobclass + item.Value + ",";
                }
                if (Request.QueryString["frm"] != null)
                {
                    responsibility = ddlResponsibility.SelectedValue;
                }
                else
                {
                    foreach (ButtonListItem item in cblResponsibility.Items)
                    {
                        if (item.Selected)
                            responsibility = responsibility + item.Value + ",";
                    }
                }


                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
                if (Request.QueryString["frm"] != null)
                {
                    if (txtTitle.Text.Equals("") && !txtWorkOrderNumber.Text.Equals(""))
                        criteria.Add("txtWorkOrderName", txtWorkOrderNumber.Text);
                    else
                        criteria.Add("txtWorkOrderName", txtTitle.Text);
                }
                else
                {
                    criteria.Add("txtWorkOrderName", txtTitle.Text);
                }
                criteria.Add("txtComponentNumber", txtComponentNumber.Text.TrimEnd("000.00.00".ToCharArray()));
                criteria.Add("txtComponentName", txtComponentName.Text.Trim());
                criteria.Add("ucRank", responsibility.TrimEnd(new Char[] { ',' }));
                criteria.Add("txtDateFrom", General.GetNullableDateTime(txtDateFrom.Text).ToString());
                criteria.Add("txtDateTo", General.GetNullableDateTime(txtDateTo.Text).ToString());
                criteria.Add("status", ststus.TrimEnd(new Char[] { ',' }));
                criteria.Add("planning", planning.TrimEnd(new Char[] { ',' }));
                criteria.Add("jobclass", jobclass.TrimEnd(new Char[] { ',' }));
                criteria.Add("ucMainType", ucMainType.SelectedQuick);
                criteria.Add("ucMainCause", ucMainCause.SelectedQuick);
                criteria.Add("ucMaintClass", ucMaintClass.SelectedQuick);                
                criteria.Add("txtPriority", txtPriority.Text);
                if (Request.QueryString["frm"] != null)
                {
                    criteria.Add("chkDefect", ddlDefect.SelectedValue);
                    criteria.Add("DUE", ddlDueDays.SelectedValue);
                }
                else
                {
                    criteria.Add("chkUnexpected", chkUnexpected.Checked == true ? "1" : string.Empty);
                    criteria.Add("chkDefect", (chkDefect.Checked == true ? "1" : string.Empty));
                }
                criteria.Add("txtClassCode", txtClassCode.Text);
                criteria.Add("chkRaPendingApproval", (chkRaPendingApproval.Checked == true ? "1" : string.Empty));
                criteria.Add("chkRaRequired", (chkRaRequired.Checked == true ? "1" : string.Empty));
                criteria.Add("chkPlannedInWo", (chkPlannedInWo.Checked == true ? "1" : string.Empty));
                criteria.Add("JobCategory", ucJobCategory.SelectedValue);
                criteria.Add("CATEGORY", ddlComponentCategory.SelectedQuick);
                Filter.CurrentWorkOrderFilter = criteria;
                if (Request.QueryString["frm"] != null)
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?");
                }
                else
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void WorkorderDataCorrection()
    {
        PhoenixCommonPlannedMaintenance.WorkorderExtnInsert();
        PhoenixCommonPlannedMaintenance.WorkOrderRAstatusUpdate();
    }
}
