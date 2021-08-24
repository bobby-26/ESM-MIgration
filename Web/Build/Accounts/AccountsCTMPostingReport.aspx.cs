using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsCTMPostingReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Voucher", "VOUCHER");
            toolbar.AddButton("Line Items", "LINEITEM");
            toolbar.AddButton("View Draft", "VIEWDRAFT");
            toolbar.AddButton("D11", "D11");
            toolbar.AddButton("Captain Cash", "CAPTAINCASH");
            toolbar.AddButton("Log", "LOG");
            MenuReportD11.AccessRights = this.ViewState;
            MenuReportD11.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                string vesselid, balanceid;              
                
                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                    vesselid = Request.QueryString["vesselid"].ToString();
                }
                else
                    vesselid = "";
                if (Request.QueryString["balanceid"] != null)
                {
                    ViewState["balanceid"] = Request.QueryString["balanceid"].ToString();
                    balanceid = Request.QueryString["balanceid"].ToString();
                }
                else
                    balanceid = "";
                DataSet ds = PhoenixAccountsCaptainCash.OfficeCaptainCashEdit(int.Parse(vesselid), new Guid(balanceid));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["month"] = dr["FLDMONTH"].ToString();
                    ViewState["year"] = dr["FLDYEAR"].ToString();
                    ViewState["from"] = dr["FLDSDATE"].ToString(); ViewState["to"] = dr["FLDEDATE"].ToString();
                  //  Title1.Text = dr["FLDVESSELNAME"].ToString() + " - Period Of  " + dr["FLDSDATE"].ToString() + " to " + dr["FLDEDATE"].ToString();
                    if (Request.QueryString["format"] != null)
                    {
                        if (Request.QueryString["format"].ToString() == "D11")
                        {
                            MenuReportD11.SelectedMenuIndex = 3;
                            ifMoreInfo.Attributes["src"] = "../Reports/ReportsViewWithSubReport.aspx?applicationcode=7&reportcode=D11&showmenu=false&showexcel=no&showword=no&vesselid=" + vesselid + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString();
                        }
                        else if (Request.QueryString["format"].ToString() == "CAPTAINCASH")
                        {
                            ifMoreInfo.Attributes["src"] = "../Reports/ReportsViewWithSubReport.aspx?applicationcode=7&reportcode=CAPTAINCASH&showmenu=false&showexcel=no&showword=no&vesselid=" + vesselid + "&FromDate=" + dr["FLDSDATE"].ToString() + "&ToDate=" + dr["FLDEDATE"].ToString();
                            MenuReportD11.SelectedMenuIndex = 4;
                        }else if (Request.QueryString["format"].ToString() == "LOG")
                        {
                            MenuReportD11.SelectedMenuIndex = 5;
                            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsPettyCashGeneral.aspx?fromdate=" + dr["FLDSDATE"].ToString() + "&todate=" + dr["FLDEDATE"].ToString() + "&vesselid=" + vesselid;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReportD11_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCash.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashVoucher.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("VIEWDRAFT"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashDraft.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("D11"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=D11");
            }
            if(CommandName.ToUpper().Equals("CAPTAINCASH"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=CAPTAINCASH");
            }
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=LOG");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
