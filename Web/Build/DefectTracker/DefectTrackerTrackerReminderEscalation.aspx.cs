using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerTrackerReminderEscalation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbaredit = new PhoenixToolbar();

            toolbaredit.AddButton("Add/Edit", "ADDEDIT");
            toolbaredit.AddButton("Mail", "MAILEDIT");
            toolbaredit.AddButton("Reminder", "REMINDER");
            toolbaredit.AddButton("Escalation", "ESCALATION");

            MenuReminderAdd.AccessRights = this.ViewState;
            MenuReminderAdd.MenuList = toolbaredit.Show();
           

            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE");
            MenuReminderSave.AccessRights = this.ViewState;
            MenuReminderSave.MenuList = toolbarsave.Show();
            cblTo.DataSource = PhoenixPatchTracker.PatchEscalationMailList();
            cblTo.DataBind();
            cblCc.DataSource = PhoenixPatchTracker.PatchEscalationMailList();
            cblCc.DataBind();

            if (Request.QueryString["projectdtkey"] != null)
            {
                ViewState["PATCHPROJECTDTKEY"] = Request.QueryString["projectdtkey"].ToString();
            }

            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDefectTracker.ReminderEdit(new Guid(ViewState["PATCHPROJECTDTKEY"].ToString()), short.Parse(Request.QueryString["type"].ToString()));

        if (dt.Rows.Count > 0)
        {
            General.BindCheckBoxList(cblTo, dt.Rows[0]["FLDTO"].ToString());
            General.BindCheckBoxList(cblCc, dt.Rows[0]["FLDCC"].ToString());

            txtMessage.Text = dt.Rows[0]["FLDTEXT"].ToString();
        }
    }
    protected void MenuReminderSave_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {

            PhoenixDefectTracker.PatchEscalationReminderSave(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["PATCHPROJECTDTKEY"].ToString())
                                                            , General.ReadCheckBoxList(cblTo)
                                                            , General.ReadCheckBoxList(cblCc)
                                                            , txtMessage.Text
                                                            , short.Parse(Request.QueryString["type"].ToString())
                                                            );
            ucStatus.Text = "Information updated sucessfully";
        }
    }

    protected void MenuReminderAdd_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

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

}

