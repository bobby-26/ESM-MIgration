using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;

public partial class OptionSupplierContacts : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

       
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
      
            txtVenderID.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Options/OptionSupplierContacts.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvSupplierContact')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../Options/OptionSupplierContacts.aspx", "Find", "search.png", "FIND");
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;                             
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
               
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCREATEDDATE", "FLDEMAILADDRESS", "FLDRELATIONSHIP", "FLDEMAILOPTION", "FLDCONTACTTYPE", "FLDNAME", "FLDUSERNAME" };
        string[] alCaptions = { "Date", "Email Address", "Purpose", "Email Option", "Type", "Supplier", "User" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixPurchaseQuotation.QuotationContactSearch(General.GetNullableInteger(txtVenderID.Text), (ddlStockType.SelectedValue == "Dummy") ? null : ddlStockType.SelectedValue, sortexpression, sortdirection, 1, iRowCount,
                    ref iRowCount, ref iTotalPageCount);
       
        Response.AddHeader("Content-Disposition", "attachment; filename=suppliercontact.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Supplier Contact </center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCREATEDDATE", "FLDEMAILADDRESS", "FLDRELATIONSHIP", "FLDEMAILOPTION", "FLDCONTACTTYPE", "FLDNAME", "FLDUSERNAME" };
        string[] alCaptions = { "Date", "Email Address", "Purpose", "Email Option", "Type", "Supplier", "User" };
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixPurchaseQuotation.QuotationContactSearch(General.GetNullableInteger(txtVenderID.Text), (ddlStockType.SelectedValue.ToString() == "Dummy") ? null : ddlStockType.SelectedValue.ToString(), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null),
                      ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSupplierContact.DataSource = ds;
            gvSupplierContact.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSupplierContact);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvSupplierContact", "Supplier Contract " , alCaptions, alColumns, ds);
    }

    protected void gvSupplierContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void gvSupplierContact_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }       
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            Label lb1 = (Label)e.Row.FindControl("lblEmailOption");
            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblEmailOption");
            if (rbl != null && lb1!=null)
            {
                rbl.SelectedValue = lb1.Text;
            }            
      
        }
    }

    protected bool IsValidTax(string strDescription, string strValueType, string strValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if(strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Email address is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Email option is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Purpose is required.";
        return (!ucError.IsError);
    }

    protected void gvSupplierContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSupplierContact_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }

    protected void gvSupplierContact_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void InsertQuotationTax(string stremailaddress, string relationship, string emailoption)
    {
        try
        {
            PhoenixPurchaseQuotation.QuotationContactsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVenderID.Text), stremailaddress, relationship, emailoption, Filter.CurrentPurchaseStockType.ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateQuotationTax(Guid addresscontactid, string stremailaddress, string relationship, string emailoption)
    {
        try
        {
            PhoenixPurchaseQuotation.QuotationContactsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscontactid, int.Parse(txtVenderID.Text), stremailaddress, relationship, emailoption);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteQuotationTax(Guid relationshipid)
    {
        PhoenixPurchaseQuotation.QuotationContactsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, relationshipid);
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

    protected void MenuVender_TabStripCommand(object sender, EventArgs e)
    {

    }

    private void ClearTextBox()
    {
        txtVenderCode.Text = "";
        txtVenderName.Text = "";
       

    }

    private void InsertOrderVender()
    {
        
        InsertUpdateEmail();
    }

    private void UpdateOrderVender()
    {
        
        InsertUpdateEmail();
    }
    private void InsertUpdateEmail()
    {
        //if(txtEmailAddress.Text.Trim().Length>1)    
        //    PhoenixPurchaseQuotation.UpdateInsertQuotationEmail(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()),
        //    txtEmailAddress.Text);
    }

    private bool IsValidVender()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtVenderID.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required. Please Select ";
        //if (General.GetDateTimeToString(txtExpirationDate.Text) != null && Convert.ToDateTime(txtExpirationDate.Text) <= Convert.ToDateTime(DateTime.Now.ToString()))
        //    ucError.ErrorMessage = "Expiry  date should be greater than today's date.";  
        if (ViewState["orderid"].ToString() == "0")
            ucError.ErrorMessage = "Please select a form to assign a vendor ";
        return (!ucError.IsError);
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvSupplierContact.SelectedIndex = -1;
            gvSupplierContact.EditIndex = -1;
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


    protected void gvSupplierContact_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }
}
