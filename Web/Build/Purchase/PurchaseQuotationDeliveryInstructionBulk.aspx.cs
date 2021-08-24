using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
public partial class PurchaseQuotationDeliveryInstructionBulk : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuDelInstruction.MenuList = toolbarmain.Show();
            
            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    BindData(Request.QueryString["quotationid"].ToString());
                }
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
        ds = PhoenixPurchaseQuotation.EditQuotationBulk(quotationid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDeliveryInstruction.Text = dr["FLDDELIVERYINSTRUCTION"].ToString();
            txtETA.SelectedDate = General.GetNullableDateTime(dr["FLDETA"].ToString());
            txtETD.SelectedDate = General.GetNullableDateTime(dr["FLDETD"].ToString());
            ucPortMulti.SelectedValue = dr["FLDPORT"].ToString();
            ucPortMulti.Text = dr["FLDPORTNAME"].ToString();
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
                if (!IsValidDates(txtETA.SelectedDate, txtETD.SelectedDate))
                {
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {                  
                    
                    PhoenixPurchaseQuotation.UpdateQuotationDeliveryInstructionBulk(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                        ViewState["quotationid"].ToString(),
                        txtDeliveryInstruction.Text,
                         txtETA.SelectedDate,
                        txtETD.SelectedDate,
                        General.GetNullableInteger(ucPortMulti.SelectedValue));
                  

                    ucStatus.Text = "Delivery Instruction has been Saved";
                    ucStatus.Visible = true;
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
}
