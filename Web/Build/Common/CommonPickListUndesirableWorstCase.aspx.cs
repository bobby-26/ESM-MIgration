using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonPickListUndesirableWorstCase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvUndesiarableEvent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvUndesiarableEvent.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixInspectionOperationalRiskControls.UndisrableEventSearch(General.GetNullableGuid(null),
                                                           gvUndesiarableEvent.CurrentPageIndex + 1,
                                                           gvUndesiarableEvent.PageSize, ref iRowCount, ref iTotalPageCount);

            gvUndesiarableEvent.DataSource = ds;
            gvUndesiarableEvent.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUndesiarableEvent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblUndesirable = (RadLabel)e.Item.FindControl("lblUndesirable");
                nvc.Add(lblUndesirable.ID, lblUndesirable.Text.ToString());
                LinkButton lnkWorstCase = (LinkButton)e.Item.FindControl("lnkWorstCase");
                nvc.Add(lnkWorstCase.ID, lnkWorstCase.Text.ToString());
                RadLabel lblUndisirableId = (RadLabel)e.Item.FindControl("lblUndisirableId");
                nvc.Add(lblUndisirableId.ID, lblUndisirableId.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblUndesirable = (RadLabel)e.Item.FindControl("lblUndesirable");
                nvc.Set(nvc.GetKey(1), lblUndesirable.Text.ToString());
                LinkButton lnkWorstCase = (LinkButton)e.Item.FindControl("lnkWorstCase");
                nvc.Set(nvc.GetKey(2), lnkWorstCase.Text.ToString());
                RadLabel lblUndisirableId = (RadLabel)e.Item.FindControl("lblUndisirableId");
                nvc.Set(nvc.GetKey(3), lblUndisirableId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvUndesiarableEvent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUndesiarableEvent.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvUndesiarableEvent_SortCommand(object sender, GridSortCommandEventArgs e)
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