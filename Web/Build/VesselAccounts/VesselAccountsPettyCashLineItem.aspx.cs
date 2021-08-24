using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class VesselAccountsPettyCashLineItem : PhoenixBasePage
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
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvPettyCash')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            MenuCTM.MenuList = toolbarmain.Show();

            gvPettyCash.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            int iTotalPageCount = 10;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDBASECURRENCYCODE", "FLDBASEAMOUNT", "FLDVCCURRENCYCODE", "FLDVCAMOUNT" };
            string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Base Currency", "Base Amount", "Vessel Currency", "Vessel Amount" };
            DataSet ds = PhoenixVesselAccountsCTM.SearchCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                            , sortexpression, sortdirection,
                                                            Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),gvPettyCash.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);
            General.SetPrintOptions("gvPettyCash", "Expenses", alCaptions, alColumns, ds);
            gvPettyCash.DataSource = ds;
            gvPettyCash.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPettyCash.SelectedIndexes.Clear();
        gvPettyCash.EditIndexes.Clear();
        gvPettyCash.DataSource = null;
        gvPettyCash.Rebind();
    }  
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                ViewState["PAGENUMBER"] = 1;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDBASECURRENCYCODE", "FLDBASEAMOUNT", "FLDVCCURRENCYCODE", "FLDVCAMOUNT" };
                string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Base Currency", "Base Amount", "Vessel Currency", "Vessel Amount" };

                DataSet ds = PhoenixVesselAccountsCTM.SearchCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                           , General.GetNullableDateTime(txtFromDate.Text)
                                                           , General.GetNullableDateTime(txtToDate.Text)
                                                           , sortexpression, sortdirection,
                                                           Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), iRowCount,
                                                           ref iRowCount,
                                                           ref iTotalPageCount);

                General.ShowExcel("Expenses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
                txtToDate.Text = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                Rebind();

            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemAdd.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPettyCash_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPettyCash.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPettyCash_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblpettycashid")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemAdd.aspx?id=" + id, true);
            }
          
            if (e.CommandName == "DELETE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblpettycashid")).Text;
                PhoenixVesselAccountsCTM.DeleteCaptainPettyCash(new Guid(id));
                Rebind();
            }            
            else if (e.CommandName == "Page")
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
    protected void gvPettyCash_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
          
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton re = (LinkButton)e.Item.FindControl("cmdRecoverable");
            if (re != null)
            {
                re.Visible = SessionUtil.CanAccess(this.ViewState, re.CommandName);
                re.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewRecoverable.aspx?pcid=" + drv["FLDPETTYCASHID"].ToString() + "'); return false;");
            }
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                  + PhoenixModule.VESSELACCOUNTS + "'); return false;");

            }

        }
    }
 
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["DATE"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}