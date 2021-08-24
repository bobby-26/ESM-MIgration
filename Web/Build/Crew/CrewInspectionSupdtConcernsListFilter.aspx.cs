using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewInspectionSupdtConcernsListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
      
        MainMenuSupdtConcernsList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           
            BindInspectionEvent();
            BindFeedbackCategory();
            BindFeedbackSubCategory();
        }
    }
    protected void BindInspectionEvent()
    {
        ddlEvent.DataSource = PhoenixInspectionEvent.ListInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null);
        ddlEvent.DataTextField = "FLDEVENTNAME";
        ddlEvent.DataValueField = "FLDINSPECTIONEVENTID";
        ddlEvent.DataBind();
        ddlEvent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackCategory()
    {
        ddlFeedbackCategory.DataSource = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
        ddlFeedbackCategory.DataTextField = "FLDFEEDBACKCATEGORYNAME";
        ddlFeedbackCategory.DataValueField = "FLDFEEDBACKCATEGORYID";
        ddlFeedbackCategory.DataBind();
        ddlFeedbackCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackSubCategory()
    {
        ddlFeedbackSubCategory.DataSource = PhoenixInspectionFeedbackSubCategory.ListFeedbackSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , null);
        ddlFeedbackSubCategory.DataTextField = "FLDFEEDBACKSUBCATEGORYNAME";
        ddlFeedbackSubCategory.DataValueField = "FLDFEEDBACKSUBCATEGORYID";
        ddlFeedbackSubCategory.DataBind();
        ddlFeedbackSubCategory.Items.Insert(0, new  RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void MainMenuSupdtConcernsList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList('Filter','ifMoreInfo');";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNumber", txtFileNumber.Text);
            criteria.Add("lstRank", lstRank.selectedlist);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlVesselType", ddlVesselType.SelectedVesseltype);
            criteria.Add("ddlFeedbackCategory", ddlFeedbackCategory.SelectedValue);
            criteria.Add("ddlFeedbackSubCategory", ddlFeedbackSubCategory.SelectedValue);
            criteria.Add("ddlEvent", ddlEvent.SelectedValue);
            criteria.Add("ucRecordedDate", ucRecordedDate.Text);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            Filter.SupdtConcernsListFilters = criteria;
            
        }
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);




    }
}
