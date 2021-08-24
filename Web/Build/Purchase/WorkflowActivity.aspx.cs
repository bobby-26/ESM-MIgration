using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Purchase/WorkflowActivity.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowActivity.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowActivityAdd.aspx')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");


            MenuWorkflowActivity.AccessRights = this.ViewState;
            MenuWorkflowActivity.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Activity Type", "ACTIVITYTYPE", ToolBarDirection.Right);          
            MenuWFActivity.MenuList = tool.Show();
            MenuWFActivity.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkflowActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkflowActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkflowActivity.CurrentPageIndex + 1;
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            Guid? ActivityTypeId = General.GetNullableGuid(UcActivityType.SelectedActivityType);

            DataTable ds = PhoenixWorkForm.ActivitySearch(ActivityTypeId,General.GetNullableString(txtshortcode.Text.Trim()),
                    General.GetNullableString(txtname.Text.Trim()),
                  sortexpression, sortdirection,
                  gvWorkflowActivity.CurrentPageIndex + 1,
                  gvWorkflowActivity.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvWorkflowActivity.DataSource = ds;
            gvWorkflowActivity.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkflowActivity_TabStripCommand(object sender, EventArgs e)
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
                UcActivityType.SelectedActivityType = "";
                txtshortcode.Text = "";
                txtname.Text = "";
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
        gvWorkflowActivity.SelectedIndexes.Clear();
        gvWorkflowActivity.EditIndexes.Clear();
        gvWorkflowActivity.DataSource = null;
        gvWorkflowActivity.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkflowActivity.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvWorkflowActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? ActivityId = General.GetNullableGuid(item.GetDataKeyValue("FLDACTIVITYID").ToString());
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','WorkflowActivity','Purchase/WorkflowActivityEdit.aspx?ACTIVITYID=" + ActivityId + "');return false");

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkflowActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixWorkForm.DeleteActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["ID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWFActivity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("ACTIVITY"))
            {
                Response.Redirect("WorkflowActivity.aspx");
            }

            if (CommandName.ToUpper().Equals("ACTIVITYTYPE"))
            {
                Response.Redirect("WorkflowActivityType.aspx");
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}