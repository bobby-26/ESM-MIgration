using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardReportCommittedCost : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            pnlOrdereddate.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ViewState["VESSELID"] = "";

                ViewState["vslAcutid"] = "";
                ViewState["date"] = "";

                if (Request.QueryString["vslAcutid"] != null)
                    ViewState["vslAcutid"] = Request.QueryString["vslAcutid"].ToString();

                if (Request.QueryString["date"] != null)
                    ViewState["date"] = Request.QueryString["date"].ToString();

                ucVessel.DataTextField = "FLDVESSELACCOUNTNAME";
                ucVessel.DataValueField = "FLDVESSELACCOUNTID";

                ucVessel.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, null);
                ucVessel.DataBind();
                ucVessel.SelectedValue = ViewState["vslAcutid"].ToString();
                ucDate.Text = ViewState["date"].ToString();

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Repost", "REPOST", ToolBarDirection.Right);
            toolbar.AddButton("Create Commited Cost Voucher", "CREATECOMMITEDCOSTVOUCHER", ToolBarDirection.Right);
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);

            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOST&vessel=null&date=null&orderedfromdate=null&orderedtodate=null" + "&visible=" + rblDspOwnerBC.SelectedValue + "&showmenu=0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOST&vessel=" + ucVessel.SelectedValue +
                        "&date=" + ucDate.Text + "&orderedfromdate=" + ucOrderedFromDate.Text + "&orderedtodate=" + ucOrderedToDate.Text + "&visible=" + rblDspOwnerBC.SelectedValue + "&showmenu=0";

                    BindVoucherDetails();
                }
            }
            if (CommandName.ToUpper().Equals("CREATECOMMITEDCOSTVOUCHER"))
            {
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsVoucher.CommittedCostVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                 General.GetNullableDateTime(ucDate.Text)
                                                                 , int.Parse(ucVessel.SelectedValue));

                    BindVoucherDetails();

                    ucStatus.Text = "Committed cost voucher posted successfully.";
                }

            }
            if (CommandName.ToUpper().Equals("REPOST"))
            {
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVoucher.CommittedCostVoucherRepost(int.Parse(ucVessel.SelectedValue), Convert.ToDateTime(ucDate.Text));

                ucStatus.Text = "Committed cost voucher reposted successfully.";
            }
            //if (dce.CommandName.ToUpper().Equals("FINALIZE"))
            //{
            //    if (!IsValidFilter(ucDate.Text))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }
            //    else
            //    {
            //        PhoenixCommonPurchase.UpdateOrderFormFinalizeDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["VESSELID"].ToString()), Convert.ToDateTime(ucDate.Text));
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }

        if (General.GetNullableInteger(ViewState["vslAcutid"].ToString()) == null)
        {
            ucError.ErrorMessage = "Vessel account is required";
        }
        return (!ucError.IsError);
    }

    protected void VesselAccount_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedValue) != null)
        {
            DataSet ds = PhoenixRegistersVesselAccount.EditVesselAccount(int.Parse(ViewState["vslAcutid"].ToString()));

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow dr = ds.Tables[0].Rows[0];
            //    ViewState["vslAcutid"] = dr["FLDVESSELID"].ToString();
            //}
        }
        else
        {
            ViewState["VESSELID"] = "";
        }
        BindVoucherDetails();
    }


    private void BindVoucherDetails()
    {
        if (!string.IsNullOrEmpty(ucVessel.SelectedValue) && !string.IsNullOrEmpty(ucDate.Text))
        {
            DataTable dt = PhoenixAccountsVoucher.CommittedCostVoucherDetails(int.Parse(ucVessel.SelectedValue), DateTime.Parse(ucDate.Text), null);
            if (dt.Rows.Count > 0)
            {
                cmdAtt.Visible = true;
                txtVoucherNumber.Text = dt.Rows[0]["FLDVOUCHERNUMBER"].ToString();
                cmdAtt.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString() + "&mod="
               + PhoenixModule.ACCOUNTS + "'); return false;");
            }
            else
            {
                txtVoucherNumber.Text = "";
                cmdAtt.Visible = false;
            }
        }
    }

}
