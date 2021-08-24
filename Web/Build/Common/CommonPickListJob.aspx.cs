using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonPickListJob : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
		MenuJob.MenuList = toolbarmain.Show();

		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
		}
	}

	protected void MenuJob_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvJob.Rebind();
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

			ds = PhoenixCommonPlannedMaintenance.JobSearch(General.GetNullableString (txtJobCode.Text),
				General.GetNullableString(txtJobTitle.Text),General.GetNullableInteger(ucJobClass.SelectedQuick)
			, sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

            gvJob.DataSource = ds;
            gvJob.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void gvJob_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName == "RowClick" || e.CommandName == "Select")
        {
            if (Request.QueryString["mode"] == "custom")
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lbl = (LinkButton)e.Item.FindControl("lblTitle");
                nvc.Add(lbl.ID, lbl.Text);
                RadLabel lb = (RadLabel)e.Item.FindControl("lnkcode");
                nvc.Add(lb.ID, lb.Text.ToString());
            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lb = (RadLabel)e.Item.FindControl("lnkcode");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                LinkButton lbl = (LinkButton)e.Item.FindControl("lblTitle");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lbl1 = (RadLabel)e.Item.FindControl("lbljobid");
                nvc.Set(nvc.GetKey(3), lbl1.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvJob_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJob.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvJob_SortCommand(object sender, GridSortCommandEventArgs e)
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
