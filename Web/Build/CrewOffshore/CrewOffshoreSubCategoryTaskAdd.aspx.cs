using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreSubCategoryTaskAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ucToBeDoneby.TypeOfTraining = PhoenixCrewOffshoreTrainingNeeds.ListTypeOfTraining(129, "TSK");
            ucToBeDoneby.bind();
            BindCategory();
            BindRankList();
            ViewState["tid"] = "";
            if (Request.QueryString["tid"] != null)
            {
                ViewState["tid"] = Request.QueryString["tid"].ToString();

                TaskEdit(Int32.Parse(Request.QueryString["tid"].ToString()));
            }
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();      
        if (Request.QueryString["tid"] != null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);           
        }
        else
        {          
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        MenuDoumentCourseList.AccessRights = this.ViewState;
        MenuDoumentCourseList.MenuList = toolbar.Show();
    }
    private void BindRankList()
    {
        DataSet ds = PhoenixRegistersRank.ListRank();
        chkRankList.DataSource = ds;
        chkRankList.DataTextField = "FLDRANKNAME";
        chkRankList.DataValueField = "FLDRANKID";
        chkRankList.DataBind();

        chkassessor.DataSource = ds;
        chkassessor.DataTextField = "FLDRANKNAME";
        chkassessor.DataValueField = "FLDRANKID";
        chkassessor.DataBind();

    }

    protected void MenuDoumentCourseList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper() == "SAVE")
            {
                String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

                String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");

                string RankList = "";
                foreach (RadComboBoxItem li in chkRankList.Items)
                {
                    if (li.Checked)
                    {
                        RankList += li.Value + ",";
                    }
                }

                if (RankList != "")
                {
                    RankList = "," + RankList;
                }

                string assessorlist = "";
                foreach (RadComboBoxItem li in chkassessor.Items)
                {
                    if (li.Checked)
                    {
                        assessorlist += li.Value + ",";
                    }
                }

                if (assessorlist != "")
                {
                    assessorlist = "," + assessorlist;
                }

                if (IsValidDocumentCourse())
                {
                    PhoenixCrewOffshoreSubCategoryTask.InsertSubCategoryTask(General.GetNullableInteger(ViewState["tid"].ToString())
                        , General.GetNullableInteger(ddsubcategory.SelectedValue.ToString())
                        ,txttask.Text,General.GetNullableInteger(ddllevel.SelectedValue.ToString())
                        ,General.GetNullableInteger(ucToBeDoneby.SelectedValue.ToString())
                        ,RankList
                        ,assessorlist
                        ,txtdescription.Text);
                    ucStatus.Text = "Information Saved";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDocumentCourse()
    {
        //Int32 result;

        ucError.HeaderMessage = "Please provide the following required information";

        if (txttask.Text.Equals(""))
            ucError.ErrorMessage = "Task is required.";
        
        if (ddllevel.SelectedIndex <0)
            ucError.ErrorMessage = "Level is required.";
        if (ucToBeDoneby.SelectedToBeDoneBy =="Dummy")
            ucError.ErrorMessage = "Do be done by is required.";
        if (chkRankList.CheckedItems.Count<=0)
            ucError.ErrorMessage = "Applicable rank is required.";

        
        return (!ucError.IsError);
    }
    public void TaskEdit(int tid)
    {
        BindCategory();
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.EditSubCategoryTask(General.GetNullableInteger(tid.ToString()));
        ddlCompetenceCategory.SelectedValue = dt.Rows[0]["FLDCATEGORYID"].ToString();
        DataTable dt1 = PhoenixCrewOffshoreSubCategoryTask.SubCategoryList(General.GetNullableInteger(ddlCompetenceCategory.SelectedValue.ToString()));
        ddsubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddsubcategory.DataTextField = "FLDNAME";

        ddsubcategory.DataSource = dt1;
        ddsubcategory.DataBind();
        ddsubcategory.SelectedValue= dt.Rows[0]["FLDSUBCATEGORYID"].ToString();
        txttask.Text= dt.Rows[0]["FLDTASKNAME"].ToString();

        General.RadBindComboBoxCheckList(chkRankList, dt.Rows[0]["FLDRANKS"].ToString());
        General.RadBindComboBoxCheckList(chkassessor, dt.Rows[0]["FLDASSESSOR"].ToString());
        ucToBeDoneby.SelectedValue = dt.Rows[0]["FLDTOBEDONEBY"].ToString();
        ddllevel.SelectedValue = dt.Rows[0]["FLDLEVEL"].ToString();
        txtdescription.Text= dt.Rows[0]["FLDTASKDESCRIPTION"].ToString();




    }
    public void BindCategory()
    {
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.CategoryList();

        ddlCompetenceCategory.Items.Clear();
        ddlCompetenceCategory.DataValueField = "FLDCATEGORYID";
        ddlCompetenceCategory.DataTextField = "FLDCATEGORYNAME";

        ddlCompetenceCategory.DataSource = dt;
        ddlCompetenceCategory.DataBind();
    }

    protected void ddlCompetenceCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.SubCategoryList(General.GetNullableInteger(ddlCompetenceCategory.SelectedValue.ToString()));
        ddsubcategory.Items.Clear();
        ddsubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddsubcategory.DataTextField = "FLDNAME";

        ddsubcategory.DataSource = dt;
        ddsubcategory.DataBind();
    }
}
