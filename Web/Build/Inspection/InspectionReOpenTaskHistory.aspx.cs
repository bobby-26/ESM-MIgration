using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionReOpenTaskHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["correctiveactionid"] != null)
            {
                ViewState["CORRECTIVEACTIONID"] = Request.QueryString["correctiveactionid"];
            }
            else
            {
                ViewState["CORRECTIVEACTIONID"] = null;
            }
            gvReopentaskReason.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionSchedule.ReOpenTaskHistorySearch( General.GetNullableGuid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvReopentaskReason.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["HISTORYID"] == null)
            {
                ViewState["HISTORYID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONREOPENTASKHISTORYID"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        gvReopentaskReason.DataSource = ds.Tables[0];
        gvReopentaskReason.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvReopentaskReason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Label lblHistoryId = ((Label)e.Item.FindControl("lblHistoryId"));
                ViewState["HISTORYID"] = lblHistoryId.Text;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReopentaskReason_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblReason = (RadLabel)e.Item.FindControl("lblReason");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("Reason");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblReason.ClientID;
            }
        }
    }

    protected void gvReopentaskReason_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        ViewState["HISTORYID"] = ((Label)gvReopentaskReason.Items[se.NewSelectedIndex].FindControl("lblHistoryId")).Text;
        Rebind();
    }

    private void SetRowSelection()
    {
        gvReopentaskReason.SelectedIndexes.Clear();
        for (int i = 0; i < gvReopentaskReason.Items.Count; i++)
        {
            if (gvReopentaskReason.MasterTableView.Items[i].GetDataKeyValue("FLDINSPECTIONREOPENTASKHISTORYID").ToString().Equals(ViewState["HISTORYID"].ToString()))
            {
                ViewState["HISTORYID"] = ((RadLabel)gvReopentaskReason.Items[i].FindControl("lblHistoryId")).Text;
                gvReopentaskReason.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }
    protected void gvReopentaskReason_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReopentaskReason.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvReopentaskReason.SelectedIndexes.Clear();
        gvReopentaskReason.EditIndexes.Clear();
        gvReopentaskReason.DataSource = null;
        gvReopentaskReason.Rebind();
    }
}

