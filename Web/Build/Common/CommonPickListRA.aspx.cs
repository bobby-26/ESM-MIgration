using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonPickListRA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuPortAgent.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["type"] = "5";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                gvPortAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                BindCategory();
                rblType.SelectedIndex = 0;

                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                string opt = Request.QueryString["opt"];
                if (opt != null && opt != string.Empty)
                {
                    foreach (ButtonListItem li in rblType.Items)
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
            }
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPortAgent.Rebind();
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
        ds = PhoenixInspectionDailyWorkPlanActivity.RiskAssessmentByCategoryList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                    , General.GetNullableInteger(rblType.SelectedValue)
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , General.ShowRecords(null)
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , txtActivity.Text
                                                    , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        gvPortAgent.DataSource = ds;
        gvPortAgent.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
    }

    protected void gvPortAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
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

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Add(lblRefNo.ID, lblRefNo.Text);

                if (rblType.SelectedValue == "1")
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblProcessName");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }
                else
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblActivity");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }

                RadLabel lblRAId = (RadLabel)e.Item.FindControl("lblRAId");
                nvc.Add(lblRAId.ID, lblRAId.Text);
                RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
                nvc.Add(lblType.ID, lblType.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Set(nvc.GetKey(1), lblRefNo.Text);

                if (rblType.SelectedValue == "1")
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblProcessName");
                    nvc.Set(nvc.GetKey(2), lblRA.Text);
                }
                else
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblActivity");
                    nvc.Set(nvc.GetKey(2), lblRA.Text);
                }

                RadLabel lblRAId = (RadLabel)e.Item.FindControl("lblRAId");
                nvc.Set(nvc.GetKey(3), lblRAId.Text);

                RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
                nvc.Set(nvc.GetKey(4), lblType.Text);

            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
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

    protected void gvPortAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipActivity");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }
            if (rblType.SelectedValue != "1")
            {
                e.Item.Cells[3].Visible = true;
            }
        }
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvPortAgent.SelectedIndex = -1;
    //    gvPortAgent.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
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
        ddlCategory.DataSource = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(int.Parse(ViewState["type"].ToString()),
                                                                                                          General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void gvPortAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPortAgent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
