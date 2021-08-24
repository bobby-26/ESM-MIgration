using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Owners;

public partial class InspectionOwnersReportComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Post", "POSTCOMMENT", ToolBarDirection.Right);

        MenuRAComments.AccessRights = this.ViewState;
        MenuRAComments.MenuList = toolbaredit.Show();

        if (!IsPostBack)
        {
            ViewState["CODE"] = Request.QueryString["CODE"].ToString();
            ViewState["DATE"] = Request.QueryString["DATE"].ToString();
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
        }

        //BindData();

        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments(ViewState["CODE"].ToString(), DateTime.Parse(ViewState["DATE"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));

        gvComments.DataSource = dt;
        //if (dt.Rows.Count > 0)
        //{
        //    repDiscussion.DataSource = dt;
        //    repDiscussion.DataBind();
        //}
        //else
        //{
        //    ShowNoRecordsFound(dt, repDiscussion);
        //}
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void MenuRAComments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("POSTCOMMENT"))
            {
                if (!IsCommentValid(txtNotesDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerReportComments.InsertOwnersReportComments(int.Parse(ViewState["VESSELID"].ToString())
                    , DateTime.Parse(ViewState["DATE"].ToString()).Month
                    , DateTime.Parse(ViewState["DATE"].ToString()).Year
                    , ViewState["CODE"].ToString()
                    , General.GetNullableString(txtNotesDescription.Text.Trim()));

                txtNotesDescription.Text = "";
                gvComments.DataSource = null;
                gvComments.Rebind();
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','keep');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsCommentValid(string strComment)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }

    protected void gvComments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvComments_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                if (drv["FLDPOSTEDBY"].ToString() == PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                    edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                else
                    edit.Visible = false;
            }
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                if (drv["FLDPOSTEDBY"].ToString() == PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                    cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                else
                    cmdDelete.Visible = false;
            }
        }
    }

    protected void gvComments_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixOwnerReportComments.UpdateOwnersReportComments(General.GetNullableGuid(((RadLabel)e.Item.FindControl("commentsEditid")).Text)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtNotesDescription")).Text));

                gvComments.DataSource = null;
                gvComments.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerReportComments.DeleteOwnersReportComments(General.GetNullableGuid(((RadLabel)e.Item.FindControl("commentsid")).Text));

                gvComments.DataSource = null;
                gvComments.Rebind();
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','keep');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}