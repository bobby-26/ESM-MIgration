using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseContractDeliveryLocation : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        BindData();
        SetPageNavigator();
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Item", "ITEM");
            toolbarmain.AddButton("Delivery","DELIVERY");
            MenuFormGeneral.MenuList = toolbarmain.Show();
            MenuFormGeneral.SetTrigger(pnlFormGeneral);
            MenuFormGeneral.SelectedMenuIndex = 2;
           
           
            if (!IsPostBack)
            {
                Title1.Text = "Delivery Location  (" + PhoenixPurchaseContract.VendorName + ")";

                if (Request.QueryString["contractid"] != null)
                {
                    ViewState["contractid"] = Request.QueryString["contractid"].ToString();
                }

                if (Request.QueryString["vendorid"] != null)
                {
                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                }
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseContractDeliveryLocation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvContractDeliveryLocation')", "Print Grid", "icon_print.png", "");
            MenuPurchaseContract.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

            ucVendorZone.SelectedVendor = int.Parse(ViewState["vendorid"].ToString());

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
        string[] alColumns = { "FLDCOUNTRYNAME", "FLDSEAPORTNAME", "FLDFACTOR" };
        string[] alCaptions = { "Country Name", "Sea Port","Factor(%)" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseContract.ContractDeliveryLocationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    int.Parse(ViewState["vendorid"].ToString()),
                                    General.GetNullableInteger(ucVendorZone.SelectedVendorZone),
                                    sortexpression,sortdirection, (int)ViewState["PAGENUMBER"], 
                                    General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvContractDeliveryLocation.DataSource = ds;
            gvContractDeliveryLocation.DataBind();
            if (ViewState["contractdeliverylocationid"] == null)
            {
                ViewState["contractdeliverylocationid"] = ds.Tables[0].Rows[0]["FLDDELIVERYLOCATIONID"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvContractDeliveryLocation);
            
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvContractDeliveryLocation","Delivery Location", alCaptions, alColumns, ds);

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOUNTRYNAME", "FLDSEAPORTNAME", "FLDFACTOR" };
        string[] alCaptions = { "Country Name", "Sea Port", "Factor(%)" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

       ds = PhoenixPurchaseContract.ContractDeliveryLocationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           int.Parse(ViewState["vendorid"].ToString()),
                           General.GetNullableInteger(ucVendorZone.SelectedVendorZone),
                           sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                           iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PurchaseContractDeliveryLocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.jpg' /></td>");
        Response.Write("<td><h3>Purchase Contract</h3></td>");
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
    
    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseContract.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseContract.aspx?contractid=" + ViewState["contractid"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("ITEM"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseVendorProductZone.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseVendorProductZone.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString());
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidZone(string vendorzoneid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vendorzoneid.Trim() == "")
            ucError.ErrorMessage = "Zone is required";

        return (!ucError.IsError);
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvContractDeliveryLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }



    protected void gvContractDeliveryLocation_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvContractDeliveryLocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvContractDeliveryLocation_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }

       
    }

    protected void gvContractDeliveryLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblContractDeliveryLocationIdEdit")).Text == "")
            {
                string factor = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucFactorEdit")).Text;

                if (!IsValidFactor(factor))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertContractDeliveryLocation(ViewState["vendorid"].ToString(), ucVendorZone.SelectedVendorZone,
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVendorZonePortIdEdit")).Text,
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucFactorEdit")).Text
                    );
            }
            else
            {
                string factor = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucFactorEdit")).Text;

                if (!IsValidFactor(factor))
                {
                    ucError.Visible = true;
                    return;
                }


                UpdateContractDeliveryLocation(((Label)_gridView.Rows[nCurrentRow].FindControl("lblContractDeliveryLocationIdEdit")).Text,
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucFactorEdit")).Text
                        );
            }
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            gvContractDeliveryLocation.SelectedIndex = -1;
            gvContractDeliveryLocation.EditIndex = -1;
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
    }

    protected void gvContractDeliveryLocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseContract.DeleteContractDeliveryLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblContractDeliveryLocationId")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvContractDeliveryLocation_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }

    private void UpdateContractDeliveryLocation(string deliverylocationid,string factor)
    {
        PhoenixPurchaseContract.UpdateDeliveryLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(deliverylocationid),
                                                            General.GetNullableDecimal(factor));
    }

    private void InsertContractDeliveryLocation(string vendorid,string vendorzoneid,string vendorzoneportid,string factor)
    {
        PhoenixPurchaseContract.InsertDeliveryLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,int.Parse(vendorid),
                                                General.GetNullableInteger(vendorzoneid),General.GetNullableInteger(vendorzoneportid),decimal.Parse(factor));
    }
    private bool IsValidFactor(string factor)
    {
        ucError.HeaderMessage = "Please Provide the necessary details.";

            if (factor.Trim().Equals(""))
                ucError.ErrorMessage = "Factor is required.";

       return (!ucError.IsError);
    }
}
