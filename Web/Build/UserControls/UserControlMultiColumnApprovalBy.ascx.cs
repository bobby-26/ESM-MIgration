using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserControlMultiColumnApprovalBy : System.Web.UI.UserControl
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvMulticolumn.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvMulticolumn.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        txtmultivalue.Attributes.Add("style", "visibility:hidden;");
        btnsearch.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            panel1.Attributes.CssStyle.Add("position", "absolute");
            panel1.Width = Unit.Parse((txtmuticolumn.Width.Value + btnmulticolumn.Width.Value).ToString());

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        else
        {
            if (panel1.Visible)
            {
                BindData();
                SetPageNavigator();
            }
        }
    }

    public void btnmulticolumn_OnClick(object sender, EventArgs e)
    {
        if (!panel1.Visible)
            panel1.Visible = true;
        else
            panel1.Visible = false;
        ViewState["SEARCH"] = null;

        gvMulticolumn.EditIndex = -1;
        ViewState["CURRENTINDEX"] = -1;
        gvMulticolumn.SelectedIndex = -1;
        ViewState["PAGENUMBER"] = 1;

        BindData();
        SetPageNavigator();
        showbtn();
    }

    private void showbtn()
    {
        if (!panel1.Visible)
        {
            btnmulticolumn.ImageUrl = Session["images"] + "/arrowDown.png";
            btnmulticolumn.ToolTip = "Show All";
        }
        else
        {
            btnmulticolumn.ImageUrl = Session["images"] + "/te_del.png";
            btnmulticolumn.ToolTip = "Close";
        }
    }
    public bool Enabled
    {
        get
        {
            return txtmuticolumn.Enabled;
        }
        set
        {
            txtmuticolumn.Enabled = value;
            btnmulticolumn.Enabled = value;
        }
    }
    public string Width
    {
        get
        {
            return panel1.Width.ToString();
        }
        set
        {
            string WidthVal = value;
            if (!string.IsNullOrEmpty(WidthVal))
            {
                if (WidthVal.Contains("%"))
                    return;
                panel1.Width = Unit.Parse(WidthVal);
                txtmuticolumn.Width = Unit.Parse((panel1.Width.Value - btnmulticolumn.Width.Value).ToString());
            }
        }
    }
    public string CssClass
    {
        set
        {
            txtmuticolumn.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            if (!string.IsNullOrEmpty(txtmuticolumn.Text))
                return txtmultivalue.Text;
            else
                return txtmuticolumn.Text;
        }
        set
        {
            txtmultivalue.Text = value;
        }

    }
    public string Text
    {
        get
        {
            return txtmuticolumn.Text;
        }
        set
        {
            txtmuticolumn.Text = value;
        }
    }
    protected void btnsearch_click(object sender, EventArgs e)
    {
        ViewState["SEARCH"] = txtmuticolumn.Text;
        panel1.Visible = true;
        gvMulticolumn.EditIndex = -1;
        ViewState["CURRENTINDEX"] = -1;
        gvMulticolumn.SelectedIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCommonPurchase.ApprovalUserSearch(
                       null,
                       null,
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       General.ShowRecords(null),
                       ref iRowCount,
                       ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMulticolumn.DataSource = ds;
            gvMulticolumn.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvMulticolumn);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvMulticolumn_RowDataBound(object sender, GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
            e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

        }
    }
    protected void gvMulticolumn_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMulticolumn, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvMulticolumn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                panel1.Visible = false;
                txtmuticolumn.Text = ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkUserName")).Text.ToString();
                txtmultivalue.Text = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblUserCode")).Text.ToString();
                showbtn();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMulticolumn_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }
    protected void gvMulticolumn_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvMulticolumn.EditIndex = -1;
        ViewState["CURRENTINDEX"] = -1;
        gvMulticolumn.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvMulticolumn.SelectedIndex = -1;
        gvMulticolumn.EditIndex = -1;
        ViewState["CURRENTINDEX"] = -1;
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
        gvMulticolumn.SelectedIndex = -1;
        gvMulticolumn.EditIndex = -1;
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
}