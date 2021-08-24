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
public partial class InspectionMachineryDamageCorrectiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["CORRECTIVEACTIONID"] = null;
                ViewState["MACHINERYDAMAGEID"] = null;
                ViewState["REFFROM"] = null;
                ViewState["VESSELID"] = null;

                if (Request.QueryString["CORRECTIVEACTIONID"] != null && Request.QueryString["CORRECTIVEACTIONID"].ToString() != string.Empty)
                    ViewState["CORRECTIVEACTIONID"] = Request.QueryString["CORRECTIVEACTIONID"].ToString();

                if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != string.Empty)
                    ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                lblCompletionDate.Visible = false;
                ucCompletionDate.Visible = false;

                BindCorrectiveAction();
                if (ViewState["CORRECTIVEACTIONID"] != null && General.GetNullableDateTime(ucTargetDate.Text) != null)
                    SetRights();

                //if (ViewState["CORRECTIVEACTIONID"] == null)
                //    MenuCARGeneral.Title = "Add";
                //else
                //    MenuCARGeneral.Title = "Edit";
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCorrectiveAction()
    {
        if (ViewState["CORRECTIVEACTIONID"] != null && ViewState["CORRECTIVEACTIONID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.EditCorrectiveAction(new Guid(ViewState["CORRECTIVEACTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCorrectiveAction.Text = dr["FLDCORRECTIVEACTION"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ucDept.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["MACHINERYDAMAGEID"].ToString() != string.Empty)
                {
                    Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx?MACHINERYDAMAGEID=" + General.GetNullableString(ViewState["MACHINERYDAMAGEID"].ToString()) + "&VESSELID=" + General.GetNullableString(ViewState["VESSELID"].ToString()), false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx", false);
                }
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CORRECTIVEACTIONID"] == null)
                    InsertCorrectiveAction();
                else
                    UpdateCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertCorrectiveAction()
    {
        if (!IsValidCorrectiveAction())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionCorrectiveAction.InsertMachineryDamageCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(ucTargetDate.Text), null, General.GetNullableInteger(ucDept.SelectedDepartment),
                null, null, General.GetNullableInteger(ucVerficationLevel.SelectedHard));

        ucStatus.Text = "Corrective Action has been added.";

        Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx?MACHINERYDAMAGEID=" + General.GetNullableString(ViewState["MACHINERYDAMAGEID"].ToString()) + "&VESSELID=" + General.GetNullableString(ViewState["VESSELID"].ToString()), false);
        BindCorrectiveAction();
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateCorrectiveAction()
    {
        if (!IsValidCorrectiveAction())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionCorrectiveAction.UpdateMachineryDamageCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["CORRECTIVEACTIONID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null,
                General.GetNullableInteger(ucDept.SelectedDepartment), null, null, General.GetNullableInteger(ucVerficationLevel.SelectedHard));

        ucStatus.Text = "Corrective Action updated.";
        BindCorrectiveAction();
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidCorrectiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["MACHINERYDAMAGEID"] == null || General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()) == null)
            ucError.ErrorMessage = "Machinery Damage details are not yet added.";

        if (string.IsNullOrEmpty(txtCorrectiveAction.Text))
            ucError.ErrorMessage = "Corrective Action is required.";

        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (General.GetNullableDateTime(ucCompletionDate.Text) != null && General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date cannot be the future date.";

        return (!ucError.IsError);
    }

    public void SetRights()
    {
        ucTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "TARGETDATE");
    }
}
