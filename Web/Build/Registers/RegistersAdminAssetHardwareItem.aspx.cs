using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.AdminAssetSpareItems;
public partial class Registers_RegistersAdminAssetHardwareItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageButton("../Registers/RegistersAdminAssetHardwareItem.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarlist.AddImageLink("javascript:CallPrint('gvAdminAssetItems')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersAdminAsset.AccessRights = this.ViewState;
            MenuRegistersAdminAsset.MenuList = toolbarlist.Show();

            PhoenixToolbar MainToolbar = new PhoenixToolbar();
            MainToolbar.AddButton("Assembly", "ASSEMBLY");
            MainToolbar.AddButton("Item", "ITEM");
            MenuAsset.AccessRights = this.ViewState;
            MenuAsset.MenuList = MainToolbar.Show();
            MenuAsset.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE");
            MenuAdminAssetAdd.AccessRights = this.ViewState;
            MenuAdminAssetAdd.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ASSETID"] = String.Empty;
               
                if (Request.QueryString["ASSETID"] != String.Empty)
                {
                    ViewState["ASSETPARENTID"] = Request.QueryString["ASSETID"];
                }
                else
                {
                    ViewState["ASSETPARENTID"] = String.Empty;
                }
                if (Request.QueryString["PGNO"] != String.Empty)
                {
                    ViewState["PGNO"] = Request.QueryString["PGNO"];
                }
                else
                {
                    ViewState["PGNO"] = String.Empty;
                }
                if (Request.QueryString["NAME"] != null)
                    ViewState["NAME"] = Request.QueryString["NAME"].ToString();
                Title1.Text = "Item  (" + ViewState["NAME"].ToString() + ") ";
                if (Request.QueryString["ZONEID"] != null)
                    ViewState["ZONEID"] = int.Parse(Request.QueryString["ZONEID"]);

                ddlLocation.SelectedZone = ViewState["ZONEID"].ToString();
                BindYear();
                BindAssembly();
            }

            if (Request.QueryString["ASSETID"] != String.Empty)
            {
                ViewState["ASSETPARENTID"] = Request.QueryString["ASSETID"];
            }
            else
            {
                ViewState["ASSETPARENTID"] = String.Empty;
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
    private void BindAssembly()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
            string[] alCaptions = { "Item", "Serial Number", "Name", "Maker", "Model" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixAdministrationAsset.SearchAssetItems(General.GetNullableGuid(ViewState["ASSETPARENTID"].ToString()), sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

           General.SetPrintOptions("gvAdminAssetItems", "Items", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAssetItems.DataSource = ds;
                gvAdminAssetItems.DataBind();
                if (General.GetNullableGuid(ViewState["ASSETID"].ToString()) == null)
                {
                    ViewState["ASSETID"] = ds.Tables[0].Rows[0]["FLDASSETID"].ToString();
                    ViewState["ASSETITEMID"] = ds.Tables[0].Rows[0]["FLDASSETTYPEID"].ToString();
                    ViewState["FLDASSETTYPENAME"] = ds.Tables[0].Rows[0]["FLDASSETTYPENAME"].ToString();
                    gvAdminAssetItems.SelectedIndex = 0;
                    txtItem.Text = ViewState["FLDASSETTYPENAME"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAssetItems);
            }
            
            if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
            else
                Reset();

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRegistersAdminAsset_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }
    protected void Asset_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("ASSEMBLY"))
            {
                Response.Redirect("../Registers/RegistersAdminAssetHardwareAssembly.aspx?ASSETID=" + ViewState["ASSETPARENTID"].ToString() + "&PGNO=" + ViewState["PGNO"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("ITEM"))
            {
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ClearFilter()
    {
        BindData();
        SetPageNavigator();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ASSETID"] = String.Empty;
            BindAssembly();
            SetPageNavigator();
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
            string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
            string[] alCaptions = { "Item", "Serial Number", "Name", "Maker", "Model" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

           ds = PhoenixAdministrationAsset.SearchAssetItems(General.GetNullableGuid(ViewState["ASSETPARENTID"].ToString()), sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvAdminAssetItems", "Items", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAssetItems.DataSource = ds;
                gvAdminAssetItems.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAssetItems);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindYear()
    {
        for (int i = 2005; i <= (DateTime.Today.Year) + 1; i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlYear.Items.Insert(0, new ListItem("--Select--", ""));
    }
    private void EditAsset(Guid? AssetId)
    {
        try
        {
            DataTable dt = PhoenixAdministrationAsset.EditAsset(AssetId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtAssetName.Text = dr["FLDNAME"].ToString();
                txtSerialNo.Text = dr["FLDSERIALNO"].ToString();
                Txtdescriptionadd.Text = dr["FLDDESCRIPTION"].ToString();
                txtModel.Text = dr["FLDIDENTIFICATIONNUMBER"].ToString();
                txtMaker.Text = dr["FLDMAKER"].ToString();
                TxtPoreference.Text = dr["FLDPOREFERENCE"].ToString();
                UcInvoiceDate.Text = dr["FLDINVOICEDATE"].ToString();
                TxtInvoiceno.Text = dr["FLDINVOICENO"].ToString();
                ddlYear.SelectedValue = dr["FLDBUDGETYEAR"].ToString();
                UcExpirydate.Text = dr["FLDEXPIRYDATE"].ToString();
                ucDisposalDate.Text = dr["FLDDISPOSALDATE"].ToString();
                txtDisposalReason.Text = dr["FLDDISPOSALREASON"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtTagNumber.Text = dr["FLDTAGNUMBER"].ToString();
                txtAssetName.Focus();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Reset()
    {
        try
        {
            txtAssetName.Text = "";
            txtSerialNo.Text = "";
            Txtdescriptionadd.Text = "";
            txtModel.Text = "";
            txtMaker.Text = "";
            TxtPoreference.Text = "";
            UcInvoiceDate.Text = "";
            TxtInvoiceno.Text = "";
            ddlYear.SelectedValue = null;
            UcExpirydate.Text = "";
            ucDisposalDate.Text = "";
            txtDisposalReason.Text = "";
            txtRemarks.Text = "";
            txtTagNumber.Text = "";
            ViewState["ASSETID"] = string.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAdminAssetAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAsset(ViewState["ASSETITEMID"].ToString(), txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                {
                    DataSet ds = new DataSet();
                    Guid? AssetId = Guid.Empty;

                    ds = PhoenixAdministrationAsset.InsertAdminAsset
                    (
                        txtAssetName.Text
                        , Txtdescriptionadd.Text
                        , null
                        , txtModel.Text
                        , null
                        , txtSerialNo.Text
                        , null
                        , 1
                        , General.GetNullableInteger(ViewState["ASSETITEMID"].ToString())
                        , TxtPoreference.Text
                        , General.GetNullableDateTime(UcInvoiceDate.Text)
                        , TxtInvoiceno.Text
                        , General.GetNullableInteger(ddlYear.SelectedValue)
                        , null
                        , General.GetNullableDateTime(UcExpirydate.Text)
                        , General.GetNullableDateTime(ucDisposalDate.Text)
                        , txtDisposalReason.Text
                        , 1
                        , txtRemarks.Text
                        , General.GetNullableInteger(ddlLocation.SelectedZone)
                        , txtMaker.Text
                        , txtTagNumber.Text
                        , General.GetNullableGuid(ViewState["ASSETPARENTID"].ToString())
                        , ref AssetId
                     );

                    ViewState["ASSETID"] = AssetId;
                    ucStatus.Text = "Asset added";
                }
                else
                {
                    if (IsValidAsset(ViewState["ASSETITEMID"].ToString(), txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
                    {
                        PhoenixAdministrationAsset.UpdateAdminAsset(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              , General.GetNullableGuid(ViewState["ASSETID"].ToString())
                                                              , txtAssetName.Text
                                                              , Txtdescriptionadd.Text
                                                              , null
                                                              , txtModel.Text
                                                              , null
                                                              , txtSerialNo.Text
                                                              , null
                                                              , 1
                                                              , General.GetNullableInteger(ViewState["ASSETITEMID"].ToString())
                                                              , TxtPoreference.Text
                                                              , General.GetNullableDateTime(UcInvoiceDate.Text)
                                                              , TxtInvoiceno.Text
                                                              , General.GetNullableInteger(ddlYear.SelectedValue)
                                                              , null
                                                              , General.GetNullableDateTime(UcExpirydate.Text)
                                                              , General.GetNullableDateTime(ucDisposalDate.Text)
                                                              , txtDisposalReason.Text
                                                              , 1
                                                              , txtMaker.Text
                                                              , txtRemarks.Text
                                                              , General.GetNullableInteger(ddlLocation.SelectedZone)
                                                              , txtTagNumber.Text
                                                              , General.GetNullableGuid(ViewState["ASSETPARENTID"].ToString())
                                                              );
                        ucStatus.Text = "Asset Updated";
                    }
                }
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidAsset(string assesttypeid, string AssetName, string maker, string SerialNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(assesttypeid))
            ucError.ErrorMessage = "Asset is Required";
        if (string.IsNullOrEmpty(AssetName))
            ucError.ErrorMessage = "Name is Required";
        if (string.IsNullOrEmpty(maker))
            ucError.ErrorMessage = "Maker is Required.";
        if (General.GetNullableDateTime(UcInvoiceDate.Text) > System.DateTime.Today)
            ucError.ErrorMessage = "Invoice Date should not be greater than today's date.";
        if (SerialNo.Trim().Equals(""))
            ucError.ErrorMessage = "Serial Number is Required.";

        return (!ucError.IsError);
    }

    public void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {

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
    private bool IsVaildDate(string date)
    {
        if (Convert.ToDateTime(date) != null && Convert.ToDateTime(date) > DateTime.Now)
        {
            // ucError.ErrorMessage = "Logged From date can't be greater than Today";
            // return (!ucError.IsError);
        }
        return (ucError.IsError);
    }
    private bool IsValidToDate(DateTime date)
    {
        if (date != null && date > DateTime.Now)
        {
            ucError.ErrorMessage = "Logged To Date can't be greater than Today.";
        }
        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
        string[] alCaptions = { "Item", "Serial Number", "Name", "Maker", "Model" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationAsset.SearchAssetItems(General.GetNullableGuid(ViewState["ASSETPARENTID"].ToString()), sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdminAssetsHardware.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Item</h3></td>");
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
    protected void gvAdminAssetItems_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton cd = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cd != null)
            {
                cd.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cd.Visible = SessionUtil.CanAccess(this.ViewState, cd.CommandName);
            }
            Label AssetId = (Label)e.Row.FindControl("lblAdminAssetID");
            ImageButton cmdScrapItem = (ImageButton)e.Row.FindControl("cmdScrapItem");
            {
                if (AssetId.Text != "")
                {
                    cmdScrapItem.Visible = true;
                }
                else
                {
                    cmdScrapItem.Visible = false;
                }
            }
            Label AssetTypeId = (Label)e.Row.FindControl("lblAssetTypeId");
            Label TypeName = (Label)e.Row.FindControl("lblItemType");
            ImageButton cmdAddItems = (ImageButton)e.Row.FindControl("cmdAddItems");
            {
                cmdAddItems.Visible = SessionUtil.CanAccess(this.ViewState, cmdAddItems.CommandName);
                cmdAddItems.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Registers/RegistersAdminAssetSpareItemsMoving.aspx?TYPE=" + 1 + "&ASSETTYPEID=" + AssetTypeId.Text + "&NAME=" + TypeName.Text + "&ASSETID=" + ViewState["ASSETPARENTID"] + "&ZONEID=" + ViewState["ZONEID"].ToString() + "');return true;");
                cmdAddItems.Visible = true;
            }
        }
    }

    protected void gvAdminAssetItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }

    protected void gvAdminAssetItems_Rowupdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAssetItems_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdminAssetItems.EditIndex = -1;
        gvAdminAssetItems.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvAdminAssetItems.EditIndex = -1;
        gvAdminAssetItems.SelectedIndex = -1;
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
        gvAdminAssetItems.SelectedIndex = -1;
        gvAdminAssetItems.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void gvAdminAssetItems_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void gvAdminAssetItems_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAssetItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            gvAdminAssetItems.SelectedIndex = nCurrentRow;

            if (e.CommandName.ToUpper().Equals("SCRAPITEM"))
            {
                ucConfirm.ErrorMessage = "Are you sure want to Scrap the Item ?";
                ucConfirm.Visible = true;
                ViewState["ASSETID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAssetID")).Text;
                BindAssembly();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["ASSETID"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAdminAssetID")).Text;
                ViewState["ASSETITEMID"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAssetTypeId")).Text;
                ViewState["FLDASSETTYPENAME"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblItemType")).Text;
                txtItem.Text = ViewState["FLDASSETTYPENAME"].ToString();

                if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                    EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
                else
                    Reset();
                
                BindData();
                SetPageNavigator();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAdministrationAsset.DeleteAssetType(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAssetID")).Text));
                Reset();
                BindAssembly();
                BindData();
                SetPageNavigator();
            }
            if (e.CommandName.ToUpper().Equals("ADDITEM"))
            {
                ViewState["ASSETID"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAdminAssetID")).Text;
                ViewState["ASSETITEMID"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAssetTypeId")).Text;
                ViewState["FLDASSETTYPENAME"] = ((Label)gvAdminAssetItems.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblItemType")).Text;
                txtItem.Text = ViewState["FLDASSETTYPENAME"].ToString();
                if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                    EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
                else
                    Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirm_ConfirmMesage(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixAdministrationAssetSpareItems.UpdateAssetSpareItemsScrap(new Guid(ViewState["ASSETID"].ToString()));
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindAssembly();
                ucStatus.Text = "Asset Scraped.";
            }
        }
        catch (Exception ex)
        {
            ucConfirm.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
