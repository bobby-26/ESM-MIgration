using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRAActivityDMSCategoryMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");                
                MenuMapping.MenuList = toolbar.Show();
                MenuMapping.SetTrigger(pnlContactType);

                ddlDmsCategory.DataSource = PhoenixInspectionRiskAssessmentActivity.ListDMSDocumentCategory();
                ddlDmsCategory.DataTextField = "FLDCATEGORYNAME";
                ddlDmsCategory.DataValueField = "FLDCATEGORYID";
                ddlDmsCategory.DataBind();

                BindSourceCheckbox();
                BindCheckbox();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindSourceCheckbox()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivitySearch(null,
            General.GetNullableInteger(null),
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {           
            cblSource.DataSource = ds.Tables[0];
            cblSource.DataTextField = "FLDNAME";
            cblSource.DataValueField = "FLDACTIVITYID";
            cblSource.DataBind();
        }     
    }

    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string activityidlist = ReadCheckBoxList(cblSource);

                PhoenixInspectionRiskAssessmentActivity.UpdateDMSCategoryForRiskAssessmentActivity(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , activityidlist
                                                                    , rdoList.SelectedValue == "0" ? General.GetNullableGuid(ddlDmsCategory.SelectedValue) : null
                                                                    , rdoList.SelectedValue == "1" ? General.GetNullableGuid(ddlDmsCategory.SelectedValue) : null);
                
                ucStatus.Text = "Category is mapped successfully.";

                BindSourceCheckbox();                
                BindCheckbox();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCheckbox()
    {

        DataSet ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivityByDMSCategory(
                                                            rdoList.SelectedValue == "0" ? General.GetNullableGuid(ddlDmsCategory.SelectedValue) : null,
                                                            rdoList.SelectedValue == "1" ? General.GetNullableGuid(ddlDmsCategory.SelectedValue) : null);
                                                                                                                  
        DataTable dt = ds.Tables[0];
        BindSourceCheckbox();
        foreach (DataRow dr in dt.Rows)
        {
            string s = dr["FLDACTIVITYID"].ToString();
            if (cblSource.Items.FindByValue(s) != null)
                cblSource.Items.FindByValue(s).Selected = true;
        }
        
        dt = new DataTable();       
        dt = ds.Tables[1];
        foreach (DataRow dr in dt.Rows)
        {
            string s = dr["FLDACTIVITYID"].ToString();
            if (cblSource.Items.FindByValue(s) != null)
                cblSource.Items.FindByValue(s).Selected = true;
        }
    }

    protected void rdoDMScategory_Changed(object sender, EventArgs e)
    {
        BindCheckbox();
    }
}
