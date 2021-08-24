using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionJHAHazardsWithIcons : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TYPE"] = Request.QueryString["TYPE"];
                ViewState["JHAID"] = "";

                ViewState["JHAYN"] = "0";

                if (Request.QueryString["JHAID"] != null && Request.QueryString["JHAID"].ToString() != "")
                    ViewState["JHAID"] = Request.QueryString["JHAID"].ToString();

                if (Request.QueryString["JHAYN"] != null && Request.QueryString["JHAYN"].ToString() != "")
                    ViewState["JHAYN"] = Request.QueryString["JHAYN"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        if (ViewState["JHAYN"].ToString() == "1")
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAHazardWithIcons(int.Parse(ViewState["TYPE"].ToString()),
                new Guid(ViewState["JHAID"].ToString()));

            gvRAHazard.DataSource = DT;
        }
        else
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAHazardWithIcons(int.Parse(ViewState["TYPE"].ToString()),
                new Guid(ViewState["JHAID"].ToString()));

            gvRAHazard.DataSource = DT;
        }
    }

    protected void gvRAHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

        }
    }
}