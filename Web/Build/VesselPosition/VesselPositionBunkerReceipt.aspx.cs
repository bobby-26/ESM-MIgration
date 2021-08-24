using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionBunkerReceipt : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["BUNKERRECEIPTID"] = "";

            if (Request.QueryString["consumptionid"] != null)
                ViewState["CONSUMPTIONID"] = Request.QueryString["consumptionid"].ToString();
            else
                ViewState["CONSUMPTIONID"] = "";

            if (Request.QueryString["oiltype"] != null)
                ViewState["OILTYPE"] = Request.QueryString["oiltype"].ToString();
            else
                ViewState["OILTYPE"] = "";

            ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            EditBunkerReceipt();
        }
    }
    private void EditBunkerReceipt()
    {
        if (General.GetNullableGuid(ViewState["CONSUMPTIONID"].ToString()) != null)
        {
            DataSet ds = PhoenixVesselPositionBunkerReceipt.ListBunkerReceipt(
                new Guid(ViewState["CONSUMPTIONID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["BUNKERRECEIPTID"] = ds.Tables[0].Rows[0]["FLDBUNKERRECEIPTID"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                Filter.CurrentBunkerReceiptSelection = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            }
        }
    }
    protected void gvBunkerReceipt_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionBunkerReceipt.BunkerReceiptSearch(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
             General.GetNullableGuid(ViewState["OILTYPE"].ToString()),
             sortexpression, sortdirection,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             General.ShowRecords(null),
             ref iRowCount,
             ref iTotalPageCount);

        gvBunkerReceipt.DataSource = ds;
        gvBunkerReceipt.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvBunkerReceipt.SelectedIndexes.Clear();
        gvBunkerReceipt.EditIndexes.Clear();
        gvBunkerReceipt.DataSource = null;
        gvBunkerReceipt.Rebind();
    }
    protected void gvBunkerReceipt_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel bdnid = (RadLabel)e.Item.FindControl("lblBunkerReceiptID");
                RadLabel consid = (RadLabel)e.Item.FindControl("lblConsID");
                ViewState["BUNKERRECEIPTID"] = bdnid.Text;
                ViewState["CONSUMPTIONID"] = consid.Text;
                EditBunkerReceipt();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel BunkerReceiptId = (RadLabel)e.Item.FindControl("lblBunkerReceiptID");
                PhoenixVesselPositionBunkerReceipt.DeleteBunkerReceipt(new Guid(BunkerReceiptId.Text));

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblBunkerReceiptIDEdit = (RadLabel)e.Item.FindControl("lblBunkerReceiptIDEdit");
                RadLabel lblConsIDEdit = (RadLabel)e.Item.FindControl("lblConsIDEdit");
                UserControlDate txtDateEdit = (UserControlDate)e.Item.FindControl("txtDateEdit");
                RadLabel lbloilcode = (RadLabel)e.Item.FindControl("lbloilcode");
                RadTextBox txtSupplier = (RadTextBox)e.Item.FindControl("txtSupplier");
                UserControlMaskNumber txtSulphur = (UserControlMaskNumber)e.Item.FindControl("txtSulphur");
                UserControlMaskNumber txtWtinTonnes = (UserControlMaskNumber)e.Item.FindControl("txtWtinTonnes");
                UserControlMaskNumber txtDensity = (UserControlMaskNumber)e.Item.FindControl("txtDensity");
                RadTextBox txtRemarks = (RadTextBox)e.Item.FindControl("txtRemarks");

                if (!IsValidBunkered(txtSulphur.Text, txtDateEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixVesselPositionBunkerReceipt.UpdateBunkerReceipt(
                            new Guid(lblBunkerReceiptIDEdit.Text)
                            , General.GetNullableDateTime(txtDateEdit.Text)
                            , General.GetNullableString(txtSupplier.Text)
                            , General.GetNullableString("")
                            , General.GetNullableString("")
                            , General.GetNullableGuid(lbloilcode.Text)
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal(txtWtinTonnes.Text)
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal(txtDensity.Text)
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal("")
                            , General.GetNullableDecimal(txtSulphur.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            );
                }
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBunkerReceipt_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELACCOUNTS + "'); return false;");
                }
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBunkered(string sulphur, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        if (General.GetNullableDecimal(sulphur) == null)
            ucError.ErrorMessage = "Sulphur % is required.";

        return (!ucError.IsError);
    }


    protected void gvBunkerReceipt_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        Rebind();
    }
}
