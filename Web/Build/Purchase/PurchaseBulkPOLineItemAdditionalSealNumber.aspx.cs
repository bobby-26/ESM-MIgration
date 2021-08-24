using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI;

public partial class PurchaseBulkPOLineItemAdditionalSealNumber : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseBulkPOLineItemAdditionalSealNumber.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealNumber')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                MenuSealExport.AccessRights = this.ViewState;
                MenuSealExport.MenuList = toolbargrid.Show();

                if (Request.QueryString["LINEITEMID"] != null && Request.QueryString["LINEITEMID"].ToString() != "")
                {
                    ViewState["LINEITEMID"] = Request.QueryString["LINEITEMID"];
                    LineItemEdit();
                }
                else
                    ViewState["LINEITEMID"] = "";

                if (Request.QueryString["STOREITEMID"] != null && Request.QueryString["STOREITEMID"].ToString() != "")
                    ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"];
                else
                    ViewState["STOREITEMID"] = "";

                gvSealNumber.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                SetMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Record", "SAVE",ToolBarDirection.Right);
        MenuSealNumber.AccessRights = this.ViewState;
        MenuSealNumber.MenuList = toolbar.Show();
    }

    private void LineItemEdit()
    {
        try
        {
            DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemEdit(new Guid(ViewState["LINEITEMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtPrefix.Text = dr["FLDSEALNOPREFIX"].ToString();
                txtNoofdigits.Text = dr["FLDNOOFDIGITSINSEALNO"].ToString();
                int fromno = int.Parse(dr["FLDTOSERIALNO"].ToString()) + 1;
                if (!IsPostBack)
                    txtFromSerialNo.Text = fromno.ToString();

                DataSet dsseal = PhoenixPurchaseBulkPurchase.GetSealNumbers(txtPrefix.Text, int.Parse(txtNoofdigits.Text),
                    General.GetNullableString(txtFromSerialNo.Text),
                    General.GetNullableString(txtToSerialNo.Text));
                ViewState["FROMSEALNO"] = dsseal.Tables[0].Rows[0]["FLDFROMNUMBER"].ToString();
                ViewState["TOSEALNO"] = dsseal.Tables[0].Rows[0]["FLDTONUMBER"].ToString();

                gvSealNumber.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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

    private bool IsValidFormat(string prefix, string fromserialno, string toserialno)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromserialno))
            ucError.ErrorMessage = "From Serial Number is required.";

        if (string.IsNullOrEmpty(toserialno))
            ucError.ErrorMessage = "To Serial Number is required.";

        return (!ucError.IsError);
    }

    protected void MenuSealNumber_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidFormat(txtPrefix.Text, txtFromSerialNo.Text, txtToSerialNo.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseBulkPurchase.AdditionalSealNumberInsert(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["LINEITEMID"].ToString())
                                                            , new Guid(ViewState["STOREITEMID"].ToString())
                                                            , int.Parse(txtFromSerialNo.Text)
                                                            , int.Parse(txtToSerialNo.Text));
                LineItemEdit();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);

                String script = String.Format("javascript:fnReloadList('IndpecDetail',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNO", "FLDSEALNO" };
            string[] alCaptions = { "S.No", "Seal Number" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixPurchaseBulkPurchase.SealNumberRangeSearch(
                                                                     General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                                    , (ViewState["FROMSEALNO"] == null ? null : General.GetNullableString(ViewState["FROMSEALNO"].ToString()))
                                                                    , (ViewState["TOSEALNO"] == null ? null : General.GetNullableString(ViewState["TOSEALNO"].ToString()))
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            General.ShowExcel("Seal Numbers", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            string[] alColumns = { "FLDROWNO", "FLDSEALNO" };
            string[] alCaptions = { "S.No", "Seal Number" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPurchaseBulkPurchase.SealNumberRangeSearch(
                                                                     General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                                    , (ViewState["FROMSEALNO"] == null ? null : General.GetNullableString(ViewState["FROMSEALNO"].ToString()))
                                                                    , (ViewState["TOSEALNO"] == null ? null : General.GetNullableString(ViewState["TOSEALNO"].ToString()))
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                    , General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            General.SetPrintOptions("gvSealNumber", "Seal Numbers", alCaptions, alColumns, ds);

            gvSealNumber.DataSource = ds;
            gvSealNumber.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            gvSealNumber.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvSealNumber_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealNumber.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
