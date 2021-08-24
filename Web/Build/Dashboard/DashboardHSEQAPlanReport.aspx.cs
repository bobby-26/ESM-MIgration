using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class Dashboard_DashboardHSEQAPlanReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Confirm", "Toggle2", ToolBarDirection.Right);
        toolbargrid.AddButton("Save", "Toggle1", ToolBarDirection.Right);

        Hseqaplanreporttab.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            confirm.Attributes.Add("style", "display:none");

            ViewState["HSEQASCHEDULEID"] = General.GetNullableGuid(Request.QueryString["hseqascheduleid"]);

            DataTable dt = PhoenixDashboardHSEQAPlanner.HSEQAScheduleDetails(General.GetNullableGuid(Request.QueryString["hseqascheduleid"]));
            radlblliname.Text = dt.Rows[0]["FLDLINAME"].ToString();
            radlblinterval.Text = dt.Rows[0]["FLDFREQUENCY"].ToString() + " " + dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();
            radlblPlanneddate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDPLANNEDDATE"].ToString());
            Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());
            attachments.Attributes["src"] = "" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                           + PhoenixModule.QUALITY ;
            int attachment = 0;
            PhoenixDashboardHSEQAPlanner.attachmentyn(flddtkey, ref attachment);
            if (attachment == 0)
            {

                Noattachmentreasontitle.Visible = true;
                Noattachmentreasontitle1.Visible = true;
                reasonfornoattachments.Visible = true;
            }
            else
            {

                Noattachmentreasontitle.Visible = false;
                Noattachmentreasontitle1.Visible = false;
                reasonfornoattachments.Visible = false;
                reasonfornoattachments.Text = string.Empty;

            }
        }
    }

    protected void Hseqaplanreporttab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? hseqascheduleid = General.GetNullableGuid(ViewState["HSEQASCHEDULEID"].ToString());
                string remarks = General.GetNullableString(tbremarksentry.Text);
                DateTime? donedate = General.GetNullableDateTime(radlastdonedate.Text);
                string status = "Draft";
                string reason = General.GetNullableString(reasonfornoattachments.Text);



                PhoenixDashboardHSEQAPlanner.HSEQAPlanReport(rowusercode, hseqascheduleid, donedate, remarks, reason, status);



                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {

                RadWindowManager1.RadConfirm("Once LI  is reported it cannot be edited. Click on Ok to  report  Or click on Cancel to continue editing.", "confirm", 320, 150, null, "Confirm");


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            Page_Load(sender, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            Guid? hseqascheduleid = General.GetNullableGuid(ViewState["HSEQASCHEDULEID"].ToString());
            string remarks = General.GetNullableString(tbremarksentry.Text);
            DateTime? donedate = General.GetNullableDateTime(radlastdonedate.Text);
            string status = "Confirm";
            string reason = General.GetNullableString(reasonfornoattachments.Text);

            PhoenixDashboardHSEQAPlanner.HSEQAPlanReport(rowusercode, hseqascheduleid, donedate, remarks, reason, status);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "closeTelerikWindow('Report', 'Filters');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}