using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessGroupMemberAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["TARGETID"] = string.Empty;
                ViewState["GROUPID"] = string.Empty;
                ViewState["PROCESSID"] = string.Empty;

                ViewState["TARGETID"] = General.GetNullableInteger(Request.QueryString["TARGETID"].ToString());
                ViewState["GROUPID"] = General.GetNullableGuid(Request.QueryString["GROUPID"].ToString());
                ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());

                ViewState["PAGENUMBER"] = 1;
                gvProcessGroupMember.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar Tool = new PhoenixToolbar();
            Tool.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWFGroupMember.AccessRights = this.ViewState;
            MenuWFGroupMember.MenuList = Tool.Show();

            PhoenixToolbar t1 = new PhoenixToolbar();
            t1.AddFontAwesomeButton("../Purchase/WorkflowProcessGroupMemberAdd.aspx?TARGETID=" + ViewState["TARGETID"] + " ", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            t1.AddFontAwesomeButton("../Purchase/WorkflowProcessGroupMemberAdd.aspx?TARGETID=" + ViewState["TARGETID"] + " ", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuGroupSearch.MenuList = t1.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessGroupMember_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProcessGroupMember.CurrentPageIndex + 1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable t1 = PhoenixWorkflow.WorkflowUserPickList(
                                            General.GetNullableInteger(ViewState["TARGETID"].ToString()),
                                            General.GetNullableString(txtUsername.Text.Trim()),
                                            General.GetNullableString(txtFirstName.Text.Trim()),
                                            General.GetNullableString(txtLastName.Text.Trim()),
                                            gvProcessGroupMember.CurrentPageIndex + 1,
                                            gvProcessGroupMember.PageSize,
                                            ref iRowCount,
                                            ref iTotalPageCount);

            gvProcessGroupMember.DataSource = t1;
            gvProcessGroupMember.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void MenuWFGroupMember_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvProcessGroupMember.Items)
                {
                    if (((RadCheckBox)(gvr.FindControl("CheckUser"))).Checked == true)
                    {
                        int? UserCode = General.GetNullableInteger(((RadLabel)gvr.FindControl("lblUserCode")).Text);

                        PhoenixWorkflow.ProcessGroupMemberInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            General.GetNullableGuid(ViewState["GROUPID"].ToString()),
                            UserCode,
                           General.GetNullableInteger(ViewState["TARGETID"].ToString()),
                           General.GetNullableGuid(ViewState["PROCESSID"].ToString())

                           );

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGroupSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtUsername.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvProcessGroupMember.SelectedIndexes.Clear();
        gvProcessGroupMember.EditIndexes.Clear();
        gvProcessGroupMember.DataSource = null;
        gvProcessGroupMember.Rebind();
    }
}


