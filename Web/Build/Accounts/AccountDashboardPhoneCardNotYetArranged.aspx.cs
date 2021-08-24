using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountDashboardPhoneCardNotYetArranged : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsPhoneCardPinNumber.AutoPhoneCardConfirmIntegration();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardArrange.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvPhonReq')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardArrange.aspx", "Find", "search.png", "FIND");
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardArrange.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["vslid"] = "";

                if (Request.QueryString["vslid"] != null)
                    ViewState["vslid"] = Request.QueryString["vslid"].ToString();

                ViewState["ARRANGEYN"] = "1";
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["REQUESTID"] = null;
                ViewState["ALLOWEDIT"] = "true";
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhonReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvPhonReq.SelectedIndexes.Clear();
        gvPhonReq.EditIndexes.Clear();
        gvPhonReq.DataSource = null;
        gvPhonReq.Rebind();
    }
    protected void ShowExcel()
    {
        try
        {
            //int iRowCount = 0;
            //int iTotalPageCount = 0;
            //string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            //string[] alCaptions = { "Request No", "Requested on", "Status" };
            //string sortexpression;
            //int? sortdirection = null;

            //sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            //    iRowCount = 10;
            //else
            //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            //DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCardArrange
            //                                                                            (
            //                                                                            General.GetNullableInteger(ViewState["vslid"].ToString())
            //                                                                          , null
            //                                                                          , null
            //                                                                          , null
            //                                                                          , null
            //                                                                          , sortexpression, sortdirection
            //                                                                          , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
            //                                                                          , gvPhonReq.PageSize
            //                                                                          , ref iRowCount
            //                                                                          , ref iTotalPageCount
            //                                                                          );
            //string name;

            //name = "Arrange Phone Cards";


            //General.ShowExcel(name, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //ViewState["PAGENUMBER"] = 1;
                //NameValueCollection criteria = new NameValueCollection();
                //criteria.Clear();
                //criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
                //criteria.Add("txtRefNo", txtRefNo.Text);
                //criteria.Add("txtFromDate", txtFromDate.Text);
                //criteria.Add("txtToDate", txtToDate.Text);
                //criteria.Add("ddlStatus", ddlStatus.SelectedPhoneCard);
                //Filter.CurrentVesselPhoneCardArrangeFilter = criteria;
                //Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //ViewState["REQUESTID"] = null;
                //Filter.CurrentVesselPhoneCardArrangeFilter = null;
                //txtFromDate.Text = "";
                //txtRefNo.Text = "";
                //txtToDate.Text = "";
                //txtVesselName.Text = "";
                //ddlStatus.SelectedPhoneCard = "";
                //ddlVessel.SelectedVessel = "";
                //Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Request No", "Requested On", "Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
         //   NameValueCollection nvc = Filter.CurrentVesselPhoneCardArrangeFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCardArrange
                                                                                        (
                                                                                        General.GetNullableInteger(ViewState["vslid"].ToString())
                                                                                      , null
                                                                                      , null
                                                                                      , null
                                                                                      , null
                                                                                      , sortexpression, sortdirection
                                                                                      , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                                      , gvPhonReq.PageSize
                                                                                      , ref iRowCount
                                                                                      , ref iTotalPageCount
                                                                                      );

            General.SetPrintOptions("gvPhonReq", "Arrange Phone Cards", alCaptions, alColumns, ds);

            gvPhonReq.DataSource = ds;
            gvPhonReq.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhonReq_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            ImageButton ab = (ImageButton)e.Item.FindControl("cmdApprove");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdUnlock");
            ImageButton cs = (ImageButton)e.Item.FindControl("cmdSend");
            ImageButton cr = (ImageButton)e.Item.FindControl("cmdRefresh");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblissueyn = (RadLabel)e.Item.FindControl("lblissueyn");
            RadLabel lblinvoice = (RadLabel)e.Item.FindControl("lblinvoice");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (lblStatus.Text == "Pending")
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to unlock?')");
                }
                if (!lblStatus.Text.Contains("Pen")) db.Visible = false;
            }
            if (ab != null)
            {
                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm ?')");
                if (drv["FLDACTIVEYN"].ToString() == "0") ab.Visible = false;
                if (!lblStatus.Text.Contains("Req")) ab.Visible = false;
            }
            if (cs != null)
            {
                cs.Visible = SessionUtil.CanAccess(this.ViewState, cs.CommandName);
                //  if (lblStatus.Text != "Pending" || Filter.CurrentMenuCodeSelection.Contains("VAC-PBL-PNC"))
                cs.Visible = false;
            }
            if (cr != null)
            {
                cr.Visible = SessionUtil.CanAccess(this.ViewState, cs.CommandName);
                if (lblissueyn.Text == "1" && lblinvoice.Text == "" && !Filter.CurrentMenuCodeSelection.Contains("VAC-PBL-PNC"))
                    cr.Visible = true;
                else
                    cr.Visible = false;
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
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
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhonReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalnew.aspx?requestid=" + requestid + "&arrangeyn=" + ViewState["ARRANGEYN"].ToString());
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
                ViewState["REQUESTID"] = null;
                Rebind();
                ucStatus.Text = "Requisition sent to Office.";
            }
            else if (e.CommandName == "UNLOCK")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (reqstatus.Contains("Pen"))
                {
                    ViewState["REQUESTID"] = null;
                    PhoenixVesselAccountsPhoneCardRequisition.UnlockPhoneCardRequest(new Guid(requestid));
                }
                Rebind();
            }
            else if (e.CommandName == "SENDTOVESSEL")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ViewState["REQUESTID"] = requestid;
                PhoenixVesselAccountsPhoneCardRequisition.SendPhoneCardReqToVessel(new Guid(requestid));
                Rebind();
                ucStatus.Text = "Phone Card sent to vessel.";
            }
            else if (e.CommandName == "REFRESH")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ViewState["REQUESTID"] = requestid;
                PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequestMappingPopulate(new Guid(requestid));
                Rebind();
            }
            else
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
    protected void gvPhonReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPhonReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
