using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Elog;
using Telerik.Web.UI;

public partial class ElectricLogEngineAttributeHistory : PhoenixBasePage
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

            if (Request.QueryString["TypeId"] != null)
            {
                ViewState["TypeId"] = Request.QueryString["TypeId"];
            }
            else
            {
                ViewState["TypeId"] = "-1";
            }

            if (Request.QueryString["VesselId"] != null)
            {
                ViewState["VesselId"] = Request.QueryString["VesselId"];
            }
            else
            {
                ViewState["VesselId"] = "-1";
            }
            gvExtensionReason.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataSet ds;

        ds = PhoenixEngineLogAttributes.EngineLogAttributesHistory(General.GetNullableInteger(ViewState["TypeId"].ToString()), General.GetNullableInteger(ViewState["VesselId"].ToString()));
        

        gvExtensionReason.DataSource = ds.Tables[0];
    }

    protected void gvExtensionReason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
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


    protected void gvExtensionReason_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkOldRevNo = (LinkButton)e.Item.FindControl("lnkOldRevNo");
            RadLabel lblRevisionId = (RadLabel)e.Item.FindControl("lblRevisionId");
            if (lnkOldRevNo != null)
            {
                lnkOldRevNo.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Log/ElectricLogEngineAttributesRevisionHistory.aspx?RevisionId=" + lblRevisionId.Text + "'); return true;");
            }
        }
    }
}
