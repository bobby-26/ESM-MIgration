using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class VesselAccountsReportRHRankWiseNonComplianceSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportRankWiseNonCompliance.AccessRights = this.ViewState;
            MenuReportRankWiseNonCompliance.MenuList = toolbar.Show();
            if (!IsPostBack)
            {


                ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

                cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                cblVesselType.DataTextField = "FLDTYPEDESCRIPTION";
                cblVesselType.DataValueField = "FLDVESSELTYPEID";
                cblVesselType.DataBind();

                cblRank.DataSource = PhoenixRegistersRank.ListRank();
                cblRank.DataTextField = "FLDRANKNAME";
                cblRank.DataValueField = "FLDRANKID";
                cblRank.DataBind();

                cblPrincipal.DataSource = PhoenixRegistersAddress.ListAddress("128");
                cblPrincipal.DataTextField = "FLDNAME";
                cblPrincipal.DataValueField = "FLDADDRESSCODE";
                cblPrincipal.DataBind();

                bindChechBox();
                BindYear();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
    public void bindChechBox()
    {
        try
        {
            chkReason.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 83);
            chkReason.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReportRankWiseNonCompliance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                StringBuilder strvesseltype = new StringBuilder();

                foreach (ListItem item in cblVesselType.Items)
                {
                    if (item.Selected == true)
                    {
                        strvesseltype.Append(item.Value.ToString());
                        strvesseltype.Append(",");
                    }
                }

                if (strvesseltype.Length > 1)
                {
                    strvesseltype.Remove(strvesseltype.Length - 1, 1);
                }
                StringBuilder strrank = new StringBuilder();
                foreach (ListItem item in cblRank.Items)
                {
                    if (item.Selected == true)
                    {
                        strrank.Append(item.Value.ToString());
                        strrank.Append(",");
                    }
                }
                if (strrank.Length > 1)
                {
                    strrank.Remove(strrank.Length - 1, 1);
                }
                StringBuilder strprincipal = new StringBuilder();
                foreach (ListItem item in cblPrincipal.Items)
                {
                    if (item.Selected == true)
                    {
                        strprincipal.Append(item.Value.ToString());
                        strprincipal.Append(",");
                    }
                }
                if (strprincipal.Length > 1)
                {
                    strprincipal.Remove(strprincipal.Length - 1, 1);
                }
                string reason = string.Empty;
                foreach (ListItem li in chkReason.Items)
                {
                    reason += (li.Selected ? li.Value + "," : string.Empty);
                }
                reason.TrimEnd(',');

                string parameters = "";
                parameters += "&month=" + ddlMonth.SelectedValue;
                parameters += "&year=" + ddlYear.SelectedValue;
                parameters += "&vesseltype=" + General.GetNullableString(strvesseltype.ToString());
                parameters += "&ranklist=" + General.GetNullableString(strrank.ToString());
                parameters += "&ownerlist=" + General.GetNullableString(strprincipal.ToString());
                parameters += "&reasonlist=" + General.GetNullableString(reason);
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=7&reportcode=RESTHOURSRANKWISENCSUMMARY&showmenu=false&showexcel=no&showword=no" + parameters);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
