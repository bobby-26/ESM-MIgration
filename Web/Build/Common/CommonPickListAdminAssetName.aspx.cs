using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class Common_CommonPickListAdminAssetName : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Search", "SEARCH");
        toolbar.AddButton("Clear", "CLEAR");
        MenuAssetNameList.MenuList = toolbar.Show();

        PhoenixToolbar MainToolbar = new PhoenixToolbar();
        MainToolbar.AddButton("Asset", "ASSET");
        MenuAsset.AccessRights = this.ViewState;
        MenuAsset.MenuList = MainToolbar.Show();
        MenuAsset.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            if (Request.QueryString["CategoryType"] != string.Empty)
            {
                ViewState["TYPE"] = int.Parse(Request.QueryString["CategoryType"]);
            }
            else
            {
                ViewState["TYPE"] = string.Empty;
            }
            if (Request.QueryString["ZoneId"] != null)
            {
                ViewState["COMPANYID"] = Request.QueryString["ZoneId"];
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
    }
    protected void Asset_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("ASSET"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAssetNameList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            txtSerialno.Text = "";
            txtAssetName.Text = "";
            BindData();
            SetPageNavigator();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 10;

        DataSet ds;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 0;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonAdministration.AssetNameSearch(
            txtAssetName.Text.Trim()
            , General.GetNullableInteger(ViewState["TYPE"].ToString())
            , txtSerialno.Text.Trim()
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , sortdirection
            , sortexpression
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAssetName.DataSource = ds;
            gvAssetName.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAssetName);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    protected void gvAssetName_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAssetName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());

                Label lbId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetId");
                nvc.Set(nvc.GetKey(2), lbId.Text.ToString());
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAssetName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());

                Label lbId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetId");
                nvc.Set(nvc.GetKey(2), lbId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        if (e.CommandName.ToUpper().Equals("ITEMLIST"))
        {
            Label lblAssetId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetId");
            Label lblCompanyId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCompanyId");
            Response.Redirect("../Common/CommonPickListAdminItemName.aspx?ASSEMBLYPARENTID=" + lblAssetId.Text + "&COMPANYID=" + lblCompanyId.Text);
        }
    }

    protected void gvAssetName_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvAssetName.SelectedIndex = -1;
        gvAssetName.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void gvAssetName_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }
    protected void gvAssetName_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
}
