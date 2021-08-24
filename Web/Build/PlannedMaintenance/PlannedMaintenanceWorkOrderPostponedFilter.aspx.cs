using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderPostponedFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuComponentFilter.MenuList = toolbarmain.Show();
            // txtMakerId.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {
                ddlResponsibility.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
                ddlResponsibility.DataBind();
                //chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
                //chkClasses.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentFilter_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                //string ststus = "";
                //string planning = "";
                //string jobclass = "";
                string responsibility = ddlResponsibility.SelectedValue;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();

                //foreach (ButtonListItem item in chkClasses.Items)
                //{
                //    if (item.Selected)
                //        jobclass = jobclass + item.Value + ",";
                //}

                criteria.Add("txtComponent", txtComponent.SelectedValue);
                criteria.Add("responsibility", responsibility);
                criteria.Add("txtclasscode", txtclasscode.Text);
                criteria.Add("ucComponentCategory", ucComponentCategory.SelectedQuick);
                criteria.Add("ucstatus", ucstatus.SelectedHard);
                criteria.Add("rblpostpone", rblpostpone.SelectedValue);
                criteria.Add("txtnumber", txtnumber.Text);


                //if (txtComponent.Text == "" && responsibility == ""  && ucComponentCategory.SelectedQuick == "" 
                //    && ucstatus.SelectedHard == "" && rblpostpone.SelectedValue=="" && txtclasscode.Text ==""
                //    && ucComponentCategory.SelectedQuick =="")
                //    Filter.CurrentComponentFilterCriteria = null;
                //else
                    Filter.CurrentComponentFilterCriteria = criteria;
            }

            string url = "PlannedMaintenanceWorkOrderPostponedApproval.aspx";
            Response.Redirect("../PlannedMaintenance/" + url, false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

}