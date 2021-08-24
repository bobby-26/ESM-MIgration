using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class AccountsCtmArrangedFollowUp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddImageButton("../Accounts/AccountsCtmArrangedFollowUp.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarmain.AddImageLink("javascript:CallPrint('gvCTM')", "Print Grid", "icon_print.png", "PRINT");
                toolbarmain.AddImageButton("../Accounts/AccountsCtmArrangedFollowUp.aspx", "Find", "search.png", "FIND");
                toolbarmain.AddImageButton("../Accounts/AccountsCtmArrangedFollowUp.aspx", "Clear", "clear-filter.png", "CLEAR");
                MenuCTM.AccessRights = this.ViewState;
                MenuCTM.MenuList = toolbarmain.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                chkStatus.Checked = true;
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
                txtToDate.Text = DateTime.Now.ToString();

                gvCTM.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void Arraivedvia_Changed(object sender, EventArgs e)
    {
        gvCTM.Rebind();
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
          
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                Rebind();

            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDVESSELNAME", "FLDETA", "FLDSEAPORTNAME", "FLDARRANGEDVIA", "FLDDELIVEREDBY", "FLDCURRENCYCODE", "FLDAMOUNTARRANGED", "FLDRECEIVEDAMOUNT", "FLDRECEIVEDDATE", "FLDSTATUS", "FLDPAYMENTVOUCHERSTATUS" };
                string[] alCaptions = { "PMV No", "Vessel", "ETA Date", "Port", "Arranged Via", "Delivered By", "Currency", "Arranged Amount", "Received Amount", "Received Date", "CTM Request Status", "Payment Status" };

                DataSet ds = PhoenixAccountsCtm.CtmGeneratedPaymentVoucherSearch(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableInteger(chkStatus.Checked == true ? "" : "165"), General.GetNullableInteger(ddlArrangedVia.SelectedHard)
                            , sortexpression, sortdirection
                            , 1
                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("CTM Arranged FollowUp", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                chkStatus.Checked = true;
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
                txtToDate.Text = DateTime.Now.ToString();
                ddlArrangedVia.SelectedHard = "";
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDVESSELNAME", "FLDETA", "FLDSEAPORTNAME", "FLDARRANGEDVIA", "FLDDELIVEREDBY", "FLDCURRENCYCODE", "FLDAMOUNTARRANGED", "FLDRECEIVEDAMOUNT", "FLDRECEIVEDDATE", "FLDSTATUS", "FLDPAYMENTVOUCHERSTATUS" };
            string[] alCaptions = { "PMV No", "Vessel", "ETA Date", "Port", "Arranged Via", "Delivered By", "Currency", "Arranged Amount", "Received Amount", "Received Date", "CTM Request Status", "Payment Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsCtm.CtmGeneratedPaymentVoucherSearch(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableInteger(chkStatus.Checked == true ? "" : "165"), General.GetNullableInteger(ddlArrangedVia.SelectedHard)
                                                                            , sortexpression, sortdirection
                                                                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                             
                                                                             , gvCTM.PageSize,
                                                                              //, Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                              //, General.ShowRecords(null),
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Arranged FollowUp", alCaptions, alColumns, ds);

            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }



    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string mnu = Filter.CurrentMenuCodeSelection;

            //if (e.Item.RowType == DataControlRowType.DataRow)
            //{
            //    if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit)
            //     && !e.Item.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                string Date = ((RadLabel)e.Item.FindControl("lblReceivedamount")).Text.Trim();
                ImageButton mb = (ImageButton)e.Item.FindControl("cmdEmail");
                if (mb != null)
                {
                    if (Date != "")
                        mb.Visible = false;
                    else
                    {
                        mb.Visible = SessionUtil.CanAccess(this.ViewState, mb.CommandName);
                        mb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsCtmfollowupMail.aspx?port=" + drv["FLDSEAPORTNAME"].ToString() + "&date=" + drv["FLDDATE"].ToString() + "&vesselid=" + drv["FLDVESSELID"].ToString() + "',false); ");

                    }
                }

            }
        }
    }

    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCTM.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
