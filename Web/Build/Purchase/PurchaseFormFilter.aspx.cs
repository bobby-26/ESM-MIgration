using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;
using System.Data;
using Telerik.Web.UI;
public partial class PurchaseFormFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("GO", "GO",ToolBarDirection.Right);
            MenuFormFilter.MenuList = toolbarmain.Show();
            //MenuFormFilter.SetTrigger(pnlDiscussion);  

            txtVendor.Attributes.Add("style", "visibility:hidden");
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtDeliveryLocationId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            //txtMakerId.Attributes.Add("style", "visibility:hidden");  

            btnSubmit.Attributes.Add("style", "border:0px;background-color:White");

            if (!IsPostBack)
            {
                UCPeority.QuickTypeCode = ((int)PhoenixQuickTypeCode.ORDERPRIORITY).ToString();
                UCrecieptCondition.HardTypeCode = ((int)PhoenixHardTypeCode.RECIEPTCONDITION).ToString();
                ucFormState.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATE).ToString();
                ucApproval.HardTypeCode = ((int)PhoenixHardTypeCode.APPROVAL).ToString();
                ucFinacialYear.QuickTypeCode = ((int)(PhoenixQuickTypeCode.YEARS)).ToString();
                ucFormStatus.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();
                ucFormType.HardTypeCode = ((int)PhoenixHardTypeCode.FORMTYPE).ToString();
                ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                BindDepartment();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFormFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStockType", ddlStockType.SelectedValue);
                criteria.Add("txtNumber", txtFormNumber.Text);
                criteria.Add("txtTitle", txtFromTitle.Text);
                criteria.Add("txtVendorid", txtVendor.Text);
                criteria.Add("txtDeliveryLocationId", txtDeliveryLocationId.Text);
                criteria.Add("txtBudgetId", txtBudgetId.Text);
                criteria.Add("txtBudgetgroupId", txtBudgetgroupId.Text);
                criteria.Add("ucFinacialYear", ucFinacialYear.SelectedQuick.ToString());
                criteria.Add("ucFormState", ucFormState.SelectedHard.ToString());
                criteria.Add("ucApproval", ucApproval.SelectedHard.ToString());
                criteria.Add("UCrecieptCondition", UCrecieptCondition.SelectedHard.ToString());
                criteria.Add("UCPeority", UCPeority.SelectedQuick.ToString());
                criteria.Add("ucFormStatus", ucFormStatus.SelectedHard.ToString());
                criteria.Add("ucFormType", ucFormType.SelectedHard.ToString());
                criteria.Add("ucComponentclass", ddlComponentClass.SelectedQuick);
                criteria.Add("txtMakerReference", txtMakerReference.Text.Trim());
                criteria.Add("txtOrderedDate", txtOrderedDate.Text);
                criteria.Add("txtOrderedToDate", txtOrderedToDate.Text);
                criteria.Add("txtCreatedDate", txtCreatedDate.Text);
                criteria.Add("txtCreatedToDate", txtCreatedToDate.Text);
                criteria.Add("txtApprovedDate", txtApprovedDate.Text);
                criteria.Add("txtApprovedToDate", txtApprovedToDate.Text);
                criteria.Add("ddlDepartment", ddlDepartment.SelectedValue);
                criteria.Add("ddlReqStatus", ddlReqStatus.SelectedValue);
                criteria.Add("ucReason4Requisition", ucReason4Requisition.SelectedValue);
                Filter.CurrentOrderFormFilterCriteria = criteria;

                //NameValueCollection makercriteria = new NameValueCollection();
                //makercriteria.Clear();
                //makercriteria.Add("txtMakerId", txtMakerId.Text);
                //Filter.CurrentMakerReference = makercriteria;
                Filter.CurrentPurchaseDashboardCode = null;


            }
            
            Response.Redirect("../Purchase/PurchaseForm.aspx",false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ddlStockType", ddlStockType.SelectedValue);
            criteria.Add("txtNumber", txtFormNumber.Text);
            criteria.Add("txtTitle", txtFromTitle.Text);
            criteria.Add("txtVendorid", txtVendor.Text);
            criteria.Add("txtDeliveryLocationId", txtDeliveryLocationId.Text);
            criteria.Add("txtBudgetId", txtBudgetId.Text);
            criteria.Add("txtBudgetgroupId", txtBudgetgroupId.Text);
            criteria.Add("ucFinacialYear", ucFinacialYear.SelectedQuick.ToString());
            criteria.Add("ucFormState", ucFormState.SelectedHard.ToString());
            criteria.Add("ucApproval", ucApproval.SelectedHard.ToString());
            criteria.Add("UCrecieptCondition", UCrecieptCondition.SelectedHard.ToString());
            criteria.Add("UCPeority", UCPeority.SelectedQuick.ToString());
            criteria.Add("ucFormStatus", ucFormStatus.SelectedHard.ToString());
            criteria.Add("ucFormType", ucFormType.SelectedHard.ToString());
            criteria.Add("ucComponentclass", ddlComponentClass.SelectedQuick);
            criteria.Add("txtMakerReference", txtMakerReference.Text.Trim());
            criteria.Add("txtOrderedDate", txtOrderedDate.Text);
            criteria.Add("txtOrderedToDate", txtOrderedToDate.Text);
            criteria.Add("txtCreatedDate", txtCreatedDate.Text);
            criteria.Add("txtCreatedToDate", txtCreatedToDate.Text);
            criteria.Add("txtApprovedDate", txtApprovedDate.Text);
            criteria.Add("txtApprovedToDate", txtApprovedToDate.Text);
            criteria.Add("ddlDepartment", ddlDepartment.SelectedValue);
            criteria.Add("ddlReqStatus", ddlReqStatus.SelectedValue);
            criteria.Add("ucReason4Requisition", ucReason4Requisition.SelectedValue);
            Filter.CurrentOrderFormFilterCriteria = criteria;

            //NameValueCollection makercriteria = new NameValueCollection();
            //makercriteria.Clear();
            //makercriteria.Add("txtMakerId", txtMakerId.Text);
            //Filter.CurrentMakerReference = makercriteria;
            Filter.CurrentPurchaseDashboardCode = null;


            Response.Redirect("../Purchase/PurchaseForm.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDepartment()
    {
        DataSet ds = PhoenixPurchaseOrderForm.DepartmentList();
        ddlDepartment.DataSource = ds;
        ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
        ddlDepartment.DataValueField = "FLDDEPARTMENTID";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new DropDownListItem("--Select--", ""));

    }
}
