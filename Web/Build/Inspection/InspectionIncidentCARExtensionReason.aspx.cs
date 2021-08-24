using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionIncidentCARExtensionReason : PhoenixBasePage
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
            gvExtensionReason.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        ds = PhoenixInspectionSchedule.ReScheduleHistorySearch(
                                                                General.GetNullableGuid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvExtensionReason.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["HISTORYID"] == null)
            {
                ViewState["HISTORYID"] = ds.Tables[0].Rows[0]["FLDHISTORYID"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }

        gvExtensionReason.DataSource = ds.Tables[0];
        gvExtensionReason.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvExtensionReason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblHistoryId = ((RadLabel)e.Item.FindControl("lblHistoryId"));
                ViewState["HISTORYID"] = lblHistoryId.Text;
                Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExtensionReason_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }

    protected void gvExtensionReason_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        ViewState["HISTORYID"] = ((RadLabel)gvExtensionReason.Items[se.NewSelectedIndex].FindControl("lblHistoryId")).Text;
        Rebind();
        SetRowSelection();
    }

    private void SetRowSelection()
    {
        gvExtensionReason.SelectedIndexes.Clear();
        for (int i = 0; i < gvExtensionReason.Items.Count; i++)
        {
            if (gvExtensionReason.MasterTableView.Items[i].GetDataKeyValue("FLDHISTORYID").ToString().Equals(ViewState["HISTORYID"].ToString()))
            {
                ViewState["HISTORYID"] = ((RadLabel)gvExtensionReason.Items[i].FindControl("lblHistoryId")).Text;
                gvExtensionReason.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }
    protected void gvExtensionReason_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExtensionReason.CurrentPageIndex + 1;
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
        gvExtensionReason.SelectedIndexes.Clear();
        gvExtensionReason.EditIndexes.Clear();
        gvExtensionReason.Rebind();
    }
}
