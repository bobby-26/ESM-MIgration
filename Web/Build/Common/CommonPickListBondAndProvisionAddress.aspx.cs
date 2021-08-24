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
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class CommonPickListBondAndProvisionAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");
        MenuAddress.MenuList = toolbarmain.Show();
        MenuAddress.SetTrigger(pnlAddressEntry);

        if (!IsPostBack)
        {
            cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            cblProductType.DataTextField = "FLDQUICKNAME";
            cblProductType.DataValueField = "FLDQUICKCODE";
            cblProductType.DataBind();

            if ((Request.QueryString["addresstype"] != null) && (Request.QueryString["addresstype"] != ""))
                ViewState["addresstype"] = "," + Request.QueryString["addresstype"].ToString() + ",";
            //else
            //    Response.Redirect("../PhoenixUnderConstruction.aspx");
            if (Request.QueryString["txtsupcode"] != null)
            {
                txtCode.Text = Request.QueryString["txtsupcode"].ToString();
            }
            if (Request.QueryString["txtsupname"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            }
            if ((Request.QueryString["producttype"] != null) && (Request.QueryString["productype"] != ""))
            {
                ViewState["producttype"] = "," + Request.QueryString["producttype"].ToString() + ",";

                string[] producttype = ViewState["producttype"].ToString().Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        cblProductType.Items.FindByValue(item).Selected = true;
                    }
                }
            }
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        BindData();
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

            string addresstype = null;
            string producttype = null;
            if (ViewState["addresstype"] != null)
                addresstype = ViewState["addresstype"].ToString();
            if (ViewState["producttype"] != null)
                producttype = ViewState["producttype"].ToString();
            else
                producttype = "";

            ds = PhoenixCommonRegisters.BondAndProvisionAddressSearch(txtCode.Text
                , txtNameSearch.Text
                , null
                , null
                , null
                , General.GetNullableString(txtCountryNameSearch.Text)
                , General.GetNullableString(addresstype)
                , General.GetNullableString(producttype)
                , null
                , null
                , null
                , null
                , null
                , sortexpression
                , sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);           

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAddress.DataSource = ds;
                gvAddress.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAddress);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;

                StringBuilder strproducttype = new StringBuilder();

                foreach (ListItem item in cblProductType.Items)
                {
                    if (item.Selected == true)
                    {
                        strproducttype.Append(item.Value.ToString());
                        strproducttype.Append(",");
                    }
                }
                if (strproducttype.Length > 1)
                {
                    strproducttype.Remove(strproducttype.Length - 1, 1);
                }

                ViewState["producttype"] = strproducttype.ToString();

                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddress_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string Script = "";
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
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
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = new NameValueCollection();
            Label lblCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Add(lblCode.ID, lblCode.Text);
            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Add(lb.ID, lb.Text.ToString());
            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Add(lbl.ID, lbl.Text.ToString());

        }
        else if (Request.QueryString["emailyn"] == "1")
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
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            Label lblCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblCode.Text);

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lbl.Text);

            Label lblemail = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmail");
            nvc.Set(nvc.GetKey(4), lblemail.Text);
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
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            Label lblCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblCode.Text);

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lbl.Text);
        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvAddress_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvAddress_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                Label lbtn = (Label)e.Row.FindControl("lblEmail1");
                Label lblAddressCode = (Label)e.Row.FindControl("lblAddressCode");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucEmailTT");

                HtmlImage img = (HtmlImage)e.Row.FindControl("imgEmail");
                if (img != null)
                    img.Attributes.Add("onclick", "javascript:Openpopup('codehelp2','', '../Registers/RegistersAddressEmail.aspx?addresscode=" + lblAddressCode.Text + "', 'medium')");

                if (lbtn != null && uct != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
                lbtn = (Label)e.Row.FindControl("lblServices");
                uct = (UserControlToolTip)e.Row.FindControl("ucServicesTT");

                if (lbtn != null && uct != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
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

    protected void gvAddress_Sorting(object sender, GridViewSortEventArgs se)
    {
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
        gvAddress.SelectedIndex = -1;
        gvAddress.EditIndex = -1;
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }
}
