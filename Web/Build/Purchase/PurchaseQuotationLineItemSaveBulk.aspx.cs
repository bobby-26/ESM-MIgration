using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;

public partial class Purchase_PurchaseQuotationLineItemSaveBulk : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvVendorItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvVendorItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                pnlErrorMessage.Style["position"] = "absolute";
                pnlErrorMessage.Style["left"] = "350px";
                pnlErrorMessage.Style["filter"] = "alpha(opacity=95)";
                pnlErrorMessage.Style["-moz-opacity"] = ".95";
                pnlErrorMessage.Style["opacity"] = ".95";
                pnlErrorMessage.Style["z-index"] = "99";
                pnlErrorMessage.Style["background-color"] = "White";
                pnlErrorMessage.Style["display"] = "none";

                PhoenixToolbar toolSave = new PhoenixToolbar();
                toolSave.AddImageLink("javascript:bulkSaveOfQuotationLine(); return false;", "Save", "", "BULKSAVE");
                toolSave.AddButton("Back", "BACK");
                menuSaveDetails.MenuList = toolSave.Show();

                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Purchase/PurchaseQuotationLineItem.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdClose_Click(object sender, EventArgs e)
    {
        spnErrorMessage.InnerHtml = "";
        this.Visible = false;
    }

    protected void pnlErrorMessage_Load(object sender, EventArgs e)
    {
        string str = "var dg = document.getElementById(\"" + pnlErrorMessage.ClientID + "\");";
        str += "if (dg != null)";
        str += "{";
        str += "dg.style.top = Number((window.pageYOffset ? window.pageYOffset : (document.documentElement ? document.documentElement.scrollTop : (document.body ? document.body.scrollTop : 0)))) + \"px\";";
        str += "}";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", str, true);
    }

    private void BindData()
    {
        int iRowCount = 1000;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        DataSet ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("",
            ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVendorItem.DataSource = ds;
            gvVendorItem.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVendorItem);
        }
    }

    protected void gvVendorItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Label lbm = (Label)e.Row.FindControl("lblMakerReferenceHeader");
            if (lbm != null && Filter.CurrentPurchaseStockType == "STORE")
                lbm.Text = "Product Code";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblComponentName = (Label)e.Row.FindControl("lblComponentName");

            Label lblvesselid = (Label)e.Row.FindControl("lblVesselid");
            Label lblcomponentid = (Label)e.Row.FindControl("lblComponentId");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
            }

            Label item = (Label)e.Row.FindControl("lblItemid");
            DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ddlUnit != null)
            {
                ddlUnit.DataSource = PhoenixRegistersUnit.ListPurchaseUnit(item.Text, Filter.CurrentPurchaseStockType
                                                                     , Filter.CurrentPurchaseVesselSelection);

                ddlUnit.DataBind();
                ddlUnit.SelectedValue = drv["FLDUNITID"].ToString();
            }
        }
    }

    protected void gvVendorItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null);", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
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

    private bool IsValidRemark()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["quotationlineid"] == null || ViewState["quotationlineid"].ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Line item selection is required. ";

        return (!ucError.IsError);
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
}
