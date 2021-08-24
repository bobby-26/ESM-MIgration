using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersAdminAssetSoftware : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageLink("javascript:Openpopup('Filter','','RegistersAdminSoftwareAssetFilter.aspx'); return false;", "Asset Search", "search.png", "SEARCH");
            toolbarlist.AddImageLink("javascript:Openpopup('Filter','','RegistersAdminSoftwareDisposedAssetFilter.aspx'); return false;", "Disposed Asset Search", "search.png", "DISPOSEDASSETSEARCH");
            toolbarlist.AddImageButton("../Registers/RegistersAdminAssetSoftware.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarlist.AddImageButton("../Registers/RegistersAdminAssetSoftware.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuRegistersAdminAsset.AccessRights = this.ViewState;
            MenuRegistersAdminAsset.MenuList = toolbarlist.Show();

            PhoenixToolbar MainToolbar = new PhoenixToolbar();
            MainToolbar.AddButton("Assembly", "ASSEMBLY");
            MainToolbar.AddButton("Item", "ITEM");
            MenuAsset.AccessRights = this.ViewState;
            MenuAsset.MenuList = MainToolbar.Show();
            MenuAsset.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("New", "NEW");
            toolbar1.AddButton("Save", "SAVE");
            MenuAdminAssetAdd.AccessRights = this.ViewState;
            MenuAdminAssetAdd.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ASSETID"] = String.Empty;
                Filter.CurrentAssetSearchFilter = null;
                Filter.CurrentDisposedAssetSearchFilter = null;

                if (Request.QueryString["PGNO"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PGNO"]);
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                }

                BindYear();
                BindAssetType();
                BindAssembly();
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
            string[] alColumns = { "FLDNAME", "FLDSERIALNO", "FLDASSETTYPENAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
            string[] alCaptions = { "Name", "Serial Number", "Asset", "Media Part No", "Identification No" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (Filter.CurrentAssetSearchFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentAssetSearchFilter;
                ds = PhoenixAdministrationAsset.SearchAsset
                    (
                    (nvc != null ? General.GetNullableString(nvc.Get("txtSearch").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtDescription").ToString()) : null)
                    , sortdirection
                    , sortexpression
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount
                    , 2
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtMaker").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtModel").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtSerialNo").ToString()) : null)
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                    );
            }
            else if (Filter.CurrentDisposedAssetSearchFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentDisposedAssetSearchFilter;
                ds = PhoenixAdministrationAsset.SearchDisposedAsset
                    (
                    (nvc != null ? General.GetNullableString(nvc.Get("txtSearch").ToString()) : null)
                    , 2
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("txtDate")) : null)
                    , sortdirection
                    , sortexpression
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                    );
            }
            else
            {
                ds = PhoenixAdministrationAsset.SearchAsset(null, null, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, 2,
                                                            null, null, null, null, null);
            }

            General.SetPrintOptions("gvAdminAsset", "Assemblies", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAsset.DataSource = ds;
                gvAdminAsset.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAsset);
            }
            if (General.GetNullableGuid(ViewState["ASSETID"].ToString()) == null)
            {
                ViewState["ASSETID"] = ds.Tables[0].Rows[0]["FLDASSETID"].ToString();
                ViewState["PARENTID"] = ds.Tables[0].Rows[0]["FLDASSETTYPEID"].ToString();
                ViewState["ZONEID"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                gvAdminAsset.SelectedIndex = 0;
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
            BindAssembly();
        }
    }
    protected void Asset_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("ASSEMBLY"))
            {

            }
            else if (dce.CommandName.ToUpper().Equals("ITEM"))
            {
                if (!IsValidAssetId(ViewState["ASSETID"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Registers/RegistersAdminAssetSoftwareItem.aspx?ASSETID=" + ViewState["ASSETID"].ToString() + "&PGNO=" + ViewState["PAGENUMBER"].ToString() + "&ZONEID=" + ddlLocation.SelectedZone, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ClearFilter()
    {
        Filter.CurrentAssetSearchFilter = null;
        Filter.CurrentDisposedAssetSearchFilter = null;
        ViewState["ASSETID"] = String.Empty;
        BindAssembly();
        SetPageNavigator();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvcAsset = Filter.CurrentAssetSearchFilter;
            NameValueCollection nvcDisposedAsset = Filter.CurrentDisposedAssetSearchFilter;
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
            string[] alColumns = { "FLDNAME", "FLDSERIALNO", "FLDASSETTYPENAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
            string[] alCaptions = { "Name", "Serial Number", "Asset", "Media Part No", "Identification No" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (Filter.CurrentAssetSearchFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentAssetSearchFilter;
                ds = PhoenixAdministrationAsset.SearchAsset
                    (
                    (nvc != null ? General.GetNullableString(nvc.Get("txtSearch").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtDescription").ToString()) : null)
                    , sortdirection
                    , sortexpression
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount
                    , 2
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtMaker").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtModel").ToString()) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("txtSerialNo").ToString()) : null)
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                    );
            }
            else if (Filter.CurrentDisposedAssetSearchFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentDisposedAssetSearchFilter;
                ds = PhoenixAdministrationAsset.SearchDisposedAsset
                    (
                    (nvc != null ? General.GetNullableString(nvc.Get("txtSearch").ToString()) : null)
                    , 2
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("txtDate")) : null)
                    , sortdirection
                    , sortexpression
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                    );
            }
            else
            {
                ds = PhoenixAdministrationAsset.SearchAsset(null, null, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, 2,
                                                            null, null, null, null, null);
            }

            General.SetPrintOptions("gvAdminAsset", "Assemblies", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAsset.DataSource = ds;
                gvAdminAsset.DataBind();
                //gvAdminAsset.SelectedIndex = 0;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAsset);
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
    protected void BindAssetType()
    {
        ddlAssetType.Items.Clear();
        DataTable dt = PhoenixRegistersAssetType.ListAssetType(2, 1);       //Type- Software, ItemType - Assembly
        ddlAssetType.DataSource = dt;
        ddlAssetType.DataBind();
        ddlAssetType.Items.Insert(0, new ListItem("--Select--", ""));
    }
    private void EditAsset(Guid? Assetid)
    {
        try
        {
            DataTable dt = PhoenixAdministrationAsset.EditAsset(Assetid);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtAssetName.Text = dr["FLDNAME"].ToString();
                txtSerialNo.Text = dr["FLDSERIALNO"].ToString();
                Txtdescriptionadd.Text = dr["FLDDESCRIPTION"].ToString();
                txtModel.Text = dr["FLDIDENTIFICATIONNUMBER"].ToString();
                txtMaker.Text = dr["FLDMAKER"].ToString();
                ddlAssetType.SelectedValue = dr["FLDASSETTYPEID"].ToString();
                TxtPoreference.Text = dr["FLDPOREFERENCE"].ToString();
                UcInvoiceDate.Text = dr["FLDINVOICEDATE"].ToString();
                TxtInvoiceno.Text = dr["FLDINVOICENO"].ToString();
                ddlYear.SelectedValue = dr["FLDBUDGETYEAR"].ToString();
                UcExpirydate.Text = dr["FLDEXPIRYDATE"].ToString();
                ucDisposalDate.Text = dr["FLDDISPOSALDATE"].ToString();
                txtDisposalReason.Text = dr["FLDDISPOSALREASON"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtTagNumber.Text = dr["FLDTAGNUMBER"].ToString();
                ucLicense.Text = dr["FLDQUANTITY"].ToString();
                ddlLocation.SelectedZone = dr["FLDCOMPANYID"].ToString();
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
            ddlAssetType.SelectedValue = null;
            TxtPoreference.Text = "";
            UcInvoiceDate.Text = "";
            TxtInvoiceno.Text = "";
            ddlYear.SelectedValue = null;
            UcExpirydate.Text = "";
            ucDisposalDate.Text = "";
            txtDisposalReason.Text = "";
            txtRemarks.Text = "";
            txtTagNumber.Text = "";
            ucLicense.Text = "";
            ddlLocation.SelectedZone = "";
            ViewState["ASSETID"] = String.Empty;
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
                if (!IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
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
                        , General.GetNullableInteger(ucLicense.Text)
                        , General.GetNullableInteger(ddlAssetType.SelectedValue)
                        , TxtPoreference.Text
                        , General.GetNullableDateTime(UcInvoiceDate.Text)
                        , TxtInvoiceno.Text
                        , General.GetNullableInteger(ddlYear.SelectedValue)
                        , null
                        , General.GetNullableDateTime(UcExpirydate.Text)
                        , General.GetNullableDateTime(ucDisposalDate.Text)
                        , txtDisposalReason.Text
                        , 2
                        , txtRemarks.Text
                        , General.GetNullableInteger(ddlLocation.SelectedZone)
                        , txtMaker.Text
                        , txtTagNumber.Text
                        , null
                        , ref AssetId
                     );
                    ViewState["ASSETID"] = AssetId;
                    ucStatus.Text = "Asset added";
                    BindAssembly();
                }
                else
                {
                    if (IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
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
                                                              , General.GetNullableInteger(ucLicense.Text)
                                                              , General.GetNullableInteger(ddlAssetType.SelectedValue)
                                                              , TxtPoreference.Text
                                                              , General.GetNullableDateTime(UcInvoiceDate.Text)
                                                              , TxtInvoiceno.Text
                                                              , General.GetNullableInteger(ddlYear.SelectedValue)
                                                              , null
                                                              , General.GetNullableDateTime(UcExpirydate.Text)
                                                              , General.GetNullableDateTime(ucDisposalDate.Text)
                                                              , txtDisposalReason.Text
                                                              , 2
                                                              , txtMaker.Text
                                                              , txtRemarks.Text
                                                              , General.GetNullableInteger(ddlLocation.SelectedZone)
                                                              , txtTagNumber.Text
                                                              , null
                                                              );
                        ucStatus.Text = "Asset Updated";
                        ViewState["ASSETID"] = String.Empty;
                        BindAssembly();
                    }
                }
            }
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
            ucError.ErrorMessage = "Media Part No is Required.";
        if (General.GetNullableDateTime(UcInvoiceDate.Text) > System.DateTime.Today)
            ucError.ErrorMessage = "Invoice Date should not be greater than today's date.";
        if (SerialNo.Trim().Equals(""))
            ucError.ErrorMessage = "Serial Nunmber is Required.";
        return (!ucError.IsError);
    }
    private bool IsValidAssetId(string AssetId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(AssetId) == null && General.GetNullableInteger(ViewState["ROWCOUNT"].ToString()) == 0)
            ucError.ErrorMessage = "Add Software before to view Items.";
        if (General.GetNullableGuid(AssetId) == null && General.GetNullableInteger(ViewState["ROWCOUNT"].ToString()) != 0)
            ucError.ErrorMessage = "Select Asset to view Items.";

        return (!ucError.IsError);
    }
    public void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
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

        string[] alColumns = { "FLDNAME", "FLDSERIALNO", "FLDASSETTYPENAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER" };
        string[] alCaptions = { "Name", "Serial Number", "Asset", "Media Part No", "Identification No" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentAssetSearchFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentAssetSearchFilter;
            ds = PhoenixAdministrationAsset.SearchAsset
                (
                (nvc != null ? nvc.Get("txtSearch").ToString() : null)
                , (nvc != null ? nvc.Get("txtDescription").ToString() : null)
                , sortdirection
                , sortexpression
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
                , 2
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                , (nvc != null ? nvc.Get("txtMaker").ToString() : null)
                , (nvc != null ? nvc.Get("txtModel").ToString() : null)
                , (nvc != null ? nvc.Get("txtSerialNo").ToString() : null)
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                );
        }
        else if (Filter.CurrentDisposedAssetSearchFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentDisposedAssetSearchFilter;
            ds = PhoenixAdministrationAsset.SearchDisposedAsset
                (
                (nvc != null ? nvc.Get("txtSearch").ToString() : null)
                , 2
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlAssetType").ToString()) : null)
                , (nvc != null ? General.GetNullableDateTime(nvc.Get("txtDate")) : null)
                , sortdirection
                , sortexpression
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ddlLocation").ToString()) : null)
                );
        }
        else
        {
            ds = PhoenixAdministrationAsset.SearchAsset(null, null, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                        General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, 2,
                                                        null, null, null, null, null);
        }

        General.SetPrintOptions("gvAdminAsset", "Software", alCaptions, alColumns, ds);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdminAssetsHardware.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Software</h3></td>");
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
    protected void gvAdminAsset_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            Label lblassetid = (Label)e.Row.FindControl("lblAdminAssetID");
            Label lblassettypeid = (Label)e.Row.FindControl("lblAssetTypeId");

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
        }
    }

    protected void gvAdminAsset_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }

    protected void gvAdminAsset_Rowupdating(object sender, GridViewUpdateEventArgs e)
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

    protected void gvAdminAsset_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdminAsset.EditIndex = -1;
        gvAdminAsset.SelectedIndex = -1;
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
        gvAdminAsset.EditIndex = -1;
        gvAdminAsset.SelectedIndex = -1;
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
        ViewState["ASSETID"] = String.Empty;
        BindAssembly();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvAdminAsset.SelectedIndex = -1;
        gvAdminAsset.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        ViewState["ASSETID"] = String.Empty;
        BindAssembly();
        SetPageNavigator();
    }
    protected void gvAdminAsset_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void gvAdminAsset_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAsset_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            gvAdminAsset.SelectedIndex = nCurrentRow;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["ASSETID"] = ((Label)gvAdminAsset.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAdminAssetID")).Text;
                ViewState["PARENTID"] = ((Label)gvAdminAsset.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblAssetTypeId")).Text;
                ViewState["ZONEID"] = ((Label)gvAdminAsset.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblZoneId")).Text;
                if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                    EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
                BindData();
                SetPageNavigator();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string DisposalDate = ((Label)gvAdminAsset.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDisposalDate")).Text;
                if (!IsValidDelete(DisposalDate))
                {
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.ErrorMessage = "Are Sure want to Dispose this Asset?";
                ucConfirm.Visible = true;
                ViewState["ASSETID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAssetID")).Text;
                BindAssembly();
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
                PhoenixAdministrationAsset.DeleteAssetType(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindAssembly();
                ucStatus.Text = "Asset Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucConfirm.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDelete(string DisposalDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(DisposalDate) == null)
            ucError.ErrorMessage = "To Dispose Asset, Please provide Disposal date and Disposal reason.";

        return (!ucError.IsError);
    }
}
