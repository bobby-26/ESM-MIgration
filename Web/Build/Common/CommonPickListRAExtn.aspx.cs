using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class CommonPickListRAExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH");
            MenuPortAgent.MenuList = toolbarmain.Show();
            MenuPortAgent.SetTrigger(pnlPortAgentEntry);

            if (!IsPostBack)
            {

                ViewState["type"] = "5";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                rblType.SelectedIndex = 0;

                string opt = Request.QueryString["opt"];
                if (opt != null && opt != string.Empty)
                {
                    foreach (ListItem li in rblType.Items)
                        li.Enabled = false;
                    string[] str = opt.Split(',');
                    foreach (string s in str)
                    {
                        if (s.ToUpper() == "G")
                        {
                            rblType.Items[0].Enabled = true;
                            rblType.SelectedIndex = 0;
                        }
                        else if (s.ToUpper() == "M")
                        {
                            rblType.Items[1].Enabled = true;
                            rblType.SelectedIndex = 1;
                        }
                        else if (s.ToUpper() == "N")
                        {
                            rblType.Items[2].Enabled = true;
                            rblType.SelectedIndex = 2;
                        }
                        else if (s.ToUpper() == "C")
                        {
                            rblType.Items[3].Enabled = true;
                            rblType.SelectedIndex = 3;
                        }
                    }
                }
                BindCategory();
            }
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPortAgent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
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
        ds = PhoenixInspectionRiskAssessmentProcessExtn.RiskAssessmentByCategoryList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                    , General.GetNullableInteger(rblType.SelectedValue)
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , General.ShowRecords(null)
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , txtActivity.Text
                                                    , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPortAgent.DataSource = ds;
            gvPortAgent.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPortAgent);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvPortAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            Label lblRefNo = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRefNo");
            nvc.Add(lblRefNo.ID, lblRefNo.Text);

            if (rblType.SelectedValue == "1")
            {
                Label lblRA = (Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessName");
                nvc.Add(lblRA.ID, lblRA.Text);
            }
            else
            {
                Label lblRA = (Label)_gridView.Rows[nCurrentRow].FindControl("lblActivity");
                nvc.Add(lblRA.ID, lblRA.Text);
            }

            Label lblRAId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRAId");
            nvc.Add(lblRAId.ID, lblRAId.Text);
            Label lblType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblType");
            nvc.Add(lblType.ID, lblType.Text);
        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            Label lblRefNo = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRefNo");
            nvc.Set(nvc.GetKey(1), lblRefNo.Text);

            if (rblType.SelectedValue == "1")
            {
                Label lblRA = (Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessName");
                nvc.Set(nvc.GetKey(2), lblRA.Text);
            }
            else
            {
                Label lblRA = (Label)_gridView.Rows[nCurrentRow].FindControl("lblActivity");
                nvc.Set(nvc.GetKey(2), lblRA.Text);
            }

            Label lblRAId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRAId");
            nvc.Set(nvc.GetKey(3), lblRAId.Text);

            Label lblType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblType");
            nvc.Set(nvc.GetKey(4), lblType.Text);

        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvPortAgent_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPortAgent_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvPortAgent_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            if (rblType.SelectedValue != "1")
            {
                e.Row.Cells[3].Visible = false;
            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lbl = (Label)e.Row.FindControl("lblPortAgent");

            //LinkButton lb = (LinkButton)e.Row.FindControl("lnkPortAgentName");
            //if (lb != null) lb.Attributes.Add("onclick", "Openpopup('codehelp3', '', 'OptionsPortAgent.aspx?usercode=" + lbl.Text + "')");

            //HtmlImage img = (HtmlImage)e.Row.FindControl("imgGroupList");
            //img.Attributes.Add("onclick", "showMoreInformation(ev, 'OptionsMoreInfoGroupList.aspx?usercode=" + lbl.Text + "')");

            //ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            //if (eb != null) eb.Attributes.Add("onclick", "Openpopup('codehelp3', '', 'OptionsPortAgent.aspx?usercode=" + lbl.Text + "')");

            Label lbtn = (Label)e.Row.FindControl("lblActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipActivity");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            if (rblType.SelectedValue != "1")
            {
                e.Row.Cells[3].Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        gvPortAgent.SelectedIndex = -1;
        gvPortAgent.EditIndex = -1;
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        if (rblType.SelectedValue == "1") //PROCESS
            ViewState["type"] = "5";
        else if (rblType.SelectedValue == "2") //GENERIC
            ViewState["type"] = "2";
        else if (rblType.SelectedValue == "3") //MACHINERY
            ViewState["type"] = "3";
        else if (rblType.SelectedValue == "4") //NAVIGATION
            ViewState["type"] = "1";
        else if (rblType.SelectedValue == "5") //CARGO
            ViewState["type"] = "4";

        BindCategory();
    }

    protected void BindCategory()
    {
        if (rblType.SelectedValue == "1") //PROCESS
            ViewState["type"] = "5";
        else if (rblType.SelectedValue == "2") //GENERIC
            ViewState["type"] = "4";
        else if (rblType.SelectedValue == "3") //MACHINERY
            ViewState["type"] = "3";
        else if (rblType.SelectedValue == "4") //NAVIGATION
            ViewState["type"] = "1";
        else if (rblType.SelectedValue == "5") //CARGO
            ViewState["type"] = "2";

        ddlCategory.DataSource = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(int.Parse(ViewState["type"].ToString()),
                                                                                                          General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
}