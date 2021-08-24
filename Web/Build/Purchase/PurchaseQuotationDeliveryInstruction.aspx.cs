using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Threading;

public partial class PurchaseQuotationDeliveryInstruction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (Request.QueryString["editable"] != null && Request.QueryString["editable"].ToString().ToUpper().Equals("TRUE"))
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuDelInstruction.MenuList = toolbarmain.Show();
            }
            else
            {
                txtDeliveryInstruction.ReadOnly = true;
                txtDeliveryInstruction.CssClass = "readonlytextbox";
            }
            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    BindData(Request.QueryString["quotationid"].ToString());
                }
                txtETA.Culture= new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name, true);
                txtETD.Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string quotationid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDeliveryInstruction.Text = dr["FLDDELIVERYINSTRUCTION"].ToString();
            txtETA.SelectedDate = General.GetNullableDateTime(dr["FLDETA"].ToString());
            txtETD.SelectedDate = General.GetNullableDateTime(dr["FLDETD"].ToString());
            //txtETATime.Text = string.Format("{0:hh:mm tt}", dr["FLDETA"]).ToString() == "12:00 AM" ? string.Empty : string.Format("{0:hh:mm tt}", dr["FLDETA"]);
            //txtETDTime.Text = string.Format("{0:hh:mm tt}", dr["FLDETD"]).ToString() == "12:00 AM" ? string.Empty : string.Format("{0:hh:mm tt}", dr["FLDETD"]);
            ucPortMulti.SelectedValue = dr["FLDPORT"].ToString();
            ucPortMulti.Text = dr["FLDPORTNAME"].ToString();
            ucPortMulti.VendorId = dr["FLDVENDORID"].ToString();
            ddlDeliveryto.SelectedHard = dr["FLDDELIVERYLOCATION"].ToString();
            ucAddrAgent.SelectedValue = dr["FLDWHDELIVERYADDRESSID"].ToString();
            ucAddrAgent.Text = dr["FLDWAREHOUSEADDRESS"].ToString();
            if (dr["FLDDELIVERYLOCATION"].ToString() != "2")
            {
                trAddress.Visible = false;
            }
            else
            {
                trAddress.Visible = true;
            }
        }
        else
        {
            trAddress.Visible = false;
        }
    }

    protected void MenuDelInstruction_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDates(txtETA.SelectedDate,txtETD.SelectedDate))
                {
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {

                    int? address = null;
                    if (ddlDeliveryto.SelectedHard == "2")
                        address = General.GetNullableInteger(ucAddrAgent.SelectedValue);

                    PhoenixPurchaseQuotation.UpdateQuotationDeliveryInstruction(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                        new Guid(ViewState["quotationid"].ToString()),
                        txtDeliveryInstruction.Text,
                        txtETA.SelectedDate,
                        txtETD.SelectedDate,
                        General.GetNullableInteger(ucPortMulti.SelectedValue),
                        General.GetNullableInteger(ddlDeliveryto.SelectedHard),
                        address);
                  

                    ucStatus.Text = "Delivery Instruction has been Saved";
                    ucStatus.Visible = true;
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " closeTelerikWindow('codehelp1','detail','true');";
                    Script += "</script>" + "\n";
                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDates(DateTime? ETA, DateTime? ETD)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ETA != null && DateTime.Compare(DateTime.Parse(ETA.ToString()), DateTime.Parse(DateTime.Now.ToLongDateString())) < 0)
            ucError.ErrorMessage = "ETA Should be greater or Equal to Today";

        if (ETD != null && DateTime.Compare(DateTime.Parse(ETD.ToString()), DateTime.Parse(DateTime.Now.ToLongDateString())) < 0)
            ucError.ErrorMessage = "ETD Should be greater or Equal to Today";

        return (!ucError.IsError);
    }

    protected void ddlDeliveryto_TextChangedEvent(object sender, EventArgs e)
    {
        if(ddlDeliveryto.SelectedHard == "1")
        {
            trAddress.Visible = false;
            ucAddrAgent.Text = "";
            ucAddrAgent.SelectedValue = "";
        }
        else
        {
            trAddress.Visible = true;
        }
    }
}
