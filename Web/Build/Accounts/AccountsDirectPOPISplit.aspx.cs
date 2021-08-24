using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsDirectPOPISplit : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    Table gridTable = (Table)gvOrderLine.Controls[0];
    //    ViewState["lblhead"] = "";
    //    foreach (GridViewRow gv in gvOrderLine.Rows)
    //    {
    //        Label lblhead = (Label)gv.FindControl("lblhead");
    //        if (lblhead != null)
    //        {
    //            if (ViewState["lblhead"].ToString().Trim().Equals("") || !ViewState["lblhead"].ToString().Trim().Equals(lblhead.Text.Trim()))
    //            {
    //                ViewState["lblhead"] = lblhead.Text.Trim();
    //                int rowIndex = gridTable.Rows.GetRowIndex(gv);
    //                // Add new group header row  

    //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                TableCell headerCell = new TableCell();

    //                headerCell.ColumnSpan = gvOrderLine.Columns.Count;

    //                headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", lblhead != null ? lblhead.Text.Trim() : "") + "</b></font>";

    //                headerCell.CssClass = "GroupHeaderRowStyle";

    //                // Add header Cell to header Row, and header Row to gridTable  

    //                headerRow.Cells.Add(headerCell);
    //                headerRow.HorizontalAlign = HorizontalAlign.Left;
    //                gridTable.Controls.AddAt(rowIndex, headerRow);
    //            }
    //        }
    //    }

    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                ViewState["ORDERID"] = Request.QueryString["orderid"];
                EditOrder();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            PhoenixToolbar toolbartitle = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsDirectPOPISplit.aspx?" + Request.QueryString, "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDPO')", "Print Grid", "icon_print.png", "PRINT");
            // toolbargrid.AddImageButton("../Accounts/AccountsDirectPOPISplit.aspx?" + Request.QueryString, "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Common/CommonPickListMedicalCase.aspx?vesselid=" + ViewState["VESSELID"] + "&orderid=" + Request.QueryString["orderid"] + "'); return false;", "Add", "Add.png", "ADD");
            MenuMedicalCase.AccessRights = this.ViewState;
            MenuMedicalCase.MenuList = toolbargrid.Show();

            MenuDirectTitle.AccessRights = this.ViewState;
            MenuDirectTitle.Title = "Medical P&I";
            MenuDirectTitle.MenuList = toolbartitle.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMedicalCase_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvMedicalCase.EditIndexes.Clear();
        gvMedicalCase.SelectedIndexes.Clear();
        gvMedicalCase.DataSource = null;
        gvMedicalCase.Rebind();
    }
    protected void gvMedicalCase_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMedicalCase.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditOrder()
    {

        try
        {
            if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
            {
                DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    txtPono.Text = dt.Rows[0]["FLDFORMNO"].ToString();
                    txtCurrency.Text = dt.Rows[0]["FLDCURRENCYCODE"].ToString();
                }
            }
            //DataSet ds = PhoenixAccountsPNIIntergeration.Pnimedicaldpolineitemedit(new Guid(ViewState["ORDERID"].ToString()), new Guid(Request.QueryString["medicalid"].ToString()));

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYNAME"].ToString();
            //    //txtEmployee.Text = ds.Tables[0].Rows[0]["FLDINJURENAME"].ToString();
            //    //txtMedical.Text = ds.Tables[0].Rows[0]["FLDMEDICALREFNO"].ToString();
            //    txtPono.Text = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
            //    //txtType.Text = ds.Tables[0].Rows[0]["FLDHARDNAME"].ToString();
            //    ViewState["CURRENCY"] = ds.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvOrderLine_PreRender(object sender, EventArgs e)
    //{
    //    GridDecorator.MergeRows(gvOrderLine);        
    //}

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblCategoryName")).Text;

                if (currentCategoryName == previousCategoryName)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                        previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }
            }

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        //NameValueCollection nvc = Filter.CurrentPickListSelection;
        //if (nvc != null)
        //{
        //    txtInstituteName.Text = nvc[1].ToString();
        //    txtInstituteId.Text = nvc[2].ToString();

        //}
        Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        DataSet ds = PhoenixAccountsPNIIntergeration.MedicalPNIPoMappingList(new Guid(Request.QueryString["ORDERID"].ToString()));
        iRowCount = ds.Tables[0].Rows.Count;

        gvMedicalCase.DataSource = ds;
        gvMedicalCase.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        // try
        // {
        //     DataSet ds = PhoenixAccountsPNIIntergeration.MedicalPNIPoMappingList(new Guid(Request.QueryString["ORDERID"].ToString()));
        //     if (ds.Tables[0].Rows.Count > 0)
        //     {
        //         gvMedicalCase.DataSource = ds;
        //         gvMedicalCase.DataBind();
        //     }
        //     else
        //     {
        //         DataTable dt = ds.Tables[0];
        //         // ShowNoRecordsFound(dt, gvMedicalCase);
        //     }
        //     //
        //     //if (ds.Tables[0].Rows.Count > 0)
        //     //{
        //     //    gvOrderLine.DataSource = ds;
        //     //    gvOrderLine.DataBind();
        //     //}
        //     //else
        //     //{
        //     //    DataTable dt = ds.Tables[0];
        //     //    ShowNoRecordsFound(dt, gvOrderLine);
        //     //}
        //
        //
        // }
        // catch (Exception ex)
        // {
        //     ucError.ErrorMessage = ex.Message;
        //     ucError.Visible = true;
        // }

    }

    protected void gvOrderLine_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvOrderLine_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    //try
    //    //{
    //    //    GridView _gridView = (GridView)sender;
    //    //    int nCurrentRow = de.RowIndex;
    //    //    string dtkey = (((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text);
    //    //    string pniid = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblmedicalcase")).Text);
    //    //    _gridView.EditIndex = -1;
    //    //    PhoenixAccountsPNIIntergeration.PnidpoLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dtkey));
    //    //    qtyTotal = 0;
    //    //    BindData();

    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    ucError.ErrorMessage = ex.Message;
    //    //    ucError.Visible = true;
    //    //}
    //}
    protected void gvOrderLine_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        qtyTotal = 0;
        BindData();
    }
    protected void gvOrderLine_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        qtyTotal = 0;
        BindData();
    }
    protected void gvOrderLine_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string Amount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text;
            string subtype = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblsubtype")).Text);
            string dtkey = (((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text);
            string pniid = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblmedicalcase")).Text);

            if (!IsValidMedical(Amount))
            {
                ucError.Visible = true;
                return;
            }
            if (dtkey == null || dtkey == string.Empty)
                PhoenixAccountsPNIIntergeration.PnidpoLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ORDERID"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(pniid), null);
            else
                PhoenixAccountsPNIIntergeration.PnidpoLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["ORDERID"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(dtkey), null);
            _gridView.EditIndex = -1;
            qtyTotal = 0;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    decimal qtyTotal = 0;
    // private readonly object lbldate;

    private bool IsValidMedical(string Amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Amount == string.Empty)
            ucError.ErrorMessage = "Amount is required.";
        return (!ucError.IsError);
    }
    protected void gvOrderLine_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdDelete");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (DataBinder.Eval(e.Row.DataItem, "FLDDTKEY").ToString() == null || DataBinder.Eval(e.Row.DataItem, "FLDDTKEY").ToString() == string.Empty)
                    eb.Visible = false;
            }
            decimal tmpTotal = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "FLDAMOUNT").ToString() == "" ? "0" : DataBinder.Eval(e.Row.DataItem, "FLDAMOUNT").ToString());
            qtyTotal += tmpTotal;
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = qtyTotal.ToString();
        }
    }
    protected void gvMedicalCase_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblmedicalcase = (RadLabel)e.Item.FindControl("lblmedicalcase");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            ImageButton cmdinvoice = (ImageButton)e.Item.FindControl("cmdinvoice");
            if (cmdinvoice != null)
            {

                // Label pniid = (Label)e.Row.FindControl("lblmedicalcase").Text;
                //lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Crew/CrewPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "'); return false;");
                cmdinvoice.Attributes.Add("onclick", "openNewWindow('CrewPage','','" + Session["sitepath"] + "/Accounts/AccountsDirectPOPISplitDetails.aspx?directpoid=" + Request.QueryString["orderid"].ToString() + "&medicalid=" + lblmedicalcase.Text + "&type=0'); return false;");
            }
            LinkButton lblCaseNo = (LinkButton)e.Item.FindControl("lblCaseNo");
            if (lblCaseNo != null)
                lblCaseNo.Attributes.Add("onclick", "javascript: parent.Openpopup('CrewPage','','../Inspection/InspectionPNIOperation.aspx?PNIID=" + lblmedicalcase.Text + "'); return false;");
        }
    }

    protected void gvMedicalCase_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                Guid? dtkey = Guid.Parse(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                RadLabel lblmedicalcase = (RadLabel)e.Item.FindControl("lblmedicalcase");
                PhoenixAccountsPNIIntergeration.MedicalPandIPoMappingDelete(dtkey, new Guid(lblmedicalcase.Text), new Guid(Request.QueryString["orderid"].ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                gvMedicalCase.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("INVOICE"))
            {
                Guid? dtkey = Guid.Parse(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                ImageButton cmdinvoice = (ImageButton)e.Item.FindControl("cmdinvoice");
                RadLabel lblCaseNo = (RadLabel)e.Item.FindControl("lblCaseNo");
                cmdinvoice.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Accounts/AccountsDirectPOPISplitDetails.aspx?directpoid=" + Request.QueryString["orderid"].ToString() + "&medicalid=" + lblCaseNo.Text + "&type=notype'); return true;");
                gvMedicalCase.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


}

