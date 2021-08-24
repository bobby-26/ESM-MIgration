using System;
using System.Web.UI;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;
public partial class PreSeaAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaAddress.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAddress')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:Openpopup('Filter','','../PreSea/PreSeaOfficeFilter.aspx'); return false;", "Filter", "search.png", "FIND");
			toolbar.AddImageButton("../PreSea/PreSeaAddress.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
			toolbar.AddImageLink("javascript:Openpopup('AddAddress','','PreSeaOffice.aspx')", "Add", "add.png", "ADDADDRESS");

            //toolbar.AddImageButton("javascript:Openpopup('PreSeaMail','','PreSeaEmail.aspx')", "Send Presenation", "Email.png", "PRESENTATION");
            toolbar.AddImageButton("../PreSea/PreSeaAddress.aspx", "Send Presenation", "Email.png", "PRESENTATION");

            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
            MenuOffice.SetTrigger(pnlAddressEntry);
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;   
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
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvAddress.EditIndex = -1;
        gvAddress.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string countryid=null;
        DataSet ds;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDSTATENAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "State", "Status" }; ;
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
                    null, 
                    General.GetNullableString(nvc.Get("txtCity").ToString()),
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
                    sortexpression, 
                    sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPreSeaAddress.PreSeaAddressSearch(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);
        }
        General.SetPrintOptions("gvAddress", "Address", alCaptions, alColumns, ds);
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

    protected void gvAddress_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaAddress.DeleteAddress(Convert.ToInt64(((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName")).CommandArgument));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddress_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAddress_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if(del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
        
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {

                Label lblDepartmentName = (Label)e.Row.FindControl("lblDepartmentName");
                UserControlToolTip ucDepartmentTT = (UserControlToolTip)e.Row.FindControl("ucDepartmentTT");
                if (lblDepartmentName != null)
                {
                    lblDepartmentName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDepartmentTT.ToolTip + "', 'visible');");
                    lblDepartmentName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDepartmentTT.ToolTip + "', 'hidden');");
                }

                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkAddressName");

                lbtn.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'PreSeaoffice.aspx?addresscode=" + lbtn.CommandArgument + "'); return false;");

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete()");                
                
                ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'PreSeaoffice.aspx?addresscode=" + lbtn.CommandArgument + "'); return false;");                    
                }

                ImageButton mail = (ImageButton)e.Row.FindControl("cmdEmail");
                if (mail != null) mail.Visible = SessionUtil.CanAccess(this.ViewState, mail.CommandName);

                if (mail != null)
                {
                    mail.Attributes.Add("onclick", "Openpopup('Email', '', '../PreSea/PreSeaEmail.aspx?mailtype=presentation&addresscodelist=" + lbtn.CommandArgument + "'); return false;");
                }


                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBlackList = (Label)e.Row.FindControl("lblIsBlacklisted");
            LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkAddressName");
            if (lblBlackList.Text.Equals("1"))
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                lbtn.ForeColor = System.Drawing.Color.Red;
            }

            ImageButton imgVerification = (ImageButton)e.Row.FindControl("cmdVerification");
            if (imgVerification != null)
            {
                Label lblAddressId = (Label)e.Row.FindControl("lblAddressCode");

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
        string[] alColumns = { "FLDCODE","FLDNAME", "FLDPHONE1","FLDEMAIL1", "FLDCITY", "FLDSTATENAME","FLDHARDNAME" };
        string[] alCaptions = {"Code", "Name", "Phone1","Email", "City","State", "Status" };
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

        Response.AddHeader("Content-Disposition", "attachment; filename=Address.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Address List</h3></td>");
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

    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
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
            else if (dce.CommandName.ToUpper().Equals("PRESENTATION"))
            {
                string address = ",";
                SelectedAddress(ref address);
                address = address == "," ? "" : address;
                string script = "parent.Openpopup('Email','','../PreSea/PreSeaEmail.aspx?mailtype=presentation&addresscodelist=" + address + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedAddress(ref string address)
    {
        if (gvAddress.Rows.Count > 0)
        {
            foreach (GridViewRow row in gvAddress.Rows)
            {
                Label lblAddressCode = (Label)row.FindControl("lblAddressCode");

                CheckBox cb = (CheckBox)row.FindControl("chkItem");

                if (cb.Checked == true)
                {
                    address += lblAddressCode.Text + ",";
                }
            }
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
}
