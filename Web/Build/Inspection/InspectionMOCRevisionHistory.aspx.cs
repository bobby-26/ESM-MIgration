using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMOCRevisionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["MOCID"] = "";
                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != "")
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
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
        gvMocRevision.SelectedIndexes.Clear();
        gvMocRevision.EditIndexes.Clear();
        gvMocRevision.DataSource = null;
        gvMocRevision.Rebind();
    }
    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixInspectionMOCTemplate.MOCRevisionList(General.GetNullableGuid(ViewState["MOCID"].ToString()));

            gvMocRevision.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMocRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMocRevision.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}