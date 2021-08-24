using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPickListComponent : PhoenixBasePage
{	 
	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
		MenuComponent.MenuList = toolbarmain.Show();

		if (!IsPostBack)
		{
            ViewState["PAGENUMBER"] = 1;
            ViewState["showcritical"] = "";
            if (Request.QueryString["showcritical"] != null && Request.QueryString["showcritical"].ToString() != "")
            {
                ViewState["showcritical"] = Request.QueryString["showcritical"].ToString();
            }
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            if ((Request.QueryString["ispopup"] != null) && (Request.QueryString["ispopup"] != ""))
                ViewState["ISPOPUP"] = Request.QueryString["ispopup"].ToString();
            else
                ViewState["ISPOPUP"] = string.Empty;
            gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                gvComponent.Rebind();
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


            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

			DataSet ds;

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixCommonInventory.ComponentSearch(vesselid, txtNumberSearch.Text, txtComponentNameSearch.Text,
                null, null, null, null, null,
                General.GetNullableByte(ViewState["showcritical"].ToString()),
                null, null, sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvComponent.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void gvComponent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                if (ViewState["framename"] != null)
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblComponentNumber");
                nvc.Add(lblComponentNumber.ID, lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)e.Item.FindControl("lnkComponentName");
                nvc.Add(lbComponentName.ID, lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
                nvc.Add(lblComponentId.ID, lblComponentId.Text);
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblComponentNumber");
                nvc.Set(nvc.GetKey(1), lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)e.Item.FindControl("lnkComponentName");
                nvc.Set(nvc.GetKey(2), lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
                nvc.Set(nvc.GetKey(3), lblComponentId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            if (!ViewState["ISPOPUP"].ToString().Equals(""))
            {
                string[] popup = ViewState["ISPOPUP"].ToString().Split(',');
                string refreshname = popup.Length > 1 ? popup[1] : string.Empty;

                Script = "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //            "BookMarkScript", "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
            }
            else
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponent.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvComponent_SortCommand(object sender, GridSortCommandEventArgs e)
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
