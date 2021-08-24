using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRANavigationAspectList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["RAID"] = "";
            if (Request.QueryString["RAID"] != null)
                ViewState["RAID"] = Request.QueryString["RAID"].ToString();

            ViewState["PAGENUMBER"] = 1;
        }
    }


    protected void gvRAOperationalHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentNavigationExtn.ListOperationalHazard(General.GetNullableGuid(ViewState["RAID"].ToString()));
            gvRAOperationalHazard.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string Script = "";
                NameValueCollection nvc;

                if (e.CommandName.ToUpper().Equals("PICKLIST"))
                {
                    if (ViewState["RAID"].ToString().Equals(""))
                    {
                        ucError.ErrorMessage = "RA is required";
                        ucError.Visible = true;
                        return;
                    }
                    if (Request.QueryString["mode"] == "custom")
                    {
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnReloadList('codehelp1','ifMoreInfo');";
                        Script += "</script>" + "\n";

                        nvc = new NameValueCollection();

                        LinkButton lnkaspets = (LinkButton)e.Item.FindControl("lnkaspets");
                        nvc.Add(lnkaspets.ID, lnkaspets.Text.ToString());
                        RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
                        nvc.Add(lblOperationalId.ID, lblOperationalId.Text);
                    }
                    else
                    {
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                        Script += "</script>" + "\n";

                        nvc = Filter.CurrentPickListSelection;

                        LinkButton lnkaspets = (LinkButton)e.Item.FindControl("lnkaspets");
                        nvc.Set(nvc.GetKey(1), lnkaspets.Text.ToString());
                        RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
                        nvc.Set(nvc.GetKey(2), lblOperationalId.Text.ToString());
                    }

                    Filter.CurrentPickListSelection = nvc;
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}