using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class VesselAccountsAdminCTM : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCTM.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvCTM')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["CTMID"] = null;
                ViewState["ACTIVEYN"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
                string[] alCaptions = { "CTM Date", "Required Port", "ETA Date", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

                DataSet ds = PhoenixVesselAccountsCTM.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                           , null, null
                           , sortexpression, sortdirection
                            , 1
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("CTM Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
            string[] alColumns = { "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "CTM Date", "Required Port", "ETA Date", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCTM.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , null, null, sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvCTM.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);

            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["CTMID"] == null)
                {
                    ViewState["CTMID"] = ds.Tables[0].Rows[0]["FLDCAPTAINCASHID"].ToString();
                    ViewState["ACTIVEYN"] = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString();
                }
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
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblCCid")).Text);
                RadTextBox txtAmount = (RadTextBox)e.Item.FindControl("txtReceived");
                UserControlDate txtEdit = (UserControlDate)e.Item.FindControl("txtEdit");
                if (!IsValidAmount(txtAmount.Text, txtEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.UpdateCtmReceivedAmount(id, decimal.Parse(txtAmount.Text), DateTime.Parse(txtEdit.Text));
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
 
    private bool IsValidAmount(string amount, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information before Confirm";

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Received Amount is Required";
        }
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Received date is required.";
        }
        return (!ucError.IsError);
    }
}
