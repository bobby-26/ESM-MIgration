using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionRegulationActionPlanEdit : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //imgPersonOfficeEdit.Attributes.Add("onclick", "return showPickList('spnActionPlanPersonOfficeEdit', 'codehelp1', '', '../Common/CommonPickListUser.aspx?Departmentid=" + ViewState["DEPARTMENTID"].ToString() + "&MOC=true', true);");
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["RegulationId"] = null;
                ViewState["ACTIONPLANID"] = null;

                if (Request.QueryString["RegulationId"] != null && Request.QueryString["RegulationId"].ToString() != string.Empty)
                    ViewState["RegulationId"] = Request.QueryString["RegulationId"].ToString();

                if (Request.QueryString["ACTIONPLANID"] != null && Request.QueryString["ACTIONPLANID"].ToString() != string.Empty)
                    ViewState["ACTIONPLANID"] = Request.QueryString["ACTIONPLANID"].ToString();

                BindActionPlanEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindActionPlanEdit()
    {
        if (ViewState["ACTIONPLANID"] != null)
        {
            DataSet ds = PhoenixInspectionNewRegulationActionPlan.RegulationEdit( new Guid((ViewState["ACTIONPLANID"]).ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                
                txtActionToBeTaken.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                txtTargetdateEdit.Text = dr["FLDTARGETDATE"].ToString();
                ucDepartmentedit.DataBind();
                ucDepartmentedit.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();               

            }            
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ACTIONPLANID"]==null)
                    InsertActionPlan();
                else
                    UpdateActionPlan();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateActionPlan()
    {

        PhoenixInspectionNewRegulationActionPlan.RegulationActionPlanUpdate(General.GetNullableString(txtActionToBeTaken.Text)                                                                     
                                                                      , General.GetNullableInteger(ucDepartmentedit.SelectedDepartment)
                                                                      , General.GetNullableDateTime(txtTargetdateEdit.Text)
                                                                      , General.GetNullableGuid(ViewState["ACTIONPLANID"].ToString()));

    }
    private void InsertActionPlan()
    {
        Guid? actionplanid = General.GetNullableGuid(null);
        PhoenixInspectionNewRegulationActionPlan.RegulationActionPlanInsert(General.GetNullableGuid( ViewState["RegulationId"].ToString())
                                                                      ,General.GetNullableString(txtActionToBeTaken.Text)
                                                                      , General.GetNullableInteger(ucDepartmentedit.SelectedDepartment)
                                                                      , General.GetNullableDateTime(txtTargetdateEdit.Text)
                                                                      , ref actionplanid);
    }
}
