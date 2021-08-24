using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonPickListVesselCrewListByDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuCrewList.AccessRights = this.ViewState;
        MenuCrewList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
            if (Request.QueryString["Date"] != null)
            {
                if (General.GetNullableDateTime(Request.QueryString["Date"].ToString()) != null)
                    ucDate.Text = Request.QueryString["Date"].ToString();
            }
            else
                ucDate.Text = DateTime.Now.ToShortDateString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

       
      
    }

    protected void MenuCrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCrewList.Rebind();
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
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixCommonInspection.SearchVesselEmployee(vesselid, General.GetNullableDateTime(ucDate.Text), General.GetNullableInteger(ucRank.SelectedRank),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvCrewList.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

            gvCrewList.DataSource = ds;
            gvCrewList.VirtualItemCount = iRowCount;
       
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }


   

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        //SetPageNavigator();
    }

    protected void gvCrewList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }


        if (e.CommandName.ToUpper() == "SELECT")
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCrew");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lblRank = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblRank");
                nvc.Add(lblRank.ID, lblRank.Text);
                RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCrewId");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCrew");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lblRank = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblRank");
                nvc.Set(nvc.GetKey(2), lblRank.Text.ToString());
                RadLabel lblCrewId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCrewId");
                nvc.Set(nvc.GetKey(3), lblCrewId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
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
