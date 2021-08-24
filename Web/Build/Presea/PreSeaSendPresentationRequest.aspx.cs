using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaSendPresentationRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Send Mail", "MAIL");
            //MenuPreSea.MenuList = toolbarmain.Show();
            //MenuPreSea.AccessRights = this.ViewState;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaSendPresentationRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPreSea')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:Openpopup('Filter','','../PreSea/PreSeaOfficeFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbar.AddImageButton("../PreSea/PreSeaSendPresentationRequest.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuPreSeaGrid.AccessRights = this.ViewState;
            MenuPreSeaGrid.MenuList = toolbar.Show();
            MenuPreSeaGrid.SetTrigger(pnlPreSeaPresentationRequest);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MAILCODE"] = String.Empty;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            }
            ViewState["MAILCODE"] = ucMail.SelectedMailTemplate;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("MAIL"))
            {
                if (!IsValidateSendBulkMail())
                {
                    ucError.Visible = true;
                    return;
                }
                // Save bulk send mail request in database and schedule it
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvPreSea.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string countryid = null;
        DataSet ds;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" }; ;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentPreSeaAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentPreSeaAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixPreSeaAddress.PreSeaAddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, General.GetNullableString(nvc.Get("txtCity").ToString()),
                null,
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                General.GetNullableString(nvc.Get("txtBusinessProfile").ToString()),
                General.GetNullableString(nvc.Get("addressdepartment").ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPreSeaAddress.PreSeaAddressSearch(null, null, null, null, null, null, General.GetNullableString("138,1212,1213"), null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);
        }
        General.SetPrintOptions("gvPreSea", "Schools, Colleges and Institutes List", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSea.DataSource = ds;
            gvPreSea.DataBind();

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSea);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvPreSea_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPreSea_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton mail = (ImageButton)e.Row.FindControl("cmdEmail");
            if (mail != null) mail.Visible = SessionUtil.CanAccess(this.ViewState, mail.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {

                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkAddressName");
                lbtn.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../PreSea/PreSeaoffice.aspx?addresscode=" + lbtn.CommandArgument + "'); return false;");

                if (mail != null)
                {
                    mail.Attributes.Add("onclick", "Openpopup('Email', '', '../PreSea/PreSeaEmail.aspx?mailtype=OCPSCH&AddressCode=" + lbtn.CommandArgument + "&mailcode=" + ViewState["MAILCODE"].ToString() + "'); return false;");
                }
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }

            ImageButton imgVerification = (ImageButton)e.Row.FindControl("cmdVerification");
            if (imgVerification != null) 
            {
                Label lblAddressId = (Label)e.Row.FindControl("lblAddressId");

                imgVerification.Visible = SessionUtil.CanAccess(this.ViewState, imgVerification.CommandName);
                imgVerification.Attributes.Add("onclick", "Openpopup('Report', '', '../Reports/ReportsView.aspx?applicationcode=10&reportcode=VERIFICATIONLETTER&addressid=" + lblAddressId.Text + "&showmenu=0');return false;");
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string countryid = null;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentPreSeaAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentPreSeaAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixPreSeaAddress.PreSeaAddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, General.GetNullableString(nvc.Get("txtCity").ToString()),
                null,
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                General.GetNullableString(nvc.Get("txtBusinessProfile").ToString()),
                General.GetNullableString(nvc.Get("addressdepartment").ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPreSeaAddress.PreSeaAddressSearch(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Schools, Colleges and Institutes List.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schools, Colleges and Institutes List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
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
        gvPreSea.SelectedIndex = -1;
        gvPreSea.EditIndex = -1;
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

    protected void MenuPreSeaGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPreSeaAddressFilterCriteria = null;
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        SetPageNavigator();
    }

    private bool IsValidateSendBulkMail()
    {
        string SelectedItems = String.Empty;
        foreach (GridViewRow r in gvPreSea.Rows)
        {

            CheckBox chk = (CheckBox)r.FindControl("chkItem");
            if (chk != null)
            {
                Label lblAddressCode = (Label)r.FindControl("lblAddressCode");
                if (lblAddressCode != null && chk.Checked)
                    SelectedItems = SelectedItems + lblAddressCode.Text + ",";
            }
        }

        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(ucMail.SelectedMailTemplate))
        {
            ucError.ErrorMessage = "Mail Format";
        }
        if (String.IsNullOrEmpty(SelectedItems.TrimEnd(',')))
        {
            ucError.ErrorMessage = "Please select atleast one address for send a mail.";
        }

        return (!ucError.IsError);
    }
}
