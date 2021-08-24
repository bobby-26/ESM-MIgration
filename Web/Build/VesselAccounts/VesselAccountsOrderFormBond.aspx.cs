using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsOrderFormBond :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsOrderFormBond.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvBondReq')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsOrderFormBond.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsOrderFormBond.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Clear", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsOrderFormBond.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Add New", "add.png", "NEW");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            MenuBondReq.SetTrigger(pnlComponent);

            if (!IsPostBack)
            {
                if (Request.QueryString["storeclass"].ToString() == "412")
                    Title1.Text = "Requisition of Bond";
                else if (Request.QueryString["storeclass"].ToString() == "411")
                    Title1.Text = "Requisition of Provisions";
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"] == null ? "1" : Request.QueryString["pageno"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDORDERSTATUS", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Order No", "Order Date", "Order Status", "Recieved on" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , nvc != null ? nvc.Get("txtRefNo") : null
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                                                                , General.GetNullableInteger(Request.QueryString["storeclass"].ToString())
                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                                                                , sortexpression, sortdirection
                                                                , 1
                                                                , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Requisition of Bond", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtFromDate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedHard);

                Filter.CurrentVesselOrderFormFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["ORDERID"] = null;
                txtRefNo.Text = string.Empty;
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                ddlStatus.SelectedHard = "0";
                Filter.CurrentVesselOrderFormFilter = null;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBondGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&pageno=" + ViewState["PAGENUMBER"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDORDERSTATUS", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Order No", "Order Date", "Order Status", "Recieved on" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(Request.QueryString["storeclass"].ToString())
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvBondReq", "Requisition of Bond", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBondReq.DataSource = ds;
                gvBondReq.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvBondReq);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBondReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString().ToUpper() == "COPY")
            {
                string orderid = _gridView.DataKeys[nCurrentRow].Value.ToString();
                PhoenixVesselAccountsOrderForm.CopyOrderForm(new Guid(orderid));
                ViewState["ORDERID"] = null;
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                _gridView.SelectedIndex = nCurrentRow;
                string orderid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderId")).Text;
                string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBondGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + orderid + "&pageno=" + ViewState["PAGENUMBER"] + "&dtkey=" + dtkey, false);
            }
            //if (e.CommandName.ToString().ToUpper() == "SENDTOVENDOR")
            //{
            //    try
            //    {
            //        DataTable dt = new DataTable();
            //        string orderid = _gridView.DataKeys[nCurrentRow].Value.ToString();

            //        dt = PhoenixVesselAccountsOrderForm.VesselOrderFormXml(General.GetNullableGuid(orderid));
            //        if (dt.Rows.Count > 0)
            //        {
            //            string orderformxml = dt.Rows[0]["FLDORDERFORMXML"].ToString();
            //            string orderlinexml = dt.Rows[0]["FLDORDERLINEXML"].ToString();
            //            string addressxml = dt.Rows[0]["FLDADDRESSXML"].ToString();

            //            PhoenixServiceWrapper.SubmitPurchaseQuery(orderformxml, orderlinexml, addressxml);
            //            PhoenixVesselAccountsOrderForm.InsertOrderFormLineItemExtension(General.GetNullableGuid(orderid));
            //            ucStatus.Text = "Requisition is sent to Arc Marine.";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ucError.ErrorMessage = ex.Message;
            //        ucError.Visible = true;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixVesselAccountsOrderForm.DeleteOrderForm(id, PhoenixSecurityContext.CurrentSecurityContext.VesselID);       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvBondReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["ORDERID"] = null;
        BindData();
    }

    //protected void gvBondReq_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    GridView _gridView = sender as GridView;
    //    gvBondReq.SelectedIndex = se.NewSelectedIndex;
    //    string orderid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblOrderId")).Text;
    //    ViewState["ORDERID"] = orderid;
    //    BindData();
    //}

    protected void gvBondReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
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
                ImageButton cpy = (ImageButton)e.Row.FindControl("cmdCopy");
                if (cpy != null)
                {
                    cpy.Visible = SessionUtil.CanAccess(this.ViewState, cpy.CommandName);
                    cpy.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Copy ?'); return false;");
                }
                ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (cmdDelete != null)
                {
                    if (drv["FLDORDERSTATUS"].ToString().ToUpper() == "PENDING")
                        cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                    else
                        cmdDelete.Visible = false;
                }
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELACCOUNTS + "'); return false;");
                }

                //ImageButton cmdSend = (ImageButton)e.Row.FindControl("cmdSend");
                //if (cmdSend != null)
                //{
                //    cmdSend.Visible = SessionUtil.CanAccess(this.ViewState, cmdSend.CommandName);
                //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                //        cmdSend.Visible = false;
                //}

                //Image imgReqSent = (Image)e.Row.FindControl("imgReqSent");
                //if (imgReqSent != null && drv["FLDREQSENTTOARCYN"] != null && drv["FLDREQSENTTOARCYN"].ToString() == "1")
                //{
                //    imgReqSent.Visible = true;

                //    if (cmdSend != null)
                //        cmdSend.Visible = false;
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
            ViewState["ORDERID"] = null;
            BindData();
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
            gvBondReq.SelectedIndex = -1;
            gvBondReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["ORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + (ViewState["TOTALPAGECOUNT"].ToString() == "0" ? ViewState["TOTALPAGECOUNT"].ToString() : ViewState["PAGENUMBER"].ToString());
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //ViewState["ORDERID"] = null;
    //        BindData();
    //        ResetMenu();
    //        for (int i = 0; i < gvBondReq.DataKeyNames.Length; i++)
    //        {
    //            if (gvBondReq.DataKeyNames[i] == ViewState["ORDERID"].ToString())
    //            {
    //                gvBondReq.SelectedIndex = i;
    //                break;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //private void ResetMenu()
    //{
    //    PhoenixToolbar toolbar = new PhoenixToolbar();
    //    toolbar.AddButton("General", "GENERAL");
    //    if (ViewState["ORDERID"] != null)
    //        toolbar.AddButton("Line Item", "LINEITEM");
    //    MenuOrderForm.AccessRights = this.ViewState;
    //    MenuOrderForm.MenuList = toolbar.Show();
    //    MenuOrderForm.SelectedMenuIndex = 0;
    //}
    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        DataList dl = (DataList)MenuOrderForm.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsOrderFormBondGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString()))
    //            {
    //                MenuOrderForm.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsOrderFormBondLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString()))
    //            {
    //                MenuOrderForm.SelectedMenuIndex = 1;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


}
