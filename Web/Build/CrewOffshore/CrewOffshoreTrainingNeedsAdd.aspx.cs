using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingNeedsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            confirm.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    
                    ucVessel.Enabled = false;
                    ucVessel.bind();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
                BindEmployee();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        BindEmployee();
    }
    protected void BindEmployee()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = PhoenixCrewOffshoreTrainingCategory.ListOffshoreOnBoardEmployee(int.Parse(ucVessel.SelectedVessel));
            ddlEmployee.DataTextField = "FLDNAME";
            ddlEmployee.DataValueField = "FLDEMPLOYEEID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlSubCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlEmployee.SelectedValue) != null)
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(int.Parse(ucVessel.SelectedVessel), int.Parse(ddlEmployee.SelectedValue));
            else
                dt = PhoenixCrewPersonal.EmployeeList(int.Parse(ddlEmployee.SelectedValue));

            ddlCategoryAdd.Items.Clear();
            ddlCategoryAdd.DataSource = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null, int.Parse(dt.Rows[0]["FLDRANKPOSTED"].ToString()));
            ddlCategoryAdd.DataTextField = "FLDCATEGORYNAME";
            ddlCategoryAdd.DataValueField = "FLDCATEGORYID";
            ddlCategoryAdd.DataBind();
            ddlCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }
    protected void ddlCategoryAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategoryAdd.Items.Clear();
        ddlSubCategoryAdd.DataSource = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingSubCategory(General.GetNullableInteger(ddlCategoryAdd.SelectedValue),1);
        ddlSubCategoryAdd.DataTextField = "FLDNAME";
        ddlSubCategoryAdd.DataValueField = "FLDSUBCATEGORYID";
        ddlSubCategoryAdd.DataBind();
        ddlSubCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }
                DataTable dt = new DataTable();
                dt = PhoenixCrewOffshoreTrainingNeeds.TrainingneedsList(
                                                     int.Parse(ddlEmployee.SelectedValue)
                                                     , General.GetNullableInteger(ucTypeofTrainingAdd.SelectedQuick)
                                                     , null
                                                     , General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue));
            
                if(dt.Rows.Count>0)
                {
                    //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
                    //ucConfirmCrew.Visible = true;
                    //ucConfirmCrew.Text = "This Course has already assigned, do you want to add again";

                    RadWindowManager1.RadConfirm("This Course has already assigned, do you want to add again", "confirm", 320, 150, null, "Confirm");

                }
                else
                {

                    PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ucVessel.SelectedVessel),
                       int.Parse(ddlEmployee.SelectedValue), null,
                       General.GetNullableInteger(ddlCategoryAdd.SelectedValue),
                       General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue),
                       General.GetNullableString(txtTrainignNeed.Text),
                       General.GetNullableInteger(ucImprovementAdd.SelectedQuick),
                       null,
                       General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue),
                       General.GetNullableInteger(ucTypeofTrainingAdd.SelectedQuick),
                       General.GetNullableString(txtRemarks.Text));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('Filter');", true);
                }


                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += " fnReloadList();";
                //Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('Filter');", true);   
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ucVessel.SelectedVessel),
                   int.Parse(ddlEmployee.SelectedValue), null,
                   General.GetNullableInteger(ddlCategoryAdd.SelectedValue),
                   General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue),
                   General.GetNullableString(txtTrainignNeed.Text),
                   General.GetNullableInteger(ucImprovementAdd.SelectedQuick),
                   null,
                   General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue),
                   General.GetNullableInteger(ucTypeofTrainingAdd.SelectedQuick),
                   General.GetNullableString(txtRemarks.Text));

              
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ddlCategoryAdd.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableInteger(ddlSubCategoryAdd.SelectedValue) == null)
            ucError.ErrorMessage = "SubCategory is required.";

        if (General.GetNullableString(txtTrainignNeed.Text) == null)
            ucError.ErrorMessage = "Identified training need is required.";

        if (General.GetNullableInteger(ucImprovementAdd.SelectedQuick) == null)
            ucError.ErrorMessage = "Level of improvement is required.";

        if (General.GetNullableInteger(ucTypeofTrainingAdd.SelectedQuick) == null)
            ucError.ErrorMessage = "Type of Training is required";

        return (!ucError.IsError);
    }
}
