using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;


public partial class PurchaseAmosForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            lblorderId.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseAmosForm.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFormDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Purchase/PurchaseAmosFormFilter.aspx", "Find", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState; 
            MenuOrderForm.MenuList = toolbargrid.Show();
            MenuOrderForm.SetTrigger(pnlOrderForm);           

            if (!IsPostBack)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                                     
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["orderid"] = null;
                ViewState["PAGEURL"] = null;

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseAmosFormGeneral.aspx?orderid=" + Request.QueryString["orderid"].ToString();
                    lblorderId.Text = ViewState["orderid"].ToString(); 
                }

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

   
    protected void gvFormDetails_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }

   
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "XSUP", "XSUB", "XDATE", "XDIV", "XSHIP" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Budget Code", "Ordered Date", "Type", "Vessel" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentAmosFormFilterCriteria != null)
        {

            NameValueCollection nvc = Filter.CurrentAmosFormFilterCriteria;
           if(General.GetNullableInteger(nvc.Get("ucVessel").ToString()) != null)
               vesselid= int.Parse(nvc.Get("ucVessel").ToString());
            ds = PhoenixPurchaseAmosForm.AmosFormSearch(vesselid, nvc.Get("txtNumber").ToString(), nvc.Get("txtTitle").ToString(), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseAmosForm.AmosFormSearch(null, null, null, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount,
                                 ref iRowCount, ref iTotalPageCount);
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=OrderForm.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Purchase order form</h3></td>");
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
                //Response.Write(dr[alColumns[i]]);
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int ? vesselid = null ;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "XSUP", "XSUB", "XDATE", "XDIV", "XSHIP" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Budget Code", "Ordered Date", "Type", "Vessel" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = new DataSet();
        if (Filter.CurrentAmosFormFilterCriteria != null)
        {

            NameValueCollection nvc = Filter.CurrentAmosFormFilterCriteria;
            if (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) != null)
                vesselid = int.Parse(nvc.Get("ucVessel").ToString());
            ds = PhoenixPurchaseAmosForm.AmosFormSearch(vesselid, nvc.Get("txtNumber").ToString(),  nvc.Get("txtTitle").ToString(), sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
          }
        else
        {
            ds = PhoenixPurchaseAmosForm.AmosFormSearch(null, null, null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null),
                                 ref iRowCount, ref iTotalPageCount);
        }



        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFormDetails.DataSource = ds;
            gvFormDetails.DataBind();

            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                 lblorderId.Text = ViewState["orderid"].ToString(); 
                gvFormDetails.SelectedIndex = 0;               
            }
                      
             ViewState["PAGEURL"] = "../Purchase/PurchaseAmosFormGeneral.aspx";          
              ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvFormDetails);
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseAmosFormGeneral.aspx"; 

        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvFormDetails", "Amos Form List", alCaptions, alColumns, ds);
    
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvFormDetails.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvFormDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvFormDetails_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormDetails_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
            gvFormDetails.SelectedIndex = -1;
            gvFormDetails.EditIndex = -1;
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

    protected void gvFormDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        BindPageURL(gvFormDetails.SelectedIndex);    
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            ViewState["orderid"] = ((Label)gvFormDetails.Rows[rowindex].FindControl("lblNumber")).Text;
             lblorderId.Text = ViewState["orderid"].ToString(); 
             ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"];
     
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvFormDetails.SelectedIndex = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
    }
}
