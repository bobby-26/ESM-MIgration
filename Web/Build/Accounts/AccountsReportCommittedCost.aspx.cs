using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsReportCommittedCost : PhoenixBasePage
{
    public string Message;
    public string Messagetext;
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
                ViewState["GOODSRECEIVEDYN"] = "";
                ViewState["DEBITNOTESTATUS"] = "";

                ucVessel.DataTextField = "FLDVESSELACCOUNTNAME";
                ucVessel.DataValueField = "FLDVESSELACCOUNTID";

                ucVessel.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, null);
                ucVessel.DataBind();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Repost", "REPOST", ToolBarDirection.Right);
            toolbar.AddButton("Create Commited Cost Voucher", "CREATECOMMITEDCOSTVOUCHER", ToolBarDirection.Right);
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);

            //toolbar.AddButton("Finalize", "FINALIZE");

            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            //ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOST&vessel=null&date=null&orderedfromdate=null&orderedtodate=null&visible=null&showmenu=0";
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
                    if (int.Parse(ViewState["GOODSRECEIVEDYN"].ToString()) == 0)
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOST&vessel=" + ucVessel.SelectedValue +
                        "&date=" + ucDate.Text + "&orderedfromdate=" + ucOrderedFromDate.Text + "&orderedtodate=" + ucOrderedToDate.Text + "&visible=" + rblDspOwnerBC.SelectedValue + "&showmenu=0";
                    }
                    else
                    {
                        DataSet dspo = new DataSet();
                        dspo = PhoenixRegistersAccount.CommittedCostValidation(General.GetNullableInteger(ucVessel.SelectedValue.ToString()), General.GetNullableDateTime(ucDate.Text.ToString()));

                        if (dspo.Tables[0].Rows.Count > 0)
                        {                           
                            Messagetext = "Update the Goods receiveddate for the following PO" + "<br>";
                            int n = dspo.Tables[0].Rows.Count;
                            while (n > 0)
                            {
                                DataRow dr = dspo.Tables[0].Rows[n - 1];
                                Message += dr["FLDFORMNO"].ToString() + "<br>";
                                n--;
                            }
                            Messagetext = Messagetext + Message;
                            
                            ucError.ErrorMessage = Messagetext;
                            ucError.Visible = true;
                            return;
                        }

                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOSTRECEIVE&vessel=" + ucVessel.SelectedValue +
                       "&date=" + ucDate.Text + "&orderedfromdate=" + ucOrderedFromDate.Text + "&orderedtodate=" + ucOrderedToDate.Text + "&visible=" + rblDspOwnerBC.SelectedValue + "&showmenu=0";
                    }
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
                    if (int.Parse(ViewState["GOODSRECEIVEDYN"].ToString()) == 0)
                    {
                        PhoenixAccountsVoucher.CommittedCostVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                     General.GetNullableDateTime(ucDate.Text)
                                                                     , int.Parse(ucVessel.SelectedValue));
                    }
                    else
                    {
                        PhoenixAccountsVoucher.CommittedCostVoucherReceiveDateInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                    General.GetNullableDateTime(ucDate.Text)
                                                                    , int.Parse(ucVessel.SelectedValue));

                    }
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
                if (ViewState["DEBITNOTESTATUS"].ToString() == "Pending")
                {
                    RadWindowManager1.RadConfirm("Committed Cost voucher has been billed, are you sure you want to repost? ", "DeleteRecord", 320, 150, null, "Confirm");
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
    protected void ucConfirmmsg_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsVoucher.CommittedCostVoucherRepost(int.Parse(ucVessel.SelectedValue), Convert.ToDateTime(ucDate.Text));
            ucStatus.Text = "Committed cost voucher reposted successfully.";
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

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Vessel account is required";
        }
        return (!ucError.IsError);
    }

    protected void VesselAccount_Changed(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ucVessel.SelectedValue) != null)
            {
                DataSet ds = PhoenixRegistersVesselAccount.EditVesselAccount(int.Parse(ucVessel.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                    DataTable dt = PhoenixRegistersBudget.ListOwnerReportingFormat(General.GetNullableInteger(dr["FLDOWNER"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["GOODSRECEIVEDYN"] = dt.Rows[0]["FLDCCGOODSRECEIVEDYN"].ToString();
                    }
                }

                if (int.Parse(ViewState["GOODSRECEIVEDYN"].ToString()) == 1)
                {
                    cmdGoodsReceiveDate.Visible = true;
                }
                else
                {
                    cmdGoodsReceiveDate.Visible = false;
                }

            }

            else
            {
                ViewState["VESSELID"] = "";
            }
            BindVoucherDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                ViewState["DEBITNOTESTATUS"] = dt.Rows[0]["FLDDEBITNOTESTATUS"].ToString();
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

    protected void cmdGoodsReceiveDate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {
            if (!IsValidFilter(ucDate.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixAccountsVoucher.CommittedCostReceivedateupdate(int.Parse(ucVessel.SelectedValue),
                                                                    General.GetNullableDateTime(ucDate.Text));

            ucStatus.Text = "Goods receive date updated successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
