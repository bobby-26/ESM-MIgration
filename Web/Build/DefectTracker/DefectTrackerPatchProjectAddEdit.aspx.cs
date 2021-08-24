using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerPatchProjectAddEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Add/Edit", "ADDEDIT");
        toolbar.AddButton("Mail", "MAILEDIT");
        toolbar.AddButton("Reminder", "REMINDER");
        toolbar.AddButton("Escalation", "ESCALATION");
        toolbar.AddButton("Save", "SAVE");
        MenuPatchProjectSave.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            ViewState["PATCHPROJECTDTKEY"] = null;
            if (Request.QueryString["projectdtkey"] != null)
            {
                ViewState["PATCHPROJECTDTKEY"] = Request.QueryString["projectdtkey"].ToString();
                BindData(Request.QueryString["projectdtkey"].ToString());
            }
        }

    }

    private void BindData(string dtkey)
    {
        DataTable dt = PhoenixPatchTracker.PatchProjectEdit(General.GetNullableGuid(dtkey));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtCatalog.Text = dr["FLDCATALOGNUMBER"].ToString();
            txtTitle.Text = dr["FLDTITLE"].ToString();
            txtCallNumber.Text = dr["FLDCALLNUMBER"].ToString();
            ucCallDate.Text = dr["FLDCALLDATE"].ToString();
            General.BindCheckBoxList(ddlPatchType, dr["FLDPATCHTYPE"].ToString());
            txtSubject.Text = dr["FLDSUBJECT"].ToString();
            txtCreatedby.SelectedValue = dr["FLDPATCHCREATEDBY"].ToString();
            txtTo.Text = dr["FLDMAILTO"].ToString();
            txtCc.Text = dr["FLDMAILCC"].ToString();

        }
    }

    protected void MenuPatchProjectSave_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {

            Guid? patchprojectdtkey = null;

            if (!IsPatchCreateValidate())
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["PATCHPROJECTDTKEY"] == null)
            {
                PhoenixPatchTracker.PatchProjectInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    txtCatalog.Text,
                    txtTitle.Text,
                    txtCallNumber.Text,
                    General.GetNullableDateTime(ucCallDate.Text),
                    General.ReadCheckBoxList(ddlPatchType),
                    txtSubject.Text,
                    txtCreatedby.SelectedValue,
                    txtTo.Text,
                    txtCc.Text,
                    ref patchprojectdtkey);

                ViewState["PATCHPROJECTDTKEY"] = patchprojectdtkey;
            }
            else
            {
                PhoenixPatchTracker.PatchProjectUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["PATCHPROJECTDTKEY"].ToString()),
                    txtCatalog.Text,
                    txtTitle.Text,
                    txtTo.Text,
                    txtCc.Text,
                    General.ReadCheckBoxList(ddlPatchType),
                    txtSubject.Text,
                    txtCreatedby.SelectedValue);
            }

            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            ucStatus.Text = "Patch Project Created";
        }

        if (dce.CommandName.ToUpper().Equals("ADDEDIT"))
        {
            Response.Redirect("DefectTrackerPatchProjectAddEdit.aspx?projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }

        if (dce.CommandName.ToUpper().Equals("MAILEDIT"))
        {
            Response.Redirect("DefectTrackerEditPatch.aspx?projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }

        if (dce.CommandName.ToUpper().Equals("REMINDER"))
        {
            Response.Redirect("DefectTrackerTrackerReminderEscalation.aspx?type=1&projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }

        if (dce.CommandName.ToUpper().Equals("ESCALATION"))
        {
            Response.Redirect("DefectTrackerTrackerReminderEscalation.aspx?type=2&projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }
    }

    private bool IsPatchCreateValidate()
    {
        if (General.GetNullableString(txtTitle.Text) == null)
            ucError.ErrorMessage = "Patch Title is required";

        if (General.GetNullableString(txtCatalog.Text) == null)
            ucError.ErrorMessage = "Patch Catalog is required";

        if (General.GetNullableString(ddlPatchType.SelectedValue) == null)
            ucError.ErrorMessage = "Patch Attachment Type is required";

        if (General.GetNullableString(txtCreatedby.SelectedValue) == null)
            ucError.ErrorMessage = "Patch Creator name is required";

        return !ucError.IsError;
    }
}
