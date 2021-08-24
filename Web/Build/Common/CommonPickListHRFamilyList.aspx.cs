using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class CommonPickListHRFamilyList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH");
            //MenuFamilyList.MenuList = toolbarmain.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["PERSONALINFOID"] = "";

            if (Request.QueryString["personalinfoid"] != null && General.GetNullableInteger(Request.QueryString["personalinfoid"].ToString()) != null)
                ViewState["PERSONALINFOID"] = Request.QueryString["personalinfoid"].ToString();

        }
        BindData();
    }

    protected void FamilyList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixCrewHRTravelRequest.HRFamilySearch(General.GetNullableInteger(ViewState["PERSONALINFOID"].ToString())
                        , null
                       );

            if (dt.Rows.Count > 0)
            {
                gvFamilyList.DataSource = dt;
                gvFamilyList.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvFamilyList);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvFamilyList_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvFamilyList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;

        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
        Script += "</script>" + "\n";

        LinkButton lbName = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkName");
        Label lblpersonalinfo = (Label)_gridView.Rows[nCurrentRow].FindControl("lblpersonalMemberId");
        Label lblfamilymemberid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFamilyMemberId");
        Label lblisfamilymember = (Label)_gridView.Rows[nCurrentRow].FindControl("lblIsFamilyMember");

        NameValueCollection nvc = Filter.CurrentPickListSelection;
        nvc.Set(nvc.GetKey(1), lbName.Text.ToString());
        nvc.Set(nvc.GetKey(2), lblpersonalinfo.Text.ToString());
        nvc.Set(nvc.GetKey(3), lblfamilymemberid.Text.ToString());
        nvc.Set(nvc.GetKey(4), lblisfamilymember.Text.ToString());
        Filter.CurrentPickListSelection = nvc;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }
    protected void gvFamilyList_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
