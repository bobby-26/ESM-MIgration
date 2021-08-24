using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersAddressByContact : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAddress.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvAddress.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }        
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();           
            toolbarmain.AddButton("Save", "SAVE");
            MenuAddress.AccessRights = this.ViewState;
            MenuAddress.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAddressByContact.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAddress')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:Openpopup('Filter','','RegistersOfficeFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersAddressByContact.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
            MenuOffice.SetTrigger(pnlAddressEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ADDRESSCODE"] = null;
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
        BindData();
        SetPageNavigator();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string countryid = null;
        DataSet ds;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDADDRESS1", "FLDADDRESS2", "FLDCOUNTRYNAME", "FLDSTATENAME", "FLDCITYNAME", "FLDPOSTALCODE", "FLDEMAIL1", "FLDPHONE1" };
        string[] alCaptions = { "Code", "Name", "Address1", "Address2", "Country", "State", "City", "Postal Code", "Email" , "Phone",  }; 
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());      

        if (Filter.CurrentAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixCommonRegisters.AddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, null,
                General.GetNullableString(nvc.Get("txtCity").ToString()),
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);            
        }
        else
        {
            ds = PhoenixCommonRegisters.AddressSearch(null, null, null, null, null, null, null, null, null, null, null,null,null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);
        }
        
        General.SetPrintOptions("gvAddress", "Address List By Contact", alCaptions, alColumns, ds);
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
    protected void Address_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAddress())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["ADDRESSCODE"] != null)
                {
                    PhoenixRegistersAddress.UpdateAddressContactDetails(int.Parse(ViewState["ADDRESSCODE"].ToString()), General.GetNullableString(txtAddress1.Text),
                        General.GetNullableString(txtAddress2.Text), General.GetNullableInteger(ucCountry.SelectedCountry),
                        General.GetNullableInteger(ucState.SelectedState), General.GetNullableInteger(ddlCity.SelectedCity),
                        General.GetNullableString(txtPostalCode.Text), General.GetNullableString(txtEmail1.Text), General.GetNullableString(txtPhone1.Text));

                    ucStatus.Text = "Contact Details are updated successfully.";
                    ucStatus.Visible = true;

                }               
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
    protected void gvAddress_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }
    protected void gvAddress_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            AddressEdit();

            BindData();
            txtAddress1.Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvAddress_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["ADDRESSCODE"] = Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAddresscode")).Text);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
            
    }
    protected void gvAddress_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvAddress_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
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
                Label lbtn = (Label)e.Row.FindControl("lblAddress1");
                if (lbtn != null)
                {
                    UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress1");
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
                }
                Label lb = (Label)e.Row.FindControl("lblAddress2");
                if (lb != null)
                {
                    UserControlToolTip uct1 = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress2");
                    lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct1.ToolTip + "', 'visible');");
                    lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct1.ToolTip + "', 'hidden');");
                }
                Label lbe = (Label)e.Row.FindControl("lblEmail1");
                if (lbe != null)
                {
                    UserControlToolTip uce = (UserControlToolTip)e.Row.FindControl("ucToolTipEmail");
                    lbe.Attributes.Add("onmouseover", "showTooltip(ev,'" + uce.ToolTip + "', 'visible');");
                    lbe.Attributes.Add("onmouseout", "showTooltip(ev,'" + uce.ToolTip + "', 'hidden');");
                }

                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) 
                    att.Visible = false;

                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
                Label lblAddresscode = (Label)e.Row.FindControl("lblAddresscode");

                if (lblIsAtt.Text == string.Empty) 
                    att.ImageUrl = Session["images"] + "/no-attachment.png";

                att.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1', '', '../Registers/RegistersAddressAttachment.aspx?ADDRESSCODE=" + lblAddresscode.Text + "'); return false;");
            }
        }        
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDADDRESS1", "FLDADDRESS2", "FLDCOUNTRYNAME", "FLDSTATENAME", "FLDCITYNAME", "FLDPOSTALCODE", "FLDEMAIL1", "FLDPHONE1" };
        string[] alCaptions = { "Code", "Name", "Address1", "Address2", "Country", "State", "City", "Postal Code", "Email", "Phone", };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonRegisters.AddressSearch(null, null, null, null, null, null, null, null, null, null, null,null,null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Address.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Address List By Contact</h3></td>");
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
                Filter.CurrentAddressFilterCriteria = null;
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
    
    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucState.SelectedState));
    }
    protected bool IsValidAddress()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtEmail1.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Email is required.";
        else if (!General.IsvalidEmail(txtEmail1.Text))
            ucError.ErrorMessage = "Please Enter valid Email Address.";
        if (txtPhone1.Text.Trim().Replace("~", "").Equals(""))
        {
            ucError.ErrorMessage = "Phone number  is required.";
        }
        else if (!General.IsValidPhoneNumber(txtPhone1.Text))
            ucError.ErrorMessage = "Enter area code for phone number.";

        return (!ucError.IsError);
    }
    private void Reset()
    {
        ViewState["ADDRESSCODE"] = null;        
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        ucCountry.SelectedCountry = "";
        ucState.SelectedState = "";
        ddlCity.SelectedCity = "";
        txtPostalCode.Text = "";
        txtEmail1.Text = "";
        txtPhone1.Text = "";
        txtCode.Text = "";
        txtName.Text = "";
    }
    protected void gvAddress_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAddress, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void AddressEdit()
    {
        if (ViewState["ADDRESSCODE"] != null)
        {
            DataSet ds = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["ADDRESSCODE"].ToString()));

            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCode.Text = dr["FLDCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtAddress1.Text = dr["FLDADDRESS1"].ToString();
                txtAddress2.Text = dr["FLDADDRESS2"].ToString();
                ucCountry.SelectedCountry = dr["FLDCOUNTRYID"].ToString();
                ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
                ucState.SelectedState = dr["FLDSTATE"].ToString();
                ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucState.SelectedState));
                ddlCity.SelectedCity = dr["FLDCITY"].ToString();
                txtPostalCode.Text = dr["FLDPOSTALCODE"].ToString();
                txtEmail1.Text = dr["FLDEMAIL1"].ToString();
                txtPhone1.Text = dr["FLDPHONE1"].ToString();                
                BindData();
            }
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

}
