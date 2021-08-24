using System;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Purchase;
using System.Web.UI;
using System.Drawing;

public partial class PurchaseBroadcast : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
        //    Response.Redirect("PhoenixLogout.aspx");

        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        Response.AppendHeader("Refresh", "300");

        if (!IsPostBack)
        {
            if (Request.QueryString["touser"] != null)
                ViewState["touser"] = Request.QueryString["touser"].ToString();

            if (Request.QueryString["tousername"] != null)
            {
                ViewState["tousername"] = Request.QueryString["tousername"].ToString();
                Title2.Text = "Purchase Comments (User: " + Request.QueryString["tousername"].ToString() + ")";
            }

            if (Request.QueryString["subject"] != null)
            {
                ViewState["SUBJECT"] = Request.QueryString["subject"].ToString();
                txtSubject.Text = ViewState["SUBJECT"].ToString();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Send", "SAVE");
            MenuPhoenixBroadcast.MenuList = toolbar.Show();
        }

        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void PhoenixBroadcast_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["touser"] != null)
            {
                PhoenixCommonBroadcast.Send(General.GetNullableInteger(ViewState["touser"].ToString()), txtSubject.Text, txtMessage.Text);
               
                Reset();
                BindData();
                SetPageNavigator();

                String refreshParent = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", refreshParent, true);
            }
            else
            {
                ucError.ErrorMessage = "Please select the user from the left side to whom you want to send the message";
                ucError.Visible = true;
            }
        }
        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }

    protected void gvPhoenixNews_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvPhoenixNews_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        //_gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

        Label lblId = (Label)_gridView.Rows[de.NewEditIndex].FindControl("lblId");
        Label lblRead = (Label)_gridView.Rows[de.NewEditIndex].FindControl("lblRead");
        Label lblToUser = (Label)_gridView.Rows[de.NewEditIndex].FindControl("lblToUser");

        if (lblRead.Text == "0" && lblToUser.Text == PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
        {
            PhoenixCommonBroadcast.UpdateAsRead(new Guid(lblId.Text));

            //String refreshParent = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", refreshParent, true);
        }
    }

    protected void gvPhoenixNews_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void gvPhoenixNews_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                LinkButton lblSubject = (LinkButton)e.Row.FindControl("lblSubject");

                if (drv["FLDREADYN"].ToString() == "0")
                {
                    lblSubject.Font.Bold = true;
                    lblSubject.ForeColor = Color.Blue;
                }
                else
                {
                    lblSubject.Font.Bold = false;
                    lblSubject.ForeColor = Color.Black;
                }
                //lblSubject.Attributes.Add("onclick", "javascript:cecontrol('row" + e.Row.DataItemIndex.ToString() + "');");
            }
        }
    }

    private void Reset()
    {
        if (ViewState["SUBJECT"] != null)
        {
            txtSubject.Text = ViewState["SUBJECT"].ToString();
            txtMessage.Text = "";
        }
        else
        {
            txtMessage.Text = "";
            txtSubject.Text = "";
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable ds = PhoenixPurchaseBroadcast.ReceiveMessage(
            int.Parse(ViewState["touser"] == null ? "0" : ViewState["touser"].ToString()),
            General.GetNullableString(txtSubject.Text),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Rows.Count > 0)
        {
            gvPhoenixNews.DataSource = ds;
            gvPhoenixNews.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds, gvPhoenixNews);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPhoenixNews.SelectedIndex = -1;
            gvPhoenixNews.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            return true;

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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
